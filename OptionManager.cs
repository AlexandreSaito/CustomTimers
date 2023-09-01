using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace T
{
    public class OptionManager
    {
        protected static Type int_type = typeof(int);

        protected static Type optionType = typeof(Option);
        protected static PropertyInfo[] properties = optionType.GetProperties();

        public static string LastSelected()
        {
            return CustomConfiguration.Singleton.LastSelected;
        }

        protected XDocument m_document;

        public List<Option> Options { get; protected set; } = new List<Option>();

        protected string filePath;
        protected DataGridView grid;

        public OptionManager(string predefName, DataGridView grid)
        {
            this.grid = grid;
            CustomConfiguration.Singleton.LastSelected = predefName;
            filePath = CustomConfiguration.PredefsFolderPath + "/" + predefName + ".xml";
            if (!Directory.Exists(CustomConfiguration.PredefsFolderPath)) Directory.CreateDirectory(CustomConfiguration.PredefsFolderPath);

            if (!File.Exists(filePath))
            {
                m_document = new XDocument();
                m_document.Add(new XElement("predefs"));
                return;
            }
            m_document = XDocument.Load(filePath);
        }

        public void Load()
        {
            IEnumerable<XElement> xElements = m_document.Root.Elements();

            foreach (XElement xElement in xElements)
            {
                Option option = new Option();

                XElement xElementCount = null;

                foreach (PropertyInfo property in properties)
                {
                    string propName = property.Name[0].ToString().ToLower() + property.Name.Substring(1, property.Name.Length - 1);

                    XElement xProp = xElement.Element(propName);
                    if (xProp == null) continue;

                    if (xProp.Name == "count") xElementCount = xProp;

                    if (property.PropertyType == int_type)
                    {
                        property.SetValue(option, Convert.ToInt32(xProp.Value));
                        continue;
                    }

                    property.SetValue(option, xProp.Value);
                }

                if (xElementCount == null)
                {
                    xElementCount = new XElement("count");
                    xElementCount.Value = "0";
                    xElement.Add(xElementCount);
                }

                DataGridViewRow row = grid.Rows[grid.Rows.Add(option.Name, option.Count, option.TimeToString(option.Time), option.AudioName)];
                option.row = row;
                option.xElementCount = xElementCount;
                Options.Add(option);
            }
        }

        public void AddItem(Option option)
        {
            XElement xOption = new XElement("option");
            XElement xElementCount = null;

            foreach (PropertyInfo property in properties)
            {
                object propValueObject = property.GetValue(option);
                if (propValueObject == null) continue;

                string propValue = Convert.ToString(propValueObject).Trim();
                string propName = property.Name[0].ToString().ToLower() + property.Name.Substring(1, property.Name.Length - 1);

                XElement element = new XElement(propName)
                {
                    Value = propValue
                };
                if (property.Name == "Count") xElementCount = element;
                xOption.Add(element);
            }

            m_document.Root.Add(xOption);
            Save();

            DataGridViewRow row = grid.Rows[grid.Rows.Add(option.Name, option.Count, option.TimeToString(option.Time), option.AudioName)];
            option.row = row;
            option.xElementCount = xElementCount;
            Options.Add(option);
        }

        public void EditItem(string name, Option option)
        {
            XElement xOption = m_document.Root.Elements().FirstOrDefault(x => x.Element("name").Value == name);

            if (xOption == null) return;

            foreach (PropertyInfo property in properties)
            {
                object propValueObject = property.GetValue(option);
                if (propValueObject == null) continue;

                string propValue = Convert.ToString(propValueObject).Trim();
                string propName = property.Name[0].ToString().ToLower() + property.Name.Substring(1, property.Name.Length - 1);


                XElement element = xOption.Element(propName);
                if (element == null)
                {
                    element = new XElement(propName)
                    {
                        Value = propValue
                    };
                    xOption.Add(element);
                    continue;
                }
                element.Value = propValue;
            }

            Save();
        }

        public Option GetOption(string name)
        {
            return Options.FirstOrDefault(x => x.Name == name);
        }

        public void Save()
        {
            m_document.Save(filePath);
        }

        ~OptionManager()
        {
            Save();
        }
    }
}
