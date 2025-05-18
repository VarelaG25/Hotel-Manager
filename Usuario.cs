using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using static AAVD.Hoteles;

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
            public bool Id_Admin { get; set; }
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
            if (Login.baseDatos == 1)
            {
                cargarTablaUsuario();
            }
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
            // Validar datos
            if (NumeroNominaTXT.Text == "" || NombreTXT.Text == "" || PrimerApellidoTXT.Text == "" || SegundoApellidoTXT.Text == "" ||
                TelefonoCelularTXT.Text == "" || TelefonoCasaTXT.Text == "" || CorreoTXT.Text == "" || ContraseniaTXT.Text == "")
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (TelefonoCelularTXT.Text.Length != 10 || TelefonoCasaTXT.Text.Length != 10)
            {
                MessageBox.Show("Cada número de teléfono debe tener exactamente 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!TelefonoCelularTXT.Text.All(char.IsDigit) || !TelefonoCasaTXT.Text.All(char.IsDigit))
            {
                MessageBox.Show("El número de teléfono debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Validar que el correo tenga un formato válido
            if (!Regex.IsMatch(CorreoTXT.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("El correo electrónico no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ContraseniaTXT.Text != ConfirmarContraseniaTXT.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(ContraseniaTXT.Text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!""#$%&/=´?¡¿:;,\.\-_\+\*\{\}\[\]]).{8,}$"))
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres, incluyendo una mayúscula, una minúscula y un carácter especial.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var NumeroNomina = int.Parse(NumeroNominaTXT.Text);
            var Nombre = NombreTXT.Text;
            var PrimerApellido = PrimerApellidoTXT.Text;
            var SegundoApellido = SegundoApellidoTXT.Text;
            var TelefonoCelular = TelefonoCelularTXT.Text;
            var TelefonoCasa = TelefonoCasaTXT.Text;
            var Correo = CorreoTXT.Text;
            var Contrasenia = ContraseniaTXT.Text;
            var FechaRegistro = DateTime.Parse(FechaActualDTP.Text);
            var FechaModificacion = DateTime.Today;
            var Id_Admin = TipoUsuario.SelectedItem.ToString() == "Administrador" ? true : false;

            // Usando SQL
            if (Login.baseDatos == 1)
            {
                // Usuario
                usuario.NumeroNomina = NumeroNomina;
                usuario.NombreUsuario = Nombre;
                usuario.PrimerApellido = PrimerApellido;
                usuario.SegundoApellido = SegundoApellido;
                usuario.TelefonoCelular = TelefonoCelular;
                usuario.TelefonoCasa = TelefonoCasa;
                usuario.Id_Admin = Id_Admin;
                usuario.CorreoUsuario = Correo;
                usuario.ContrasenaUsuario = Contrasenia;
                usuario.FechaRegistroUsuario = FechaRegistro;
                usuario.FechaModificacionUsuario = FechaModificacion;

                // Enlace a la base de datos
                var enlace = new EnlaceDB();
                bool resultado = enlace.Insertar_Usuario(usuario);
                if (!resultado)
                {
                    MessageBox.Show("Error al registrar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cargarTablaUsuario();

            }
            MessageBox.Show("Usuario registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void NombreCompletoCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seleccion = NombreCompletoCB.SelectedValue != null ? Convert.ToInt32(NombreCompletoCB.SelectedValue) : -1;
            if (seleccion < 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Login.baseDatos == 1)
            {
                var enlace = new EnlaceDB();
                var tabla = new DataTable();
                tabla = enlace.consultarUsuarioEspecifico(seleccion);
                var usuario = tabla.Rows[0];
                NumeroNominaSeleccionadoTXT.Text = usuario["numeroNomina"].ToString();
                NombreSeleccionadoTXT.Text = usuario["nombreUsuario"].ToString();
                PrimerApellidoSeleccionadoTXT.Text = usuario["primerApellido"].ToString();
                SegundoApellidoSeleccionadoTXT.Text = usuario["segundoApellido"].ToString();
                TelefonoCelularSeleccionadoTXT.Text = usuario["telefonoCelular"].ToString();
                TelefonoCasaSeleccionadoTXT.Text = usuario["telefonoCasa"].ToString();
                CorreoSeleccionadoTXT.Text = usuario["correoUsuario"].ToString();
                contraseniaActual = usuario["contrasenaUsuario"].ToString();
            }
        }

        private void EliminarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro de que desea eliminar el usuario?", "Eliminar Usuario", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (Login.baseDatos == 1)
                {
                    // Eliminar usuario
                    var enlace = new EnlaceDB();
                    bool resultado = enlace.Borrar_Usuario(int.Parse(NumeroNominaSeleccionadoTXT.Text));
                    if (!resultado)
                    {
                        MessageBox.Show("Error al eliminar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cargarTablaUsuario();
                }
            }
            MessageBox.Show("Usuario eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            NombreCompletoCB.SelectedIndex = 0;
        }

        private void ModificarBTN_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("¿Está seguro de que desea modificar el usuario?", "Modificar Usuario", buttons, MessageBoxIcon.Warning);
            UsuarioDatos usuario = new UsuarioDatos();
            if (result == DialogResult.Yes)
            {
                // Validar datos
                if (NumeroNominaSeleccionadoTXT.Text == "" || NombreSeleccionadoTXT.Text == "" || PrimerApellidoSeleccionadoTXT.Text == "" || SegundoApellidoSeleccionadoTXT.Text == "" ||
                    TelefonoCelularSeleccionadoTXT.Text == "" || TelefonoCasaSeleccionadoTXT.Text == "" || CorreoSeleccionadoTXT.Text == "" || ConfirmarContraseniaSeleccionadoTXT.Text == "")
                {
                    MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!TelefonoCelularSeleccionadoTXT.Text.All(char.IsDigit) || !TelefonoCasaSeleccionadoTXT.Text.All(char.IsDigit))
                {
                    MessageBox.Show("El número de teléfono debe contener solo números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (TelefonoCelularSeleccionadoTXT.Text.Length != 10 || TelefonoCasaSeleccionadoTXT.Text.Length != 10)
                {
                    MessageBox.Show("Cada número de teléfono debe tener exactamente 10 dígitos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var NumeroNomina = int.Parse(NumeroNominaSeleccionadoTXT.Text);
                var Nombre = NombreSeleccionadoTXT.Text;
                var PrimerApellido = PrimerApellidoSeleccionadoTXT.Text;
                var SegundoApellido = SegundoApellidoSeleccionadoTXT.Text;
                var TelefonoCelular = TelefonoCelularSeleccionadoTXT.Text;
                var TelefonoCasa = TelefonoCasaSeleccionadoTXT.Text;
                var Correo = CorreoSeleccionadoTXT.Text;
                var Contrasenia = ContraseniaSeleccionadoTXT.Text;
                var FechaModificacion = DateTime.Parse(FechaModificadoTXT.Text);

                // Usando SQL
                if (Login.baseDatos == 1)
                {
                    usuario.NumeroNomina = NumeroNomina;
                    usuario.NombreUsuario = Nombre;
                    usuario.PrimerApellido = PrimerApellido;
                    usuario.SegundoApellido = SegundoApellido;
                    usuario.TelefonoCelular = TelefonoCelular;
                    usuario.TelefonoCasa = TelefonoCasa;
                    usuario.CorreoUsuario = Correo;
                    usuario.ContrasenaUsuario = Contrasenia;
                    usuario.FechaModificacionUsuario = FechaModificacion;
                    var Confirmar = contraseniaActual;

                    if (ConfirmarContraseniaSeleccionadoTXT.Text != Confirmar)
                    {
                        MessageBox.Show("La contraseña no coincide con la anterior", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var enlace = new EnlaceDB();
                    bool resultado = enlace.Actualizar_Usuario(usuario);
                    if (!resultado)
                    {
                        MessageBox.Show("Error al registrar el usuario", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    cargarTablaUsuario();
                }

                MessageBox.Show("Usuario registrado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void cargarTablaUsuario()
        {
            var enlace = new EnlaceDB();
            var tablaUsuarioActual = enlace.ConsultarBarraUsuario();
            UsuarioActualTXT.Text = tablaUsuarioActual.Rows[0]["nombreUsuario"].ToString();
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

            TipoUsuario.SelectedIndex = 0;
        }
    }
}