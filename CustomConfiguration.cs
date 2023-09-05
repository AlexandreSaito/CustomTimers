using System.IO;
using System.Linq;

namespace T
{
    public class CustomConfiguration
    {
        public const string AudiosFolder = "./data/audios";
        public const string XmlFileName = "./data/audios_option.xml";
        public const string PredefsFolderPath = "./data/predefs";
        public const string ConfigsFilePath = "./data/configs";
        public const string TimersFolderPath = "./data/timers";

        protected const string configLine = "{0}: {1}\n";

        private static CustomConfiguration _customConfiguration;
        public static CustomConfiguration Singleton
        {
            get
            {
                if (_customConfiguration == null)
                    lock (ConfigsFilePath)
                        if (_customConfiguration == null) _customConfiguration = new CustomConfiguration();
                return _customConfiguration;
            }
        }

        public string LastSelected { get; set; }
        public bool PlayAlertInLoop { get; set; }

        public CustomConfiguration()
        {
            if (!Directory.Exists("./data")) Directory.CreateDirectory("./data");
            string[] configs;
            if (File.Exists(ConfigsFilePath))
            {
                using (var file = new FileInfo(ConfigsFilePath).OpenRead())
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        configs = sr.ReadToEnd().Split('\n');
                    }
                }
            }
            else
            {
                configs = new string[0];
            }

            LastSelected = GetProperty(configs, "LAST_SELECTED", "Default");
            PlayAlertInLoop = GetProperty(configs, "PLAY_LOOP", "FALSE") == "TRUE";
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
            configs += string.Format(configLine, "PLAY_LOOP", PlayAlertInLoop ? "TRUE" : "FALSE");
            File.WriteAllText(ConfigsFilePath, configs);
        }

        ~CustomConfiguration()
        {
            Save();
        }

    }
}
