using System;
using System.Linq;
using System.Windows.Forms;

namespace T
{
    public partial class Form1 : Form
    {
        // Packages: NAudio
        const int indexName = 1;
        const int indexTime = 2;

        OptionManager OM = new OptionManager();
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
            OM.Load(gvOpcoes);
            FillDDLOptions();
            FillDDLAudios();
            gvTimers.CellValueChanged += GvTimers_CellValueChanged;
            Schedule.StartTimer();
            gvTimers.Columns.Insert(4, new DataGridViewButtonColumn()
            {
                Name= "btnDelete",
                Text= "Remove"
            });
            gvTimers.CellClick += GvTimers_CellClick;
            gvOpcoes.CellClick += GvOpcoes_CellClick;
        }
        private void GvTimers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gvTimers.Columns["btnDelete"].Index)
            {
                var selected = Schedule.Schedules.FirstOrDefault(x => x.Row.Index == e.RowIndex);
                if (selected == null) return;

                gvTimers.Rows.Remove(selected.Row);
                Schedule.RemoveSchedule(selected);
                selected.Option.Stop();
            }
        }

        private void GvTimers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            var selected = Schedule.Schedules.FirstOrDefault(x => x.Row.Index == e.RowIndex);
            if (selected == null) return;

            switch (e.ColumnIndex)
            {
                case indexName: selected.Name = selected.Row.Cells[indexName].Value.ToString(); break;
                case indexTime: break;
                default: break;
            }
        }

        protected int ConvertTimeStringToSeconds(string time)
        {
            string[] times = time.Split(':');
            return (Convert.ToInt32(times[0]) * 60 + Convert.ToInt32(times[1])) * 60 + Convert.ToInt32(times[2]);
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

            DataGridViewRow row = gvTimers.Rows[gvTimers.Rows.Add(schedule.ID, schedule.Name, schedule.TimeToString(schedule.Time), schedule.TimeToString(schedule.CurrentTime))];

            schedule.Row = row;
            Schedule.AddSchedule(schedule);

            option.AddCount();
        }

        private void btnAddAudio_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Video Files (*.mp4)|*.mp4|Wav Files Only (*.wav)|*.wav";
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

            if (OM.GetOption(name) != null)
            {
                Alert("Já existe uma opção com o mesmo nome!");
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
                if(option == null)
                {
                    return;
                }

                option.Name = name;
                option.Time = time;
                option.AudioName = fileName;

                OM.Save();

                option.row.Cells[0].Value = option.Name;
                option.row.Cells[2].Value = option.TimeToString(option.Time);
                option.row.Cells[3].Value = option.AudioName;

            }
            else
            {
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

    }
}
