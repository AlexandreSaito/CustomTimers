using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T
{
    public class CustomConfiguration
    {
        protected const string configsFilePath = "./configs";

        protected const string configLine = "{0}: {1}";

        private static CustomConfiguration _customConfiguration;
        public static CustomConfiguration Singleton
        {
            get
            {
                if (_customConfiguration == null)
                    lock (configsFilePath)
                        if (_customConfiguration == null) _customConfiguration = new CustomConfiguration();
                return _customConfiguration;
            }
        }

        public string LastSelected { get; set; }

        public CustomConfiguration()
        {
            string[] configs;
            using (var file = new FileInfo(configsFilePath).OpenRead())
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    configs = sr.ReadToEnd().Split('\n');
                }
            }

            LastSelected = GetProperty(configs, "LAST_SELECTED", "Default");
        }

        protected string GetProperty(string[] configs, string prop, string defaultValue = null)
        {
            if (configs == null || configs.Length == 0) return defaultValue;
            string lastSelected = configs.FirstOrDefault(x => x.StartsWith(prop));
            if (defaultValue != null && string.IsNullOrEmpty(lastSelected)) return defaultValue;
            return lastSelected.Replace($"{prop}:", "").Trim();
        }

        public void Save()
        {
            string configs = string.Format(configLine, "LAST_SELECTED", LastSelected);
            File.WriteAllText(configsFilePath, configs);
        }

        ~CustomConfiguration()
        {
            Save();
        }

    }
}
