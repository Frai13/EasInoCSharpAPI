using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasInoAPI;
using EasInoAPI.Configuration;
using static EasInoExamples.Common;

namespace EasInoWFExamples.Examples
{
    public partial class ConfigurationWindow : Form
    {
        public EasIno easino;
        private bool connected = false;

        public ConfigurationWindow()
        {
            InitializeComponent();
        }

        private void Configuration_Load(object sender, EventArgs e)
        {
            lbl_api.Text = $"API Version: {EasIno.GetVersion()}";
            foreach (var item in GetConfigs())
            {
                cb_config.Items.Add(item.GetType().Name);
            }
            cb_config.SelectedIndex = 0;
        }

        private static IEnumerable<GenericConfiguration> GetConfigs()
        {
            var type = typeof(GenericConfiguration);
            IEnumerable<GenericConfiguration> configsNull = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p != type)
                .Select(s => (GenericConfiguration)Activator.CreateInstance(s));

            IList<GenericConfiguration> configs = new List<GenericConfiguration>();
            foreach (var config in configsNull)
            {
                if (config != null) configs.Add(config);
            }
            return configs;
        }

        private void AddComboBoxCell(string propName, string defaultValue, IEnumerable<string> values)
        {
            DataGridViewRow row = new DataGridViewRow();
            DataGridViewTextBoxCell cell_tb = new DataGridViewTextBoxCell();
            cell_tb.Value = propName;
            DataGridViewComboBoxCell cell_cb = new DataGridViewComboBoxCell();
            foreach (var v in values)
            {
                cell_cb.Items.Add(v.ToString());
            }
            cell_cb.Value = defaultValue == "" ? cell_cb.Items[0] : defaultValue;
            row.Cells.Add(cell_tb);
            row.Cells.Add(cell_cb);
            dgv_properties.Rows.Add(row);
        }

        private void cb_config_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_properties.Rows.Clear();
            GenericConfiguration config =  GetConfigs().Where(c => c.GetType().Name == cb_config.Text).FirstOrDefault();
            if (config.GetType() == typeof(SerialComConfiguration))
            {
                SerialComConfiguration serialConf = (SerialComConfiguration)config;
                AddComboBoxCell("ComPort", serialConf.ComPort, System.IO.Ports.SerialPort.GetPortNames());
                dgv_properties.Rows.Add("BaudRate", serialConf.BaudRate);
                AddComboBoxCell("Parity", serialConf.Parity.ToString(), Enum.GetValues(serialConf.Parity.GetType()).OfType<System.IO.Ports.Parity>().Select(a => a.ToString()));
                dgv_properties.Rows.Add("DataBits", serialConf.DataBits);
                AddComboBoxCell("StopBits", serialConf.StopBits.ToString(), Enum.GetValues(serialConf.StopBits.GetType()).OfType<System.IO.Ports.StopBits>().Select(a => a.ToString()));
                dgv_properties.Rows.Add("Timeout", serialConf.Timeout);
            }
        }

        private string GetPropValue(string prop)
        {
            DataGridViewRow row = dgv_properties.Rows.OfType<DataGridViewRow>().Where(r => prop == r.Cells[0].Value.ToString()).FirstOrDefault();
            return row.Cells[1].Value.ToString();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            try
            {
                GenericConfiguration config = GetConfigs().Where(c => c.GetType().Name == cb_config.Text).FirstOrDefault();
                if (config.GetType() == typeof(SerialComConfiguration))
                {
                    SerialComConfiguration serialConfig = new SerialComConfiguration()
                    {
                        ComPort = GetPropValue("ComPort"),
                        BaudRate = Convert.ToInt32(GetPropValue("BaudRate")),
                        Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity), GetPropValue("Parity")),
                        DataBits = Convert.ToInt32(GetPropValue("DataBits")),
                        StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), GetPropValue("StopBits")),
                        Timeout = Convert.ToInt32(GetPropValue("Timeout"))
                    };
                    easino = new SerialCom(serialConfig);
                    easino.Start();
                }

                string boardVersion = easino.GetBoardVersion();
                connected = true;
                lbl_board.Text = $"Board Version: {boardVersion}";
                MessageBox.Show($"Successfully connected to EasIno board");
            }
            catch (Exception ex)
            {
                connected = false;
                easino.Stop();
                MessageBox.Show($"Error while trying to connect: {ex.Message}", "Error");
            }
        }

        private void ConfigurationWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = connected ? DialogResult.OK : DialogResult.Cancel;
        }
    }
}
