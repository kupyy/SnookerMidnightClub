using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebSockets;
using System.Windows.Forms;

namespace SnookerMidnightClub
{
    public partial class FormAdmin : Form
    {
        public FormAdmin()
        {
            InitializeComponent();
        }

        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SnookerMidNight;Trusted_Connection=True;";

        private void mostrarUsuarios(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "Select * from usuarios";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            guna2DataGridView1.DataSource = dt;
        }
    }
}
