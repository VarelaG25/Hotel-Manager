using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static AAVD.Hoteles;

namespace AAVD
{
    public partial class Hoteles : Form
    {
        public Hoteles()
        {
            InitializeComponent();
        }
        // --------------------------------------------------- Tipo Habitacion -------------------------------------------------
        public class TipoHab
        {
            public int Id_TipoHab { get; set; }
            public int Id_Hotel { get; set; }
            public string nivelHabitacion { get; set; }
            public int numeroCamas { get; set; }
            public string tipoCama { get; set; }
            public double precio { get; set; }
            public int numeroPersonas { get; set; }
            public string frenteA { get; set; }
            public override string ToString()
            {
                return nivelHabitacion;
            }
        }
        // --------------------------------------------------- Habitaciones -------------------------------------------------
        public class Habitacion
        {
            public int Id_Habitaciones { get; set; }
            public int Id_Hotel { get; set; }
            public int Id_TipoHab { get; set; }
            public int NumeroHabitacion { get; set; }
            public int Piso { get; set; }
            public string Estatus { get; set; }
        }
        // --------------------------------------------------- Hotel -------------------------------------------------
        public class Hotel : Ubicacion
        {
            public int Id_Hotel { get; set; }
            public int Id_Usuario { get; set; }
            public string NombreHotel { get; set; }
            public string ZonaTuristica { get; set; }
            public int NumPisos { get; set; }
            public DateTime FechaOperacion { get; set; }
            public int FrentePlaya { get; set; }
            public int NumPiscinas { get; set; }
            public int SalonEventos { get; set; }
            public int NumHabitaciones { get; set; }
            public DateTime FechaRegistro { get; set; }
            public DateTime FechaModificacion { get; set; }
        }
        // --------------------------------------------------- Amenidades -------------------------------------------------
        public class Amenidad
        {
            public int Id_Amenidad { get; set; }
            public string nombreAmenidad { get; set; }
            public override string ToString()
            {
                return nombreAmenidad;
            }
        }
        // --------------------------------------------------- Caracteristicas -------------------------------------------------
        public class Caracteristica
        {
            public int Id_Caracteristica { get; set; }
            public string nombreCaracteristica { get; set; }

            public override string ToString()
            {
                return nombreCaracteristica;
            }
        }
        // --------------------------------------------------- Servicio Adicional -------------------------------------------------
        public class ServicioAdicional
        {
            public int Id_ServicioAdicional { get; set; }
            public string nombreServicio { get; set; }
            public double costo { get; set; }

            public override string ToString()
            {
                return nombreServicio;
            }
        }
        // --------------------------------------------------- Inicializacion de ventana -------------------------------------------------
        private void AbrirControlEnPanel(System.Windows.Forms.UserControl control)
        {
            MenuContenedor.Controls.Clear();
            control.Dock = DockStyle.Fill;
            MenuContenedor.Controls.Add(control);
            control.BringToFront();
        }
        private void Hoteles_Load(object sender, EventArgs e)
        {
            AbrirControlEnPanel(new Menu());
            var NuevoForm = new Login();
            if (Login.baseDatos == 1)
            {
                // Implementar la lógica para la base de datos SQL
                CargarTablasTipoHab();
            }
            else if (Login.baseDatos == 2)
            {
                // Implementar la lógica para la base de datos CQL
            }
            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;
        }
        // --------------------------------------------------- Botones -------------------------------------------------
        private void BTN_Registrar_Click(object sender, EventArgs e)
        {
            // Validaciones para registrar el tipo de habitacion
            TipoHab tipoHab = new TipoHab();
            Amenidad amenidad = new Amenidad();
            Caracteristica caracteristica = new Caracteristica();
            if (string.IsNullOrEmpty(NivelHabitacionTXT.Text) || string.IsNullOrEmpty(NumeroCamasTXT.Text) || string.IsNullOrEmpty(TipoCamaTXT.Text) || string.IsNullOrEmpty(PrecioTXT.Text) || string.IsNullOrEmpty(CantidadPersonasTXT.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!NumeroCamasTXT.Text.All(char.IsDigit))
            {
                MessageBox.Show("El número de camas debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!PrecioTXT.Text.All(c => char.IsDigit(c) || c == '.') || PrecioTXT.Text.Count(c => c == '.') > 1)
            {
                MessageBox.Show("El precio debe contener solo números y un punto decimal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(PrecioTXT.Text) < 800)
            {
                MessageBox.Show("El precio no puede ser menor a 800", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(NumeroCamasTXT.Text) < 1 || Convert.ToInt32(NumeroCamasTXT.Text) > 5)
            {
                MessageBox.Show("El número de camas no puede ser menor a 1 o mayor a 5", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CheckJardinTXT.Checked && CheckPiscinaTXT.Checked && CheckPlayaTXT.Checked)
            {
                MessageBox.Show("Seleccione solo una opción donde estara mirando la habitacion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Se guardan los campos de texto en variables
            var nivelHabitacion = NivelHabitacionTXT.Text;
            var tipoCama = TipoCamaTXT.Text;
            var numeroCamas = Convert.ToInt32(NumeroCamasTXT.Text);
            var precio = Convert.ToDouble(PrecioTXT.Text);
            var numeroPersonas = Convert.ToInt32(CantidadPersonasTXT.Text);
            var frenteA = string.Empty;
            if (CheckJardinTXT.Checked) frenteA = "Jardin";
            if (CheckPiscinaTXT.Checked) frenteA = "Piscina";
            if (CheckPlayaTXT.Checked) frenteA = "Playa";
            var Id_Hotel = Convert.ToInt32(HotelTHCB.SelectedValue);

            // Usando SQL
            if (Login.baseDatos == 1)
            {
                tipoHab.nivelHabitacion = nivelHabitacion;
                tipoHab.numeroCamas = numeroCamas;
                tipoHab.tipoCama = tipoCama;
                tipoHab.precio = precio;
                tipoHab.numeroPersonas = numeroPersonas;
                tipoHab.frenteA = frenteA;
                tipoHab.Id_Hotel = Id_Hotel;
                EnlaceDB enlace = new EnlaceDB();
                int Id_Generado;
                if ((enlace.Insertar_TipoHab(tipoHab, out Id_Generado)) == false)
                {
                    MessageBox.Show("Error al registrar el tipo de habitacion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    TablaTipoHabitacion.DataSource = enlace.consultarTipoHabitacion();
                    foreach (var item in AmenidadesCB.CheckedItems)
                    {
                        if (item is Amenidad amenidadSeleccionada)
                        {
                            enlace.Insertar_TipoHab_Amenidad(amenidadSeleccionada, Id_Generado);
                        }
                    }
                    foreach (var item in CaracteristicasCB.CheckedItems)
                    {
                        if (item is Caracteristica caracteristicaSeleccionada)
                        {
                            enlace.Insertar_TipoHab_Caracteristica(caracteristicaSeleccionada, Id_Generado);
                        }
                    }
                    CargarTablasTipoHab();
                }
            }
            else if (Login.baseDatos == 2)
            {
                // Implementar la lógica para la base de datos CQL
            }
            NivelHabitacionTXT.Text = "";
            TipoCamaTXT.Text = "";
            NumeroCamasTXT.Text = "";
            PrecioTXT.Text = "";
            CantidadPersonasTXT.Text = "";
            if (CheckJardinTXT.Checked) CheckJardinTXT.Checked = false;
            if (CheckPiscinaTXT.Checked) CheckPiscinaTXT.Checked = false;
            if (CheckPlayaTXT.Checked) CheckPlayaTXT.Checked = false;
            AmenidadesTXT.Text = "";
            CaracteristicasTXT.Text = "";
            MessageBox.Show("Tipo de habitacion registrado correctamente: Nivel " + tipoHab.nivelHabitacion, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void AgregarAmenidadBTN7_Click(object sender, EventArgs e)
        {
            // Validaciones para registrar la amenidad
            Amenidad amenidad = new Amenidad();
            if (string.IsNullOrEmpty(CampoAmenidadTXT.Text))
            {
                MessageBox.Show("Error al registrar la amenidad, no debe de estar vacio el campo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Guardamos el campo de texto en una variable
            var nombreAmenidad = CampoAmenidadTXT.Text;
            // Usando SQL
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                amenidad.nombreAmenidad = nombreAmenidad;
                int Id_Generado;
                if ((enlace.Insertar_Amenidad(amenidad, out Id_Generado)) == false)
                {
                    MessageBox.Show("Error al registrar la amenidad", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CargarTablasTipoHab();
            }
            else if (Login.baseDatos == 2)
            {
                // Implementar la lógica para la base de datos CQL
            }
            MessageBox.Show("Amenidad registrada correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CampoAmenidadTXT.Clear();
        }
        private void AgregarCaracteristicaBTN_Click(object sender, EventArgs e)
        {
            // Validaciones para registrar la caracteristica
            Caracteristica caracteristica = new Caracteristica();
            if (string.IsNullOrEmpty(CampoCaracteristicasTXT.Text))
            {
                MessageBox.Show("Error al registrar la caracteristica, no debe de estar vacio el campo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Guardamos el campo de texto en una variable
            var nombreCaracteristica = CampoCaracteristicasTXT.Text;
            // Usando SQL
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                caracteristica.nombreCaracteristica = nombreCaracteristica;
                int Id_Generado;
                if ((enlace.Insertar_Caracteristica(caracteristica, out Id_Generado)) == false)
                {
                    MessageBox.Show("Error al registrar la caracteristica", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CargarTablasTipoHab();
            }
            MessageBox.Show("Caracteristica registrada correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CampoCaracteristicasTXT.Clear();
        }
        private void AgregarBTN_Click(object sender, EventArgs e)
        {
            ServicioAdicional servicioAdicional = new ServicioAdicional();
            if (string.IsNullOrEmpty(ServicioTXT.Text) || string.IsNullOrEmpty(CostoServicioTXT.Text) || !CostoServicioTXT.Text.All(char.IsDigit))
            {
                MessageBox.Show("Error al registrar el servicio adicional, no debe de estar vacios los campos o el precio debe de contener solo digitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Guardamos el campo de texto en una variable
            var nombreServicio = ServicioTXT.Text;
            var costo = Convert.ToInt32(CostoServicioTXT.Text);
            // Usando SQL
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                servicioAdicional.nombreServicio = nombreServicio;
                servicioAdicional.costo = costo;
                bool resultado = enlace.Insertar_ServicioAdicional(servicioAdicional);
                if (!resultado)
                {
                    MessageBox.Show("Error al registrar el servicio adicional", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CargarTablasTipoHab();
            }
            MessageBox.Show("Servicio Adicional registrado correctamente: " + servicioAdicional.nombreServicio, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ServicioTXT.Clear();
            CostoServicioTXT.Clear();
        }
        private void BTN_RegistrarHotel_Click(object sender, EventArgs e)
        {
            // Validaciones para registrar el hotel
            Hotel hotel = new Hotel();
            ServicioAdicional servicio = new ServicioAdicional();
            if (string.IsNullOrEmpty(NombreHotelTXT.Text) || string.IsNullOrEmpty(ZonaTuristicaTXT.Text) || string.IsNullOrEmpty(NumeroPisosTXT.Text) || string.IsNullOrEmpty(NumeroHabitacionesTXT.Text) || string.IsNullOrEmpty(NumeroPiscinasTXT.Text) || string.IsNullOrEmpty(DomicilioTXT.Text) || string.IsNullOrEmpty(PaisCB.Text) || string.IsNullOrEmpty(EstadoCB.Text) || string.IsNullOrEmpty(CiudadCB.Text) || string.IsNullOrEmpty(FechaOperacionDTP.Text) || string.IsNullOrEmpty(FechaRegistroDTP.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!NumeroHabitacionesTXT.Text.All(char.IsDigit) || !NumeroPisosTXT.Text.All(char.IsDigit) || !NumeroPiscinasTXT.Text.All(char.IsDigit))
            {
                MessageBox.Show("El número de pisos, piscinas o habitaciones deben contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PlayaCH.Checked && NoPlayaCH.Checked)
            {
                MessageBox.Show("Seleccione solo una opción para el frente de playa.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (siSalon.Checked && noSalon.Checked)
            {
                MessageBox.Show("Seleccione solo una opción para el salón de eventos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(NumeroHabitacionesTXT.Text) < 0 || Convert.ToInt32(NumeroPisosTXT.Text) < 0 || Convert.ToInt32(NumeroPiscinasTXT.Text) < 0)
            {
                MessageBox.Show("El número de pisos, habitaciones o piscinas no puede ser menor a 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var nombreHotel = NombreHotelTXT.Text;
            var zonaTuristica = ZonaTuristicaTXT.Text;
            var numeroPisos = Convert.ToInt32(NumeroPisosTXT.Text);
            var fechaOperacion = DateTime.Parse(FechaOperacionDTP.Text);
            var fechaRegistro = DateTime.Parse(FechaRegistroDTP.Text);
            var numeroPiscinas = Convert.ToInt32(NumeroPiscinasTXT.Text);
            var numeroHabitaciones = Convert.ToInt32(NumeroHabitacionesTXT.Text);
            var pais = PaisCB.Text;
            var estado = EstadoCB.Text;
            var ciudad = CiudadCB.Text;
            var domicilio = DomicilioTXT.Text;
            var Playa = 0;
            var SalonEventos = 0;
            if (PlayaCH.Checked) Playa = 1;
            if (NoPlayaCH.Checked) Playa = 0;
            if (siSalon.Checked) SalonEventos = 1;
            if (noSalon.Checked) SalonEventos = 0;
            // Usando SQL
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarUsuarioLogeado(Login.IdUsuarioActual);
                hotel.Id_Usuario = Convert.ToInt32(tabla.Rows[0]["Id_Usuario"]);
                hotel.NombreHotel = nombreHotel;
                hotel.ZonaTuristica = zonaTuristica;
                hotel.NumPisos = numeroPisos;
                hotel.FechaOperacion = fechaOperacion;
                hotel.FechaRegistro = fechaRegistro;
                hotel.NumPiscinas = numeroPiscinas;
                hotel.NumHabitaciones = numeroHabitaciones;
                hotel.Pais = pais;
                hotel.Estado = estado;
                hotel.Ciudad = ciudad;
                hotel.Domicilio = domicilio;
                hotel.FrentePlaya = Playa;
                hotel.SalonEventos = SalonEventos;
                // Sacar el Id de la ubicacion, y si se repite guardar la que ya esta
                var tablaUbicacion = new DataTable();
                tablaUbicacion = enlace.ValidarUbicacionUnica(hotel.Pais, hotel.Estado, hotel.Ciudad);
                hotel.Id_Ubicacion = tablaUbicacion.Rows.Count < 1 ? 0 : Convert.ToInt32(tablaUbicacion.Rows[0]["Id_Ubicacion"]);
                int Id_Generado;
                if ((enlace.Insertar_Hotel(hotel, out Id_Generado)) == false)
                {
                    MessageBox.Show("Error al registrar el hotel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    foreach (var item in ServicioAdicionalCB.CheckedItems)
                    {
                        if (item is ServicioAdicional servicioSeleccionado)
                        {
                            enlace.Insertar_Hotel_ServicioAdicional(servicioSeleccionado, Id_Generado);
                        }
                    }
                }
                CargarTablasTipoHab();
            }
            else if (Login.baseDatos == 2)
            {
                // Implementar la lógica para la base de datos CQL
            }
            MessageBox.Show("Se registro correctamente el hotel: " + hotel.NombreHotel, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void EliminarAmenidadBTN_Click(object sender, EventArgs e)
        {

        }
        // --------------------------------------------------- Para rellenar el textbox con cada seleccion -------------------------------------------------
        private void AmenidadesCB_ItemCheck(object sender, EventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in AmenidadesCB.CheckedItems)
                {
                    if (item is Amenidad amenidad)
                        sb.Append(amenidad.nombreAmenidad + ", ");
                }

                AmenidadesTXT.Text = sb.ToString().TrimEnd(' ', ',');
            }));
        }
        private void CaracteristicasCB_ItemCheck(object sender, EventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in CaracteristicasCB.CheckedItems)
                {
                    if (item is Caracteristica caracteristica)
                        sb.Append(caracteristica.nombreCaracteristica + ", ");
                }
                CaracteristicasTXT.Text = sb.ToString().TrimEnd(' ', ',');
            }));
        }
        private void ServicioAdicionalCB_ItemCheck(object sender, EventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in ServicioAdicionalCB.CheckedItems)
                {
                    if (item is ServicioAdicional servicioAdicional)
                        sb.Append(servicioAdicional.nombreServicio + ", ");
                }
                ServiciosTXT.Text = sb.ToString().TrimEnd(' ', ',');
            }));
        }
        private void listaHabitaciones_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Solo permitir un elemento marcado a la vez
            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < listaHabitaciones.Items.Count; i++)
                {
                    if (i != e.Index && listaHabitaciones.GetItemChecked(i))
                    {
                        listaHabitaciones.SetItemChecked(i, false);
                    }
                }
            }
            // Actualizar el TextBox después del cambio de estado
            BeginInvoke((Action)(() =>
            {
                var item = listaHabitaciones.Items[e.Index];
                if (item is TipoHab tipoHab && listaHabitaciones.GetItemChecked(e.Index))
                {
                    NivelHabitacionHotelTXT.Text = tipoHab.nivelHabitacion;
                }
                else
                {
                    NivelHabitacionHotelTXT.Clear();
                }
            }));
        }
        // --------------------------------------------------- Carga de datos -------------------------------------------------

        private void HotelCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccion = HotelCB.SelectedValue != null ? Convert.ToInt32(HotelCB.SelectedValue) : -1;
            if (seleccion <= 0)
            {
                MessageBox.Show("Seleccione un hotel válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                var tablaRestante = new DataTable();
                tabla = enlace.consultarHotelXId(seleccion);
                tablaRestante = enlace.consultarHabitacionesRestantes(seleccion);
                HabitacionesTotalesTXT.Text = tabla.Rows[0]["numHabitaciones"].ToString();
                int habTotales = Convert.ToInt32(HabitacionesTotalesTXT.Text);
                int habRegistrada = Convert.ToInt32(tablaRestante.Rows[0]["Restantes"].ToString());
                int habRestante = habTotales - habRegistrada;
                HabitacionesRestantesTXT.Text = habRestante.ToString();
                var tablaHabitaciones = enlace.consultarHabitaciones(seleccion);
                TablaTHAsignadasDTG.DataSource = tablaHabitaciones;
                var tablaTipoHabitacion = enlace.consultarTipoHabitacionId(seleccion);
                listaHabitaciones.Items.Clear();
                foreach (DataRow fila in tablaTipoHabitacion.Rows)
                {
                    TipoHab tipoHab = new TipoHab()
                    {
                        Id_TipoHab = Convert.ToInt32(fila["Id_TipoHab"]),
                        nivelHabitacion = fila["nivelHabitacion"].ToString()
                    };
                    listaHabitaciones.Items.Add(tipoHab);
                }
            }
        }
        private void BTN_AsignarHabitacion_Click(object sender, EventArgs e)
        {
            // Validaciones para registrar la habitacion
            Habitacion habitacion = new Habitacion();
            if (!HabitacionesAsignadasTXT.Text.All(char.IsDigit) || string.IsNullOrEmpty(HabitacionesAsignadasTXT.Text) || Convert.ToInt32(HabitacionesAsignadasTXT.Text) < 0)
            {
                MessageBox.Show("El número de habitaciones asignadas debe contener solo números mayores a 0 y no debe estar vacio el campo.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!(listaHabitaciones.CheckedItems.Count == 1))
            {
                MessageBox.Show("Debe seleccionar solo un tipo de habitación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (((listaHabitaciones.CheckedItems[0] as TipoHab) != null) == false)
            {
                MessageBox.Show("Error al obtener el tipo de habitación seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var NumeroHabitacion = Convert.ToInt32(HabitacionesAsignadasTXT.Text);
            var tipoSeleccionado = listaHabitaciones.CheckedItems[0] as TipoHab;
            var Id_Hotel = Convert.ToInt32(HotelCB.SelectedValue);

            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                habitacion.NumeroHabitacion = NumeroHabitacion;
                habitacion.Id_TipoHab = tipoSeleccionado.Id_TipoHab;
                habitacion.Id_Hotel = Id_Hotel;
                tabla = enlace.consultarPisosXHotel(habitacion.Id_Hotel);
                if (tabla.Rows.Count > 0)
                {
                    habitacion.Piso = Convert.ToInt32(tabla.Rows[0]["numeroPisos"]);
                }
                int restantes = Convert.ToInt32(HabitacionesRestantesTXT.Text);
                if (habitacion.NumeroHabitacion > restantes)
                {
                    MessageBox.Show("Ya no quedan habitaciones para asignar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                bool resultado = enlace.SP_InsertarHabitaciones(habitacion);
                if (!resultado)
                {
                    MessageBox.Show("Error al registrar habitaciones.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CargarTablasTipoHab();
            }
            MessageBox.Show("Habitacion registrada correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void CargarTablasTipoHab()
        {

            var enlace = new EnlaceDB();
            var tablaUsuarioActual = enlace.ConsultarBarraUsuario();
            UsuarioActualTXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();
            UsuarioActual1TXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();
            UsuarioActual2TXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();
            var tabla = new DataTable();
            tabla = enlace.consultarTipoHabitacion();
            var tablaHotel = new DataTable();
            tablaHotel = enlace.consultarHotel();
            HotelCB.DisplayMember = "nombreHotel";
            HotelCB.ValueMember = "Id_Hotel";
            int Id_Hotel = Convert.ToInt32(HotelCB.SelectedValue);
            HotelCB.DataSource = tablaHotel;
            HotelTHCB.DisplayMember = "nombreHotel";
            HotelTHCB.ValueMember = "Id_Hotel";
            HotelTHCB.DataSource = tablaHotel;
            // Cargar tabla
            int Id = Convert.ToInt32(HotelCB.SelectedValue);
            var tablaHabitaciones = enlace.consultarHabitaciones(Id);
            var tablaRestantes = enlace.consultarHabitacionesRestantes(Id);
            if (tabla.Rows.Count > 0)
            {
                // Cargar tabla
                TablaTipoHabitacion.DataSource = tabla;
                // Visibles
                TablaTipoHabitacion.Columns["Id_TipoHab"].Visible = false;
                // Nombre de la columna
                TablaTipoHabitacion.Columns["nivelHabitacion"].HeaderText = "Nivel Habitacion";
                TablaTipoHabitacion.Columns["numeroCamas"].HeaderText = "# de Camas";
                TablaTipoHabitacion.Columns["tipoCama"].HeaderText = "Tipo de Cama";
                TablaTipoHabitacion.Columns["precio"].HeaderText = "Precio";
                TablaTipoHabitacion.Columns["numeroPersonas"].HeaderText = "# de Personas";
                TablaTipoHabitacion.Columns["frenteA"].HeaderText = "Frente a";

                TablaTipoHabitacion.Columns["Id_TipoHab"].DisplayIndex = 0;
                TablaTipoHabitacion.Columns["nivelHabitacion"].DisplayIndex = 1;
                TablaTipoHabitacion.Columns["numeroCamas"].DisplayIndex = 2;
                TablaTipoHabitacion.Columns["tipoCama"].DisplayIndex = 3;
                TablaTipoHabitacion.Columns["precio"].DisplayIndex = 4;
                TablaTipoHabitacion.Columns["numeroPersonas"].DisplayIndex = 5;
                TablaTipoHabitacion.Columns["frenteA"].DisplayIndex = 6;
                TablaTipoHabitacion.Columns["precio"].DefaultCellStyle.Format = "C2";
                // Cargar tabla
                TablaTipoHabDTG.DataSource = tabla;
                // Visibles
                TablaTipoHabDTG.Columns["Id_TipoHab"].Visible = false;
                // Nombre de la columna
                TablaTipoHabDTG.Columns["nivelHabitacion"].HeaderText = "Nivel Habitacion";
                TablaTipoHabDTG.Columns["numeroCamas"].HeaderText = "# de Camas";
                TablaTipoHabDTG.Columns["tipoCama"].HeaderText = "Tipo de Cama";
                TablaTipoHabDTG.Columns["precio"].HeaderText = "Precio";
                TablaTipoHabDTG.Columns["numeroPersonas"].HeaderText = "# de Personas";
                TablaTipoHabDTG.Columns["frenteA"].HeaderText = "Frente a";
                // Orden de la tabla
                TablaTipoHabDTG.Columns["Id_TipoHab"].DisplayIndex = 0;
                TablaTipoHabDTG.Columns["nivelHabitacion"].DisplayIndex = 1;
                TablaTipoHabDTG.Columns["numeroCamas"].DisplayIndex = 2;
                TablaTipoHabDTG.Columns["tipoCama"].DisplayIndex = 3;
                TablaTipoHabDTG.Columns["precio"].DisplayIndex = 4;
                TablaTipoHabDTG.Columns["numeroPersonas"].DisplayIndex = 5;
                TablaTipoHabDTG.Columns["frenteA"].DisplayIndex = 6;
                TablaTipoHabDTG.Columns["precio"].DefaultCellStyle.Format = "C2";
            }
            if (tablaHotel.Rows.Count > 0)
            {
                // Cargar tabla
                HotelesRegistradosDTG.DataSource = tablaHotel;
                // Visibles
                HotelesRegistradosDTG.Columns["Id_Hotel"].Visible = false;
                HotelesRegistradosDTG.Columns["Id_Usuario"].Visible = false;
                HotelesRegistradosDTG.Columns["Id_Ubicacion"].Visible = false;
                HotelesRegistradosDTG.Columns["fechaModificacionHotel"].Visible = false;
                // Nombre de la columna
                HotelesRegistradosDTG.Columns["nombreHotel"].HeaderText = "Nombre Hotel";
                HotelesRegistradosDTG.Columns["zonaTuristica"].HeaderText = "Zona Turistica";
                HotelesRegistradosDTG.Columns["numeroPisos"].HeaderText = "# de Pisos";
                HotelesRegistradosDTG.Columns["fechaOperacion"].HeaderText = "Fecha Operacion";
                HotelesRegistradosDTG.Columns["numeroPiscinas"].HeaderText = "# de Piscinas";
                HotelesRegistradosDTG.Columns["SalonEventos"].HeaderText = "Salon Eventos";
                HotelesRegistradosDTG.Columns["FrentePlaya"].HeaderText = "Frente a la Playa";
                HotelesRegistradosDTG.Columns["numHabitaciones"].HeaderText = "# de Habitaciones";
                HotelesRegistradosDTG.Columns["fechaRegistroHotel"].HeaderText = "Fecha de registro";
                HotelesRegistradosDTG.Columns["Estatus"].HeaderText = "Estatus";
                // Orden de la tabla
                HotelesRegistradosDTG.Columns["nombreHotel"].DisplayIndex = 1;
                HotelesRegistradosDTG.Columns["zonaTuristica"].DisplayIndex = 2;
                HotelesRegistradosDTG.Columns["numeroPisos"].DisplayIndex = 3;
                HotelesRegistradosDTG.Columns["fechaOperacion"].DisplayIndex = 4;
                HotelesRegistradosDTG.Columns["numeroPiscinas"].DisplayIndex = 5;
                HotelesRegistradosDTG.Columns["SalonEventos"].DisplayIndex = 6;
                HotelesRegistradosDTG.Columns["FrentePlaya"].DisplayIndex = 7;
                HotelesRegistradosDTG.Columns["numHabitaciones"].DisplayIndex = 8;
                HotelesRegistradosDTG.Columns["fechaRegistroHotel"].DisplayIndex = 9;
                HotelesRegistradosDTG.Columns["Estatus"].DisplayIndex = 10;
                // Cargar tabla
                TablaHotelDTG.DataSource = tablaHotel;
                // Visibles
                TablaHotelDTG.Columns["Id_Hotel"].Visible = false;
                TablaHotelDTG.Columns["Id_Usuario"].Visible = false;
                TablaHotelDTG.Columns["Id_Ubicacion"].Visible = false;
                TablaHotelDTG.Columns["fechaModificacionHotel"].Visible = false;
                // Nombre de la columna
                TablaHotelDTG.Columns["nombreHotel"].HeaderText = "Nombre Hotel";
                TablaHotelDTG.Columns["zonaTuristica"].HeaderText = "Zona Turistica";
                TablaHotelDTG.Columns["numeroPisos"].HeaderText = "# de Pisos";
                TablaHotelDTG.Columns["fechaOperacion"].HeaderText = "Fecha Operacion";
                TablaHotelDTG.Columns["numeroPiscinas"].HeaderText = "# de Piscinas";
                TablaHotelDTG.Columns["SalonEventos"].HeaderText = "Salon Eventos";
                TablaHotelDTG.Columns["FrentePlaya"].HeaderText = "Frente a la Playa";
                TablaHotelDTG.Columns["numHabitaciones"].HeaderText = "# de Habitaciones";
                TablaHotelDTG.Columns["fechaRegistroHotel"].HeaderText = "Fecha de registro";
                TablaHotelDTG.Columns["Estatus"].HeaderText = "Estatus";
                // Orden de la tabla
                TablaHotelDTG.Columns["nombreHotel"].DisplayIndex = 1;
                TablaHotelDTG.Columns["zonaTuristica"].DisplayIndex = 2;
                TablaHotelDTG.Columns["numeroPisos"].DisplayIndex = 3;
                TablaHotelDTG.Columns["fechaOperacion"].DisplayIndex = 4;
                TablaHotelDTG.Columns["numeroPiscinas"].DisplayIndex = 5;
                TablaHotelDTG.Columns["SalonEventos"].DisplayIndex = 6;
                TablaHotelDTG.Columns["FrentePlaya"].DisplayIndex = 7;
                TablaHotelDTG.Columns["numHabitaciones"].DisplayIndex = 8;
                TablaHotelDTG.Columns["fechaRegistroHotel"].DisplayIndex = 9;
                TablaHotelDTG.Columns["estatus"].DisplayIndex = 10;

            }
            if (tablaHabitaciones.Rows.Count > 0)
            {
                TablaTHAsignadasDTG.DataSource = tablaHabitaciones;
            }
            if (tablaRestantes.Rows.Count > 0)
            {
                // Validar primero el valor en la tabla
                int habRegistradas = 0;
                if (!int.TryParse(tablaRestantes.Rows[0]["Restantes"]?.ToString(), out habRegistradas)) habRegistradas = 0;
                // Validar el textbox
                int habTotales = 0;
                if (!int.TryParse(HabitacionesTotalesTXT.Text, out habTotales)) habTotales = 0;
                int restantes = habTotales - habRegistradas;
                if (restantes > 0) HabitacionesRestantesTXT.Text = restantes.ToString();
                else HabitacionesRestantesTXT.Text = "0";
            }
            else HabitacionesRestantesTXT.Text = HabitacionesTotalesTXT.Text;

            var tablaAmenidad = enlace.consultarAmenidad();
            AmenidadesCB.Items.Clear();
            foreach (DataRow fila in tablaAmenidad.Rows)
            {
                Amenidad amenidad = new Amenidad()
                {
                    Id_Amenidad = Convert.ToInt32(fila["Id_Amenidad"]),
                    nombreAmenidad = fila["nombreAmenidad"].ToString()
                };
                AmenidadesCB.Items.Add(amenidad);
            }
            var tablaCaracteristica = enlace.consultarCaracteristica();
            CaracteristicasCB.Items.Clear();
            foreach (DataRow fila in tablaCaracteristica.Rows)
            {
                Caracteristica caracteristica = new Caracteristica()
                {
                    Id_Caracteristica = Convert.ToInt32(fila["Id_Caracteristica"]),
                    nombreCaracteristica = fila["nombreCaracteristica"].ToString()
                };
                CaracteristicasCB.Items.Add(caracteristica);
            }
            var tablaServicioAdicional = enlace.consultarServicioAdicional();
            ServicioAdicionalCB.Items.Clear();
            foreach (DataRow fila in tablaServicioAdicional.Rows)
            {
                ServicioAdicional servicioAdicional = new ServicioAdicional()
                {
                    Id_ServicioAdicional = Convert.ToInt32(fila["Id_ServicioAdicional"]),
                    nombreServicio = fila["nombreServicio"].ToString()
                };
                ServicioAdicionalCB.Items.Add(servicioAdicional);
            }
            var tablaTipoHabitacion = enlace.consultarTipoHabitacionId(Id_Hotel);
            listaHabitaciones.Items.Clear();
            foreach (DataRow fila in tablaTipoHabitacion.Rows)
            {
                TipoHab tipoHab = new TipoHab()
                {
                    Id_TipoHab = Convert.ToInt32(fila["Id_TipoHab"]),
                    nivelHabitacion = fila["nivelHabitacion"].ToString()
                };
                listaHabitaciones.Items.Add(tipoHab);
            }
            var tablaUbicacion = new DataTable();
            tablaUbicacion = enlace.ConsultarUbicacion();
            var paises = tablaUbicacion.AsEnumerable()
                .GroupBy(row => row.Field<string>("pais"))
                .Select(group => new
                {
                    pais = group.Key,
                    Id_Ubicacion = group.First().Field<int>("Id_Ubicacion")
                })
                .ToList();
            var estados = tablaUbicacion.AsEnumerable()
                .GroupBy(row => row.Field<string>("estado"))
                .Select(group => new
                {
                    estado = group.Key,
                    Id_Ubicacion = group.First().Field<int>("Id_Ubicacion")
                })
                .ToList();
            var ciudades = tablaUbicacion.AsEnumerable()
                .GroupBy(row => row.Field<string>("ciudad"))
                .Select(group => new
                {
                    ciudad = group.Key,
                    Id_Ubicacion = group.First().Field<int>("Id_Ubicacion")
                })
                .ToList();
            PaisCB.DataSource = paises;
            PaisCB.DisplayMember = "pais";
            PaisCB.ValueMember = "Id_Ubicacion";
            EstadoCB.DataSource = estados;
            EstadoCB.DisplayMember = "estado";
            EstadoCB.ValueMember = "Id_Ubicacion";
            CiudadCB.DataSource = ciudades;
            CiudadCB.ValueMember = "Id_Ubicacion";
            CiudadCB.DisplayMember = "ciudad";
        }

        // ---------------------------------------------------------------------------------------------------------------------
    }
}
