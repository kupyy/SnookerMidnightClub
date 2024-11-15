using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnookerMidnightClub
{
    public partial class FormInicioSesion : Form
    {
        FormRegistro formRegistro;
        FormInicio formInicio;
        FormAdmin formAdmin;

        public FormInicioSesion()
        {
            InitializeComponent();
        }



        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SnookerMidNight;Trusted_Connection=True;";

        private void boton_Login(object sender, EventArgs e)
        {
            string usuario = textBoxUsuario.Text;
            string pass = textBoxContrasenna.Text;

            if (CompruebaInicio(usuario, pass) == true)
            {
                if(usuario == "admin")
                {
                    formAdmin = new FormAdmin();
                    formAdmin.ShowDialog();
                }
                else
                {
                    formInicio = new FormInicio();
                    formInicio.ShowDialog();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("ERROR");
            }
        }



        public bool CompruebaInicio(string usuario, string pass)
        {
            DataTable dt = new DataTable();
            bool existe = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Usuario FROM Usuarios WHERE Usuario LIKE @Usuario AND Pass LIKE @Pass COLLATE Modern_Spanish_CS_AS";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Usuario", usuario);
                    command.Parameters.AddWithValue("@Pass", pass);
                    command.ExecuteNonQuery();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }

            if (dt.Rows.Count == 1)
            {
                existe = true;
            }

            return existe;
        }

        private void nuevoUsuario(object sender, EventArgs e)
        {
            if( formRegistro == null )
            {
                formRegistro = new FormRegistro();
                this.Hide();
                formRegistro.ShowDialog();
            }
            else
            {
                this.Hide();
                formRegistro.Show();
            }
        }
    }
}
