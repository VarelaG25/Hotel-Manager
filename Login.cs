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
        public static int tipoUsuario { get; set; }
        public static int baseDatos = 0;
        /*
            Si baseDatos es 1, entonces se usa SQL
            Si baseDatos es 2, entonces se usa CQL
         */
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
            LoginUsuario login = new LoginUsuario();
            if (!Login_SQL.Checked || !Login_SQL.Checked)
            {
                MessageBox.Show("Por favor, seleccione una base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            } 
            if (Login_SQL.Checked) Login.baseDatos = 1;
            if (Login_CQL.Checked) Login.baseDatos = 2;

            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
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
                int IdUsuario = Convert.ToInt32(tabla.Rows[0]["Id_Credenciales"]);
                int admin = Convert.ToInt32(tabla.Rows[0]["Id_Admin"]);
                Login.IdUsuarioActual = IdUsuario;
                Login.tipoUsuario = admin;
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
            else if (Login.baseDatos == 2)
            {

            }
        }
    }

}
