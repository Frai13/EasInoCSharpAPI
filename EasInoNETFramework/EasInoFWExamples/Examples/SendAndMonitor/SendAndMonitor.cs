using EasInoAPI;
using EasInoExamples;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EasInoExamples.Common;

namespace EasInoWFExamples.Examples
{
    internal partial class SendAndMonitor : Form, Common.IExample
    {
        public IEnumerable<string> Command => new List<string>() { "4", "SendAndMonitor" };

        public ActionArgs Run => (IEnumerable<string> ArgsProvided) =>
        {
            Console.WriteLine($"Starting SendAndMonitor example");
            Application.Run(new SendAndMonitor());
        };

        EasIno easino;

        public SendAndMonitor()
        {
            InitializeComponent();

            dgv_send.Columns.Add("Operation", "Operation");
            formatColumn(dgv_send.Columns["Operation"]);
            dgv_send.Rows.Add();

            dgv_received.Columns.Add("Timestamp", "Timestamp");
            formatColumn(dgv_received.Columns["Timestamp"]);
            dgv_received.Columns.Add("n", "n");
            formatColumn(dgv_received.Columns["n"]);
            dgv_received.Columns.Add("Operation", "Operation");
            formatColumn(dgv_received.Columns["Operation"]);
        }

        private void SendAndMonitor_Load(object sender, EventArgs e)
        {
            nud_args.Value = 3;

            if (!OpenEasIno())
            {
                this.Close();
            }
        }

        private bool OpenEasIno()
        {
            ConfigurationWindow configWdw = new ConfigurationWindow();
            DialogResult result = configWdw.ShowDialog();
            if (result != DialogResult.OK) return false;
            easino = configWdw.easino;
            easino.DataReceived += Easino_DataReceived;
            return true;
        }

        private void formatColumn(DataGridViewColumn col)
        {
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void nud_args_ValueChanged(object sender, EventArgs e)
        {
            for (int i = dgv_send.ColumnCount - 1; i > nud_args.Value; i--)
            {
                dgv_send.Columns.RemoveAt(i);
            }
            for (int i = dgv_send.ColumnCount; i < nud_args.Value + 1; i++)
            {
                dgv_send.Columns.Add($"Arg{i}", $"Arg{i}");
                formatColumn(dgv_send.Columns[$"Arg{i}"]);
            }
        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            OpenEasIno();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            var op_value = dgv_send.Rows[0].Cells["Operation"].Value;
            string operation = op_value == null ? "" : op_value.ToString();
            List<string> args = new List<string>();
            for (int i = 1; i < dgv_send.ColumnCount; i++)
            {
                var arg_value = dgv_send.Rows[0].Cells[$"Arg{i}"].Value;
                string arg = arg_value == null ? "" : arg_value.ToString();
                args.Add(arg);
            }

            try
            {

                easino.Send(new DataCom()
                {
                    Operation = operation,
                    Args = args
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while sending to EasIno: {ex.Message}", "Error");
            }
        }

        private void Easino_DataReceived(DataReceivedEventArgs args)
        {
            dgv_received.Invoke(new Action(() =>
            {
                for (int i = dgv_received.ColumnCount - 3; i < args.Data.Args.Count() + 1; i++)
                {
                    dgv_received.Columns.Add($"Arg{i}", $"Arg{i}");
                    formatColumn(dgv_received.Columns[$"Arg{i}"]);
                }

                List<string> cols = new List<string>();
                cols.Add(DateTime.Now.ToString("HH:mm:ss.fff"));
                var last_n = dgv_received.RowCount > 0 ? Convert.ToInt32(dgv_received.Rows[0].Cells["n"].Value) : 0;
                cols.Add((1 + last_n).ToString());
                cols.Add(args.Data.Operation);
                cols.AddRange(args.Data.Args);
                dgv_received.Rows.Insert(0, cols.ToArray());
            }));
        }
    }
}
