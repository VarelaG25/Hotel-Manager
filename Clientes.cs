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
using System.Text.RegularExpressions;

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
            if (Login.baseDatos == 1)
            {
                CargarTablas();
            }
            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;

        }
        private void AceptarBTN_Click(object sender, EventArgs e)
        {
            ClienteDatos cliente = new ClienteDatos();
            cliente.Domicilio = null;
            if (string.IsNullOrEmpty(PaisCB.Text) || string.IsNullOrEmpty(EstadoCB.Text) || string.IsNullOrEmpty(CiudadCB.Text)
                || string.IsNullOrEmpty(CodigoPostalTXT.Text) || string.IsNullOrEmpty(TelefonoCasaTXT.Text) || string.IsNullOrEmpty(TelefonoCelularTXT.Text)
                || string.IsNullOrEmpty(RFCTXT.Text) || string.IsNullOrEmpty(CorreoTXT.Text) || string.IsNullOrEmpty(NombreTXT.Text) || string.IsNullOrEmpty(PrimerApellidoTXT.Text)
                || string.IsNullOrEmpty(SegundoApellidoTXT.Text) || string.IsNullOrEmpty(FechaNacimientoDTP.Text) || string.IsNullOrEmpty(EstadoCivilCB.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (TelefonoCelularTXT.Text.All(char.IsDigit) == false || TelefonoCasaTXT.Text.All(char.IsDigit) == false)
            {
                MessageBox.Show("El número de teléfono debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (TelefonoCasaTXT.Text.Length != 10 && TelefonoCelularTXT.Text.Length != 10)
            {
                MessageBox.Show("El número de teléfono debe tener exactamente 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(CorreoTXT.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("El correo electrónico no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var pais = PaisCB.Text;
            var estado = EstadoCB.Text;
            var ciudad = CiudadCB.Text;
            var codigoPostal = CodigoPostalTXT.Text;
            var telefonoCasa = TelefonoCasaTXT.Text;
            var telefonoCelular = TelefonoCelularTXT.Text;
            var rfc = RFCTXT.Text;
            var correo = CorreoTXT.Text;
            var nombre = NombreTXT.Text;
            var primerApellido = PrimerApellidoTXT.Text;
            var segundoApellido = SegundoApellidoTXT.Text;
            var fechaNacimiento = FechaNacimientoDTP.Value;
            var fechaRegistro = DateTime.Parse(FechaRegistroDTP.Text);
            var estadoCivil = EstadoCivilCB.Text;
            if (estadoCivil == "Soltero") estadoCivil = "S";
            else if (estadoCivil == "Casado") estadoCivil = "C";
            else if (estadoCivil == "Divorciado") estadoCivil = "D";
            else if (estadoCivil == "Viudo") estadoCivil = "V";

            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarUsuarioLogeado(Login.IdUsuarioActual);
                cliente.Id_Usuario = Convert.ToInt32(tabla.Rows[0]["Id_Usuario"]);
                cliente.Pais = pais;
                cliente.Estado = estado;
                cliente.Ciudad = ciudad;
                cliente.CodigoPostal = codigoPostal;
                cliente.TelefonoCasa = telefonoCasa;
                cliente.TelefonoCelular = telefonoCelular;
                cliente.RFC = rfc;
                cliente.Correo = correo;
                cliente.Nombre = nombre;
                cliente.PrimerApellido = primerApellido;
                cliente.SegundoApellido = segundoApellido;
                cliente.FechaNacimiento = fechaNacimiento;
                cliente.FechaRegistro = fechaRegistro;
                cliente.EstadoCivil = estadoCivil;
                var tablaUbicacion = new DataTable();
                tablaUbicacion = enlace.ValidarUbicacionUnica(cliente.Pais, cliente.Estado, cliente.Ciudad);
                cliente.Id_Ubicacion = tablaUbicacion.Rows.Count < 1 ? 0 : Convert.ToInt32(tablaUbicacion.Rows[0]["Id_Ubicacion"]);

                bool insertarCliente = enlace.Insertar_Cliente(cliente);
                if (!insertarCliente)
                {
                    MessageBox.Show("Error al registrar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MessageBox.Show("Cliente registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarTablas();
        }
        private void ClienteCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccion = ClienteCB.SelectedValue != null ? Convert.ToInt32(ClienteCB.SelectedValue) : -1;
            if (seleccion < 0)
            {
                MessageBox.Show("Por favor, seleccione un cliente.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarClienteEspecifico(seleccion);
                if (tabla.Rows.Count > 0)
                {
                    var cliente = tabla.Rows[0];
                    NombreSeleccionado.Text = cliente["nombreCliente"].ToString();
                    PrimerApellidoSeleccionado.Text = cliente["primerApellidoCliente"].ToString();
                    SegundoApellidoSeleccionado.Text = cliente["segundoApellidoCliente"].ToString();
                    TelefonoCasaSeleccionado.Text = cliente["telefonoCasa"].ToString();
                    TelefonoCelularSeleccionado.Text = cliente["telefonoCelular"].ToString();
                    RFCSeleccionado.Text = cliente["rfcCliente"].ToString();
                    CorreoSeleccionado.Text = cliente["correoCliente"].ToString();
                    FechaNacimientoSeleccionada.Value = DateTime.Parse(cliente["fechaNacimientoCliente"].ToString());
                    PaisSeleccionado.Text = cliente["pais"].ToString();
                    EstadoSeleccionado.Text = cliente["estado"].ToString();
                    CiudadSeleccionado.Text = cliente["ciudad"].ToString();
                    CodigoPostalSeleccionado.Text = cliente["codigoPostal"].ToString();
                }
            }
        }
        private void ModificarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro de que desea modificar el cliente?", "Modificar Cliente", buttons, MessageBoxIcon.Question);
            ClienteDatos cliente = new ClienteDatos();

            if (result == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(NombreSeleccionado.Text) || string.IsNullOrEmpty(PrimerApellidoSeleccionado.Text) || string.IsNullOrEmpty(SegundoApellidoSeleccionado.Text) ||
                    string.IsNullOrEmpty(TelefonoCasaSeleccionado.Text) || string.IsNullOrEmpty(TelefonoCelularSeleccionado.Text) || string.IsNullOrEmpty(RFCSeleccionado.Text) ||
                    string.IsNullOrEmpty(CorreoSeleccionado.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!TelefonoCasaSeleccionado.Text.All(char.IsDigit) || !TelefonoCelularSeleccionado.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El número de teléfono debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (TelefonoCelularSeleccionado.Text.Length != 10 && TelefonoCasaSeleccionado.Text.Length != 10)
                {
                    MessageBox.Show("El número de teléfono debe tener exactamente 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!Regex.IsMatch(CorreoSeleccionado.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("El correo electrónico no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var nombre = NombreSeleccionado.Text;
                var primerApellido = PrimerApellidoSeleccionado.Text;
                var segundoApellido = SegundoApellidoSeleccionado.Text;
                var telefonoCasa = TelefonoCasaSeleccionado.Text;
                var telefonoCelular = TelefonoCelularSeleccionado.Text;
                var rfc = RFCSeleccionado.Text;
                var correo = CorreoSeleccionado.Text;
                var fechaNacimiento = FechaNacimientoSeleccionada.Value;
                var fechaModificacion = FechaNacimientoSeleccionada.Value;

                if (Login.baseDatos == 1)
                {
                    var enlace = new EnlaceDB();
                    var tabla = new DataTable();
                    cliente.Id_Cliente = Convert.ToInt32(ClienteCB.SelectedValue);
                    cliente.Nombre = nombre;
                    cliente.PrimerApellido = primerApellido;
                    cliente.SegundoApellido = segundoApellido;
                    cliente.TelefonoCasa = telefonoCasa;
                    cliente.TelefonoCelular = telefonoCelular;
                    cliente.RFC = rfc;
                    cliente.Correo = correo;
                    cliente.FechaNacimiento = fechaNacimiento;
                    cliente.FechaModificacion = fechaModificacion;
                    tabla = enlace.consultarIdUsuario(Login.IdUsuarioActual);
                    cliente.Id_Usuario = Convert.ToInt32(tabla.Rows[0]["Id_Usuario"]);
                    bool resultado = enlace.Actualizar_Cliente(cliente);
                    if (!resultado)
                    {
                        MessageBox.Show("Error al modificar al cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    CargarTablas();
                }
                MessageBox.Show("Cliente modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BorrarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro de que desea borrar al cliente?", "Borrar Cliente", buttons, MessageBoxIcon.Question);
            ClienteDatos cliente = new ClienteDatos();

            if (result == DialogResult.Yes)
            {
                if (Login.baseDatos == 1)
                {
                    var enlace = new EnlaceDB();
                    cliente.Id_Cliente = Convert.ToInt32(ClienteCB.SelectedValue);
                    bool resultado = enlace.Borrar_Cliente(cliente.Id_Cliente);
                    if (!resultado)
                    {
                        MessageBox.Show("Error al borrar al cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    CargarTablas();
                }
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
