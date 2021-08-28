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

namespace MovieListC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Load();
        }

        SqlConnection con = new SqlConnection("Data Source= DHRUV; Initial Catalog= moviec#; Persist Security Info=True; User ID= dhruv; Password= dhruv123");
        SqlCommand cmd;
        SqlDataReader read;
        SqlDataAdapter drr;
        string id;
        bool Mode = true;
        string sql;


        public void Load()
        {
            try
            {
                sql = "select * from movietable";
                cmd = new SqlCommand(sql, con);
                con.Open();
                read = cmd.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3], read[4]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getID(String id)
        {
            sql = "select * from movietable where id = '" + id + "' ";
            cmd = new SqlCommand(sql, con);
            con.Open();
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                textName.Text = read[1].ToString();
                textLang.Text = read[2].ToString();
                textYear.Text = read[3].ToString();
                textIMDB.Text = read[4].ToString();
            }
            con.Close();
        }



        //if mode= true -> add record , else -> update record
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textName.Text;
            string language = textLang.Text;
            string year = textYear.Text;
            string imdb = textIMDB.Text;

            if (Mode == true)
            {
                //sql = "insert into movietable(name,language,year,imdb) values(@name,@language,@year,@imdb)";
                con.Close();
                sql = "insert into movietable(name,language,year,imdb) values(@name,@language,@year,@imdb)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@language", language);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@imdb", imdb);
                MessageBox.Show("Movie Added !!");
                cmd.ExecuteNonQuery();

                textName.Clear();
                textLang.Clear();
                textYear.Clear();
                textIMDB.Clear();
                textName.Focus();
            }
            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "update movietable set name =@name, language =@language, year =@year, imdb =@imdb where id = @id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@language", language);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@imdb", imdb);
                cmd.Parameters.AddWithValue("@id", id);

                MessageBox.Show("Movie Updated !!");
                cmd.ExecuteNonQuery();

                textName.Clear();
                textLang.Clear();
                textYear.Clear();
                textIMDB.Clear();
                textName.Focus();
                button1.Text = "SAVE";
                Mode = true;

            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                button1.Text = "EDIT";
            }
            else if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from movietable where id =@id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Movie Data Deleted !!");
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textName.Clear();
            textLang.Clear();
            textYear.Clear();
            textIMDB.Clear();
            textName.Focus();
            button1.Text = "SAVE";
            Mode = true;
        }
    }
}
