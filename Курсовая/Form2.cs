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
    public partial class Form2 : Form
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog='C:\USERS\RUSLAN\DOCUMENTS\УЧЕБА\КУРСАЧ\КУРСОВАЯ РАБОТА(ЛОСКУТОВ)\КУРСОВАЯ РАБОТА(ЛОСКУТОВ)\BIN\DEBUG\MEDDB.MDF';Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        string sql = "SELECT * FROM SupplyDetails WHERE supplyId=" + Form1.SupplyId;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        DataSet ds;
        string sqlSupply = "SELECT * FROM Supply WHERE Id="+Form1.SupplyId;
        SqlDataAdapter adapterSupply;
        SqlCommandBuilder commandBuilderSupply;
        private DataSet dsSupply;
        public Form2()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            //Data.DataVoid();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Storage";
            string sql2= "SELECT * FROM Supp";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //ComboBox2
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    DataSet ds1 = new DataSet();
                    adapter.Fill(ds1);
                    comboBox2.DataSource = ds1.Tables[0];
                    comboBox2.DisplayMember = "name";
                    comboBox2.ValueMember = "Id";
                    comboBox2.SelectedValue = Convert.ToInt32(Form1.Row.Cells[2].Value.ToString());
                    //ComboBox3
                    SqlDataAdapter adapter2 = new SqlDataAdapter(sql2, connection);
                    DataSet ds2 = new DataSet();
                    adapter2.Fill(ds2);
                    comboBox3.DataSource = ds2.Tables[0];
                    comboBox3.DisplayMember = "name";
                    comboBox3.ValueMember = "Id";
                    comboBox3.SelectedValue = Convert.ToInt32(Form1.Row.Cells[1].Value.ToString());
                    dateTimePicker1.Text = Form1.Row.Cells[3].Value.ToString();
                }
                catch (Exception) { }
            }
            SelectAll(Data.tableNames[10], Data.comboboxNames[10], Data.comboboxHeads[10], Data.columnsHeads[10]);
        }
        public void SelectAll(string tableName, Dictionary<string, string> comboboxNames, Dictionary<string, string> comboboxHeads, string[] columnsHeads)
        {
            //try
            //{
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sql, connection);
                    ds = new DataSet();
                    // Заполняем Dataset
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                    // НОВЫЙ КОД
                    string combosql = "SELECT * FROM Products";
                    DataSet combods = new DataSet();
                    SqlDataAdapter comboadapter = new SqlDataAdapter(combosql, connection);
                    combods = new DataSet();
                    comboadapter.Fill(combods);
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[3].HeaderText = "Кол-во";
                    // combobox
                    DataGridViewComboBoxColumn comboboxColumn = new DataGridViewComboBoxColumn();
                    dataGridView1.Columns.Add(comboboxColumn);
                    comboboxColumn.DataSource = combods.Tables[0];
                    comboboxColumn.ValueMember = "Id";
                    comboboxColumn.DisplayMember = "name";
                    comboboxColumn.DataPropertyName = "prodId";
                    comboboxColumn.HeaderText = "Товар";
                }
            //}
            //catch (Exception) { }
        }
        private void button1_Click(object sender, EventArgs e)//ДОБАВЛЕНИЕ
        {
            // добавляем новую строку в DataTable
            DataRow row = ds.Tables[0].NewRow();
            row[1] = Form1.SupplyId;
            ds.Tables[0].Rows.Add(row);
        }
        private void button2_Click(object sender, EventArgs e)//УДАЛЕНИЕ
        {
            // удаляем выделенные строки из dataGridView2
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.Remove(row);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Update(ds);
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    dsSupply = new DataSet();
                    adapterSupply = new SqlDataAdapter(sqlSupply, connection);
                    adapterSupply.Fill(dsSupply);
                    dsSupply.Tables[0].Rows[0][2] = comboBox2.SelectedValue;
                    dsSupply.Tables[0].Rows[0][1] = comboBox3.SelectedValue;
                    dsSupply.Tables[0].Rows[0][3] = dateTimePicker1.Text;
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapterSupply = new SqlDataAdapter(sqlSupply, connection);
                    commandBuilderSupply = new SqlCommandBuilder(adapterSupply);
                    adapterSupply.Update(dsSupply);
                }
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Ошибка" + ex.Message); }
        }
    }
}
