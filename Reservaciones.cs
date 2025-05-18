using System;
using System.Globalization;
using System.Text.RegularExpressions;
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
using static AAVD.Hoteles;

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
            public int estatus { get; set; }
            public int Id_Reservacion { get; set; }
            public int Id_TipoHab { get; set; }
            public int Id_Habitaciones { get; set; }
            public int Id_Cliente { get; set; }
            public int Id_Usuario { get; set; }
            public string codigoReserva { get; set; }
            public DateTime fechaLlegada { get; set; }
            public DateTime fechaSalida { get; set; }
            public DateTime fechaRegistro { get; set; }
            public decimal anticipo { get; set; }
            public decimal restante { get; set; }
            public string metodoPago { get; set; }
            public int personasHospedadas { get; set; }
            public int checkIn { get; set; }
            public DateTime? fehcaCheckIn { get; set; }
            public int checkOut { get; set; }
            public DateTime? fechaCheckOut { get; set; }
        }
        public class ServicioAdicional
        {
            public int Id_ServicioAdicional { get; set; }
            public string nombreServicio { get; set; }
            public decimal precio { get; set; }

            public override string ToString()
            {
                return $"{nombreServicio} - {precio.ToString("C2")}";
            }
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
            if (Login.baseDatos == 1)
            {
                CargarTablas();
            }
            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;
        }

        private void HotelCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccionHotel = HotelCB.SelectedValue != null ? Convert.ToInt32(HotelCB.SelectedValue) : -1;
            if (seleccionHotel < 0)
            {
                MessageBox.Show("Seleccione un hotel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Login.baseDatos == 1)
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
                var ubicacion = tablaUbicacion.Rows[0];
                PaisTXT.Text = ubicacion["pais"].ToString();
                EstadoTXT.Text = ubicacion["estado"].ToString();
                CiudadTXT.Text = ubicacion["ciudad"].ToString();
                int Validar = Convert.ToInt32(TipoHabCB.SelectedValue);
                if (Validar == 0)
                {
                    AmenidadTXT.Text = "No hay amenidades disponibles.";
                    CaracteristicaTXT.Text = "No hay características disponibles.";
                    PrecioXNocheTXT.Text = "Sin habitacion asignada.";
                }

                var tablaServicioAdicional = enlace.consultarServiciosXHotel(seleccionHotel);
                ServicioAdicionalCB.Items.Clear();
                foreach (DataRow fila in tablaServicioAdicional.Rows)
                {
                    var servicioAdicional = new ServicioAdicional()
                    {
                        Id_ServicioAdicional = Convert.ToInt32(fila["Id_ServicioAdicional"]),
                        nombreServicio = fila["nombreServicio"].ToString(),
                        precio = Convert.ToDecimal(fila["precio"])
                    };
                    ServicioAdicionalCB.Items.Add(servicioAdicional);
                }
            }
        }
        private void TipoHabCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccionTipoHab = TipoHabCB.SelectedValue != null ? Convert.ToInt32(TipoHabCB.SelectedValue) : -1;
            if (seleccionTipoHab < 0)
            {
                MessageBox.Show("Seleccione un tipo de habitación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarPrecio(seleccionTipoHab);
                decimal precio = Convert.ToDecimal(tabla.Rows[0]["precio"]);
                PrecioXNocheTXT.Text = precio.ToString("C2");

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
            if (habitacionDisponible == false)
            {
                MessageBox.Show("No hay habitaciones disponibles, pruebe intentar otro tipo o fecha.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Guid guid = Guid.NewGuid();
            string guidStr = guid.ToString("N");
            string codigoReserva = guidStr.Substring(guidStr.Length - 15).ToUpper();

            int tipoHab = Convert.ToInt32(TipoHabReservaCB.SelectedValue);
            int personasPermitidas = Convert.ToInt32(PersonasTXT.Text);
            int personasIngresadas = Convert.ToInt32(NumPersonas.Text);

            string anticipoLimpio = Regex.Replace(AnticipoTXT.Text, @"[^\d.,\-]", "");
            string restanteLimpio = Regex.Replace(RestanteTXT.Text, @"[^\d.,\-]", "");

            Reservacion reservacion = new Reservacion
            {
                Id_TipoHab = tipoHab,
                fechaLlegada = DateTime.Parse(FechaLlegada.Text),
                fechaSalida = DateTime.Parse(FechaSalida.Text),
                codigoReserva = codigoReserva,
                anticipo = decimal.Parse(anticipoLimpio, CultureInfo.CurrentCulture),
                restante = decimal.Parse(restanteLimpio, CultureInfo.CurrentCulture),
                metodoPago = MetPagoCB.Text,
                personasHospedadas = personasIngresadas,
                fechaRegistro = DateTime.Parse(FechaActualRegistro.Text)
            };

            if (Login.baseDatos == 1) // SQL Server
            {
                var enlace = new EnlaceDB();
                var tablaHabitaciones = enlace.consultarHabitacionesDisponibles(reservacion);
                reservacion.Id_Habitaciones = Convert.ToInt32(tablaHabitaciones.Rows[0]["Id_Habitaciones"]);

                reservacion.Id_Cliente = Convert.ToInt32(RFCCB.SelectedValue);

                var tablaUsuario = enlace.consultarUsuarioLogeado(Login.IdUsuarioActual);
                reservacion.Id_Usuario = Convert.ToInt32(tablaUsuario.Rows[0]["Id_Usuario"]);

                if (personasPermitidas <= personasIngresadas)
                {
                    MessageBox.Show("No se admiten más de: " + "'" + personasPermitidas.ToString() + "'" + " personas por habitación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int Id_Reservacion;
                if (!enlace.InsertarReserva(reservacion, out Id_Reservacion))
                {
                    MessageBox.Show("Error al registrar la reservación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (var item in ServicioAdicionalCB.CheckedItems)
                {
                    if (item is ServicioAdicional servicioSeleccionado)
                    {
                        enlace.Insertar_Reservacion_ServicioAdicional(servicioSeleccionado, Id_Reservacion);
                    }
                }
            }
            else if (Login.baseDatos == 2)
            {

            }

            MessageBox.Show("Reserva realizada correctamente.\nCódigo de reserva generado: " + codigoReserva, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarTablas();
        }


        private void TipoHabReservaCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccionTipoHab = TipoHabReservaCB.SelectedValue != null ? Convert.ToInt32(TipoHabReservaCB.SelectedValue) : -1;
            if (seleccionTipoHab < 0)
            {
                MessageBox.Show("Seleccione un tipo de habitación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarPrecio(seleccionTipoHab);

                string precioTexto = tabla.Rows[0]["precio"].ToString();

                if (decimal.TryParse(precioTexto, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal precioDecimal))
                {
                    PrecioTXT.Text = precioDecimal.ToString("C2");
                }
                else
                {
                    PrecioTXT.Text = precioTexto;
                }

                PersonasTXT.Text = tabla.Rows[0]["numeroPersonas"].ToString();
            }
        }
        private void FechaLlegada_ValueChanged(object sender, EventArgs e)
        {
            CargarPrecios();
        }
        private void FechaSalida_ValueChanged(object sender, EventArgs e)
        {
            CargarPrecios();
        }
        private void AnticipoTXT_TextChanged(object sender, EventArgs e)
        {
            CargarPrecios();

            if (!string.IsNullOrEmpty(AnticipoTXT.Text) && AnticipoTXT.Text.All(char.IsDigit))
            {
                decimal anticipo = Convert.ToDecimal(AnticipoTXT.Text);

                decimal total;
                if (decimal.TryParse(TotalTXT.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out total))
                {
                    decimal restante = total - anticipo;
                    RestanteTXT.Text = restante.ToString("C2");
                }
                else
                {
                    RestanteTXT.Text = "Total inválido";
                }
            }
            else
            {
                RestanteTXT.Text = "";
            }
        }
        private void ComprobarCB_Click(object sender, EventArgs e)
        {
            int tipoHab = Convert.ToInt32(TipoHabReservaCB.SelectedValue);

            var reservacion = new Reservacion
            {
                Id_TipoHab = tipoHab,
                fechaLlegada = DateTime.Parse(FechaLlegada.Text),
                fechaSalida = DateTime.Parse(FechaSalida.Text)
            };

            DataTable tabla = new DataTable();

            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                tabla = enlace.consultarHabitacionesDisponibles(reservacion);
            }
            else if (Login.baseDatos == 2)
            {

            }

            if (tabla.Rows.Count <= 0)
            {
                MessageBox.Show("No hay habitaciones asignadas de este tipo en el hotel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Existe una habitación disponible.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            habitacionDisponible = true;
        }
        private void RFCCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccion;
            if (int.TryParse(RFCCombo.SelectedValue?.ToString(), out seleccion) && seleccion > 0)
            {
                var tabla = new DataTable();
                if (Login.baseDatos == 1)
                {
                    var enlace = new EnlaceDB(); // conexión original
                    tabla = enlace.consultarDatosReservaciones(seleccion);
                }
                else if (Login.baseDatos == 2)
                {
                }

                if (tabla.Rows.Count > 0)
                {
                    var fila = tabla.Rows[0];
                    CodigoReserva.Text = fila["codigoReserva"].ToString();
                    FechaLlegadaTXT.Text = fila["fechaLlegada"].ToString();
                    FechaSalidaTXT.Text = fila["fechaSalida"].ToString();
                    FechaRegistroTXT.Text = fila["fechaRegistro"].ToString();

                    decimal precioRestante = Convert.ToDecimal(fila["restante"]);
                    PrecioRestanteTXT.Text = precioRestante.ToString("C2");

                    MetodoPagoTXT.Text = fila["metodoPago"].ToString();

                    double anticipo = Convert.ToDouble(fila["anticipo"]);
                    double restante = Convert.ToDouble(fila["restante"]);
                    double total = anticipo + restante;
                    PrecioTotal.Text = total.ToString("C2");
                }
                else
                {
                    MessageBox.Show("No se encontraron datos para la selección actual.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ServicioAdicionalCB_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Usamos BeginInvoke para que CheckedItems ya esté actualizado cuando accedamos a él
            this.BeginInvoke(new Action(() =>
            {
                decimal total = 0;

                foreach (var item in ServicioAdicionalCB.CheckedItems)
                {
                    if (item is ServicioAdicional servicio)
                    {
                        total += servicio.precio;
                    }
                }

                PrecioServiciosTXT.Text = total.ToString("C2");
            }));
        }


        public void CargarPrecios()
        {
            if (!decimal.TryParse(PrecioTXT.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal precio))
            {
                // Precio inválido
                TotalTXT.Text = "Precio inválido";
                return;
            }

            if (!decimal.TryParse(PrecioServiciosTXT.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal precioServicios))
            {
                // Precio servicios inválido
                TotalTXT.Text = "Precio servicios inválido";
                return;
            }

            DateTime fechaInicio = FechaLlegada.Value.Date;
            DateTime fechaFin = FechaSalida.Value.Date;

            int dias = (fechaFin - fechaInicio).Days;

            if (dias <= 0)
            {
                TotalTXT.Text = "Rango de fechas inválido.";
                return;
            }

            decimal total = (precio * dias) + precioServicios;

            TotalTXT.Text = total.ToString("C2");
        }
        private void ConfirmarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro que desea confirmar asistencia?", "Advertencia", buttons, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Reservacion reserva = new Reservacion
                {
                    estatus = 1,
                    Id_Reservacion = Convert.ToInt32(RFCCombo.SelectedValue),
                    fehcaCheckIn = DateTime.Parse(FechaConfirmacionDTG.Text),
                    fechaCheckOut = null
                };

                bool resultado = false;

                if (Login.baseDatos == 1)
                {
                    var enlace = new EnlaceDB();
                    resultado = enlace.ActualizarReservacion(reserva);
                }
                else if (Login.baseDatos == 2)
                {

                }

                if (!resultado)
                {
                    MessageBox.Show("Error al actualizar reservación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Asistencia confirmada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarTablas();
            }
        }


        private void SalidaBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro que desea confirmar salida de la estancia?", "Advertencia", buttons, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Reservacion reserva = new Reservacion
                {
                    estatus = 0,
                    Id_Reservacion = Convert.ToInt32(RFCCombo.SelectedValue),
                    fehcaCheckIn = null,
                    fechaCheckOut = DateTime.Parse(FechaConfirmacionDTG.Text)
                };

                bool resultado = false;

                if (Login.baseDatos == 1)
                {
                    var enlace = new EnlaceDB();
                    resultado = enlace.ActualizarReservacion(reserva);
                }
                else if (Login.baseDatos == 2)
                {

                }

                if (!resultado)
                {
                    MessageBox.Show("Error al actualizar reservación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Salida confirmada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarTablas();
            }
        }


        private void CancelarBTN_Click(object sender, EventArgs e)
        {
            if (Login.tipoUsuario == 0)
            {
                MessageBox.Show("No tiene permisos para acceder a esta función.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar la estadía?", "Advertencia", buttons, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int Id_Reservacion = Convert.ToInt32(RFCCombo.SelectedValue);

                DataTable tablaReservaciones = new DataTable();
                bool resultado = false;

                if (Login.baseDatos == 1)
                {
                    var enlace = new EnlaceDB();
                    tablaReservaciones = enlace.validarCheck(Id_Reservacion);
                    var validar = enlace.consultarDatosReservaciones(Id_Reservacion);
                    DateTime fechaLlegada = Convert.ToDateTime(validar.Rows[0]["fechaLlegada"]);
                    DateTime fechaActual = FechaConfirmacionDTG.Value;
                    int diasPermitidos = 3;
                    TimeSpan diferencia = fechaLlegada.Date - fechaActual.Date;
                    if (diferencia.Days < diasPermitidos)
                    {
                        MessageBox.Show("No puede cancelarse si faltan 3 días o menos para la asistencia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int checkIn = Convert.ToInt32(tablaReservaciones.Rows[0]["checkIn"]);
                    int checkOut = Convert.ToInt32(tablaReservaciones.Rows[0]["checkOut"]);


                    if (checkIn == 1 || checkOut == 1)
                    {
                        MessageBox.Show("No puede cancelarse si ya se confirmó asistencia o salida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    resultado = enlace.borrarReservacion(Id_Reservacion);
                }
                else if (Login.baseDatos == 2)
                {
                }

                if (!resultado)
                {
                    MessageBox.Show("Error al cancelar reservación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Cancelación confirmada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarTablas();
            }
        }

        private void ServicioAdicionalCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPrecios();
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
            tablaUbicacion = enlace.consultarHotelXCiudad();
            var ciudades = tablaUbicacion.AsEnumerable()
            .Select(row => row.Field<string>("ciudad"))
            .Where(c => !string.IsNullOrEmpty(c))
            .Distinct()
            .Select(c => new { ciudad = c })
            .ToList();

            // Insertar una opción vacía o "Seleccione..."
            ciudades.Insert(0, new { ciudad = "" });

            CiudadCB.DataSource = ciudades;
            CiudadCB.DisplayMember = "ciudad";
            CiudadCB.ValueMember = "ciudad";

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

            TablaCheckInDTG.Columns["primerApellidoCliente"].Visible = false;
            TablaCheckInDTG.Columns["segundoApellidoCliente"].Visible = false;

            tablaCheckIn.Columns.Add("Apellidos", typeof(string), "primerApellidoCliente + ' ' + segundoApellidoCliente");

            TablaCheckInDTG.Columns["Apellidos"].Visible = true;
            TablaCheckInDTG.Columns["rfcCliente"].HeaderText = "RFC";
            TablaCheckInDTG.Columns["Apellidos"].HeaderText = "Apellidos";
            TablaCheckInDTG.Columns["checkIn"].HeaderText = "Asistencia";

            TablaCheckInDTG.Columns["rfcCliente"].DisplayIndex = 0;
            TablaCheckInDTG.Columns["Apellidos"].DisplayIndex = 1;
            TablaCheckInDTG.Columns["checkIn"].DisplayIndex = 2;

            TablaCheckOutDTG.DataSource = tablaCheckOut;

            TablaCheckOutDTG.Columns["primerApellidoCliente"].Visible = false;
            TablaCheckOutDTG.Columns["segundoApellidoCliente"].Visible = false;

            tablaCheckOut.Columns.Add("Apellidos", typeof(string), "primerApellidoCliente + ' ' + segundoApellidoCliente");

            TablaCheckOutDTG.Columns["Apellidos"].Visible = true;
            TablaCheckOutDTG.Columns["rfcCliente"].HeaderText = "RFC";
            TablaCheckOutDTG.Columns["Apellidos"].HeaderText = "Apellidos";
            TablaCheckOutDTG.Columns["checkOut"].HeaderText = "Salida";

            TablaCheckOutDTG.Columns["rfcCliente"].DisplayIndex = 0;
            TablaCheckOutDTG.Columns["Apellidos"].DisplayIndex = 1;
            TablaCheckOutDTG.Columns["checkOut"].DisplayIndex = 2;

            MetPagoCB.SelectedIndex = 0;
            habitacionDisponible = false;
            PrecioServiciosTXT.Text = "0";

            var tablaServicioAdicional = enlace.consultarServiciosXHotel(Id_Hotel);
            ServicioAdicionalCB.Items.Clear();

            foreach (DataRow fila in tablaServicioAdicional.Rows)
            {
                ServicioAdicional servicioAdicional = new ServicioAdicional()
                {
                    Id_ServicioAdicional = Convert.ToInt32(fila["Id_ServicioAdicional"]),
                    nombreServicio = fila["nombreServicio"].ToString(),
                    precio = Convert.ToDecimal(fila["precio"])
                };
                ServicioAdicionalCB.Items.Add(servicioAdicional);
            }

        }

        private void CiudadCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ciudadSeleccionada = CiudadCB.Text;
            
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tablaHoteles = enlace.hotelXCiudad(ciudadSeleccionada);

                HotelCB.DisplayMember = "nombreHotel";
                HotelCB.ValueMember = "Id_Hotel";
                HotelCB.DataSource = tablaHoteles;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (Login.baseDatos == 1)
            {
                CargarTablas();
            }
        }
    }
}

