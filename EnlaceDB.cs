/*
Autor: Alejandro Villarreal

LMAD

PARA EL PROYECTO ES OBLIGATORIO EL USO DE ESTA CLASE, 
EN EL SENTIDO DE QUE LOS DATOS DE CONEXION AL SERVIDOR ESTAN DEFINIDOS EN EL App.Config
Y NO TENER ESOS DATOS EN CODIGO DURO DEL PROYECTO.

NO SE PERMITE HARDCODE.

LOS MÉTODOS QUE SE DEFINEN EN ESTA CLASE SON EJEMPLOS, PARA QUE SE BASEN Y USTEDES HAGAN LOS SUYOS PROPIOS
Y DEFINAN Y PROGRAMEN TODOS LOS MÉTODOS QUE SEAN NECESARIOS PARA SU PROYECTO.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;


/*
Se tiene que cambiar el namespace para el que usen en su proyecto
*/
namespace AAVD
{
    public class EnlaceDB
    {
        static private string _aux { set; get; }
        static private SqlConnection _conexion;
        static private SqlDataAdapter _adaptador = new SqlDataAdapter();
        static private SqlCommand _comandosql = new SqlCommand();
        static private DataTable _tabla = new DataTable();
        static private DataSet _DS = new DataSet();

        public DataTable obtenertabla
        {
            get
            {
                return _tabla;
            }
        }

        private static void conectar()
        {
            string cnn = ConfigurationManager.ConnectionStrings["BaseDatos"].ToString(); 
            _conexion = new SqlConnection(cnn);
            _conexion.Open();
        }
        private static void desconectar()
        {
            _conexion.Close();
        }
        public DataTable ConsultarUsuarios()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        } 
        public DataTable ConsultarUbicacion()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarUbicacion";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public DataTable ConsultarBarraUsuario()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarBarraUsuario";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                var Id_Credencial = Login.IdUsuarioActual;
                _comandosql.Parameters.AddWithValue("@Id_Credenciales", Id_Credencial);

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public DataTable consultarLogin(Login.LoginUsuario param)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarLogin";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@correoUsuario", param.Usuario);
                _comandosql.Parameters.AddWithValue("@contraseniaUsuario", param.Contrasena);

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);

            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public DataTable consultarUsuarioEspecifico(int NumeroNomina)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarUsuarioEspecifico";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@NumeroNomina", NumeroNomina);

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public DataTable consultarUsuarioLogeado(int Id_Credenciales)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarUsuarioLogeado";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Credenciales", Id_Credenciales);

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public DataTable consultarTipoHabitacion()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarTipoHabitacion";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public DataTable consultarAmenidad()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarAmenidad";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public DataTable consultarCaracteristica()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarCaracteristica";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public DataTable consultarCliente()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarCliente";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _adaptador.SelectCommand = _comandosql;
                _adaptador.Fill(tabla);
            }
            catch (SqlException e)
            {
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return tabla;
        }
        public bool Insertar_Usuario(Usuario.UsuarioDatos param)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_InsertarUsuario";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                // Parámetros para la tabla Usuario
                _comandosql.Parameters.AddWithValue("@numeroNomina", param.NumeroNomina);
                _comandosql.Parameters.AddWithValue("@nombreUsuario", param.NombreUsuario);
                _comandosql.Parameters.AddWithValue("@primerApellido", param.PrimerApellido);
                _comandosql.Parameters.AddWithValue("@segundoApellido", param.SegundoApellido);
                _comandosql.Parameters.AddWithValue("@fechaRegistroUsuario", param.FechaRegistroUsuario);
                _comandosql.Parameters.AddWithValue("@fechaModificacionUsuario", param.FechaModificacionUsuario);
                _comandosql.Parameters.AddWithValue("@Id_Admin", param.Id_Admin);

                // Parámetros para la tabla InicioSesion
                _comandosql.Parameters.AddWithValue("@correoUsuario", param.CorreoUsuario);
                _comandosql.Parameters.AddWithValue("@contrasenaUsuario", param.ContrasenaUsuario);

                // Parámetros para la tabla Telefono
                _comandosql.Parameters.AddWithValue("@telefonoCelular", param.TelefonoCelular);
                _comandosql.Parameters.AddWithValue("@telefonoCasa", param.TelefonoCasa);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }
        public bool Insertar_Cliente(Clientes.ClienteDatos param)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_InsertarCliente";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Usuario", param.Id_Usuario);
                _comandosql.Parameters.AddWithValue("@Id_Ubicacion", param.Id_Ubicacion);
                _comandosql.Parameters.AddWithValue("@nombreCliente", param.Nombre);
                _comandosql.Parameters.AddWithValue("@primerApellidoCliente", param.PrimerApellido);
                _comandosql.Parameters.AddWithValue("@segundoApellidoCliente", param.SegundoApellido);
                _comandosql.Parameters.AddWithValue("@fechaNacimientoCliente", param.FechaNacimiento);
                _comandosql.Parameters.AddWithValue("@estadoCivil", param.EstadoCivil);
                _comandosql.Parameters.AddWithValue("@rfcCliente", param.RFC);
                _comandosql.Parameters.AddWithValue("@fechaRegistroCliente", param.FechaRegistro);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }
        public bool Insertar_TipoHab(Hoteles.TipoHab param, out int Id_TipoHab)
        {
            var msg = "";
            var add = true;
            Id_TipoHab = -1;
            try
            {
                conectar();
                string qry = "SP_InsertarTipoHabitacion";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                // Parámetros para la tabla Usuario
                _comandosql.Parameters.AddWithValue("@nivelHabitacion", param.nivelHabitacion);
                _comandosql.Parameters.AddWithValue("@numeroCamas", param.numeroCamas);
                _comandosql.Parameters.AddWithValue("@tipoCama", param.tipoCama);
                _comandosql.Parameters.AddWithValue("@precio", param.precio);
                _comandosql.Parameters.AddWithValue("@numeroPersonas", param.numeroPersonas);
                _comandosql.Parameters.AddWithValue("@frenteA", param.frenteA);

                SqlParameter outputParam = new SqlParameter("@ExtraidoId_TipoHab", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                _comandosql.Parameters.Add(outputParam);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();

                Id_TipoHab = Convert.ToInt32(outputParam.Value); 
                add = true;
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }
        public bool Insertar_Ubicacion(Clientes.ClienteDatos param, out int Id_Ubicacion)
        {
            var msg = "";
            var add = true;
            Id_Ubicacion = -1;
            try
            {
                conectar();
                string qry = "SP_InsertarUbicacion";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                // Parámetros para la tabla Usuario
                _comandosql.Parameters.AddWithValue("@pais", param.Pais);
                _comandosql.Parameters.AddWithValue("@estado", param.Estado);
                _comandosql.Parameters.AddWithValue("@ciudad", param.Ciudad);
                _comandosql.Parameters.AddWithValue("@domicilio", param.Domicilio);
                _comandosql.Parameters.AddWithValue("@codigoPostal", param.CodigoPostal);

                SqlParameter outputParam = new SqlParameter("@Id_Ubicacion", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                _comandosql.Parameters.Add(outputParam);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();

                Id_Ubicacion = Convert.ToInt32(outputParam.Value); 
                add = true;
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }
        public bool Insertar_Amenidad(Hoteles.Amenidad param, out int Id_Amenidad)
        {
            var msg = "";
            var add = true;
            Id_Amenidad = -1;
            try
            {
                conectar();
                string qry = "SP_InsertarAmenidad";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@nombreAmenidad", param.nombreAmenidad);

                SqlParameter outputParam = new SqlParameter("@ExtraidoId_Amenidad", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                _comandosql.Parameters.Add(outputParam);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
                Id_Amenidad = Convert.ToInt32(outputParam.Value);
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }
        public bool Insertar_Caracteristica(Hoteles.Caracteristica param, out int Id_Caracteristica)
        {
            var msg = "";
            var add = true;
            Id_Caracteristica = -1;
            try
            {
                conectar();
                string qry = "SP_InsertarCaracteristica";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@nombreCaracteristica", param.nombreCaracteristica);

                SqlParameter outputParam = new SqlParameter("@ExtraidoId_Caracteristica", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                _comandosql.Parameters.Add(outputParam);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
                Id_Caracteristica = Convert.ToInt32(outputParam.Value);
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }
        public bool Insertar_TipoHab_Amenidad(Hoteles.Amenidad param, int Id_TipoHab)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_InsertarTipoHab_Amenidad";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_TipoHab", Id_TipoHab);
                _comandosql.Parameters.AddWithValue("@Id_Amenidad", param.Id_Amenidad);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }
        public bool Insertar_TipoHab_Caracteristica(Hoteles.Caracteristica param, int Id_TipoHab)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_InsertarTipoHab_Caracteristica";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_TipoHab", Id_TipoHab);
                _comandosql.Parameters.AddWithValue("@Id_Caracteristica", param.Id_Caracteristica);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }
        public bool Borrar_Usuario(int numeroNomina)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_BorrarUsuario";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                // Parámetros para la tabla Usuario
                _comandosql.Parameters.AddWithValue("@numeroNomina", numeroNomina);
               
                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }

        public bool Actualizar_Usuario(Usuario.UsuarioDatos param)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_ActualizarUsuario";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                // Parámetros para la tabla Usuario
                _comandosql.Parameters.AddWithValue("@numeroNomina", param.NumeroNomina);
                _comandosql.Parameters.AddWithValue("@nombreUsuario", param.NombreUsuario);
                _comandosql.Parameters.AddWithValue("@primerApellido", param.PrimerApellido);
                _comandosql.Parameters.AddWithValue("@segundoApellido", param.SegundoApellido);
                _comandosql.Parameters.AddWithValue("@fechaModificacionUsuario", param.FechaModificacionUsuario);

                // Parámetros para la tabla InicioSesion
                _comandosql.Parameters.AddWithValue("@correoUsuario", param.CorreoUsuario);
                _comandosql.Parameters.AddWithValue("@contrasenaUsuario", param.ContrasenaUsuario);

                // Parámetros para la tabla Telefono
                _comandosql.Parameters.AddWithValue("@telefonoCelular", param.TelefonoCelular);
                _comandosql.Parameters.AddWithValue("@telefonoCasa", param.TelefonoCasa);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
            }
            catch (SqlException e)
            {
                add = false;
                msg = "Excepción de base de datos: \n";
                msg += e.Message;
                MessageBox.Show(msg, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                desconectar();
            }

            return add;
        }

    }
}
