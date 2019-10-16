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
    public partial class Form3 : Form
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog='C:\USERS\RUSLAN\DOCUMENTS\УЧЕБА\КУРСАЧ\КУРСОВАЯ РАБОТА(ЛОСКУТОВ)\КУРСОВАЯ РАБОТА(ЛОСКУТОВ)\BIN\DEBUG\MEDDB.MDF';Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //string sql = "SELECT * FROM SellingDetails WHERE sellingId=" + Form1.SellingId;
        string sql = "SELECT * FROM SellingDetails LEFT OUTER JOIN Recipe LEFT OUTER JOIN RecipeDetails ON Recipe.recipeDetId=RecipeDetails.Id ON SellingDetails.recipeId=Recipe.Id WHERE sellingId=" + Form1.SellingId;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        DataSet ds;
        string sqlSelling = "SELECT * FROM Selling WHERE Id=" + Form1.SellingId;
        SqlDataAdapter adapterSelling;
        SqlCommandBuilder commandBuilderSelling;
        private DataSet dsSelling;
        public Form3()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Storage";
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
                    dateTimePicker1.Text = Form1.Row.Cells[3].Value.ToString();
                }
                catch (Exception) { }
            }
            SelectAll();
        }
        public void SelectAll()
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
                /*dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[2].Visible = false;*/
                dataGridView1.Columns[4].HeaderText = "Кол-во";
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

        private void button6_Click(object sender, EventArgs e)//UPDATE
        {
            //try
            //{
            string[] recipeNames = new string[dataGridView1.Rows.Count];
            int []isRecipe=new int[dataGridView1.Rows.Count];
            int []RDID=new int [dataGridView1.Rows.Count];
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    recipeNames[i] = dataGridView1.Rows[i].Cells[9].Value.ToString();
                    connection.Open();
                    string sqlInsert = "INSERT INTO RecipeDetails (name) VALUES ('" + recipeNames[i] + "')";
                    SqlCommand command = new SqlCommand(sqlInsert, connection);
                    int number = command.ExecuteNonQuery();

                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlInsert = "SELECT * FROM RecipeDetails WHERE name='"+recipeNames[i]+"'" ;
                    adapter = new SqlDataAdapter(sqlInsert, connection);
                    adapter.Fill(ds);
                    RDID[i] = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    MessageBox.Show(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    if (dataGridView1.Rows[i].Cells[6].ToString() == null) isRecipe[i] = 0; else isRecipe[i] = 1;
                    connection.Open();
                    string sqlInsert = "INSERT INTO Recipe (bool, recipeDetId) VALUES (" + isRecipe[i] + ", " + RDID[i] + ")";
                    SqlCommand command = new SqlCommand(sqlInsert, connection);
                    int number = command.ExecuteNonQuery();

                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlInsert = "SELECT * FROM Recipe WHERE recipeDetId=";
                    adapter = new SqlDataAdapter(sqlInsert, connection);
                    adapter.Fill(ds);
                }
            }
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sqlInsert = "INSERT INTO SellingDetails (prodId, sellingId,recipeId, num,) VALUES (" + dataGridView1.Rows[i].Cells[1].Value +", "+ dataGridView1.Rows[i].Cells[2].Value + ", "+dataGridView1.Rows[i].Cells[3].Value + ", "+dataGridView1.Rows[i].Cells[4].Value +")";
                        //adapter = new SqlDataAdapter(sql, connection);
                        adapter.InsertCommand = new SqlCommand(sqlInsert, connection);
                    SqlCommand command = new SqlCommand(sqlInsert, connection);
                    int number = command.ExecuteNonQuery();
                    //commandBuilder = new SqlCommandBuilder(adapter);
                    //adapter.Update(ds);
                }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    dsSelling = new DataSet();
                    adapterSelling = new SqlDataAdapter(sqlSelling, connection);
                    adapterSelling.Fill(dsSelling);
                    dsSelling.Tables[0].Rows[0][1] = comboBox2.SelectedValue;
                    dsSelling.Tables[0].Rows[0][2] = dateTimePicker1.Text;
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    adapterSelling = new SqlDataAdapter(sqlSelling, connection);
                    commandBuilderSelling = new SqlCommandBuilder(adapterSelling);
                    adapterSelling.Update(dsSelling);
                }
                this.Close();
            //}
            //catch (Exception ex) { MessageBox.Show("Ошибка" + ex.Message); }
        }

        private void button1_Click(object sender, EventArgs e)//INSERT
        {
            // добавляем новую строку в DataTable
            DataRow row = ds.Tables[0].NewRow();
            row[2] = Form1.SellingId;
            ds.Tables[0].Rows.Add(row);
        }
    }
}
