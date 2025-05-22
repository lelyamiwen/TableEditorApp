using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TableEditorApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void newTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopicSelectionForm topicForm = new TopicSelectionForm();

            if (topicForm.ShowDialog() == DialogResult.OK)
            {
                string topic = topicForm.SelectedTopic;

                ChildForm child = new ChildForm();
                child.MdiParent = this;
                child.Text = topic == "Пустая таблица" ? "Новая таблица" : topic;
                child.Show();

                var dgv = child.Table;
                dgv.Columns.Clear();

                switch (topic)
                {
                    case "Ученики":
                        dgv.Columns.Add("ФИО", "ФИО");
                        dgv.Columns.Add("Возраст", "Возраст");
                        dgv.Columns.Add("Класс", "Класс");
                        dgv.Columns.Add("Оценка", "Оценка");
                        break;

                    case "Товары":
                        dgv.Columns.Add("Название", "Название");
                        dgv.Columns.Add("Категория", "Категория");
                        dgv.Columns.Add("Цена", "Цена");
                        dgv.Columns.Add("Количество", "Количество");
                        break;

                    case "Сотрудники":
                        dgv.Columns.Add("Имя", "Имя");
                        dgv.Columns.Add("Должность", "Должность");
                        dgv.Columns.Add("Отдел", "Отдел");
                        dgv.Columns.Add("Зарплата", "Зарплата");
                        break;

                    case "Пустая таблица":
                        // ничего не добавляем — пользователь сам добавит столбцы
                        break;
                }
            }
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChildForm active = this.ActiveMdiChild as ChildForm;
            if (active == null) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV files (*.csv)|*.csv";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName, false, new UTF8Encoding(true)))
                {
                    // Заголовки
                    for (int i = 0; i < active.Table.Columns.Count; i++)
                    {
                        sw.Write(active.Table.Columns[i].HeaderText);
                        if (i < active.Table.Columns.Count - 1) sw.Write(";");
                    }
                    sw.WriteLine();

                    // Данные
                    foreach (DataGridViewRow row in active.Table.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            for (int i = 0; i < active.Table.Columns.Count; i++)
                            {
                                sw.Write(row.Cells[i].Value?.ToString());
                                if (i < active.Table.Columns.Count - 1) sw.Write(";");
                            }
                            sw.WriteLine();
                        }
                    }
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV files (*.csv)|*.csv";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ChildForm child = new ChildForm();
                child.MdiParent = this;
                this.ActiveMdiChild?.Close(); // закрываем старую вкладку, если нужно
                child.Show();

                DataGridView dgv = child.Table;
                dgv.Columns.Clear();
                dgv.Rows.Clear();

                using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.UTF8))
                {
                    string headerLine = sr.ReadLine();
                    if (string.IsNullOrWhiteSpace(headerLine)) return;

                    // Выбираем подходящий разделитель
                    char delimiter = headerLine.Contains(";") ? ';' :
                                     headerLine.Contains("\t") ? '\t' :
                                     ',';

                    string[] headers = headerLine.Split(delimiter);
                    foreach (string header in headers)
                    {
                        dgv.Columns.Add(header, header);
                    }

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] cells = line.Split(delimiter);
                        dgv.Rows.Add(cells);
                    }
                }
            }
        }


        private void addColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChildForm active = this.ActiveMdiChild as ChildForm;
            if (active == null) return;

            string columnName = Microsoft.VisualBasic.Interaction.InputBox("Имя столбца:", "Добавить столбец", "НовыйСтолбец");
            if (!string.IsNullOrWhiteSpace(columnName))
            {
                active.Table.Columns.Add(columnName, columnName);
            }
        }

        private void removeColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChildForm active = this.ActiveMdiChild as ChildForm;
            if (active == null || active.Table.Columns.Count == 0) return;

            // Сбор всех названий столбцов
            var columnNames = active.Table.Columns.Cast<DataGridViewColumn>()
                                    .Select(c => c.HeaderText)
                                    .ToArray();

            // Создание диалога для выбора столбца
            string columnNameToRemove = Microsoft.VisualBasic.Interaction.InputBox(
                "Выберите столбец для удаления из списка:\n" + string.Join("\n", columnNames),
                "Удаление столбца");

            // Если название столбца введено
            if (!string.IsNullOrEmpty(columnNameToRemove))
            {
                // Поиск столбца с введённым именем
                var columnToRemove = active.Table.Columns.Cast<DataGridViewColumn>()
                                             .FirstOrDefault(c => c.HeaderText.Equals(columnNameToRemove, StringComparison.OrdinalIgnoreCase));

                // Если столбец найден, удаляем его
                if (columnToRemove != null)
                {
                    active.Table.Columns.Remove(columnToRemove);
                }
                else
                {
                    MessageBox.Show("Столбец с таким именем не найден.");
                }
            }
        }

    }
}
