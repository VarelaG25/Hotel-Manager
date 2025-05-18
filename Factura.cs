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
    public partial class Factura : Form
    {
        public Factura()
        {
            InitializeComponent();
        }
        public class Filtro
        {
            public string Apellidos { get; set; }
            public string RFC { get; set; }
            public string Correo { get; set; }
        }
        private void AbrirControlEnPanel(System.Windows.Forms.UserControl control)
        {
            MenuContenedor.Controls.Clear();
            control.Dock = DockStyle.Fill;
            MenuContenedor.Controls.Add(control);
            control.BringToFront();
        }

        private void Factura_Load(object sender, EventArgs e)
        {
            AbrirControlEnPanel(new Menu());
            var NuevoForm = new Login();
            if (Login.baseDatos == 1)
            {
                CargarTabla();
            }
            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;
        }

        public void CargarTabla()
        {
            Guid guid = Guid.NewGuid();
            string guidStr = guid.ToString("N");
            string folioFiscal = guidStr.Substring(guidStr.Length - 10);
            FolioFiscal.Text = folioFiscal.ToUpper();

            Guid guid2 = Guid.NewGuid();
            string guidStr2 = guid2.ToString("N");
            string folioInterno = guidStr2.Substring(guidStr2.Length - 10);
            FolioInterno.Text = folioInterno.ToUpper();

            Guid guid3 = Guid.NewGuid();
            string guidStr3 = guid3.ToString("N");
            string noSerie = guidStr3.Substring(guidStr3.Length - 10);
            NumeroSerie.Text = noSerie.ToUpper();

            var enlace = new EnlaceDB();
            var tabla1 = new DataTable();
            var tabla2 = new DataTable();
            var tabla3 = new DataTable();
            tabla1 = enlace.consultarFacturaCliente();
            tabla2 = enlace.consultarFacturaCliente();
            tabla3 = enlace.consultarFacturaCliente();
            tabla1.Columns.Add("Apellidos", typeof(string), "primerApellidoCliente + ' ' + segundoApellidoCliente");
            tabla2.Columns.Add("Apellidos", typeof(string), "primerApellidoCliente + ' ' + segundoApellidoCliente");
            tabla3.Columns.Add("Apellidos", typeof(string), "primerApellidoCliente + ' ' + segundoApellidoCliente");

            ApellidosCB.DisplayMember = "Apellidos";
            ApellidosCB.ValueMember = "Id_Cliente";
            ApellidosCB.DataSource = tabla1;
            ApellidosCB.SelectedIndex = -1;

            RFCCB.DisplayMember = "rfcCliente";
            RFCCB.ValueMember = "Id_Cliente";
            RFCCB.DataSource = tabla2;
            RFCCB.SelectedIndex = -1;

            CorreoCB.DisplayMember = "correoCliente";
            CorreoCB.ValueMember = "Id_Cliente";
            CorreoCB.DataSource = tabla3;
            CorreoCB.SelectedIndex = -1;

        }

        private void BuscarBTN_Click(object sender, EventArgs e)
        {
            Filtro filtro = new Filtro
            {
                Apellidos = ApellidosCB.Text,
                RFC = RFCCB.Text,
                Correo = CorreoCB.Text
            };

            if (string.IsNullOrWhiteSpace(filtro.Apellidos) && string.IsNullOrWhiteSpace(filtro.RFC) && string.IsNullOrWhiteSpace(filtro.Correo))
            {
                MessageBox.Show("Seleccione al menos un criterio de búsqueda (apellidos, RFC o correo).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NombreCompleto.Text = "";
                RFCTXT.Text = "";
                TelefonoCasa.Text = "";
                TelefonoCelular.Text = "";
                CiudadTXT.Text = "";
                CodigoPostal.Text = "";
                NombreHotel.Text = "";
                NivelHabitacion.Text = "";
                DomicilioHotel.Text = "";
                MetodoPago.Text = "";
                Anticipo.Text = "";
                TotalTXT.Text = "";
                return;
            }

            var enlace = new EnlaceDB();
            var tabla = enlace.factura(filtro);

            if (tabla.Rows.Count == 0)
            {
                MessageBox.Show("No se encontró ningún cliente con los datos proporcionados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tabla.Rows.Count > 1)
            {
                MessageBox.Show("Agregue un criterio de busqueda adicional, existen varios clientes con los mismos datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var cliente = tabla.Rows[0];
            NombreCompleto.Text = cliente["nombreCompleto"].ToString();
            RFCTXT.Text = cliente["rfcCliente"].ToString();
            TelefonoCasa.Text = cliente["telefonoCasa"].ToString();
            TelefonoCelular.Text = cliente["telefonoCelular"].ToString();
            CiudadTXT.Text = cliente["ciudad"].ToString();
            CodigoPostal.Text = cliente["codigoPostal"].ToString();
            NombreHotel.Text = cliente["nombreHotel"].ToString();
            NivelHabitacion.Text = cliente["nivelHabitacion"].ToString();
            DomicilioHotel.Text = cliente["domicilio"].ToString();
            MetodoPago.Text = cliente["metodoPago"].ToString();
            Servicios.Text = Convert.ToDecimal(cliente["totalServicios"]).ToString("C2");
            decimal total = Convert.ToDecimal(cliente["totalHospedaje"]);
            decimal iva = total * 0.16m;
            decimal subtotal = total - iva;
            RestanteTXT.Text = Convert.ToDecimal(cliente["restante"]).ToString("C2");
            Anticipo.Text = Convert.ToDecimal(cliente["anticipo"]).ToString("C2");
            Subtotal.Text = subtotal.ToString("C2");
            TotalImpuesto.Text = iva.ToString("C2");
            TotalTXT.Text = Convert.ToDecimal(cliente["totalHospedaje"]).ToString("C2");
            string totalLetra = NumeroALetras.Convertir(total);
            TotalLetra.Text = totalLetra.ToUpper() + " M.N.";
            FechaExpedicion.Value = DateTime.Parse(cliente["fechaCheckOut"].ToString());
            filtro.RFC = RFCTXT.Text;
            var tablaServicios = new DataTable();
            tablaServicios = enlace.facturaServicios(filtro);
            ServiciosDTG.DataSource = tablaServicios;
            ServiciosDTG.Columns["nombreServicio"].HeaderText = "Servicio";
            ServiciosDTG.Columns["precio"].HeaderText = "Precio";

            ServiciosDTG.Columns["precio"].DefaultCellStyle.Format = "C2";

            HospedajeDTG.DataSource = tabla;
            HospedajeDTG.Columns["nombreCompleto"].Visible = false;
            HospedajeDTG.Columns["rfcCliente"].Visible = false;
            HospedajeDTG.Columns["telefonoCasa"].Visible = false;
            HospedajeDTG.Columns["telefonoCelular"].Visible = false;
            HospedajeDTG.Columns["ciudad"].Visible = false;
            HospedajeDTG.Columns["codigoPostal"].Visible = false;
            HospedajeDTG.Columns["nombreHotel"].Visible = false;
            HospedajeDTG.Columns["domicilio"].Visible = false;
            HospedajeDTG.Columns["metodoPago"].Visible = false;
            HospedajeDTG.Columns["totalServicios"].Visible = false;
            HospedajeDTG.Columns["fechaCheckOut"].Visible = false;

            HospedajeDTG.Columns["nivelHabitacion"].Visible = true;
            HospedajeDTG.Columns["anticipo"].Visible = true;
            HospedajeDTG.Columns["restante"].Visible = true;
            HospedajeDTG.Columns["totalHospedaje"].Visible = true;

            HospedajeDTG.Columns["nivelHabitacion"].HeaderText = "Nivel Habitacion";
            HospedajeDTG.Columns["anticipo"].HeaderText = "Anticipo";
            HospedajeDTG.Columns["restante"].HeaderText = "Restante";
            HospedajeDTG.Columns["totalHospedaje"].HeaderText = "Total Hospedaje";

            HospedajeDTG.Columns["nivelHabitacion"].DisplayIndex = 0;
            HospedajeDTG.Columns["anticipo"].DisplayIndex = 1;
            HospedajeDTG.Columns["restante"].DisplayIndex = 2;
            HospedajeDTG.Columns["totalHospedaje"].DisplayIndex = 3;

            HospedajeDTG.Columns["anticipo"].DefaultCellStyle.Format = "C2";
            HospedajeDTG.Columns["restante"].DefaultCellStyle.Format = "C2";
            HospedajeDTG.Columns["totalHospedaje"].DefaultCellStyle.Format = "C2";

        }
    }
    public static class NumeroALetras
    {
        public static string Convertir(decimal numero)
        {
            long entero = (long)Math.Floor(numero);
            int centavos = (int)((numero - entero) * 100);

            string letras = NumeroEnLetras(entero) + " pesos";

            if (centavos > 0)
            {
                letras += " con " + NumeroEnLetras(centavos) + " centavos";
            }

            return letras;
        }

        private static string NumeroEnLetras(long numero)
        {
            if (numero == 0) return "cero";
            if (numero < 0) return "menos " + NumeroEnLetras(Math.Abs(numero));

            string[] unidades = { "", "uno", "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve",
                              "diez", "once", "doce", "trece", "catorce", "quince", "dieciséis", "diecisiete",
                              "dieciocho", "diecinueve" };

            string[] decenas = { "", "", "veinte", "treinta", "cuarenta", "cincuenta", "sesenta", "setenta",
                             "ochenta", "noventa" };

            string[] centenas = { "", "ciento", "doscientos", "trescientos", "cuatrocientos", "quinientos",
                              "seiscientos", "setecientos", "ochocientos", "novecientos" };

            string resultado = "";

            if (numero == 100) return "cien";

            if (numero < 20)
            {
                resultado = unidades[numero];
            }
            else if (numero < 100)
            {
                resultado = decenas[numero / 10];
                if (numero % 10 > 0)
                    resultado += " y " + unidades[numero % 10];
            }
            else if (numero < 1000)
            {
                resultado = centenas[numero / 100];
                if (numero % 100 > 0)
                    resultado += " " + NumeroEnLetras(numero % 100);
            }
            else if (numero < 1000000)
            {
                long miles = numero / 1000;
                long resto = numero % 1000;

                if (miles == 1)
                    resultado = "mil";
                else
                    resultado = NumeroEnLetras(miles) + " mil";

                if (resto > 0)
                    resultado += " " + NumeroEnLetras(resto);
            }
            else if (numero < 1000000000000)
            {
                long millones = numero / 1000000;
                long resto = numero % 1000000;

                if (millones == 1)
                    resultado = "un millón";
                else
                    resultado = NumeroEnLetras(millones) + " millones";

                if (resto > 0)
                    resultado += " " + NumeroEnLetras(resto);
            }

            return resultado.Trim();
        }
    }

}
