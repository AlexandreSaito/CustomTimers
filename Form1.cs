using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace T
{
    public partial class Form1 : Form
    {
        // Packages: NAudio
        protected const string AudioFilters = "Video Files (*.mp4)|*.mp4|Wav Files (*.wav)|*.wav";

        protected int IndexBtnDeleteTimer = 4;

        OptionManager OM;
        AudioOptions AudioOptions = new AudioOptions();

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
            gvTimers.Columns.Insert(gvTimers.Columns.Count, new DataGridViewButtonColumn()
            {
                Name = "btnDelete",
                Text = "Remove"
            });

            gvTimers.CellValueChanged += GvTimers_CellValueChanged;
            gvTimers.CellClick += GvTimers_CellClick;
            gvOpcoes.CellClick += GvOpcoes_CellClick;

            if (gvTimers.Columns["Name"] != null) Schedule.IndexName = gvTimers.Columns["Name"].Index;
            if (gvTimers.Columns["Time"] != null) Schedule.IndexTime = gvTimers.Columns["Time"].Index;
            if (gvTimers.Columns["CurrentTime"] != null) Schedule.IndexCurrentTime = gvTimers.Columns["CurrentTime"].Index;
            if (gvTimers.Columns["btnDelete"] != null) IndexBtnDeleteTimer = gvTimers.Columns["btnDelete"].Index;

            string lastSelected = OptionManager.LastSelected();
            FillDDLAlertGroup(lastSelected);
            LoadAlertDefinition(lastSelected);
        }

        protected void LoadAlertDefinition(string definition)
        {
            gvTimers.Rows.Clear();
            gvOpcoes.Rows.Clear();
            OM = null;
            OM = new OptionManager(definition, gvOpcoes);
            OM.Load();
            FillDDLOptions();
            FillDDLAudios();
            Schedule.StartTimer();
        }

        private void GvTimers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == IndexBtnDeleteTimer)
            {
                Schedule selected = Schedule.Schedules.FirstOrDefault(x => x.Row.Index == e.RowIndex);
                if (selected == null) return;

                gvTimers.Rows.Remove(selected.Row);
                Schedule.RemoveSchedule(selected);
                selected.Option.StopSound();
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
                if(!int.TryParse(selected.Row.Cells[Schedule.IndexTime].Value.ToString().Replace(":", ""), out int result))
                {
                    Alert("O valor não pode conter letrar ou caracteres especiais exceto ':'");
                    return;
                }
                string[] value = selected.Row.Cells[Schedule.IndexTime].Value.ToString().Split(':');
                if(value.Length > 3)
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
                selected.Row.Cells[Schedule.IndexTime].Value = Schedule.TimeToString(selected.Time);
            }
        }

        protected int ConvertTimeStringToSeconds(string time)
        {
            string[] times = time.Split(':');
            return (Convert.ToInt32(times[0]) * 60 + Convert.ToInt32(times[1])) * 60 + Convert.ToInt32(times[2]);
        }

        protected void FillDDLAlertGroup(string selected)
        {
            ddlAlertGroup.Items.Clear();
            DirectoryInfo directory = new DirectoryInfo(OptionManager.PredefsFolderPath);
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

        public void AddTimer(string name, string customName)
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
            };

            DataGridViewRow row = gvTimers.Rows[gvTimers.Rows.Add(schedule.ID, schedule.Name, Schedule.TimeToString(schedule.Time), Schedule.TimeToString(schedule.CurrentTime))];

            schedule.Row = row;
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
            AddTimer(name, customName);
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

                OM.EditItem(txtOldName.Text, option);

                option.row.Cells[0].Value = option.Name;
                option.row.Cells[2].Value = option.TimeToString(option.Time);
                option.row.Cells[3].Value = option.AudioName;

                var s = Schedule.Schedules.Where(x => x.Option == option);
                foreach (var item in s)
                {
                    item.Row.Cells[Schedule.IndexName].Value = item.Name;
                    item.Row.Cells[Schedule.IndexTime].Value = option.TimeToString(item.Time);
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

        private void GvOpcoes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var option = OM.Options.Find(x => x.row != null && x.row.Index == e.RowIndex);
            if (option == null) return;

            txtOldName.Text = option.Name;
            txtOptionName.Text = option.Name;
            txtOptionTime.Text = option.TimeToString(option.Time);
            ddlAudio.SelectedIndex = ddlAudio.Items.IndexOf(option.AudioName);
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
    }
}
