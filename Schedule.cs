using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace T
{
    public class Schedule
    {
        protected static int _maxId = 0;
        protected static List<Schedule> _schedules = new List<Schedule>();

        protected static DateTime DateNull = DateTime.Parse("1900-01-01T00:00:00");
        protected static System.Timers.Timer Timer;

        public static int IndexName = -1;
        public static int IndexTime = -1;
        public static int IndexMaxTime = -1;
        public static int IndexCurrentTime = -1;
        public static int IndexStackedCount = -1;
        public static int IndexCount = -1;

        protected static string currentDefinition;

        public static List<Schedule> Schedules { get { return _schedules; } }

        public static void OnUpdate(Schedule schedule)
        {
            if (schedule.Row == null) return;
            schedule.Row.Cells[IndexCurrentTime].Value = Schedule.TimeToString(schedule.CurrentTime);
            if (schedule.Time < schedule.CurrentTime) schedule.Row.DefaultCellStyle.BackColor = Color.Yellow;
        }

        public static void OnDone(Schedule schedule, bool addCount)
        {
            if (schedule.Row != null)
            {
                schedule.Row.DefaultCellStyle.BackColor = Color.Green;
            }
            if (addCount)
            {
                schedule.Option.AddCount(schedule.Count);
                schedule.Option.PlaySound();
                var list = Schedules.Where(x => x.Option == schedule.Option);
                foreach (var item in list)
                {
                    item.UpdateDataGridViewRow();
                }
            }
        }

        public static void Validate(Schedule schedule, bool addCount = true)
        {
            if (schedule.CurrentTime >= schedule.MaxTimer)
            {
                schedule.Done = true;
                OnDone(schedule, addCount);
            }
        }

        public static void DoForAll(Action<Schedule> action)
        {
            var list = new Schedule[Schedules.Count()];
            Schedules.CopyTo(list);
            foreach (var item in list)
            {
                action(item);
            }
        }

        public static void StartTimer()
        {
            if (Timer == null)
            {
                Timer = new System.Timers.Timer(1000);
                Timer.Elapsed += Timer_Elapsed;
                Timer.Start();
            }
        }

        public static void AddSchedule(Schedule schedule)
        {
            _schedules.Add(schedule);
        }

        public static void RemoveSchedule(Schedule schedule)
        {
            schedule.Option.StopSound();
            if (!schedule.Done)
            {
                int count = schedule.Count > 0 ? schedule.CurrentTime / schedule.Time : 0;
                if (count > 0) schedule.Option.AddCount(count);
            }
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
                Validate(item);
            }
        }

        public static void LoadFromDefinition(string definition, OptionManager om)
        {
            SaveCurrentTimers();
            if (currentDefinition == definition) return;
            _schedules.Clear();
            _maxId = 0;
            currentDefinition = definition;

            if (!File.Exists(CustomConfiguration.TimersFolderPath + "/" + currentDefinition + ".xml")) return;
            XDocument document = XDocument.Load(CustomConfiguration.TimersFolderPath + "/" + currentDefinition + ".xml");
            foreach (var item in document.Root.Elements())
            {
                try
                {
                    Option option = om.GetOption(item.Element("option_name").Value);
                    if (option == null) continue;
                    Schedule s = new Schedule(option)
                    {
                        Name = item.Element("timer_name").Value,
                        Time = Convert.ToInt32(item.Element("timer_time").Value),
                        CurrentTime = Convert.ToInt32(item.Element("timer_current_time").Value),
                        Count = Convert.ToInt32(item.Element("timer_stack").Value),
                    };
                    AddSchedule(s);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
        }

        public static void SaveCurrentTimers()
        {
            if (string.IsNullOrEmpty(currentDefinition)) return;
            if (!Directory.Exists(CustomConfiguration.TimersFolderPath)) Directory.CreateDirectory(CustomConfiguration.TimersFolderPath);

            XDocument document = new XDocument();
            document.Add(new XElement("timers"));
            DoForAll((Schedule s) =>
            {
                XElement element = new XElement("timer");
                element.Add(
                    new XElement("option_name") { Value = s.Option.Name },
                    new XElement("timer_name") { Value = s.Name },
                    new XElement("timer_time") { Value = s.Time.ToString() },
                    new XElement("timer_current_time") { Value = s.CurrentTime.ToString() },
                    new XElement("timer_stack") { Value = s.Count.ToString() }
                    );
                document.Root.Add(element);
            });
            document.Save(CustomConfiguration.TimersFolderPath + "/" + currentDefinition + ".xml");

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
            Row.Cells[IndexCount].Value = this.Option.Count;
        }

    }
}
