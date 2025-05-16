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
    public partial class Reportes : Form
    {
        public Reportes()
        {
            InitializeComponent();
        }
        public class filtros
        {
            public string pais { get; set; }
            public string ciudad { get; set; }
            public string hotel { get; set; }
            public string primerApellido { get; set; }
            public string segundoApellido { get; set; }
            public string correo { get; set; }
            public string rfc { get; set; }
            public int? anio { get; set; }
        }
        private void AbrirControlEnPanel(System.Windows.Forms.UserControl control)
        {
            MenuContenedor.Controls.Clear();
            control.Dock = DockStyle.Fill;
            MenuContenedor.Controls.Add(control);
            control.BringToFront();
        }

        private void Reportes_Load(object sender, EventArgs e)
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
            var tabla = new DataTable();
            var tabla2 = new DataTable();
            filtros filtro = new filtros();
            filtro.pais = null;
            filtro.ciudad = null;
            filtro.anio = null;
            filtro.hotel = null;
            tabla2 = enlace.reporteOcupacionHotel(filtro);
            tabla = enlace.reporteOcupacion(filtro);
            ReporteOcupacionDTG.DataSource = tabla;
            ReporteOcupacionDTG.Columns["ciudad"].HeaderText = "Ciudad";
            ReporteOcupacionDTG.Columns["pais"].Visible = false;
            ReporteOcupacionDTG.Columns["nombreHotel"].HeaderText = "Hotel";
            ReporteOcupacionDTG.Columns["anioVenta"].HeaderText = "Año";
            ReporteOcupacionDTG.Columns["mesVenta"].HeaderText = "Mes";
            ReporteOcupacionDTG.Columns["tipoHabitacion"].HeaderText = "Nivel de habitacion";
            ReporteOcupacionDTG.Columns["cantidadReservaciones"].HeaderText = "Habitaciones ocupadas";
            ReporteOcupacionDTG.Columns["totalPersonasHospedadas"].HeaderText = "Personas hospedadas";
            ReporteOcupacionDTG.Columns["porcentajeOcupacion"].HeaderText = "Porcentaje de ocupacion";
            foreach (DataGridViewRow row in ReporteOcupacionDTG.Rows)
            {
                if (row.Cells["porcentajeOcupacion"].Value != null &&
                    double.TryParse(row.Cells["porcentajeOcupacion"].Value.ToString(), out double valor))
                {
                    row.Cells["porcentajeOcupacion"].Value = valor / 100.0;
                }
            }
            ReporteOcupacionDTG.Columns["porcentajeOcupacion"].DefaultCellStyle.Format = "P2";
            ReporteOcupacionResumen.DataSource = tabla2;
            ReporteOcupacionResumen.Columns["pais"].Visible = false;
            ReporteOcupacionResumen.Columns["ciudad"].HeaderText = "Ciudad";
            ReporteOcupacionResumen.Columns["nombreHotel"].HeaderText = "Hotel";
            ReporteOcupacionResumen.Columns["anioVenta"].HeaderText = "Año";
            ReporteOcupacionResumen.Columns["mesVenta"].HeaderText = "Mes";
            ReporteOcupacionResumen.Columns["habitacionesOcupadas"].HeaderText = "Habitaciones ocupadas";
            ReporteOcupacionResumen.Columns["porcentajeOcupacion"].HeaderText = "Porcentaje de ocupacion";
            foreach (DataGridViewRow row in ReporteOcupacionResumen.Rows)
            {
                if (row.Cells["porcentajeOcupacion"].Value != null &&
                    double.TryParse(row.Cells["porcentajeOcupacion"].Value.ToString(), out double valor))
                {
                    row.Cells["porcentajeOcupacion"].Value = valor / 100.0;
                }
            }
            ReporteOcupacionResumen.Columns["porcentajeOcupacion"].DefaultCellStyle.Format = "P2";

            var paises = tabla.AsEnumerable()
                .Select(row => row.Field<string>("pais"))
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .Select(p => new { pais = p })
                .ToList();
            paises.Insert(0, new { pais = "" });
            var paises2 = tabla2.AsEnumerable()
                .Select(row => row.Field<string>("pais"))
                .Where(p => !string.IsNullOrEmpty(p))
                .Distinct()
                .Select(p => new { pais = p })
                .ToList();
            paises2.Insert(0, new { pais = "" });
            PaisCB1.DataSource = paises;
            PaisCB1.DisplayMember = "pais";
            PaisCB1.ValueMember = "pais";
            PaisCB2.DataSource = paises2;
            PaisCB2.DisplayMember = "pais";
            PaisCB2.ValueMember = "pais";
            var ciudades = tabla.AsEnumerable()
                .Select(row => row.Field<string>("ciudad"))
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .Select(c => new { ciudad = c })
                .ToList();
            ciudades.Insert(0, new { ciudad = "" });
            var ciudades2 = tabla2.AsEnumerable()
                .Select(row => row.Field<string>("ciudad"))
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .Select(c => new { ciudad = c })
                .ToList();
            ciudades2.Insert(0, new { ciudad = "" });
            CiudadCB1.DataSource = ciudades;
            CiudadCB1.DisplayMember = "ciudad";
            CiudadCB1.ValueMember = "ciudad";
            CiudadCB2.DataSource = ciudades2;
            CiudadCB2.DisplayMember = "ciudad";
            CiudadCB2.ValueMember = "ciudad";
            var hoteles = tabla.AsEnumerable()
                .Select(row => row.Field<string>("nombreHotel"))
                .Where(h => !string.IsNullOrEmpty(h))
                .Distinct()
                .Select(h => new { nombreHotel = h })
                .ToList();
            hoteles.Insert(0, new { nombreHotel = "" });
            var hoteles2 = tabla2.AsEnumerable()
                .Select(row => row.Field<string>("nombreHotel"))
                .Where(h => !string.IsNullOrEmpty(h))
                .Distinct()
                .Select(h => new { nombreHotel = h })
                .ToList();
            hoteles2.Insert(0, new { nombreHotel = "" });
            HotelCB1.DataSource = hoteles;
            HotelCB1.DisplayMember = "nombreHotel";
            HotelCB1.ValueMember = "nombreHotel";
            HotelCB2.DataSource = hoteles;
            HotelCB2.DisplayMember = "nombreHotel";
            HotelCB2.ValueMember = "nombreHotel";


            var reporteVentas = new DataTable();
            reporteVentas = enlace.reporteVentas(filtro);
            ReporteVentasDTG.DataSource = reporteVentas;
            ReporteVentasDTG.Columns["ciudad"].HeaderText = "Ciudad";
            ReporteVentasDTG.Columns["pais"].Visible = false;
            ReporteVentasDTG.Columns["nombreHotel"].HeaderText = "Hotel";
            ReporteVentasDTG.Columns["anio"].HeaderText = "Año";
            ReporteVentasDTG.Columns["mes"].HeaderText = "Mes (letra)";
            ReporteVentasDTG.Columns["ingresosHospedaje"].HeaderText = "Ingresos por hospedaje";
            ReporteVentasDTG.Columns["ingresosServicios"].HeaderText = "Ingresos por servicios adicionales";
            ReporteVentasDTG.Columns["ingresosTotales"].HeaderText = "Ingresos totales";

            ReporteVentasDTG.Columns["nombreHotel"].DisplayIndex = 0;
            ReporteVentasDTG.Columns["pais"].DisplayIndex = 1;
            ReporteVentasDTG.Columns["ciudad"].DisplayIndex = 2;
            ReporteVentasDTG.Columns["anio"].DisplayIndex = 3;
            ReporteVentasDTG.Columns["mes"].DisplayIndex = 4;
            ReporteVentasDTG.Columns["ingresosHospedaje"].DisplayIndex = 5;
            ReporteVentasDTG.Columns["ingresosServicios"].DisplayIndex = 6;
            ReporteVentasDTG.Columns["ingresosTotales"].DisplayIndex = 7;

            ReporteVentasDTG.Columns["ingresosHospedaje"].DefaultCellStyle.Format = "C2";
            ReporteVentasDTG.Columns["ingresosServicios"].DefaultCellStyle.Format = "C2";
            ReporteVentasDTG.Columns["ingresosTotales"].DefaultCellStyle.Format = "C2";

            var paises3 = reporteVentas.AsEnumerable()
               .Select(row => row.Field<string>("pais"))
               .Where(p => !string.IsNullOrEmpty(p))
               .Distinct()
               .Select(p => new { pais = p })
               .ToList();
            paises3.Insert(0, new { pais = "" });
            PaisCB3.DataSource = paises3;
            PaisCB3.DisplayMember = "pais";
            PaisCB3.ValueMember = "pais";
            var ciudades3 = reporteVentas.AsEnumerable()
                .Select(row => row.Field<string>("ciudad"))
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .Select(c => new { ciudad = c })
                .ToList();
            ciudades3.Insert(0, new { ciudad = "" });
            CiudadCB3.DataSource = ciudades3;
            CiudadCB3.DisplayMember = "ciudad";
            CiudadCB3.ValueMember = "ciudad";
            var hoteles3 = reporteVentas.AsEnumerable()
                .Select(row => row.Field<string>("nombreHotel"))
                .Where(h => !string.IsNullOrEmpty(h))
                .Distinct()
                .Select(h => new { nombreHotel = h })
                .ToList();
            hoteles3.Insert(0, new { nombreHotel = "" });
            HotelCB3.DataSource = hoteles3;
            HotelCB3.DisplayMember = "nombreHotel";
            HotelCB3.ValueMember = "nombreHotel";

            //var historial = enlace.historialCliente(filtro);
            //HistorialDTG.DataSource = historial;

            //HistorialDTG.Columns["nombreCompleto"].HeaderText = "Nombre completo";
            //HistorialDTG.Columns[""].HeaderText = "";
            //HistorialDTG.Columns[""].HeaderText = "";
            //HistorialDTG.Columns[""].HeaderText = "";
            //HistorialDTG.Columns[""].HeaderText = "";
            //HistorialDTG.Columns[""].HeaderText = "";
            //HistorialDTG.Columns[""].HeaderText = "";
            //HistorialDTG.Columns[""].HeaderText = "";
            //HistorialDTG.Columns[""].HeaderText = "";

        }
        private void LimpiarFiltrosBTN_Click(object sender, EventArgs e)
        {
            CargarTablas();
        }

        private void AplicarBTN1_Click(object sender, EventArgs e)
        {
            filtros filtro = new filtros();

            var paisSeleccionado = PaisCB1.SelectedValue?.ToString();
            filtro.pais = string.IsNullOrWhiteSpace(paisSeleccionado) ? null : paisSeleccionado;

            var ciudadSeleccionada = CiudadCB1.SelectedValue?.ToString();
            filtro.ciudad = string.IsNullOrWhiteSpace(ciudadSeleccionada) ? null : ciudadSeleccionada;

            var hotelSeleccionado = HotelCB1.SelectedValue?.ToString();
            filtro.hotel = string.IsNullOrWhiteSpace(hotelSeleccionado) ? null : hotelSeleccionado;

            if (AnioCB1.Checked) 
            {
                filtro.anio = AnioCB1.Value.Year;
            }
            else
            {
                filtro.anio = null;
            }
            var enlace = new EnlaceDB();
            var tabla = new DataTable();
            tabla = enlace.reporteOcupacion(filtro);
            ReporteOcupacionDTG.DataSource = tabla;
            ReporteOcupacionDTG.Columns["ciudad"].HeaderText = "Ciudad";
            ReporteOcupacionDTG.Columns["nombreHotel"].HeaderText = "Hotel";
            ReporteOcupacionDTG.Columns["anioVenta"].HeaderText = "Año";
            ReporteOcupacionDTG.Columns["mesVenta"].HeaderText = "Mes";
            ReporteOcupacionDTG.Columns["tipoHabitacion"].HeaderText = "Nivel de habitacion";
            ReporteOcupacionDTG.Columns["cantidadReservaciones"].HeaderText = "Habitaciones ocupadas";
            ReporteOcupacionDTG.Columns["totalPersonasHospedadas"].HeaderText = "Personas hospedadas";
            ReporteOcupacionDTG.Columns["porcentajeOcupacion"].HeaderText = "Porcentaje de ocupacion";
            foreach (DataGridViewRow row in ReporteOcupacionDTG.Rows)
            {
                if (row.Cells["porcentajeOcupacion"].Value != null &&
                    double.TryParse(row.Cells["porcentajeOcupacion"].Value.ToString(), out double valor))
                {
                    row.Cells["porcentajeOcupacion"].Value = valor / 100.0;
                }
            }
            ReporteOcupacionDTG.Columns["porcentajeOcupacion"].DefaultCellStyle.Format = "P2";
        }

        private void AplicarBTN2_Click(object sender, EventArgs e)
        {
            filtros filtro = new filtros();

            var paisSeleccionado = PaisCB2.SelectedValue?.ToString();
            filtro.pais = string.IsNullOrWhiteSpace(paisSeleccionado) ? null : paisSeleccionado;

            var ciudadSeleccionada = CiudadCB2.SelectedValue?.ToString();
            filtro.ciudad = string.IsNullOrWhiteSpace(ciudadSeleccionada) ? null : ciudadSeleccionada;

            var hotelSeleccionado = HotelCB2.SelectedValue?.ToString();
            filtro.hotel = string.IsNullOrWhiteSpace(hotelSeleccionado) ? null : hotelSeleccionado;

            if (AnioCB2.Checked)
            {
                filtro.anio = AnioCB2.Value.Year;
            }
            else
            {
                filtro.anio = null;
            }
            var enlace = new EnlaceDB();
            var tabla = new DataTable();
            tabla = enlace.reporteOcupacionHotel(filtro);
            ReporteOcupacionResumen.DataSource = tabla;
            ReporteOcupacionResumen.Columns["ciudad"].HeaderText = "Ciudad";
            ReporteOcupacionResumen.Columns["nombreHotel"].HeaderText = "Hotel";
            ReporteOcupacionResumen.Columns["anioVenta"].HeaderText = "Año";
            ReporteOcupacionResumen.Columns["mesVenta"].HeaderText = "Mes";
            ReporteOcupacionResumen.Columns["habitacionesOcupadas"].HeaderText = "Habitaciones ocupadas";
            ReporteOcupacionResumen.Columns["porcentajeOcupacion"].HeaderText = "Porcentaje de ocupacion";
            foreach (DataGridViewRow row in ReporteOcupacionResumen.Rows)
            {
                if (row.Cells["porcentajeOcupacion"].Value != null &&
                    double.TryParse(row.Cells["porcentajeOcupacion"].Value.ToString(), out double valor))
                {
                    row.Cells["porcentajeOcupacion"].Value = valor / 100.0;
                }
            }
            ReporteOcupacionResumen.Columns["porcentajeOcupacion"].DefaultCellStyle.Format = "P2";
        }

        private void LimpiarBTN_Click(object sender, EventArgs e)
        {
            CargarTablas();
        }

        private void Aplicar_Click(object sender, EventArgs e)
        {
            filtros filtro = new filtros();

            var paisSeleccionado = PaisCB3.SelectedValue?.ToString();
            filtro.pais = string.IsNullOrWhiteSpace(paisSeleccionado) ? null : paisSeleccionado;

            var ciudadSeleccionada = CiudadCB3.SelectedValue?.ToString();
            filtro.ciudad = string.IsNullOrWhiteSpace(ciudadSeleccionada) ? null : ciudadSeleccionada;

            var hotelSeleccionado = HotelCB3.SelectedValue?.ToString();
            filtro.hotel = string.IsNullOrWhiteSpace(hotelSeleccionado) ? null : hotelSeleccionado;

            if (AnioCB3.Checked)
            {
                filtro.anio = AnioCB3.Value.Year;
            }
            else
            {
                filtro.anio = null;
            }
            var enlace = new EnlaceDB();
            var reporteVentas = new DataTable();
            reporteVentas = enlace.reporteVentas(filtro);
            ReporteVentasDTG.DataSource = reporteVentas;
            ReporteVentasDTG.Columns["ciudad"].HeaderText = "Ciudad";
            ReporteVentasDTG.Columns["pais"].Visible = false;
            ReporteVentasDTG.Columns["nombreHotel"].HeaderText = "Hotel";
            ReporteVentasDTG.Columns["anio"].HeaderText = "Año";
            ReporteVentasDTG.Columns["mes"].HeaderText = "Mes (letra)";
            ReporteVentasDTG.Columns["ingresosHospedaje"].HeaderText = "Ingresos por hospedaje";
            ReporteVentasDTG.Columns["ingresosServicios"].HeaderText = "Ingresos por servicios adicionales";
            ReporteVentasDTG.Columns["ingresosTotales"].HeaderText = "Ingresos totales";

            ReporteVentasDTG.Columns["nombreHotel"].DisplayIndex = 0;
            ReporteVentasDTG.Columns["pais"].DisplayIndex = 1;
            ReporteVentasDTG.Columns["ciudad"].DisplayIndex = 2;
            ReporteVentasDTG.Columns["anio"].DisplayIndex = 3;
            ReporteVentasDTG.Columns["mes"].DisplayIndex = 4;
            ReporteVentasDTG.Columns["ingresosHospedaje"].DisplayIndex = 5;
            ReporteVentasDTG.Columns["ingresosServicios"].DisplayIndex = 6;
            ReporteVentasDTG.Columns["ingresosTotales"].DisplayIndex = 7;

            ReporteVentasDTG.Columns["ingresosHospedaje"].DefaultCellStyle.Format = "C2";
            ReporteVentasDTG.Columns["ingresosServicios"].DefaultCellStyle.Format = "C2";
            ReporteVentasDTG.Columns["ingresosTotales"].DefaultCellStyle.Format = "C2";
        }

        private void LimpiarHistorial_Click(object sender, EventArgs e)
        {
            CargarTablas();
        }

        private void AplicarHistorial_Click(object sender, EventArgs e)
        {

        }
    }
}
