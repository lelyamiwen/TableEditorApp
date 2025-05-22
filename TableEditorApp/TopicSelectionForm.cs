using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TableEditorApp
{
    public partial class TopicSelectionForm : Form
    {
        public string SelectedTopic { get; private set; }

        public TopicSelectionForm()
        {
            InitializeComponent();

            comboBoxTopic.Items.AddRange(new string[] {
            "Пустая таблица","Ученики", "Товары", "Сотрудники"
        });

            comboBoxTopic.SelectedIndex = 0;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SelectedTopic = comboBoxTopic.SelectedItem?.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

}
