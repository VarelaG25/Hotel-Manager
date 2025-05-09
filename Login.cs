using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace AAVD
{
    public partial class Login : Form
    {
        public static int IdUsuarioActual { get; set; }
        public Login()
        {
            InitializeComponent();
        }
        public class LoginUsuario
        {
            public string Usuario { get; set; }
            public string Contrasena { get; set; }
        }
        private void Login_Usuario_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = false;
        }

        private void Login_Contrasena_TextChanged(object sender, EventArgs e)
        {
            label2.Visible = false;
        }

        private void Login_Continuar_Click(object sender, EventArgs e)
        {
            var enlace = new EnlaceDB();
            var tabla = new DataTable();
            LoginUsuario login = new LoginUsuario();
            login.Usuario = Login_Usuario.Text;
            login.Contrasena = Login_Contrasena.Text;
            tabla = enlace.consultarLogin(login);

            if (login.Usuario == "" || login.Contrasena == "")
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (tabla.Rows.Count == 0)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                int IdUsuario = Convert.ToInt32(tabla.Rows[0]["Id_Credenciales"]);
                Login.IdUsuarioActual = IdUsuario;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }

}
