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
    public partial class Reservaciones : Form
    {
        public Reservaciones()
        {
            InitializeComponent();
        }
        private void AbrirControlEnPanel(System.Windows.Forms.UserControl control)
        {
            MenuContenedor.Controls.Clear();
            control.Dock = DockStyle.Fill;
            MenuContenedor.Controls.Add(control);
            control.BringToFront();
        }

        private void Reservaciones_Load(object sender, EventArgs e)
        {
            AbrirControlEnPanel(new Menu());
            var NuevoForm = new Login();
            CargarTablas();
            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;
        }
        public void CargarTablas()
        {
            var enlace = new EnlaceDB();
            var tablaUsuarioActual = enlace.ConsultarBarraUsuario();
            UsuarioActualTXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();

            var tabla = new DataTable();
            tabla = enlace.consultarCliente();
            tabla.Columns.Add("Apellidos", typeof(string), "primerApellidoCliente + ' ' + segundoApellidoCliente");

            ApellidosTXT.DisplayMember = "Apellidos";
            ApellidosTXT.ValueMember = "Id_Cliente";
            ApellidosTXT.DataSource = tabla;

            RFCCB.DisplayMember = "rfcCliente";
            RFCCB.ValueMember = "Id_Cliente";
            RFCCB.DataSource = tabla;

            CorreoCB.DisplayMember = "correoCliente";
            CorreoCB.ValueMember = "Id_Cliente";
            CorreoCB.DataSource = tabla;

            TablaClientesDTG.DataSource = tabla;
            TablaClientesDTG.Columns["nombreCliente"].HeaderText = "Nombre(s)";
            TablaClientesDTG.Columns["primerApellidoCliente"].HeaderText = "Primer Apellido";
            TablaClientesDTG.Columns["segundoApellidoCliente"].HeaderText = "Segundo Apellido";
            TablaClientesDTG.Columns["fechaNacimientoCliente"].HeaderText = "Fecha de nacimiento";
            TablaClientesDTG.Columns["EstadoCivil"].HeaderText = "Estado Civil";
            TablaClientesDTG.Columns["rfcCliente"].HeaderText = "RFC";
            TablaClientesDTG.Columns["fechaRegistroCliente"].HeaderText = "Fecha de registro";
            TablaClientesDTG.Columns["correoCliente"].HeaderText = "Correo electrónico";

            TablaClientesDTG.Columns["rfcCliente"].DisplayIndex = 0;
            TablaClientesDTG.Columns["nombreCliente"].DisplayIndex = 1;
            TablaClientesDTG.Columns["primerApellidoCliente"].DisplayIndex = 2;
            TablaClientesDTG.Columns["segundoApellidoCliente"].DisplayIndex = 3;
            TablaClientesDTG.Columns["EstadoCivil"].DisplayIndex = 4;
            TablaClientesDTG.Columns["fechaNacimientoCliente"].DisplayIndex = 5;
            TablaClientesDTG.Columns["correoCliente"].DisplayIndex = 6;
            TablaClientesDTG.Columns["fechaRegistroCliente"].DisplayIndex = 7;

            TablaClientesDTG.Columns["Apellidos"].Visible = false;
            TablaClientesDTG.Columns["Id_Cliente"].Visible = false;
            TablaClientesDTG.Columns["fechaModificacionCliente"].Visible = false;
        }
    }
}
