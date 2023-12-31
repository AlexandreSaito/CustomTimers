﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace T
{
    public partial class Form1 : Form
    {
        // Packages: NAudio
        protected const string AudioFilters = "Video Files (*.mp4)|*.mp4|Wav Files (*.wav)|*.wav";

        protected int IndexBtnPauseTimer = 4;
        protected int IndexBtnResetTimer = 4;
        protected int IndexBtnDeleteTimer = 4;

        OptionManager OM;
        readonly AudioOptions AudioOptions = new AudioOptions();

        public Form1()
        {
            InitializeComponent();
        }

        protected void Alert(string message)
        {
            MessageBox.Show(message, "OPA!");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chkPlayAlertInLoop.Checked = CustomConfiguration.Singleton.PlayAlertInLoop;

            gvTimers.CellValueChanged += GvTimers_CellValueChanged;
            gvTimers.CellClick += GvTimers_CellClick;
            gvOpcoes.CellDoubleClick += GvOpcoes_CellDoubleClick;

            Schedule.IndexName = gvTimers.Columns["Name"].Index;
            Schedule.IndexTime = gvTimers.Columns["Time"].Index;
            Schedule.IndexMaxTime = gvTimers.Columns["TotalTime"].Index;
            Schedule.IndexCurrentTime = gvTimers.Columns["CurrentTime"].Index;
            Schedule.IndexStackedCount = gvTimers.Columns["Stack"].Index;
            Schedule.IndexCount = gvTimers.Columns["Count"].Index;
            IndexBtnDeleteTimer = gvTimers.Columns["btnDelete"].Index;
            IndexBtnPauseTimer = gvTimers.Columns["btnPause"].Index;
            IndexBtnResetTimer = gvTimers.Columns["btnReset"].Index;

            Option.IndexName = gvOpcoes.Columns["OptionName"].Index;
            Option.IndexTime = gvOpcoes.Columns["OptionTimer"].Index;
            Option.IndexFile = gvOpcoes.Columns["OptionFile"].Index;
            Option.IndexCount = gvOpcoes.Columns["OptionCount"].Index;

            Schedule.StartTimer();
            Cronometer.StartTimer();
            string lastSelected = OptionManager.LastSelected();
            FillDDLAlertGroup(lastSelected);
            //LoadAlertDefinition(lastSelected);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            OM.Save();
            Schedule.SaveCurrentTimers();
        }

        protected void LoadAlertDefinition(string definition)
        {
            var list = Schedule.Schedules.GroupBy(x => x.Option).Select(x => x.Key).Distinct();
            foreach (var item in list) item.StopSound();
            ClearOptionsFields();
            gvTimers.Rows.Clear();
            gvOpcoes.Rows.Clear();
            if (OM != null) OM.Save();
            OM = new OptionManager(definition, gvOpcoes);
            OM.Load();
            FillDDLOptions();
            FillDDLAudios();
            Schedule.LoadFromDefinition(definition, OM);
            Schedule.DoForAll((Schedule s) =>
            {
                DataGridViewRow row = gvTimers.Rows[gvTimers.Rows.Add(s.ID, s.Name)];
                s.Row = row;
                Schedule.Validate(s, false);
                if (!s.Done) s.Paused = true;
                s.UpdateDataGridViewRow();
            });
        }

        protected void ResetTimer(Schedule schedule)
        {
            schedule.Option.StopSound();
            gvTimers.Rows.Remove(schedule.Row);
            Schedule.RemoveSchedule(schedule);
            AddTimer(schedule.Option.Name, schedule.Name, schedule.Count);
        }

        private void GvTimers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == IndexBtnDeleteTimer)
            {
                Schedule selected = Schedule.Schedules.FirstOrDefault(x => x.Row.Index == e.RowIndex);
                if (selected == null) return;

                gvTimers.Rows.Remove(selected.Row);
                Schedule.RemoveSchedule(selected);
            }
            else if (e.ColumnIndex == IndexBtnPauseTimer)
            {
                Schedule selected = Schedule.Schedules.FirstOrDefault(x => x.Row.Index == e.RowIndex);
                if (selected == null) return;

                if (selected.Done) selected.Option.StopSound();
                else
                {
                    selected.Paused = !selected.Paused;
                    DataGridViewButtonCell button = (DataGridViewButtonCell)gvTimers.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    button.UseColumnTextForButtonValue = !selected.Paused;
                    if (selected.Paused) button.Value = "Retomar";
                    else button.Value = "Pausar";
                }
            }
            else if (e.ColumnIndex == IndexBtnResetTimer)
            {
                Schedule selected = Schedule.Schedules.FirstOrDefault(x => x.Row.Index == e.RowIndex);
                if (selected == null) return;
                ResetTimer(selected);
            }
        }

        private void GvTimers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var selected = Schedule.Schedules.FirstOrDefault(x => x.Row.Index == e.RowIndex);
            if (selected == null) return;

            if (e.ColumnIndex == Schedule.IndexName)
            {
                selected.Name = selected.Row.Cells[Schedule.IndexName].Value.ToString();
            }
            else if (e.ColumnIndex == Schedule.IndexTime)
            {
                if (!int.TryParse(selected.Row.Cells[Schedule.IndexTime].Value.ToString().Replace(":", ""), out int result))
                {
                    Alert("O valor não pode conter letrar ou caracteres especiais exceto ':'");
                    return;
                }
                string[] value = selected.Row.Cells[Schedule.IndexTime].Value.ToString().Split(':');
                if (value.Length > 3)
                {
                    Alert("O valor deve conter no maximo hora:minuto:segundo");
                    return;
                }
                int seconds = 0;
                if (value.Length == 1)
                {
                    seconds = Convert.ToInt32(value[0]);
                }
                else if (value.Length == 2)
                {
                    seconds = Convert.ToInt32(value[0]) * 60 + Convert.ToInt32(value[1]);
                }
                else if (value.Length == 3)
                {
                    seconds = (Convert.ToInt32(value[0]) * 60 + Convert.ToInt32(value[1])) * 60 + Convert.ToInt32(value[2]);
                }
                selected.Time = seconds;
            }
            else if (e.ColumnIndex == Schedule.IndexStackedCount)
            {
                if (!int.TryParse(selected.Row.Cells[Schedule.IndexStackedCount].Value.ToString().Replace(":", ""), out int result))
                {
                    Alert("O valor não pode conter letrar ou caracteres especiais.");
                    return;
                }
                selected.Count = result;
            }
            selected.UpdateDataGridViewRow();
        }

        protected int ConvertTimeStringToSeconds(string time)
        {
            string[] times = time.Split(':');
            return (Convert.ToInt32(times[0]) * 60 + Convert.ToInt32(times[1])) * 60 + Convert.ToInt32(times[2]);
        }

        protected void FillDDLAlertGroup(string selected)
        {
            ddlAlertGroup.Items.Clear();
            DirectoryInfo directory = new DirectoryInfo(CustomConfiguration.PredefsFolderPath);
            var files = directory.GetFiles();
            foreach (var item in files)
            {
                ddlAlertGroup.Items.Add(item.Name.Replace(".xml", ""));
            }
            ddlAlertGroup.SelectedIndex = ddlAlertGroup.Items.IndexOf(selected);
        }

        protected void FillDDLOptions()
        {
            ddlOption.Items.Clear();
            foreach (var item in OM.Options)
            {
                ddlOption.Items.Add(item.Name);
            }
        }

        protected void FillDDLAudios()
        {
            ddlAudio.Items.Clear();
            foreach (var item in AudioOptions.GetFileNames())
            {
                ddlAudio.Items.Add(item);
            }
        }

        public void AddTimer(string name, string customName, int stack)
        {
            Option option = OM.GetOption(name);

            if (option == null)
            {
                Alert("Opção não encontrada!");
                return;
            }

            Schedule schedule = new Schedule(option)
            {
                Name = customName,
                Count = stack
            };

            DataGridViewRow row = gvTimers.Rows[gvTimers.Rows.Add(schedule.ID, schedule.Name)];
            schedule.Row = row;
            schedule.UpdateDataGridViewRow();
            Schedule.AddSchedule(schedule);
        }

        private void btnAddAudio_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = AudioFilters
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                AudioOptions.SaveAudio(ofd.FileName);
                FillDDLAudios();
            }
        }

        private void btnAddTimer_Click(object sender, EventArgs e)
        {
            if (ddlOption.SelectedIndex == -1) return;
            string name = ddlOption.SelectedItem.ToString();
            if (string.IsNullOrEmpty(name)) return;
            string customName = txtCustomName.Text;
            int stack = (int)txtStack.Value;
            AddTimer(name, customName, stack);
            txtStack.Value = 1;
        }

        private void btnSaveOption_Click(object sender, EventArgs e)
        {
            if (ddlAudio.SelectedIndex == -1)
            {
                Alert("Um arquivo de audio deve ser escolhido!");
                return;
            }

            string name = txtOptionName.Text;
            int time = ConvertTimeStringToSeconds(txtOptionTime.Text);
            int count = (int)txtOptionCount.Value;
            string fileName = ddlAudio.SelectedItem.ToString();

            if (string.IsNullOrEmpty(name))
            {
                Alert("O nome não pode ser vazio!");
                return;
            }

            if (time == 0)
            {
                Alert("O tempo não pode ser vazio e/ou 0");
                return;
            }

            if (!string.IsNullOrEmpty(txtOldName.Text))
            {
                var option = OM.GetOption(txtOldName.Text);
                if (option == null)
                {
                    return;
                }

                option.Name = name;
                option.Time = time;
                option.AudioName = fileName;
                option.Count = count;

                OM.EditItem(txtOldName.Text, option);

                option.UpdateDataGridRow();

                var s = Schedule.Schedules.Where(x => x.Option == option);
                foreach (var item in s)
                {
                    item.UpdateDataGridViewRow();
                }
            }
            else
            {
                if (OM.GetOption(name) != null)
                {
                    Alert("Já existe uma opção com o mesmo nome!");
                    return;
                }
                OM.AddItem(new Option { Name = name, Time = time, AudioName = fileName });
            }

            FillDDLOptions();
            ClearOptionsFields();
        }

        private void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOption.SelectedIndex == -1) return;
            txtCustomName.Text = ddlOption.SelectedItem.ToString();
        }


        private void GvOpcoes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var option = OM.Options.Find(x => x.row != null && x.row.Index == e.RowIndex);
            if (option == null) return;

            txtOldName.Text = option.Name;
            txtOptionName.Text = option.Name;
            txtOptionTime.Text = option.TimeToString(option.Time);
            ddlAudio.SelectedIndex = ddlAudio.Items.IndexOf(option.AudioName);
            txtOptionCount.Value = option.Count;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ClearOptionsFields();
        }

        protected void ClearOptionsFields()
        {
            txtOldName.Text = "";
            txtOptionName.Text = "";
            txtOptionTime.Text = "00:00:00";
            txtOptionCount.Value = 0;
        }

        private void btnAddDefAlert_Click(object sender, EventArgs e)
        {
            FormNewDefAlert formNewDefAlert = new FormNewDefAlert();
            if (formNewDefAlert.ShowDialog() == DialogResult.OK)
            {
                LoadAlertDefinition(formNewDefAlert.TextValue);
                OM.Save();
                FillDDLAlertGroup(formNewDefAlert.TextValue);
            }
        }

        private void ddlAlertGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAlertDefinition(ddlAlertGroup.SelectedItem.ToString());
        }

        private void chkPlayAlertInLoop_CheckedChanged(object sender, EventArgs e)
        {
            CustomConfiguration.Singleton.PlayAlertInLoop = chkPlayAlertInLoop.Checked;
            CustomConfiguration.Singleton.Save();
        }

        private void btnStartCronometer_Click(object sender, EventArgs e)
        {
            Cronometer.ClearCronometers();
            var c = new Cronometer();
            c.TextBox = txtCronometer;
            btnToggleCronometer.Text = c.Paused ? "Retomar" : "Pausar";
            Cronometer.AddCronometer(c);
        }

        private void btnToggleCronometer_Click(object sender, EventArgs e)
        {
            var c = Cronometer.Cronometers.FirstOrDefault();
            if (c == null) return;
            c.Paused = !c.Paused;
            btnToggleCronometer.Text = c.Paused ? "Retomar" : "Pausar";
        }

        private void btnResumeTimers_Click(object sender, EventArgs e)
        {
            Schedule.DoForAll((Schedule s) => { if (!s.Done) s.Paused = false; });
        }

        private void btnPauseTimers_Click(object sender, EventArgs e)
        {
            Schedule.DoForAll((Schedule s) => { if(!s.Done) s.Paused = true; });
        }

        private void btnResetTimers_Click(object sender, EventArgs e)
        {
            Schedule.DoForAll((Schedule s) => { ResetTimer(s); });
        }

        private void btnClearTimers_Click(object sender, EventArgs e)
        {
            Schedule.DoForAll((Schedule s) => { Schedule.RemoveSchedule(s); gvTimers.Rows.Remove(s.Row); });
        }
    }
}
