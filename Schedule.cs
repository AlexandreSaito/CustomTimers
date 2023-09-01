using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public static int IndexMaxTime = 3;
        public static int IndexCurrentTime = 3;
        public static int IndexStackedCount = 4;
        public static List<Schedule> Schedules { get { return _schedules; } }

        public static void OnUpdate(Schedule schedule)
        {
            if (schedule.Row == null) return;
            schedule.Row.Cells[IndexCurrentTime].Value = Schedule.TimeToString(schedule.CurrentTime);
            if (schedule.Time < schedule.CurrentTime)
                schedule.Row.DefaultCellStyle.BackColor = Color.Yellow;
        }

        public static void OnDone(Schedule schedule)
        {
            if (schedule.Row != null)
            {
                schedule.Row.DefaultCellStyle.BackColor = Color.Green;
            }
            schedule.Option.AddCount(schedule.Count);
            schedule.Option.PlaySound();
        }


        public static void StartTimer()
        {
            if (Timer == null)
            {
                Timer = new System.Timers.Timer(1000);
                Timer.Elapsed += Timer_Elapsed;
                Timer.Start();
            }
            _schedules.Clear();
            _maxId = 0;
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
                if (item.Done || item.Paused) continue;
                item.CurrentTime += 1;
                OnUpdate(item);
                if (item.CurrentTime >= item.MaxTimer)
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
        private int m_count = 1;
        private bool m_paused;

        public int ID { get; protected set; }
        public Option Option { get; }
        public string Name { get { return string.IsNullOrEmpty(m_name) ? Option.Name : m_name; } set { m_name = value; } }
        public int Time { get { return m_time == 0 ? Option.Time : m_time; } set { m_time = value; } }
        public int MaxTimer { get { return Time * Count; } }
        public int CurrentTime { get; set; }

        public int Count { get { return m_count; } set { if (value < 1) value = 1; m_count = value; } }

        public bool Done { get; set; }
        public bool Paused
        {
            get { return m_paused; }
            set
            {
                m_paused = value; if (Row != null) { if (m_paused) Row.DefaultCellStyle.BackColor = Color.Gray; else Row.DefaultCellStyle.BackColor = Color.Empty; }
            }
        }
        public DataGridViewRow Row { get; set; }

        public void UpdateDataGridViewRow()
        {
            Row.Cells[IndexName].Value = this.Name;
            Row.Cells[IndexTime].Value = Schedule.TimeToString(this.Time);
            Row.Cells[IndexMaxTime].Value = Schedule.TimeToString(this.MaxTimer);
            Row.Cells[IndexCurrentTime].Value = Schedule.TimeToString(this.CurrentTime);
            Row.Cells[IndexStackedCount].Value = this.Count;
        }

    }
}
