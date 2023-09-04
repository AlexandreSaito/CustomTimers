using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T
{
    public class Cronometer
    {
        protected static int _maxId = 0;
        protected static System.Timers.Timer Timer;

        protected static List<Cronometer> _cronometers = new List<Cronometer>();

        public static List<Cronometer> Cronometers { get { return _cronometers; } }

        public static void StartTimer()
        {
            if (Timer == null)
            {
                Timer = new System.Timers.Timer(1000);
                Timer.Elapsed += Timer_Elapsed;
                Timer.Start();
            }
            _cronometers.Clear();
            _maxId = 0;
        }

        public static void ClearCronometers()
        {
            _cronometers.Clear();
        }

        public static void AddCronometer(Cronometer cronometer)
        {
            _cronometers.Add(cronometer);
        }

        protected static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var list = new Cronometer[Cronometers.Count()];
            Cronometers.CopyTo(list);
            foreach (var item in list)
            {
                if (item.Paused) continue;
                item.CurrentTime += 1;
                item.Update();
            }
        }

        
        public int CurrentTime { get; set; }
        public bool Paused { get; set; }
        public TextBox TextBox { get; set; }
        public void Update()
        {
            if (TextBox.InvokeRequired)
            {
                Action safeWrite = delegate { TextBox.Text = Schedule.TimeToString(CurrentTime); };
                TextBox.Invoke(safeWrite);
            }
            else
                TextBox.Text = Schedule.TimeToString(CurrentTime);

        }
    }
}
