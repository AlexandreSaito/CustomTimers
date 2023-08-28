using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NAudio.Wave;

namespace T
{
    public class AudioOptions
    {
        public const string AudiosFolder = "./audios";
        protected const string xmlFileName = "./audios_option.xml";

        protected XDocument m_document;

        public AudioOptions()
        {
            if (!File.Exists(xmlFileName))
            {
                m_document = new XDocument();
                m_document.Add(new XElement("audios"));
                return;
            }
            m_document = XDocument.Load(xmlFileName);
        }

        public List<string> GetFileNames()
        {
            List<string> list = new List<string>();

            var items = m_document.Root.Elements();

            foreach (var item in items)
            {
                list.Add(item.Value);
            }

            return list;
        }

        public string GetFileName(string filePath)
        {
            int lastIndex = (filePath.LastIndexOf('/') > filePath.LastIndexOf('\\') ? filePath.LastIndexOf('/') : filePath.LastIndexOf('\\')) + 1;
            if (lastIndex == 0) return filePath;
            int dotIndex = filePath.LastIndexOf('.');

            return filePath.Substring(lastIndex, filePath.Length - lastIndex - (filePath.Length - dotIndex));
        }

        public bool FileExists(string filePath)
        {
            string fileName = GetFileName(filePath);
            return m_document.Root.Elements().Where(x => x.Value == fileName).Count() != 0;
        }

        public bool SaveAudio(string filePath)
        {
            string fileName = GetFileName(filePath);
            string outputFilePath = AudiosFolder + "/" + fileName + ".wav";
            
            if (!Directory.Exists(AudiosFolder))
            {
                Directory.CreateDirectory(AudiosFolder);
            }

            if(filePath.Substring(filePath.LastIndexOf('.') + 1, filePath.Length - (filePath.LastIndexOf('.') + 1)) != "wav")
            {
                // Use the Mp3FileReader to obtain the content of the audio and use the wavfilewriter
                // to create the new file in wav format in the providen path with the content of the file
                using (MediaFoundationReader reader = new MediaFoundationReader(filePath))
                {
                    WaveFileWriter.CreateWaveFile(outputFilePath, reader);
                }
            }

            // Case file already exists he is just replaced
            if (!FileExists(fileName))
            {
                m_document.Root.Add(new XElement("audio") { Value = fileName });
                m_document.Save(xmlFileName);
            }

            return true;
        }
    }
}
