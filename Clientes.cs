using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;


namespace AAVD
{
    public partial class Clientes : Form
    {
        private void AbrirControlEnPanel(System.Windows.Forms.UserControl control)
        {
            MenuContenedor.Controls.Clear();
            control.Dock = DockStyle.Fill;
            MenuContenedor.Controls.Add(control);
            control.BringToFront();
        }
        public Clientes()
        {
            InitializeComponent();
        }
        public class ClienteDatos : Ubicacion
        {
            public int Id_Usuario { get; set; }
            public string Nombre { get; set; }
            public string PrimerApellido { get; set; }
            public string SegundoApellido { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string TelefonoCasa { get; set; }
            public string TelefonoCelular { get; set; }
            public string EstadoCivil { get; set; }
            public string RFC { get; set; }
            public string Correo { get; set; }
            public DateTime FechaRegistro { get; set; }
            public DateTime FechaModificacion { get; set; }

        }
        private void Clientes_Load_1(object sender, EventArgs e)
        {
            AbrirControlEnPanel(new Menu());
            var NuevoForm = new Login();
            CargarTablas();
            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;

        }
        private void AceptarBTN_Click(object sender, EventArgs e)
        {
            ClienteDatos cliente = new ClienteDatos();
            cliente.Pais = PaisCB.Text;
            cliente.Estado = EstadoCB.Text;
            cliente.Ciudad = CiudadCB.Text;
            cliente.CodigoPostal = CodigoPostalTXT.Text;
            cliente.TelefonoCasa = TelefonoCasaTXT.Text;
            cliente.TelefonoCelular = TelefonoCelularTXT.Text;
            if (string.IsNullOrEmpty(NombreTXT.Text) || string.IsNullOrEmpty(PrimerApellidoTXT.Text) || string.IsNullOrEmpty(SegundoApellidoTXT.Text) || string.IsNullOrEmpty(CorreoTXT.Text) || string.IsNullOrEmpty(FechaNacimientoDTP.Text) || string.IsNullOrEmpty(EstadoCivilCB.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if ((cliente.TelefonoCasa.Length != 10 && cliente.TelefonoCasa.Length > 0) ||
                 (cliente.TelefonoCelular.Length != 10 && cliente.TelefonoCelular.Length > 0))
            {
                MessageBox.Show("Cada número de teléfono debe tener exactamente 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!cliente.TelefonoCasa.All(char.IsDigit) || !cliente.TelefonoCelular.All(char.IsDigit))
            {
                MessageBox.Show("El número de teléfono debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CorreoTXT.Text.Length < 5)
            {
                MessageBox.Show("El correo electrónico debe tener al menos 5 caracteres.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!CorreoTXT.Text.Contains("@") || !CorreoTXT.Text.Contains("."))
            {
                MessageBox.Show("El correo electrónico no es válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            cliente.Nombre = NombreTXT.Text;
            cliente.PrimerApellido = PrimerApellidoTXT.Text;
            cliente.SegundoApellido = SegundoApellidoTXT.Text;
            cliente.EstadoCivil = EstadoCivilCB.Text;
            if(cliente.EstadoCivil == "Soltero") cliente.EstadoCivil = "S";
            else if (cliente.EstadoCivil == "Casado") cliente.EstadoCivil = "C";
            else if (cliente.EstadoCivil == "Divorciado") cliente.EstadoCivil = "D";
            else if (cliente.EstadoCivil == "Viudo") cliente.EstadoCivil = "V";
            cliente.RFC = RFCTXT.Text;
            cliente.Correo = CorreoTXT.Text;
            var enlace = new EnlaceDB();
            bool resultado = enlace.Insertar_Ubicacion(cliente, out int Id_Generado);
            var tabla = new DataTable();
            cliente.FechaRegistro = DateTime.Parse(FechaRegistroDTP.Text);
            cliente.FechaNacimiento = DateTime.Parse(FechaNacimientoDTP.Text);
            cliente.Id_Ubicacion = Id_Generado;
            Usuario.UsuarioDatos usuario = new Usuario.UsuarioDatos();
            usuario.Id_Usuario = Convert.ToInt32(tabla.Rows[0]["Id_Usuario"]);
            cliente.Id_Usuario = usuario.Id_Usuario;
            tabla = enlace.consultarUsuarioLogeado(Login.IdUsuarioActual);
            bool insertarCliente = enlace.Insertar_Cliente(cliente);
            if (!insertarCliente)
            {
                MessageBox.Show("Error al registrar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else MessageBox.Show("Cliente registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void CargarTablas()
        {
            var enlace = new EnlaceDB();
            var tablaUsuarioActual = enlace.ConsultarBarraUsuario();
            UsuarioActualTXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();
            var tabla = new DataTable();
            tabla = enlace.ConsultarUbicacion();

            var paises = tabla.AsEnumerable()
                .GroupBy(row => row.Field<string>("pais"))
                .Select(group => new
                {
                    pais = group.Key,
                    Id_Ubicacion = group.First().Field<int>("Id_Ubicacion")
                })
                .ToList();

            var estados = tabla.AsEnumerable()
                .GroupBy(row => row.Field<string>("estado"))
                .Select(group => new
                {
                    estado = group.Key,
                    Id_Ubicacion = group.First().Field<int>("Id_Ubicacion")
                })
                .ToList();

            var ciudades = tabla.AsEnumerable()
                .GroupBy(row => row.Field<string>("ciudad"))
                .Select(group => new
                {
                    ciudad = group.Key,
                    Id_Ubicacion = group.First().Field<int>("Id_Ubicacion")
                })
                .ToList();

            EstadoCivilCB.SelectedIndex = 0;

            PaisCB.DataSource = paises;
            PaisCB.DisplayMember = "pais";
            PaisCB.ValueMember = "Id_Ubicacion";

            EstadoCB.DataSource = estados;
            EstadoCB.DisplayMember = "estado";
            EstadoCB.ValueMember = "Id_Ubicacion";

            CiudadCB.DataSource = ciudades;
            CiudadCB.ValueMember = "Id_Ubicacion";
            CiudadCB.DisplayMember = "ciudad";

            var tabla2 = new DataTable();
            tabla2 = enlace.consultarCliente();

            TablaClientesDTG.DataSource = tabla2;
            TablaClientesDTG.Columns["nombreCliente"].HeaderText = "Nombre(s)";
            TablaClientesDTG.Columns["primerApellidoCliente"].HeaderText = "Primer Apellido";
            TablaClientesDTG.Columns["segundoApellidoCliente"].HeaderText = "Segundo Apellido";
            TablaClientesDTG.Columns["fechaNacimientoCliente"].HeaderText = "Fecha de nacimiento";
            TablaClientesDTG.Columns["EstadoCivil"].HeaderText = "Estado Civil";
            TablaClientesDTG.Columns["rfcCliente"].HeaderText = "RFC";
            TablaClientesDTG.Columns["fechaRegistroCliente"].HeaderText = "Fecha de registro";

            TablaClientesDTG.Columns["rfcCliente"].DisplayIndex = 0;
            TablaClientesDTG.Columns["nombreCliente"].DisplayIndex = 1;
            TablaClientesDTG.Columns["primerApellidoCliente"].DisplayIndex = 2;
            TablaClientesDTG.Columns["segundoApellidoCliente"].DisplayIndex = 3;
            TablaClientesDTG.Columns["EstadoCivil"].DisplayIndex = 4;
            TablaClientesDTG.Columns["fechaNacimientoCliente"].DisplayIndex = 5;
            TablaClientesDTG.Columns["fechaRegistroCliente"].DisplayIndex = 6;

        }
    }
    public class Ubicacion
    {
        public int Id_Ubicacion { get; set; }
        public string Ciudad { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public string Domicilio { get; set; }
        public string CodigoPostal { get; set; }
    }
}
