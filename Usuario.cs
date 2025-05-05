using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAVD
{
    public partial class Usuario : Form
    {
        // Variables para el usuario
        public int Id_Usuario { get; set; }
        public int Id_Admin { get; set; }
        public int NumeroNomina { get; set; }
        public string NombreUsuario { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string CorreoUsuario { get; set; }
        public string ContrasenaUsuario { get; set; }
        public DateTime FechaRegistroUsuario { get; set; }
        public DateTime FechaModificacionUsuario { get; set; }

        public Usuario()
        {
            InitializeComponent();
        }

        public Usuario (int numeroNomina)
        {
            
        }

        private void AbrirControlEnPanel(System.Windows.Forms.UserControl control)
        {
            MenuContenedor.Controls.Clear();
            control.Dock = DockStyle.Fill;
            MenuContenedor.Controls.Add(control);
            control.BringToFront();
        }

        private void Usuario_Load(object sender, EventArgs e)
        {
            AbrirControlEnPanel(new Menu());
            var NuevoForm = new Login();
            
            //Cargar tabla Usuarios SQL
            var enlace = new EnlaceDB();
            var tabla = new DataTable();
            tabla = enlace.get_Deptos();
            dataGridView2.DataSource = tabla;

            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;

        }
    }
}
