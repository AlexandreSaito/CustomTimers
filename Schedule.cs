using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace T
{
    public class Schedule
    {
        protected static int _maxId = 0;
        protected static List<Schedule> _schedules = new List<Schedule>();

        protected static DateTime DateNull = DateTime.Parse("1900-01-01T00:00:00");
        protected static System.Timers.Timer Timer;

        public static int IndexName = 1;
        public static int IndexTime = 2;
        public static int IndexCurrentTime = 3;
        public static List<Schedule> Schedules { get { return _schedules; } }

        public static void OnUpdate(Schedule schedule)
        {
            if (schedule.Row == null) return;
            schedule.Row.Cells[IndexCurrentTime].Value = Schedule.TimeToString(schedule.CurrentTime);
        }

        public static void OnDone(Schedule schedule)
        {
            if (schedule.Row != null)
            {
                schedule.Row.DefaultCellStyle.BackColor = Color.Green;
            }
            schedule.Option.Play();
            schedule.Option.AddCount();
        }


        public static void StartTimer()
        {
            if (Timer == null)
            {
                Timer = new System.Timers.Timer(1000);
                Timer.Elapsed += Timer_Elapsed;
            }
            Timer.Start();
        }

        public static void AddSchedule(Schedule schedule)
        {
            _schedules.Add(schedule);
        }

        public static void RemoveSchedule(Schedule schedule)
        {
            _schedules.Remove(schedule);
        }
        public static string TimeToString(int time)
        {
            DateTime dt = DateNull.AddSeconds(time);
            return dt.ToString("HH:mm:ss");
        }

        protected static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var list = new Schedule[Schedules.Count()];
            Schedules.CopyTo(list);
            foreach (var item in list)
            {
                if (item.Done) continue;
                item.CurrentTime += 1;
                OnUpdate(item);
                if (item.CurrentTime >= item.Time)
                {
                    item.Done = true;
                    OnDone(item);
                }
            }
        }

        public Schedule(Option option)
        {
            Option = option;
            ID = ++_maxId;
        }

        private string m_name = "";
        private int m_time = 0;
        public int ID { get; protected set; }
        public Option Option { get; }
        public string Name { get { return string.IsNullOrEmpty(m_name) ? Option.Name : m_name; } set { m_name = value; } }
        public int Time { get { return m_time == 0 ? Option.Time : m_time; } set { m_time = value; } }
        public int CurrentTime { get; set; }

        public bool Done { get; set; }
        public DataGridViewRow Row { get; set; }

    }
}
