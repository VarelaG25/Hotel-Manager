using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace AAVD
{
    public partial class Reservaciones : Form
    {
        public bool habitacionDisponible = false;
        public Reservaciones()
        {
            InitializeComponent();
        }
        public class Reservacion
        {
            public int Id_TipoHab { get; set; }
            public int Id_Habitaciones { get; set; }
            public int Id_Cliente { get; set; }
            public int Id_Usuario { get; set; }
            public string codigoReserva { get; set; }
            public DateTime fechaLlegada { get; set; }
            public DateTime fechaSalida { get; set; }
            public DateTime fechaRegistro { get; set; }
            public double anticipo { get; set; }
            public double restante { get; set; }
            public string metodoPago { get; set; }
            public int personasHospedadas { get; set; }
            public int checkIn { get; set; }
            public int checkOut { get; set; }
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
            TotalTXT.Text = "0";
            AnticipoTXT.Text = "0";
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

            var tablaHotel = new DataTable();
            tablaHotel = enlace.consultarHotel();
            HotelCB.DisplayMember = "nombreHotel";
            HotelCB.ValueMember = "Id_Hotel";
            HotelCB.DataSource = tablaHotel;

            var tablaTH = new DataTable();
            int Id_Hotel = Convert.ToInt32(HotelCB.SelectedValue);
            tablaTH = enlace.consultarTipoHabitacionId(Id_Hotel);
            TipoHabCB.DisplayMember = "nivelHabitacion";
            TipoHabCB.ValueMember = "Id_TipoHab";
            TipoHabCB.DataSource = tablaTH;

            TipoHabReservaCB.DisplayMember = "nivelHabitacion";
            TipoHabReservaCB.ValueMember = "Id_TipoHab";
            TipoHabReservaCB.DataSource = tablaTH;

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

            var tablaUbicacion = new DataTable();
            tablaUbicacion = enlace.ConsultarUbicacion();
            var ciudades = tablaUbicacion.AsEnumerable()
                .GroupBy(row => row.Field<string>("ciudad"))
                .Select(group => new
                {
                    ciudad = group.Key,
                    Id_Ubicacion = group.First().Field<int>("Id_Ubicacion")
                })
                .ToList();
            CiudadCB.DataSource = ciudades;
            CiudadCB.ValueMember = "Id_Ubicacion";
            CiudadCB.DisplayMember = "ciudad";

            var tablaReservaciones = new DataTable();
            var tablaCheckIn = new DataTable();
            var tablaCheckOut = new DataTable();
            tablaReservaciones = enlace.consultarReservaciones();
            tablaReservaciones.Columns.Add("Apellidos", typeof(string), "primerApellidoCliente + ' ' + segundoApellidoCliente");
            tablaCheckIn = enlace.consultarCheckIn();
            tablaCheckOut = enlace.consultarCheckOut();

            TablaReservacionesDTG.DataSource = tablaReservaciones;
            TablaReservacionesDTG.Columns["Id_Reservacion"].Visible = false;
            TablaReservacionesDTG.Columns["checkIn"].Visible = false;
            TablaReservacionesDTG.Columns["checkOut"].Visible = false;
            TablaReservacionesDTG.Columns["fechaRegistro"].Visible = false;
            TablaReservacionesDTG.Columns["fechaSalida"].Visible = false;
            TablaReservacionesDTG.Columns["fechaLlegada"].Visible = false;
            TablaReservacionesDTG.Columns["primerApellidoCliente"].Visible = false;
            TablaReservacionesDTG.Columns["segundoApellidoCliente"].Visible = false;
            TablaReservacionesDTG.Columns["Apellidos"].Visible = true;

            TablaReservacionesDTG.Columns["nombreHotel"].HeaderText = "Hotel";
            TablaReservacionesDTG.Columns["nivelHabitacion"].HeaderText = "Nivel de habitacion";
            TablaReservacionesDTG.Columns["numeroHabitacion"].HeaderText = "# de habitacion";
            TablaReservacionesDTG.Columns["piso"].HeaderText = "# de piso";
            TablaReservacionesDTG.Columns["rfcCliente"].HeaderText = "RFC";
            TablaReservacionesDTG.Columns["correoCliente"].HeaderText = "Correo";

            TablaReservacionesDTG.Columns["nombreHotel"].DisplayIndex = 0;
            TablaReservacionesDTG.Columns["nivelHabitacion"].DisplayIndex = 1;
            TablaReservacionesDTG.Columns["numeroHabitacion"].DisplayIndex = 2;
            TablaReservacionesDTG.Columns["piso"].DisplayIndex = 3;
            TablaReservacionesDTG.Columns["Apellidos"].DisplayIndex = 4;
            TablaReservacionesDTG.Columns["rfcCliente"].DisplayIndex = 5;
            TablaReservacionesDTG.Columns["correoCliente"].DisplayIndex = 6;

            ApellidosCB.DisplayMember = "Apellidos";
            ApellidosCB.ValueMember = "Id_Reservacion";
            ApellidosCB.DataSource = tablaReservaciones;

            RFCCombo.DisplayMember = "rfcCliente";
            RFCCombo.ValueMember = "Id_Reservacion";
            RFCCombo.DataSource = tablaReservaciones;

            CorreoCombo.DisplayMember = "correoCliente";
            CorreoCombo.ValueMember = "Id_Reservacion";
            CorreoCombo.DataSource = tablaReservaciones;

            TablaCheckInDTG.DataSource = tablaCheckIn;
            TablaCheckOutDTG.DataSource = tablaCheckOut;

            MetPagoCB.SelectedIndex = 0;
            habitacionDisponible = false;
        }
        private void HotelCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HotelCB.SelectedValue == null)
            {
                MessageBox.Show("Primero compruebe disponibilidad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int seleccionHotel = Convert.ToInt32(HotelCB.SelectedValue);
            if (seleccionHotel > 0)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarTipoHabitacionId(seleccionHotel);
                TipoHabCB.DisplayMember = "nivelHabitacion";
                TipoHabCB.ValueMember = "Id_TipoHab";
                TipoHabCB.DataSource = tabla;

                TipoHabReservaCB.DisplayMember = "nivelHabitacion";
                TipoHabReservaCB.ValueMember = "Id_TipoHab";
                TipoHabReservaCB.DataSource = tabla;

                var tablaUbicacion = new DataTable();
                tablaUbicacion = enlace.consultarUbicacionHotel(seleccionHotel);
                PaisTXT.Text = tablaUbicacion.Rows[0]["pais"].ToString();
                EstadoTXT.Text = tablaUbicacion.Rows[0]["estado"].ToString();
                CiudadTXT.Text = tablaUbicacion.Rows[0]["ciudad"].ToString();
                int Validar = Convert.ToInt32(TipoHabCB.SelectedValue);
                if (Validar == 0)
                {
                    AmenidadTXT.Text = "No hay amenidades disponibles.";
                    CaracteristicaTXT.Text = "No hay características disponibles.";
                    PrecioXNocheTXT.Text = "Sin habitacion asignada.";
                }
                var tablaServicios = new DataTable();
                tablaServicios = enlace.consultarServicio(seleccionHotel);
                if (tablaServicios.Rows.Count > 0) ServicioAdicionalTXT.Text = string.Join(", ", tablaServicios.AsEnumerable().Select(f => f.Field<string>("nombreServicio")));
                else ServicioAdicionalTXT.Text = "No hay servicios adicionales disponibles.";
            }
        }
        private void TipoHabCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccionTipoHab = Convert.ToInt32(TipoHabCB.SelectedValue);
            if (seleccionTipoHab > 0)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarPrecio(seleccionTipoHab);
                PrecioXNocheTXT.Text = tabla.Rows[0]["precio"].ToString();

                var tablaAmenidades = new DataTable();
                var tablaCaracteristicas = new DataTable();

                tablaAmenidades = enlace.consultarAmenidades(seleccionTipoHab);
                tablaCaracteristicas = enlace.consultarCaracteristicas(seleccionTipoHab);

                if (tablaAmenidades.Rows.Count > 0) AmenidadTXT.Text = string.Join(", ", tablaAmenidades.AsEnumerable().Select(f => f.Field<string>("nombreAmenidad")));
                else AmenidadTXT.Text = "No hay amenidades disponibles.";
                if (tablaCaracteristicas.Rows.Count > 0) CaracteristicaTXT.Text = string.Join(", ", tablaCaracteristicas.AsEnumerable().Select(f => f.Field<string>("nombreCaracteristica")));
                else CaracteristicaTXT.Text = "No hay características disponibles.";
            }
        }

        private void ReservarBTN_Click(object sender, EventArgs e)
        {
            var enlace = new EnlaceDB();
            var tabla = new DataTable();
            var tablaUsuario = new DataTable();
            Reservacion reservacion = new Reservacion();
            if(habitacionDisponible == false)
            {
                MessageBox.Show("No hay habitaciones disponibles, pruebe intentar otro tipo o fecha.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Guid guid = Guid.NewGuid();
            string guidStr = guid.ToString("N");
            string codigoReserva = guidStr.Substring(guidStr.Length - 15);
            int tipoHab = Convert.ToInt32(TipoHabReservaCB.SelectedValue);
            reservacion.Id_TipoHab = tipoHab;
            reservacion.fechaLlegada = DateTime.Parse(FechaLlegada.Text);
            reservacion.fechaSalida = DateTime.Parse(FechaSalida.Text);
            tabla = enlace.consultarHabitacionesDisponibles(reservacion);
            reservacion.Id_Habitaciones = Convert.ToInt32(tabla.Rows[0]["Id_Habitaciones"]);
            reservacion.Id_Cliente = Convert.ToInt32(RFCCB.SelectedValue);
            tablaUsuario = enlace.consultarUsuarioLogeado(Login.IdUsuarioActual);
            reservacion.Id_Usuario = Convert.ToInt32(tablaUsuario.Rows[0]["Id_Usuario"]);
            reservacion.codigoReserva = codigoReserva.ToString();
            reservacion.anticipo = Convert.ToInt32(AnticipoTXT.Text);
            reservacion.restante = Convert.ToInt32(RestanteTXT.Text);
            reservacion.metodoPago = MetPagoCB.Text;
            int personasPermitidas = Convert.ToInt32(PersonasTXT.Text);
            reservacion.personasHospedadas = Convert.ToInt32(NumPersonas.Text);
            reservacion.fechaRegistro = DateTime.Parse(FechaActualRegistro.Text);
            if (personasPermitidas <= reservacion.personasHospedadas)
            {
                MessageBox.Show("No se admiten mas de: " + "'" + personasPermitidas.ToString() + "'" + " personas por habitacion", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool resultado = enlace.InsertarReserva(reservacion);
            if (!resultado)
            {
                MessageBox.Show("Error al realizar la reservacion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MessageBox.Show("Reserva realizada correctamente.\nCodigo de reserva generado: " + codigoReserva, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarTablas();
        }

        private void TipoHabReservaCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccionTipoHab = Convert.ToInt32(TipoHabReservaCB.SelectedValue);
            if (seleccionTipoHab > 0)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarPrecio(seleccionTipoHab);
                PrecioTXT.Text = tabla.Rows[0]["precio"].ToString();
                PersonasTXT.Text = tabla.Rows[0]["numeroPersonas"].ToString();
                double precio = Convert.ToDouble(PrecioTXT.Text);
            }
        }

        private void FechaSalida_ValueChanged(object sender, EventArgs e)
        {
            double precio = Convert.ToDouble(PrecioTXT.Text);
            DateTime fechaInicio = FechaLlegada.Value.Date;
            DateTime fechaFin = FechaSalida.Value.Date;
            int dias = (fechaFin - fechaInicio).Days;
            var total = precio * dias;
            TotalTXT.Text = total > 0 ? total.ToString() : "Rango de fechas invalido.";
        }

        private void AnticipoTXT_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(AnticipoTXT.Text))
            {
                double anticipo = Convert.ToDouble(AnticipoTXT.Text);
                double total = Convert.ToDouble(TotalTXT.Text);
                double restante = total - anticipo;
                RestanteTXT.Text = restante.ToString();
            }
        }

        private void ComprobarCB_Click(object sender, EventArgs e)
        {
            int tipoHab = Convert.ToInt32(TipoHabReservaCB.SelectedValue);
            var enlace = new EnlaceDB();
            var tabla = new DataTable();
            Reservacion reservacion = new Reservacion();
            reservacion.Id_TipoHab = tipoHab;
            reservacion.fechaLlegada = DateTime.Parse(FechaLlegada.Text);
            reservacion.fechaSalida = DateTime.Parse(FechaSalida.Text);
            tabla = enlace.consultarHabitacionesDisponibles(reservacion);
            if (tabla.Rows.Count <= 0)
            {
                MessageBox.Show("No hay habitaciones disponibles de este tipo en esta fecha.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Existe una habitacion disponible.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            habitacionDisponible = true;
        }

        private void RFCCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccion = Convert.ToInt32(RFCCombo.SelectedValue);
            if (seleccion > 0)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarDatosReservaciones(seleccion);
                CodigoReserva.Text = tabla.Rows[0]["codigoReserva"].ToString();
                FechaLlegadaTXT.Text = tabla.Rows[0]["fechaLlegada"].ToString();
                FechaSalidaTXT.Text = tabla.Rows[0]["fechaSalida"].ToString();
                FechaRegistroTXT.Text = tabla.Rows[0]["fechaRegistro"].ToString();
                PrecioRestanteTXT.Text = tabla.Rows[0]["restante"].ToString();
                MetodoPagoTXT.Text = tabla.Rows[0]["metodoPago"].ToString();
                double anticipo = Convert.ToDouble(tabla.Rows[0]["anticipo"]);
                double restante = Convert.ToDouble(tabla.Rows[0]["restante"]);
                double total = anticipo + restante;
                PrecioTotal.Text = total.ToString();
            }
        }
    }
}

