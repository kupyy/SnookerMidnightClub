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
    public partial class FormRegistro : Form
    {

        //referencia al formulario de inicio de sesión 
        FormInicioSesion formInicioSesion = Application.OpenForms.OfType<FormInicioSesion>().FirstOrDefault();

        public FormRegistro()
        {
            InitializeComponent();
        }

        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=SnookerMidNight;Trusted_Connection=True;";


        //comprueba que no se intente registrar con un usuario ya existente
        public bool ComprobarUsuario(string usuario)
        {
            DataTable dt = new DataTable();
            bool existe = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Usuario FROM Usuarios WHERE Usuario LIKE @Usuario COLLATE Modern_Spanish_CS_AS";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Usuario", usuario);
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


        //registra al usuario en la base de datos
        public void RegistroUsuario(string usuario, string pass, string correo)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Usuarios (Usuario, Pass, Correo) VALUES (@Usuario, @Pass, @Correo)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Usuario", usuario);
                    command.Parameters.AddWithValue("@Pass", pass);
                    command.Parameters.AddWithValue("@Correo", correo);
                    command.ExecuteNonQuery();
                }
            }
            volver_Login();
        }


        //comprueba las credenciales introducidas y las manda al método registrar en caso de que sí
        private void boton_Registro(object sender, EventArgs e)
        {
            string usuario = textBoxNombre.Text;
            string correo = textBoxCorreo.Text;
            string pass = textBoxContrasenna.Text;
            string repite = textBoxConfirmar.Text;

            if (ComprobarUsuario(usuario) == true)
            {
                MessageBox.Show("Error el usuario introducido ya esta en uso");
            }
            else
            {
                if(pass != repite)
                {
                    MessageBox.Show("Las contraseñas introducidas no son iguales");
                }
                else
                {
                    RegistroUsuario(usuario, pass, correo);
                    MessageBox.Show("Usuario registrado correctamente");
                    textBoxNombre.Text = "";
                    textBoxCorreo.Text = "";
                    textBoxContrasenna.Text = "";
                    textBoxConfirmar.Text = "";
                }
            }
        }

        //alterna entre este formulario y el de inicio de sesión
        private void volver_Login()
        {
            this.Hide();
            formInicioSesion.Show();
        }

        //llama al método volverLogin al hacer click en el botón "Atrás"
        private void botonAtras(object sender, EventArgs e)
        {
            volver_Login();
        }


        //cierra la aplicación cuando se clica el botón de cerrar
        //sólo se usa aquí ya que al haber otro form oculto no se cierra completamente
        private void cerrarApp(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
