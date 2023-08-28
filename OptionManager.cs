using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace T
{
    public class OptionManager
    {
        protected static Type int_type = typeof(int);

        public const string PredefsFolderPath = "./predefs";
        protected static Type optionType = typeof(Option);
        protected static PropertyInfo[] properties = optionType.GetProperties();

        public static string LastSelected()
        {
            if (!File.Exists("./configs")) return "Default";
            string[] configs;
            using (var file = new FileInfo("./configs").OpenRead())
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    configs = sr.ReadToEnd().Split('\n');
                }
            }

            string lastSelected = configs.FirstOrDefault(x => x.StartsWith("LAST_SELECTED"));
            if (string.IsNullOrEmpty(lastSelected)) return "Default";

            return lastSelected.Replace("LAST_SELECTED:", "").Trim();
        }

        protected XDocument m_document;

        public List<Option> Options { get; protected set; } = new List<Option>();

        protected string fileName;
        protected static string name;
        protected DataGridView grid;

        public OptionManager(string predefName, DataGridView grid)
        {
            this.grid = grid;
            name = predefName;
            fileName = PredefsFolderPath + "/" + predefName + ".xml";

            if (!Directory.Exists(PredefsFolderPath)) Directory.CreateDirectory(PredefsFolderPath);

            if (!File.Exists(fileName))
            {
                m_document = new XDocument();
                m_document.Add(new XElement("predefs"));
                return;
            }
            m_document = XDocument.Load(fileName);
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
            m_document.Save(fileName);
            File.WriteAllText("./configs", "LAST_SELECTED: " + name);
        }

        ~OptionManager()
        {
            Save();
        }
    }
}
