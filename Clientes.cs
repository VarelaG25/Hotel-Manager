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
            public int Id_Cliente { get; set; }
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
            var enlace = new EnlaceDB();
            ClienteDatos cliente = new ClienteDatos();
            cliente.Pais = PaisCB.Text;
            cliente.Estado = EstadoCB.Text;
            cliente.Ciudad = CiudadCB.Text;
            cliente.CodigoPostal = CodigoPostalTXT.Text;
            cliente.TelefonoCasa = TelefonoCasaTXT.Text;
            cliente.TelefonoCelular = TelefonoCelularTXT.Text;

            if (string.IsNullOrEmpty(NombreTXT.Text) || string.IsNullOrEmpty(PrimerApellidoTXT.Text) 
                || string.IsNullOrEmpty(SegundoApellidoTXT.Text) || string.IsNullOrEmpty(CorreoTXT.Text) 
                || string.IsNullOrEmpty(FechaNacimientoDTP.Text) || string.IsNullOrEmpty(EstadoCivilCB.Text)
                || string.IsNullOrEmpty(TelefonoCasaTXT.Text) || string.IsNullOrEmpty(TelefonoCelularTXT.Text))
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

            cliente.RFC = RFCTXT.Text;
            cliente.Correo = CorreoTXT.Text;
            cliente.Nombre = NombreTXT.Text;
            cliente.PrimerApellido = PrimerApellidoTXT.Text;
            cliente.SegundoApellido = SegundoApellidoTXT.Text;
            cliente.EstadoCivil = EstadoCivilCB.Text;

            if(cliente.EstadoCivil == "Soltero") cliente.EstadoCivil = "S";
            else if (cliente.EstadoCivil == "Casado") cliente.EstadoCivil = "C";
            else if (cliente.EstadoCivil == "Divorciado") cliente.EstadoCivil = "D";
            else if (cliente.EstadoCivil == "Viudo") cliente.EstadoCivil = "V";


            var tabla = new DataTable();
            tabla = enlace.consultarUsuarioLogeado(Login.IdUsuarioActual);
            cliente.Id_Usuario = Convert.ToInt32(tabla.Rows[0]["Id_Usuario"]);
            cliente.FechaRegistro = DateTime.Parse(FechaRegistroDTP.Text);
            cliente.FechaNacimiento = DateTime.Parse(FechaNacimientoDTP.Text);
            cliente.Domicilio = null;

            var tablaUbicacion = new DataTable();
            tablaUbicacion = enlace.ValidarUbicacionUnica(cliente.Pais, cliente.Estado, cliente.Ciudad);
            if (tablaUbicacion.Rows.Count < 1)
            {
                cliente.Id_Ubicacion = 0;
            }
            else
            {
                cliente.Id_Ubicacion = Convert.ToInt32(tablaUbicacion.Rows[0]["Id_Ubicacion"]);
            }

            bool insertarCliente = enlace.Insertar_Cliente(cliente);
            if (!insertarCliente)
            {
                MessageBox.Show("Error al registrar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else MessageBox.Show("Cliente registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarTablas();
        }
        private void ClienteCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccion = Convert.ToInt32(ClienteCB.SelectedValue);
            if (seleccion >= 0)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarClienteEspecifico(seleccion);
                if (tabla.Rows.Count > 0)
                {
                    NombreSeleccionado.Text = tabla.Rows[0]["nombreCliente"].ToString();
                    PrimerApellidoSeleccionado.Text = tabla.Rows[0]["primerApellidoCliente"].ToString();
                    SegundoApellidoSeleccionado.Text = tabla.Rows[0]["segundoApellidoCliente"].ToString();
                    TelefonoCasaSeleccionado.Text = tabla.Rows[0]["telefonoCasa"].ToString();
                    TelefonoCelularSeleccionado.Text = tabla.Rows[0]["telefonoCelular"].ToString();
                    EstadoCivilSeleccionado.Text = tabla.Rows[0]["EstadoCivil"].ToString();
                    RFCSeleccionado.Text = tabla.Rows[0]["rfcCliente"].ToString();
                    CorreoSeleccionado.Text = tabla.Rows[0]["correoCliente"].ToString();
                    FechaNacimientoSeleccionada.Value = DateTime.Parse(tabla.Rows[0]["fechaNacimientoCliente"].ToString());
                    PaisSeleccionado.Text = tabla.Rows[0]["pais"].ToString();
                    EstadoSeleccionado.Text = tabla.Rows[0]["estado"].ToString();
                    CiudadSeleccionado.Text = tabla.Rows[0]["ciudad"].ToString();
                    CodigoPostalSeleccionado.Text = tabla.Rows[0]["codigoPostal"].ToString();
                }
            }
        }
        private void ModificarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro de que desea modificar el cliente?", "Modificar Cliente", buttons, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var enlace = new EnlaceDB();
                ClienteDatos cliente = new ClienteDatos();
                cliente.Id_Cliente = Convert.ToInt32(ClienteCB.SelectedValue);
                cliente.Nombre = NombreSeleccionado.Text;
                cliente.PrimerApellido = PrimerApellidoSeleccionado.Text;
                cliente.SegundoApellido = SegundoApellidoSeleccionado.Text;
                cliente.TelefonoCasa = TelefonoCasaSeleccionado.Text;
                cliente.TelefonoCelular = TelefonoCelularSeleccionado.Text;
                cliente.EstadoCivil = EstadoCivilSeleccionado.Text;
                cliente.RFC = RFCSeleccionado.Text;
                cliente.Correo = CorreoSeleccionado.Text;
                cliente.FechaNacimiento = FechaNacimientoSeleccionada.Value;
                cliente.FechaModificacion = FechaModificacion.Value;
                var tabla = new DataTable();
                tabla = enlace.consultarIdUsuario(Login.IdUsuarioActual);
                cliente.Id_Usuario = Convert.ToInt32(tabla.Rows[0]["Id_Usuario"]);
                if (string.IsNullOrEmpty(cliente.Nombre) || string.IsNullOrEmpty(cliente.PrimerApellido) || string.IsNullOrEmpty(cliente.SegundoApellido) ||
                    string.IsNullOrEmpty(cliente.TelefonoCelular) || string.IsNullOrEmpty(cliente.TelefonoCasa) || string.IsNullOrEmpty(cliente.EstadoCivil) ||
                    string.IsNullOrEmpty(cliente.RFC) || string.IsNullOrEmpty(cliente.Correo))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!cliente.TelefonoCasa.All(char.IsDigit) || !cliente.TelefonoCelular.All(char.IsDigit))
                {
                    MessageBox.Show("El número de teléfono debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if ((cliente.TelefonoCasa.Length != 10 && cliente.TelefonoCasa.Length > 0) ||
                (cliente.TelefonoCelular.Length != 10 && cliente.TelefonoCelular.Length > 0))
                {
                    MessageBox.Show("Cada número de teléfono debe tener exactamente 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(cliente.RFC.Length < 0 || cliente.RFC.Length > 20)
                {
                    MessageBox.Show("El RFC debe contener almenos 20 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                bool resultado = enlace.Actualizar_Cliente(cliente);
                if(!resultado)
                {
                    MessageBox.Show("Error al modificar al cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CargarTablas();
            }
        }
        private void BorrarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro de que desea borrar al cliente?", "Borrar Cliente", buttons, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var enlace = new EnlaceDB();
                ClienteDatos cliente = new ClienteDatos();
                cliente.Id_Cliente = Convert.ToInt32(ClienteCB.SelectedValue);
                bool resultado = enlace.Borrar_Cliente(cliente.Id_Cliente);
                if (!resultado)
                {
                    MessageBox.Show("Error al borrar al cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CargarTablas();
                ClienteCB.SelectedIndex = 0;
            }
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
            tabla2.Columns.Add("nombreCompleto", typeof(string), "nombreCliente + ' ' + primerApellidoCliente + ' ' + segundoApellidoCliente");

            ClienteCB.DisplayMember = "nombreCompleto";
            ClienteCB.ValueMember = "Id_Cliente";
            ClienteCB.DataSource = tabla2;

            TablaClientesDTG.DataSource = tabla2;
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

            TablaClientesDTG.Columns["nombreCompleto"].Visible = false;
            TablaClientesDTG.Columns["Id_Cliente"].Visible = false;
            TablaClientesDTG.Columns["fechaModificacionCliente"].Visible = false;

            TablaClientesRegistradosDTG.DataSource = tabla2;
            TablaClientesRegistradosDTG.Columns["nombreCliente"].HeaderText = "Nombre(s)";
            TablaClientesRegistradosDTG.Columns["primerApellidoCliente"].HeaderText = "Primer Apellido";
            TablaClientesRegistradosDTG.Columns["segundoApellidoCliente"].HeaderText = "Segundo Apellido";
            TablaClientesRegistradosDTG.Columns["fechaNacimientoCliente"].HeaderText = "Fecha de nacimiento";
            TablaClientesRegistradosDTG.Columns["EstadoCivil"].HeaderText = "Estado Civil";
            TablaClientesRegistradosDTG.Columns["rfcCliente"].HeaderText = "RFC";
            TablaClientesRegistradosDTG.Columns["fechaRegistroCliente"].HeaderText = "Fecha de registro";
            TablaClientesRegistradosDTG.Columns["correoCliente"].HeaderText = "Correo electrónico";
            TablaClientesRegistradosDTG.Columns["fechaModificacionCliente"].HeaderText = "Fecha de modificacion";

            TablaClientesRegistradosDTG.Columns["rfcCliente"].DisplayIndex = 0;
            TablaClientesRegistradosDTG.Columns["nombreCliente"].DisplayIndex = 1;
            TablaClientesRegistradosDTG.Columns["primerApellidoCliente"].DisplayIndex = 2;
            TablaClientesRegistradosDTG.Columns["segundoApellidoCliente"].DisplayIndex = 3;
            TablaClientesRegistradosDTG.Columns["EstadoCivil"].DisplayIndex = 4;
            TablaClientesRegistradosDTG.Columns["fechaNacimientoCliente"].DisplayIndex = 5;
            TablaClientesRegistradosDTG.Columns["correoCliente"].DisplayIndex = 6;
            TablaClientesRegistradosDTG.Columns["fechaRegistroCliente"].DisplayIndex = 7;
            TablaClientesRegistradosDTG.Columns["fechaModificacionCliente"].DisplayIndex = 8;

            TablaClientesRegistradosDTG.Columns["nombreCompleto"].Visible = false;
            TablaClientesRegistradosDTG.Columns["Id_Cliente"].Visible = false;
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
