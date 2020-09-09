using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace wftest
{
    public partial class Form1 : Form
    {

        private SQLiteConnection DB;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DB = new SQLiteConnection("Data source = db.db");
            DB.Open();
            dataGridView1.Rows.Add();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DB.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                SQLiteCommand CMD = DB.CreateCommand();
                CMD.CommandText = "insert into Пользователи(Пользователь) values('"+textBox1.Text.ToUpper()+"')";
                CMD.ExecuteNonQuery();
                textBox1.Clear();
                button2_Click(sender, e);
            }
            else MessageBox.Show("Заполните строку с именем пользователя", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = true;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            int i = 0;
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select * from Пользователи";
            SQLiteDataReader SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                while(SQL.Read() )
                {
                    listBox1.Items.Insert(i,SQL["Пользователь"]);
                    i++;
                    //listBox1.Text += "Пользователь:" + SQL["Пользователь"] + "\r\n";
                }
            }
            else MessageBox.Show("Пользователей нет", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                int count = 0;
                SQLiteCommand CMD = DB.CreateCommand();
                CMD.CommandText = "select count(*) from Проекты where Пользователь = @Пользователь";
                CMD.Parameters.Add("@Пользователь", System.Data.DbType.String).Value = listBox1.SelectedItem;
                count = Convert.ToInt32(CMD.ExecuteScalar());
                if (count >= 1)
                {
                    MessageBox.Show("У пользователя есть проекты, удаление невозможно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    CMD.CommandText = "delete from Пользователи where Пользователь = @fio";
                    CMD.Parameters.Add("@fio", System.Data.DbType.String).Value = listBox1.SelectedItem.ToString();
                    CMD.ExecuteNonQuery();
                    button2_Click(sender, e);
                    MessageBox.Show("Пользователь удален", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя, которого нужно удалить", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите пользователя", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            if (textBox2.Text != "")
            {
                SQLiteCommand CMD = DB.CreateCommand();
                CMD.CommandText = "insert into Проекты(Проект, Пользователь) values( @Проект, @Пользователь)";
                CMD.Parameters.Add("@Проект", System.Data.DbType.String).Value = textBox2.Text.ToUpper();
                CMD.Parameters.Add("@Пользователь", System.Data.DbType.String).Value = listBox1.SelectedItem;
                CMD.ExecuteNonQuery();
                textBox2.Clear();
                button10_Click(sender, e);
            }
            else
            {
                MessageBox.Show("Введите название проекта", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button7.Enabled = false;
            listBox2.Items.Clear();
            int j = 0;
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select * from Проекты";
            SQLiteDataReader SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    listBox2.Items.Insert(j, SQL["Проект"]);
                    j++;
                    //listBox1.Text += "Пользователь:" + SQL["Пользователь"] + "\r\n";
                }
            }
            else MessageBox.Show("Проектов нет", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

        }

        private void button10_Click(object sender, EventArgs e)
        {
            button7.Enabled = true;
            listBox1.Enabled = false;
            listBox2.Items.Clear();
            int j = 0;
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select * from Проекты where Пользователь = @Выбрпольз";
            CMD.Parameters.Add("@Выбрпольз", System.Data.DbType.String).Value = listBox1.SelectedItem;
            SQLiteDataReader SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    listBox2.Items.Insert(j, SQL["Проект"]);
                    j++;
                }
            }
            else
            { MessageBox.Show("Проектов нет", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly); listBox1.Enabled = true; }
        }

        private void button7_Click(object sender, EventArgs e)
        {
                if (listBox1.SelectedIndex != -1 && listBox2.SelectedIndex !=-1)
            {
                if (dataGridView1[0, 0].Value != null && dataGridView1[1, 0].Value != null && dataGridView1[2, 0].Value != null)
                {
                    SQLiteCommand CMD = DB.CreateCommand();
                    CMD.CommandText = "insert into Задачи(Пользователь, Проект, Тема, Тип, Приоритет, Описание) values(@Пользователь, @Проект, @Тема, @Тип, @Приоритет, @Описание)";
                    CMD.Parameters.Add("@Пользователь", System.Data.DbType.String).Value = listBox1.SelectedItem.ToString();
                    CMD.Parameters.Add("@Проект", System.Data.DbType.String).Value = listBox2.SelectedItem.ToString();
                    CMD.Parameters.Add("@Тема", System.Data.DbType.String).Value = dataGridView1[0, 0].Value.ToString();
                    CMD.Parameters.Add("@Тип", System.Data.DbType.String).Value = dataGridView1[1, 0].Value.ToString();
                    CMD.Parameters.Add("@Приоритет", System.Data.DbType.String).Value = dataGridView1[2, 0].Value.ToString();
                    CMD.Parameters.Add("@Описание", System.Data.DbType.String).Value = dataGridView1[3, 0].Value.ToString();
                    CMD.ExecuteNonQuery();
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add();
                }
                else MessageBox.Show("Пустое значение недопустимо", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                button11_Click(sender, e); return;
            }
            else MessageBox.Show("Выберите проект", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            button11_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex==-1)
            {
                MessageBox.Show("Выберите пользователя", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                button11_Click(sender, e);
                return;
            }
            dataGridView2.Rows.Clear();
            int j = 0;
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select * from Задачи where Пользователь = @Пользователь";
            CMD.Parameters.Add("@Пользователь", System.Data.DbType.String).Value = listBox1.SelectedItem;
            SQLiteDataReader SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    dataGridView2.Rows.Add();
                    dataGridView2[0, j].Value = SQL["Пользователь"];
                    dataGridView2[1, j].Value = SQL["Проект"];
                    dataGridView2[2, j].Value = SQL["Тема"];
                    dataGridView2[3, j].Value = SQL["Тип"];
                    dataGridView2[4, j].Value = SQL["Приоритет"];
                    dataGridView2[5, j].Value = SQL["Описание"];
                    j++;
                }
            }
            else MessageBox.Show("Задач нет", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            button11_Click(sender, e);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            int j = 0;
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select * from Задачи where Проект = @Проект";
            CMD.Parameters.Add("@Проект", System.Data.DbType.String).Value = listBox2.SelectedItem;
            SQLiteDataReader SQL = CMD.ExecuteReader();
            if (SQL.HasRows)
            {
                while (SQL.Read())
                {
                    dataGridView2.Rows.Add();
                    dataGridView2[0, j].Value = SQL["Пользователь"];
                    dataGridView2[1, j].Value = SQL["Проект"];
                    dataGridView2[2, j].Value = SQL["Тема"];
                    dataGridView2[3, j].Value = SQL["Тип"];
                    dataGridView2[4, j].Value = SQL["Приоритет"];
                    dataGridView2[5, j].Value = SQL["Описание"];
                    j++;
                }
            }
            else MessageBox.Show("Задач нет", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly); return;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите проект", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            int count = 0;
            SQLiteCommand CMD = DB.CreateCommand();
            CMD.CommandText = "select count(*) from Задачи where Проект = @Проект";
            CMD.Parameters.Add("@Проект", System.Data.DbType.String).Value = listBox2.SelectedItem;
            count = Convert.ToInt32(CMD.ExecuteScalar());
            if (count >= 1)
            {
                MessageBox.Show("У проекта есть задачи, удаление невозможно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                CMD.CommandText = "delete from Проекты where Проект = @Проект";
                CMD.Parameters.Add("@Проект", System.Data.DbType.String).Value = listBox2.SelectedItem;
                CMD.ExecuteNonQuery();
                button2_Click(sender, e);
                MessageBox.Show("Проект удален", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView2.RowCount >= 1)
            {
                int selRowNum = dataGridView2.SelectedCells[0].RowIndex;
                SQLiteCommand CMD = DB.CreateCommand();
                CMD.CommandText = "delete from Задачи where Тема=@Тема and Описание=@Описание";
                CMD.Parameters.Add("@Тема", System.Data.DbType.String).Value = dataGridView2[2, selRowNum].Value.ToString();
                CMD.Parameters.Add("@Описание", System.Data.DbType.String).Value = dataGridView2[5, selRowNum].Value.ToString();
                CMD.ExecuteNonQuery();
                MessageBox.Show("Задача удалена", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                button3_Click(sender, e);
            }
        }
    }
}
