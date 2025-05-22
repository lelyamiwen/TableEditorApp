using System;
using System.Windows.Forms;

namespace TableEditorApp
{
    public partial class ChildForm : Form
    {
        public ChildForm()
        {
            InitializeComponent();
        }

        public DataGridView Table => dataGridView1;

        // Обработчик для правого клика на заголовке столбца
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Проверяем, был ли клик на заголовке столбца (e.RowIndex == -1 означает заголовок)
            if (e.RowIndex == -1)
            {
                // Показать контекстное меню на позиции курсора
                columnContextMenu.Show(Cursor.Position);
            }
        }

        // Обработчик для изменения названия столбца
        private void ChangeColumnNameMenuItem_Click(object sender, EventArgs e)
        {
            // Получаем индекс столбца, на котором был клик
            var columnIndex = dataGridView1.HitTest(dataGridView1.PointToClient(Cursor.Position).X, dataGridView1.PointToClient(Cursor.Position).Y).ColumnIndex;
            if (columnIndex >= 0)
            {
                // Получаем старое имя столбца
                string oldColumnName = dataGridView1.Columns[columnIndex].HeaderText;

                // Запрашиваем новое имя у пользователя
                string newColumnName = Microsoft.VisualBasic.Interaction.InputBox("Введите новое название столбца:", "Изменить название столбца", oldColumnName);

                // Если имя не пустое, обновляем заголовок столбца
                if (!string.IsNullOrWhiteSpace(newColumnName))
                {
                    dataGridView1.Columns[columnIndex].HeaderText = newColumnName;
                }
            }
        }
    }
}

