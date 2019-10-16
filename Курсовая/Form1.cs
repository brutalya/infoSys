using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Курсовая
{
    public partial class Form1 : Form
    {
        public static int SupplyId;
        public static int SellingId;
        public static DataGridViewRow Row;
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog='C:\USERS\RUSLAN\DOCUMENTS\УЧЕБА\КУРСАЧ\КУРСОВАЯ РАБОТА(ЛОСКУТОВ)\КУРСОВАЯ РАБОТА(ЛОСКУТОВ)\BIN\DEBUG\MEDDB.MDF';Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        DataSet ds;
        bool selectEnd;
        int pageSize = 3; // размер страницы
        int pageNumber = 0; // текущая страница
        DataSet ds3;
        SqlDataAdapter adapter3;
        SqlCommandBuilder commandBuilder3;
        string sql3 = "SELECT * FROM Supply";
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            Data.DataVoid();

        }
        public void selectAll(string tableName, Dictionary<string, string> comboboxNames, Dictionary<string, string> comboboxHeads, string[] columnsHeads, DataGridView dataGridView1)
        {
            string sql = "SELECT * FROM " + tableName;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                // Работа с комбобоксами
                if (comboboxNames.Count > 0)
                {
                    for (int i = 0; i < comboboxNames.Count; i++)
                    {
                        string combosql = "SELECT * FROM " + comboboxNames.ElementAt(i).Value;
                        DataSet combods = new DataSet();
                        SqlDataAdapter comboadapter = new SqlDataAdapter(combosql, connection);
                        combods = new DataSet();
                        comboadapter.Fill(combods);
                        dataGridView1.Columns[0].Visible = false;
                        for (int j = 1; j < dataGridView1.Columns.Count; j++)
                        {
                            if (dataGridView1.Columns[j].HeaderText == comboboxNames.ElementAt(i).Key)
                                dataGridView1.Columns[j].Visible = false;
                            if (dataGridView1.Columns[j].HeaderText == comboboxHeads.ElementAt(i).Value)
                                dataGridView1.Columns[j].Visible = false;
                        }
                        // Combobox
                        DataGridViewComboBoxColumn comboboxColumn = new DataGridViewComboBoxColumn();
                        dataGridView1.Columns.Add(comboboxColumn);
                        comboboxColumn.DataSource = combods.Tables[0];
                        comboboxColumn.ValueMember = "Id";
                        comboboxColumn.DisplayMember = "name";
                        comboboxColumn.DataPropertyName = comboboxNames.ElementAt(i).Key;
                        comboboxColumn.HeaderText = comboboxHeads.ElementAt(i).Value;
                    }
                }
                //Пользовательские наименования
                int n = 0;
                dataGridView1.Columns[0].Visible = false;
                string d = dataGridView1.Columns[0].HeaderText;
                for (int j = 1; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Columns[j].Visible == true)
                    {
                        dataGridView1.Columns[j].HeaderText = columnsHeads[n];
                        n++;
                    }
                }
                //Проверка типов данных входных окон
                for (int j = 1; j < dataGridView1.Columns.Count; j++)
                {
                    if (dataGridView1.Columns[j].HeaderText == "Дата и время")
                    {
                        dataGridView1.Columns[j].DefaultCellStyle.Format = "F";
                    }
                }
                selectEnd = false;
            }
        }
        public void updateAll(string tableName)//ФУНКЦИЯ АПДЕЙТА
        {
            try
            {
                string sql = "SELECT * FROM " + tableName;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(ds);
                }
            }
            catch (Exception e) { MessageBox.Show("Ошибка" + e.Message); }
        }
        private void button1_Click(object sender, EventArgs e)//ДОБАВЛЕНИЕ
        {
            DataRow row = ds.Tables[0].NewRow(); // добавляем новую строку в DataTable
            ds.Tables[0].Rows.Add(row);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)//COMBOBOX SELECT
        {
            try
            {
                if (selectEnd == true)
                    selectAll(Data.tableNames[comboBox1.SelectedIndex], Data.comboboxNames[comboBox1.SelectedIndex], Data.comboboxHeads[comboBox1.SelectedIndex], Data.columnsHeads[comboBox1.SelectedIndex], dataGridView1);
                else selectEnd = true;
            }catch(Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)//UPDATE
        {
            try { 
            updateAll(Data.tableNames[comboBox1.SelectedIndex]);
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }
        private void button2_Click(object sender, EventArgs e)//УДАЛЕНИЕ
        {
            // удаляем выделенные строки из dataGridView1
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)//ВЫДЕЛЕНИЕ СТРОКИ ДЛЯ УДАЛЕНИЯ
        {
            if (e.ColumnIndex == -1)
            {
                dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
                dataGridView1.EndEdit();
            }
            else if (dataGridView1.EditMode != DataGridViewEditMode.EditOnEnter)
            {
                dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
                dataGridView1.BeginEdit(false);
            }
        }
        private void button4_Click(object sender, EventArgs e)//ПЕРЕЛИСТЫВАНИЕ
        {
            if (pageNumber == 0) return;
            pageNumber--;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);

                ds.Tables[0].Rows.Clear();

                adapter.Fill(ds);
            }
        }

        private void button5_Click(object sender, EventArgs e)//ПЕРЕЛИСТЫВАНИЕ
        {
            if (ds.Tables[0].Rows.Count < pageSize) return;

            pageNumber++;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                adapter = new SqlDataAdapter(GetSql(), connection);

                ds.Tables[0].Rows.Clear();

                adapter.Fill(ds);
            }
        }
        private string GetSql()//ПЕРЕЛИСТЫВАНИЕ
        {
            return "SELECT * FROM " + Data.tableNames[comboBox1.SelectedIndex] + " ORDER BY Id OFFSET ((" + pageNumber + ") * " + pageSize + ") " +
                "ROWS FETCH NEXT " + pageSize + "ROWS ONLY";
        }
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try { 
            if (tabControl1.SelectedIndex == 1)
            {
                string sql = "SELECT * FROM Storage";
                string sql2 = "SELECT * FROM Supp";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //Заполняем Supply
                    connection.Open();
                    adapter3 = new SqlDataAdapter(sql3, connection);
                    ds3 = new DataSet();
                    adapter3.Fill(ds3);

                    //ComboBox2
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataSet ds1 = new DataSet();
                    adapter.Fill(ds1);
                    comboBox2.DataSource = ds1.Tables[0];
                    comboBox2.DisplayMember = "name";
                    comboBox2.ValueMember = "Id";
                    //ComboBox3
                    SqlDataAdapter adapter2 = new SqlDataAdapter(sql2, connection);
                    DataSet ds2 = new DataSet();
                    adapter2.Fill(ds2);
                    comboBox3.DataSource = ds2.Tables[0];
                    comboBox3.DisplayMember = "name";
                    comboBox3.ValueMember = "Id";
                }
                selectAll(Data.tableNames[8], Data.comboboxNames[8], Data.comboboxHeads[8], Data.columnsHeads[8], dataGridView2);
            }//ПОКУПКА
            if (tabControl1.SelectedIndex == 2)//ПРОДАЖА
            {
                string sql = "SELECT * FROM Storage";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //ComboBox2
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataSet ds1 = new DataSet();
                    adapter.Fill(ds1);
                    comboBox5.DataSource = ds1.Tables[0];
                    comboBox5.DisplayMember = "name";
                    comboBox5.ValueMember = "Id";
                }
                selectAll(Data.tableNames[9], Data.comboboxNames[9], Data.comboboxHeads[9], Data.columnsHeads[9], dataGridView3);

            }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataRow row = ds.Tables[0].NewRow(); // добавляем новую строку в DataTable
                row[1] = comboBox3.SelectedValue;
                row[2] = comboBox2.SelectedValue;
                row[3] = dateTimePicker1.Text;
                ds.Tables[0].Rows.Add(row);
            }
            updateAll("Supply");
            SupplyId = getSupplyId();
            Form2 newfrm = new Form2();
            newfrm.Show();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка: " + ex.Message); }
        }
        public int getSupplyId()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Заполняем Supply
                connection.Open();
                adapter3 = new SqlDataAdapter(sql3, connection);
                ds3 = new DataSet();
                adapter3.Fill(ds3);
                return Convert.ToInt32(ds3.Tables[0].Rows[ds3.Tables[0].Rows.Count - 1][0]);
            }
        }

        private void tabPage2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView2.SelectedRows)
                {
                    Row = row;
                }
                SupplyId = Convert.ToInt32(Row.Cells[0].Value);
                Form2 newfrm = new Form2();
                newfrm.Show();
            }
            catch (Exception) { }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                dataGridView2.Rows.Remove(row);
            }
            updateAll("Supply");
            selectAll(Data.tableNames[8], Data.comboboxNames[8], Data.comboboxHeads[8], Data.columnsHeads[8], dataGridView2);
        }
        //ПРОДАЖА
        public int getSellingId()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Заполняем Supply
                sql3 = "SELECT * FROM Selling";
                connection.Open();
                adapter3 = new SqlDataAdapter(sql3, connection);
                ds3 = new DataSet();
                adapter3.Fill(ds3);
                return Convert.ToInt32(ds3.Tables[0].Rows[ds3.Tables[0].Rows.Count - 1][0]);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataRow row = ds.Tables[0].NewRow(); // добавляем новую строку в DataTable
                row[1] = comboBox5.SelectedValue;
                row[2] = dateTimePicker1.Value.Date;
                ds.Tables[0].Rows.Add(row);
            }
            updateAll("Selling");
            SellingId = getSellingId();
            Form3 newfrm = new Form3();
            newfrm.Show();
        }

        private void dataGridView3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView3.SelectedRows)
                {
                    Row = row;
                }
                SellingId = Convert.ToInt32(Row.Cells[0].Value);
                Form3 newfrm = new Form3();
                newfrm.Show();
            }
            catch (Exception) { }
        }


    }
}
