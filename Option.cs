using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace T
{
    public class Option
    {
        protected SoundPlayer m_soundPlayer;

        public XElement xElementCount;

        public DataGridViewRow row;

        public string Name { get; set; }
        public int Time { get; set; }
        public string AudioName { get; set; }

        public int Count { get; set; }

        public void AddCount(int quantity)
        {
            if (xElementCount == null) return;
            Count += quantity;
            xElementCount.Value = Count.ToString();
            row.Cells[1].Value = Count.ToString();
        }

        protected void CreateSoundPlayer()
        {
            if (string.IsNullOrEmpty(this.AudioName)) return;
            m_soundPlayer = new SoundPlayer(AudioOptions.AudiosFolder + "/" + this.AudioName + ".wav");
        }

        public void LoadSound()
        {
            if (m_soundPlayer == null)
            {
                CreateSoundPlayer();
                m_soundPlayer.Load();
            }
        }

        public void PlaySound()
        {
            if (m_soundPlayer == null)
            {
                CreateSoundPlayer();
            }
            if (m_soundPlayer == null) return;
            m_soundPlayer.PlayLooping();
        }

        public void StopSound()
        {
            if (m_soundPlayer == null) return;
            m_soundPlayer.Stop();
        }

        public string TimeToString(int time)
        {
            DateTime dt = DateTime.Parse("1900-01-01T00:00:00");

            dt = dt.AddSeconds(time);

            return dt.ToString("HH:mm:ss");
        }
    }
}
