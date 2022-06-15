using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace conditional_delete_after_button_click
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private bool IsHandleInitialized = false;

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            // Do not block the message look while you do this.
            BeginInvoke((MethodInvoker)delegate 
            {
                onButtonRemove();
            });
        }
        void onButtonRemove()
        {
            // Perform Linq query to see what needs to be removed
            var removes = 
                DataSource
                .Where(record => string.IsNullOrWhiteSpace(record.ColumnB));

            // Cast to an array before iterating to
            // avoid "CollectionWasModified" exception.
            foreach (var record in removes.ToArray())
            {
                DataSource.Remove(record);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if(!(DesignMode || IsHandleInitialized))
            {
                IsHandleInitialized = true;
                InitializeDataGridView();
            }
        }

        BindingList<Record> DataSource = new BindingList<Record>();
        private void InitializeDataGridView()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = DataSource;
            // Add one or more records to auto-create columns.
            DataSource.Add(new Record { ColumnB = "Not empty or null"});
            DataSource.Add(new Record { ColumnB = String.Empty});
            DataSource.Add(new Record { ColumnB = null});

            // Column formatting
            dataGridView1.Columns[nameof(Record.ColumnA)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[nameof(Record.ColumnB)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
    class Record
    {
        public string ColumnA { get; set; } = "SomeValue";
        public string ColumnB { get; set; }
    }
}
