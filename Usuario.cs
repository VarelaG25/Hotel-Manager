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
    public partial class Usuario : Form
    {
        private string contraseniaActual;
        public Usuario()
        {
            InitializeComponent();
        }

        public class UsuarioDatos
        {
            public int Id_Usuario { get; set; }
            public int Id_Admin { get; set; }
            public int Id_Credenciales { get; set; }
            public int NumeroNomina { get; set; }
            public string NombreUsuario { get; set; }
            public string PrimerApellido { get; set; }
            public string SegundoApellido { get; set; }
            public string TelefonoCelular { get; set; }
            public string TelefonoCasa { get; set; }
            public DateTime FechaRegistroUsuario { get; set; }
            public DateTime FechaModificacionUsuario { get; set; }
            public string CorreoUsuario { get; set; }
            public string ContrasenaUsuario { get; set; }
        }

        private void AbrirControlEnPanel(System.Windows.Forms.UserControl control)
        {
            MenuContenedor.Controls.Clear();
            control.Dock = DockStyle.Fill;
            MenuContenedor.Controls.Add(control);
            control.BringToFront();
        }

        private void Usuario_Load(object sender, EventArgs e)
        {
            AbrirControlEnPanel(new Menu());
            var NuevoForm = new Login();
            // Cargar tabla Usuarios SQL
            cargarTablaUsuario();
            // Generar número aleatorio
            Random rnd = new Random();
            int rand = rnd.Next(100000, 999999);
            NumeroNominaTXT.Text = rand.ToString();
            this.FindForm().Size = NuevoForm.Size;
            this.FindForm().StartPosition = FormStartPosition.Manual;
            this.FindForm().Location = NuevoForm.Location;
        }


        private void AceptarBTN_Click(object sender, EventArgs e)
        {
            // Clase UsuarioDatos
            UsuarioDatos usuario = new UsuarioDatos();
            // Usuario
            usuario.NumeroNomina = int.Parse(NumeroNominaTXT.Text);
            usuario.NombreUsuario = NombreTXT.Text;
            usuario.PrimerApellido = PrimerApellidoTXT.Text;
            usuario.SegundoApellido = SegundoApellidoTXT.Text;
            // Validar que los teléfonos contengan solo números
            usuario.TelefonoCelular = TelefonoCelularTXT.Text;
            usuario.TelefonoCasa = TelefonoCasaTXT.Text;
            if ((usuario.TelefonoCasa.Length != 10 && usuario.TelefonoCasa.Length > 0) ||
                (usuario.TelefonoCelular.Length != 10 && usuario.TelefonoCelular.Length > 0))
            {
                MessageBox.Show("Cada número de teléfono debe tener exactamente 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Credenciales
            usuario.CorreoUsuario = CorreoTXT.Text;
            usuario.ContrasenaUsuario = ContraseniaTXT.Text;
            var ConfirmarContrasenia = ConfirmarContraseniaTXT.Text;
            // Fechas
            usuario.FechaRegistroUsuario = DateTime.Parse(FechaActualDTP.Text);
            usuario.FechaModificacionUsuario = DateTime.Today;

            if (usuario.ContrasenaUsuario != ConfirmarContrasenia)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Enlace a la base de datos
            var enlace = new EnlaceDB();
            bool resultado = enlace.Insertar_Usuario(usuario);
            if (!resultado)
            {
                MessageBox.Show("Error al registrar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Usuario registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            cargarTablaUsuario();
        }

        private void NombreCompletoCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccion = Convert.ToInt32(NombreCompletoCB.SelectedValue);
            if (seleccion > 0)
            {
                // Asignar el valor al TextBox
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarUsuarioEspecifico(seleccion);
                // Asignar los valores a los TextBox
                NumeroNominaSeleccionadoTXT.Text = tabla.Rows[0]["numeroNomina"].ToString();
                NombreSeleccionadoTXT.Text = tabla.Rows[0]["nombreUsuario"].ToString();
                PrimerApellidoSeleccionadoTXT.Text = tabla.Rows[0]["primerApellido"].ToString();
                SegundoApellidoSeleccionadoTXT.Text = tabla.Rows[0]["segundoApellido"].ToString();
                TelefonoCelularSeleccionadoTXT.Text = tabla.Rows[0]["telefonoCelular"].ToString();
                TelefonoCasaSeleccionadoTXT.Text = tabla.Rows[0]["telefonoCasa"].ToString();
                CorreoSeleccionadoTXT.Text = tabla.Rows[0]["correoUsuario"].ToString();
                contraseniaActual = tabla.Rows[0]["contrasenaUsuario"].ToString();
            }
        }

        private void EliminarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro de que desea eliminar el usuario?", "Eliminar Usuario", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Eliminar usuario
                var enlace = new EnlaceDB();
                bool resultado = enlace.Borrar_Usuario(int.Parse(NumeroNominaSeleccionadoTXT.Text));
                if (!resultado)
                {
                    MessageBox.Show("Error al eliminar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Usuario eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cargarTablaUsuario();
                }
            }
        }

        private void ModificarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro de que desea modificar el usuario?", "Modificar Usuario", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                // Clase UsuarioDatos
                UsuarioDatos usuario = new UsuarioDatos();
                // Usuario
                usuario.NumeroNomina = int.Parse(NumeroNominaSeleccionadoTXT.Text);
                usuario.NombreUsuario = NombreSeleccionadoTXT.Text;
                usuario.PrimerApellido = PrimerApellidoSeleccionadoTXT.Text;
                usuario.SegundoApellido = SegundoApellidoSeleccionadoTXT.Text;
                usuario.TelefonoCelular = TelefonoCelularSeleccionadoTXT.Text;
                usuario.TelefonoCasa = TelefonoCasaSeleccionadoTXT.Text;
                if ((usuario.TelefonoCasa.Length != 10 && usuario.TelefonoCasa.Length > 0) ||
                (usuario.TelefonoCelular.Length != 10 && usuario.TelefonoCelular.Length > 0))
                {
                    MessageBox.Show("Cada número de teléfono debe tener exactamente 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Credenciales
                usuario.CorreoUsuario = CorreoSeleccionadoTXT.Text;
                usuario.ContrasenaUsuario = ContraseniaSeleccionadoTXT.Text;
                var ConfirmarContrasenia = contraseniaActual;
                // Fechas
                usuario.FechaModificacionUsuario = DateTime.Parse(FechaModificadoTXT.Text);
                if (ConfirmarContraseniaSeleccionadoTXT.Text != ConfirmarContrasenia)
                {
                    MessageBox.Show("La contraseñas no coincide con la anterior", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Enlace a la base de datos
                var enlace = new EnlaceDB();
                bool resultado = enlace.Actualizar_Usuario(usuario);
                if (!resultado)
                {
                    MessageBox.Show("Error al registrar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Usuario registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarTablaUsuario();
            }
        }
        public void cargarTablaUsuario()
        {
            var enlace = new EnlaceDB();
            var tabla = new DataTable();
            tabla = enlace.ConsultarUsuarios();
            // Asignar el DataSource a la columna combinada
            tabla.Columns.Add("nombreCompleto", typeof(string), "nombreUsuario + ' ' + primerApellido + ' ' + segundoApellido");
            // Asignar al ComboBox el nombre completo
            NombreCompletoCB.DisplayMember = "nombreCompleto";
            NombreCompletoCB.ValueMember = "numeroNomina";
            NombreCompletoCB.DataSource = tabla;
            // Asignar el DataSource antes de agregar la columna combinada
            TablaRegistroUsuarioDGV.DataSource = tabla;
            TablaRegistroUsuarioDGV.Columns["numeroNomina"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["nombreUsuario"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["primerApellido"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["segundoApellido"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["fechaRegistroUsuario"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["fechaModificacionUsuario"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["correoUsuario"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["contrasenaUsuario"].Visible = false;
            TablaRegistroUsuarioDGV.Columns["telefonoCelular"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["telefonoCasa"].Visible = true;
            TablaRegistroUsuarioDGV.Columns["nombreCompleto"].Visible = false;

            TablaRegistroUsuarioDGV.Columns["numeroNomina"].HeaderText = "Número de nómina";
            TablaRegistroUsuarioDGV.Columns["nombreUsuario"].HeaderText = "Nombre";
            TablaRegistroUsuarioDGV.Columns["primerApellido"].HeaderText = "Primer apellido";
            TablaRegistroUsuarioDGV.Columns["segundoApellido"].HeaderText = "Segundo apellido";
            TablaRegistroUsuarioDGV.Columns["fechaRegistroUsuario"].HeaderText = "Fecha de registro";
            TablaRegistroUsuarioDGV.Columns["fechaModificacionUsuario"].HeaderText = "Fecha de modificación";
            TablaRegistroUsuarioDGV.Columns["correoUsuario"].HeaderText = "Correo electrónico";
            TablaRegistroUsuarioDGV.Columns["contrasenaUsuario"].HeaderText = "Contraseña";
            TablaRegistroUsuarioDGV.Columns["telefonoCelular"].HeaderText = "Teléfono celular";
            TablaRegistroUsuarioDGV.Columns["telefonoCasa"].HeaderText = "Teléfono de casa";

            TablaRegistroUsuarioDGV.Columns["numeroNomina"].DisplayIndex = 0;
            TablaRegistroUsuarioDGV.Columns["nombreUsuario"].DisplayIndex = 1;
            TablaRegistroUsuarioDGV.Columns["primerApellido"].DisplayIndex = 2;
            TablaRegistroUsuarioDGV.Columns["segundoApellido"].DisplayIndex = 3;
            TablaRegistroUsuarioDGV.Columns["correoUsuario"].DisplayIndex = 4;
            TablaRegistroUsuarioDGV.Columns["contrasenaUsuario"].DisplayIndex = 5;
            TablaRegistroUsuarioDGV.Columns["telefonoCelular"].DisplayIndex = 6;
            TablaRegistroUsuarioDGV.Columns["telefonoCasa"].DisplayIndex = 7;
            TablaRegistroUsuarioDGV.Columns["fechaRegistroUsuario"].DisplayIndex = 8;
            TablaRegistroUsuarioDGV.Columns["fechaModificacionUsuario"].DisplayIndex = 9;

            TablaModificarUsuarioDGV.DataSource = tabla;
            TablaModificarUsuarioDGV.Columns["numeroNomina"].Visible = true;
            TablaModificarUsuarioDGV.Columns["nombreUsuario"].Visible = true;
            TablaModificarUsuarioDGV.Columns["primerApellido"].Visible = true;
            TablaModificarUsuarioDGV.Columns["segundoApellido"].Visible = true;
            TablaModificarUsuarioDGV.Columns["fechaRegistroUsuario"].Visible = true;
            TablaModificarUsuarioDGV.Columns["fechaModificacionUsuario"].Visible = true;
            TablaModificarUsuarioDGV.Columns["correoUsuario"].Visible = true;
            TablaModificarUsuarioDGV.Columns["contrasenaUsuario"].Visible = false;
            TablaModificarUsuarioDGV.Columns["telefonoCelular"].Visible = true;
            TablaModificarUsuarioDGV.Columns["telefonoCasa"].Visible = true;
            TablaModificarUsuarioDGV.Columns["nombreCompleto"].Visible = false;

            TablaModificarUsuarioDGV.Columns["numeroNomina"].HeaderText = "Número de nómina";
            TablaModificarUsuarioDGV.Columns["nombreUsuario"].HeaderText = "Nombre";
            TablaModificarUsuarioDGV.Columns["primerApellido"].HeaderText = "Primer apellido";
            TablaModificarUsuarioDGV.Columns["segundoApellido"].HeaderText = "Segundo apellido";
            TablaModificarUsuarioDGV.Columns["fechaRegistroUsuario"].HeaderText = "Fecha de registro";
            TablaModificarUsuarioDGV.Columns["fechaModificacionUsuario"].HeaderText = "Fecha de modificación";
            TablaModificarUsuarioDGV.Columns["correoUsuario"].HeaderText = "Correo electrónico";
            TablaModificarUsuarioDGV.Columns["contrasenaUsuario"].HeaderText = "Contraseña";
            TablaModificarUsuarioDGV.Columns["telefonoCelular"].HeaderText = "Teléfono celular";
            TablaModificarUsuarioDGV.Columns["telefonoCasa"].HeaderText = "Teléfono de casa";

            TablaModificarUsuarioDGV.Columns["numeroNomina"].DisplayIndex = 0;
            TablaModificarUsuarioDGV.Columns["nombreUsuario"].DisplayIndex = 1;
            TablaModificarUsuarioDGV.Columns["primerApellido"].DisplayIndex = 2;
            TablaModificarUsuarioDGV.Columns["segundoApellido"].DisplayIndex = 3;
            TablaModificarUsuarioDGV.Columns["correoUsuario"].DisplayIndex = 4;
            TablaModificarUsuarioDGV.Columns["contrasenaUsuario"].DisplayIndex = 5;
            TablaModificarUsuarioDGV.Columns["telefonoCelular"].DisplayIndex = 6;
            TablaModificarUsuarioDGV.Columns["telefonoCasa"].DisplayIndex = 7;
            TablaModificarUsuarioDGV.Columns["fechaRegistroUsuario"].DisplayIndex = 8;
            TablaModificarUsuarioDGV.Columns["fechaModificacionUsuario"].DisplayIndex = 9;
        }
    }
}