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
            public string nivelHabitacion { get; set; }
            public int numeroCamas { get; set; }
            public string tipoCama { get; set; }
            public double precio { get; set; }
            public int numeroPersonas { get; set; }
            public string frenteA { get; set; }
        }
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
        public class Amenidad
        {
            public int Id_Amenidad { get; set; }
            public string nombreAmenidad { get; set; }
            public override string ToString()
            {
                return nombreAmenidad;
            }
        }
        public class Caracteristica
        {
            public int Id_Caracteristica { get; set; }
            public string nombreCaracteristica { get; set; }

            public override string ToString()
            {
                return nombreCaracteristica;
            }
        }
        public class ServicioAdicional
        {
            public int Id_ServicioAdicional { get; set; }
            public string nombreServicio { get; set; }

            public override string ToString()
            {
                return nombreServicio;
            }
        }
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
            CargarTablasTipoHab();
            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;
        }
        private void BTN_Registrar_Click(object sender, EventArgs e)
        {
            TipoHab tipoHab = new TipoHab();
            Amenidad amenidad = new Amenidad();
            Caracteristica caracteristica = new Caracteristica();
            tipoHab.nivelHabitacion = NivelHabitacionTXT.Text;
            if (!NumeroCamasTXT.Text.All(char.IsDigit))
            {
                MessageBox.Show("El número de camas debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!PrecioTXT.Text.All(c => char.IsDigit(c) || c == '.'))
            {
                MessageBox.Show("El precio debe contener solo números y un punto decimal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PrecioTXT.Text.Count(c => c == '.') > 1)
            {
                MessageBox.Show("El precio solo puede contener un punto decimal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (tipoHab.precio < 800)
            {
                MessageBox.Show("El precio no puede ser menor a 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            tipoHab.numeroCamas = Convert.ToInt32(NumeroCamasTXT.Text);
            if (tipoHab.numeroCamas < 1 || tipoHab.numeroCamas > 5)
            {
                MessageBox.Show("El número de camas no puede ser menor a 1 o mayor a 5", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            tipoHab.tipoCama = TipoCamaTXT.Text;
            tipoHab.precio = Convert.ToDouble(PrecioTXT.Text);
            tipoHab.numeroPersonas = Convert.ToInt32(CantidadPersonasTXT.Text);
            if (CheckJardinTXT.Checked)
            {
                tipoHab.frenteA = "Jardin";
                CheckPiscinaTXT.Checked = false;
                CheckPlayaTXT.Checked = false;
            }
            else if (CheckPiscinaTXT.Checked)
            {
                tipoHab.frenteA = "Piscina";
                CheckJardinTXT.Checked = false;
                CheckPlayaTXT.Checked = false;
            }
            else if (CheckPlayaTXT.Checked)
            {
                tipoHab.frenteA = "Playa";
                CheckJardinTXT.Checked = false;
                CheckPiscinaTXT.Checked = false;
            }
            else
            {
                MessageBox.Show("Seleccione una opción de frente a donde estara la habitacion", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
                MessageBox.Show("Tipo de habitacion registrado correctamente: Nivel " + tipoHab.nivelHabitacion, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarTablasTipoHab();
            }
        }
        private void AgregarAmenidadBTN7_Click(object sender, EventArgs e)
        {
            var enlace = new EnlaceDB();
            Amenidad amenidad = new Amenidad();
            amenidad.nombreAmenidad = CampoAmenidadTXT.Text;
            int Id_Generado;
            if ((enlace.Insertar_Amenidad(amenidad, out Id_Generado)) == false)
            {
                MessageBox.Show("Error al registrar la amenidad", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Amenidad registrada correctamente: " + Id_Generado, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CampoAmenidadTXT.Clear();
            }
            CargarTablasTipoHab();
        }
        private void AgregarCaracteristicaBTN_Click(object sender, EventArgs e)
        {
            var enlace = new EnlaceDB();
            Caracteristica caracteristica = new Caracteristica();
            caracteristica.nombreCaracteristica = CampoCaracteristicasTXT.Text;
            int Id_Generado;
            if ((enlace.Insertar_Caracteristica(caracteristica, out Id_Generado)) == false)
            {
                MessageBox.Show("Error al registrar la caracteristica", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Caracteristica registrada correctamente: " + Id_Generado, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CampoCaracteristicasTXT.Clear();
            }
            CargarTablasTipoHab();
        }
        private void AgregarBTN_Click(object sender, EventArgs e)
        {
            var enlace = new EnlaceDB();
            ServicioAdicional servicioAdicional = new ServicioAdicional();
            servicioAdicional.nombreServicio = ServicioTXT.Text;
            int Id_Generado;
            bool resultado = enlace.Insertar_ServicioAdicional(servicioAdicional, out Id_Generado);
            if (!resultado)
            {
                MessageBox.Show("Error al registrar el servicio adicional", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Servicio Adicional registrado correctamente: " + Id_Generado, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ServicioTXT.Clear();
            }
            CargarTablasTipoHab();

        }
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
        private void CargarTablasTipoHab()
        {

            var enlace = new EnlaceDB();
            var tablaUsuarioActual = enlace.ConsultarBarraUsuario();
            UsuarioActualTXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();
            UsuarioActual1TXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();
            UsuarioActual2TXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();
            var tabla = new DataTable();
            tabla = enlace.consultarTipoHabitacion();
            TipoHabCB.DisplayMember = "nivelHabitacion";
            TipoHabCB.ValueMember = "Id_TipoHab";
            TipoHabCB.DataSource = tabla;

            if (tabla.Rows.Count > 0)
            {
                TablaTipoHabitacion.DataSource = tabla;
                TablaTipoHabitacion.Columns["Id_TipoHab"].Visible = false;
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

                TablaTipoHabDTG.DataSource = tabla;
                TablaTipoHabDTG.Columns["Id_TipoHab"].Visible = false;
                TablaTipoHabDTG.Columns["nivelHabitacion"].HeaderText = "Nivel Habitacion";
                TablaTipoHabDTG.Columns["numeroCamas"].HeaderText = "# de Camas";
                TablaTipoHabDTG.Columns["tipoCama"].HeaderText = "Tipo de Cama";
                TablaTipoHabDTG.Columns["precio"].HeaderText = "Precio";
                TablaTipoHabDTG.Columns["numeroPersonas"].HeaderText = "# de Personas";
                TablaTipoHabDTG.Columns["frenteA"].HeaderText = "Frente a";

                TablaTipoHabDTG.Columns["Id_TipoHab"].DisplayIndex = 0;
                TablaTipoHabDTG.Columns["nivelHabitacion"].DisplayIndex = 1;
                TablaTipoHabDTG.Columns["numeroCamas"].DisplayIndex = 2;
                TablaTipoHabDTG.Columns["tipoCama"].DisplayIndex = 3;
                TablaTipoHabDTG.Columns["precio"].DisplayIndex = 4;
                TablaTipoHabDTG.Columns["numeroPersonas"].DisplayIndex = 5;
                TablaTipoHabDTG.Columns["frenteA"].DisplayIndex = 6;

                TablaTipoHabDTG.Columns["precio"].DefaultCellStyle.Format = "C2";
            }

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

        private void BTN_RegistrarHotel_Click(object sender, EventArgs e)
        {
            var enlace = new EnlaceDB();
            Hotel hotel = new Hotel();
            ServicioAdicional servicio = new ServicioAdicional();
            // Sacar el Id de la ubicacion, y si se repite guardar la que ya esta
            var tablaUbicacion = new DataTable();
            tablaUbicacion = enlace.ValidarUbicacionUnica(hotel.Pais, hotel.Estado, hotel.Ciudad);
            hotel.Id_Ubicacion = tablaUbicacion.Rows.Count < 1 ? 0 : Convert.ToInt32(tablaUbicacion.Rows[0]["Id_Ubicacion"]); 
            var tabla = new DataTable();
            tabla = enlace.consultarUsuarioLogeado(Login.IdUsuarioActual);
            hotel.Id_Usuario = Convert.ToInt32(tabla.Rows[0]["Id_Usuario"]);
            hotel.NombreHotel = NombreHotelTXT.Text;
            hotel.ZonaTuristica = ZonaTuristicaTXT.Text;
            hotel.NumPisos = Convert.ToInt32(NumeroPisosTXT.Text);
            hotel.FechaOperacion = DateTime.Parse(FechaOperacionDTP.Text);
            hotel.NumPiscinas = Convert.ToInt32(NumeroPiscinasTXT.Text);
            hotel.NumHabitaciones = Convert.ToInt32(NumeroHabitacionesTXT.Text);
            if((!PlayaCH.Checked && !NoPlayaCH.Checked) || (PlayaCH.Checked && NoPlayaCH.Checked) 
                || (!siSalon.Checked && !noSalon.Checked) || (siSalon.Checked && noSalon.Checked))
            {
                MessageBox.Show("Debe seleccionar una de las opciones disponibles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (PlayaCH.Checked) hotel.FrentePlaya = 1;
            if (NoPlayaCH.Checked) hotel.FrentePlaya = 0;
            if (siSalon.Checked) hotel.SalonEventos = 1;
            if (noSalon.Checked) hotel.SalonEventos = 0;
            int Id_Generado;
           ;
            if ((enlace.Insertar_Hotel(hotel, out Id_Generado)) == false)
            {
                MessageBox.Show("Error al registrar el hotel.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                foreach(var item in ServicioAdicionalCB.CheckedItems)
                {
                    if (item is ServicioAdicional servicioSeleccionado)
                    {
                        enlace.Insertar_Hotel_ServicioAdicional(servicioSeleccionado, Id_Generado);
                    }
                }
            }
            MessageBox.Show("Se registro correctamente el hotel: " + hotel.NombreHotel, "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarTablasTipoHab();
        }

       

        private void EliminarAmenidadBTN_Click(object sender, EventArgs e)
        {
            
        }

        // ---------------------------------------------------------------------------------------------------------------------

    }
}
