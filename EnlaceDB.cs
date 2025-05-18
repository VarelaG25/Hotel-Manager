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
        public DataTable consultarIdUsuario(int Id_Credenciaes)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsutarIdUsuarios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Credenciales", Id_Credenciaes);
                
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
        public DataTable consultarClienteEspecifico(int Id_Cliente)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarClienteEspecifico";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Cliente", Id_Cliente);

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
        public DataTable consultarHotelXId(int Id_Hotel)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarHotelId";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id_Hotel);

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
        public DataTable consultarHotelXCiudad()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarUbiXHotel";
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
        public DataTable hotelXCiudad(string ciudad)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarHotelesPorCiudad";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@ciudad", ciudad);

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
        public DataTable consultarServicioAdicional()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarServicioAdicional";
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
        public DataTable consultarFacturaCliente()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarFacturaCliente";
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
        public DataTable consultarHotel()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarHotel";
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
        public DataTable consultarPisosXHotel(int Id_Hotel)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarPisosHotel";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id_Hotel);

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
        public DataTable consultarHabitaciones(int Id)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarHabitacionesContadas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id);

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
        public DataTable consultarTipoHabitacionId(int Id)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarTipoHabitacionId";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id);

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
        public DataTable consultarUbicacionHotel(int Id_Hotel)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarUbicacionHotel";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id_Hotel);

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
        public DataTable consultarPrecio(int Id_TipoHab)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarPrecio";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_TipoHab", Id_TipoHab);

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
        public DataTable consultarAmenidades(int Id_TipoHab)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarAmenidades";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_TipoHab", Id_TipoHab);

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
        public DataTable consultarCaracteristicas(int Id_TipoHab)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarCaracteristicas";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_TipoHab", Id_TipoHab);

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
        public DataTable consultarHabitacionesDisponibles(Reservaciones.Reservacion param)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarHabitacionesDisponibles";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_TipoHab", param.Id_TipoHab);
                _comandosql.Parameters.AddWithValue("@fechaLlegada", param.fechaLlegada);
                _comandosql.Parameters.AddWithValue("@fechaSalida", param.fechaSalida);

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
        public DataTable consultarServicio(int Id_Hotel)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarServicio";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id_Hotel);

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
        public DataTable consultarHabitacionesRestantes(int Id_Hotel)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_HabitacionesRestantes";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id_Hotel);

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
        public DataTable consultarReservaciones()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarReservaciones";
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
        public DataTable consultarDatosReservaciones(int Id_Reservacion)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarDatosReservaciones";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Reservacion", Id_Reservacion);

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
        public DataTable consultarCheckIn()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarCheckIn";
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
        public DataTable consultarCheckOut()
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarCheckOut";
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
        public DataTable consultarPrecioServicios(int Id_Hotel)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarPrecioServicios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id_Hotel);

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
        public DataTable consultarServiciosXHotel(int Id_Hotel)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarServicioAdicionalXHotel";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id_Hotel);

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
        public DataTable reporteOcupacion(Reportes.filtros param)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ReporteOcupacion";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@pais", param.pais);
                _comandosql.Parameters.AddWithValue("@ciudad", param.ciudad);
                _comandosql.Parameters.AddWithValue("@hotel", param.hotel);
                _comandosql.Parameters.AddWithValue("@anio", param.anio);

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
        public DataTable reporteOcupacionHotel(Reportes.filtros param)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ReporteOcupacionHotel";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@pais", param.pais);
                _comandosql.Parameters.AddWithValue("@ciudad", param.ciudad);
                _comandosql.Parameters.AddWithValue("@hotel", param.hotel);
                _comandosql.Parameters.AddWithValue("@anio", param.anio);

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
        public DataTable reporteVentas(Reportes.filtros param)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ReporteIngresosCompletos";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@pais", param.pais);
                _comandosql.Parameters.AddWithValue("@ciudad", param.ciudad);
                _comandosql.Parameters.AddWithValue("@hotel", param.hotel);
                _comandosql.Parameters.AddWithValue("@anio", param.anio);

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
        public DataTable factura(Factura.Filtro param)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ConsultarClienteFactura";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Apellidos", param.Apellidos);
                _comandosql.Parameters.AddWithValue("@RFC", param.RFC);
                _comandosql.Parameters.AddWithValue("@Correo", param.Correo);

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
        public DataTable facturaServicios(Factura.Filtro param)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "facturaServicios";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@RFC", param.RFC);

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
        public DataTable historialCliente(Reportes.filtros param)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_HistorialCliente";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@apellido1", param.primerApellido);
                _comandosql.Parameters.AddWithValue("@apellido2", param.segundoApellido);
                _comandosql.Parameters.AddWithValue("@correo", param.correo);
                _comandosql.Parameters.AddWithValue("@rfc", param.rfc);
                _comandosql.Parameters.AddWithValue("@anio", param.anio);

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
        public DataTable validarCheck(int Id_Reservacion)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ValidarCheck";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Reservacion", Id_Reservacion);

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
        public DataTable ValidarUbicacionUnica(string pais, string estado, string ciudad)
        {
            var msg = "";
            DataTable tabla = new DataTable();
            try
            {
                conectar();
                string qry = "SP_ValidarUbicacionUnica";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@pais", pais);
                _comandosql.Parameters.AddWithValue("@estado", estado);
                _comandosql.Parameters.AddWithValue("@ciudad", ciudad);

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
        public bool SP_InsertarHabitaciones(Hoteles.Habitacion param)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_InsertarHabitaciones";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                // Parámetros para la tabla Habitaciones
                _comandosql.Parameters.AddWithValue("@Id_Hotel", param.Id_Hotel);
                _comandosql.Parameters.AddWithValue("@Id_TipoHab", param.Id_TipoHab);
                _comandosql.Parameters.AddWithValue("@habitacionesAsigadas", param.NumeroHabitacion);
                _comandosql.Parameters.AddWithValue("@pisos", param.Piso);

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
                string qry = "SP_InsertarClientes";
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
                _comandosql.Parameters.AddWithValue("@correoCliente", param.Correo);

                _comandosql.Parameters.AddWithValue("@telefonoCelular", param.TelefonoCelular);
                _comandosql.Parameters.AddWithValue("@telefonoCasa", param.TelefonoCasa);

                _comandosql.Parameters.AddWithValue("@pais", param.Pais);
                _comandosql.Parameters.AddWithValue("@ciudad", param.Ciudad);
                _comandosql.Parameters.AddWithValue("@estado", param.Estado);
                _comandosql.Parameters.AddWithValue("@codigoPostal", param.CodigoPostal);
                _comandosql.Parameters.AddWithValue("@domicilio", param.Domicilio);

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
                _comandosql.Parameters.AddWithValue("@Id_Hotel", param.Id_Hotel);

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
        public bool Insertar_ServicioAdicional(Hoteles.ServicioAdicional param)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_InsertarServicioAdicional";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@nombreServicio", param.nombreServicio);
                _comandosql.Parameters.AddWithValue("@precio", param.costo);

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
        public bool Insertar_Hotel_ServicioAdicional(Hoteles.ServicioAdicional param, int Id_Hotel)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_InsertarHotel_ServicioAdicional";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_ServicioAdicional", param.Id_ServicioAdicional);
                _comandosql.Parameters.AddWithValue("@Id_Hotel", Id_Hotel);

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
        public bool Insertar_Reservacion_ServicioAdicional(Reservaciones.ServicioAdicional param, int Id_Reservacion)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_InsertarReservacion_ServicioAdicional";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_ServicioAdicional", param.Id_ServicioAdicional);
                _comandosql.Parameters.AddWithValue("@Id_Reservacion", Id_Reservacion);

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
        public bool Insertar_Hotel(Hoteles.Hotel param, out int ExtraidoId_Hotel)
        {
            var msg = "";
            var add = true;
            ExtraidoId_Hotel = -1;
            try
            {
                conectar();
                string qry = "SP_InsertarHotel";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Usuario", param.Id_Usuario);
                _comandosql.Parameters.AddWithValue("@Id_Ubicacion", param.Id_Ubicacion);
                _comandosql.Parameters.AddWithValue("@nombreHotel", param.NombreHotel);
                _comandosql.Parameters.AddWithValue("@zonaTuristica", param.ZonaTuristica);
                _comandosql.Parameters.AddWithValue("@numeroPisos", param.NumPisos);
                _comandosql.Parameters.AddWithValue("@fechaOperacion", param.FechaOperacion);
                _comandosql.Parameters.AddWithValue("@frentePlaya", param.FrentePlaya);
                _comandosql.Parameters.AddWithValue("@numeroPiscinas", param.NumPiscinas);
                _comandosql.Parameters.AddWithValue("@salonEventos", param.SalonEventos);
                _comandosql.Parameters.AddWithValue("@numHabitaciones", param.NumHabitaciones);
                _comandosql.Parameters.AddWithValue("@fechaRegistro", param.FechaRegistro);

                _comandosql.Parameters.AddWithValue("@pais", param.Pais);
                _comandosql.Parameters.AddWithValue("@ciudad", param.Ciudad);
                _comandosql.Parameters.AddWithValue("@estado", param.Estado);
                _comandosql.Parameters.AddWithValue("@codigoPostal", param.CodigoPostal);
                _comandosql.Parameters.AddWithValue("@domicilio", param.Domicilio);

                SqlParameter outputParam = new SqlParameter("@ExtraidoId_Hotel", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                _comandosql.Parameters.Add(outputParam);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
                ExtraidoId_Hotel = Convert.ToInt32(outputParam.Value);
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
        public bool InsertarReserva(Reservaciones.Reservacion param, out int Id_Reservacion)
        {
            var msg = "";
            var add = true;
            Id_Reservacion = -1;
            try
            {
                conectar();
                string qry = "SP_InsertarReserva";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Habitaciones", param.Id_Habitaciones);
                _comandosql.Parameters.AddWithValue("@Id_Cliente", param.Id_Cliente);
                _comandosql.Parameters.AddWithValue("@Id_Usuario", param.Id_Usuario);
                _comandosql.Parameters.AddWithValue("@codigoReserva", param.codigoReserva);
                _comandosql.Parameters.AddWithValue("@fechaLlegada", param.fechaLlegada);
                _comandosql.Parameters.AddWithValue("@fechaSalida", param.fechaSalida);
                _comandosql.Parameters.AddWithValue("@anticipo", param.anticipo);
                _comandosql.Parameters.AddWithValue("@restante", param.restante);
                _comandosql.Parameters.AddWithValue("@metodoPago", param.metodoPago);
                _comandosql.Parameters.AddWithValue("@personasHospedadas", param.personasHospedadas);
                _comandosql.Parameters.AddWithValue("@fechaRegistro", param.fechaRegistro);

                SqlParameter outputParam = new SqlParameter("@Id_Reservacion", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                _comandosql.Parameters.Add(outputParam);

                // Ejecutar el comando
                _comandosql.ExecuteNonQuery();
                add = true;
                Id_Reservacion = Convert.ToInt32(outputParam.Value);
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
        public bool Borrar_Cliente(int Id_Cliente)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_BorrarCliente";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                // Parámetros para la tabla Usuario
                _comandosql.Parameters.AddWithValue("@Id_Cliente", Id_Cliente);

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
        public bool Actualizar_Cliente(Clientes.ClienteDatos param)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_ActualizarClientes";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                // Parámetros para la tabla Usuario
                _comandosql.Parameters.AddWithValue("@Id_Cliente", param.Id_Cliente);
                _comandosql.Parameters.AddWithValue("@Id_Usuario", param.Id_Usuario);
                _comandosql.Parameters.AddWithValue("@nombreCliente", param.Nombre);
                _comandosql.Parameters.AddWithValue("@primerApellidoCliente", param.PrimerApellido);
                _comandosql.Parameters.AddWithValue("@segundoApellidoCliente", param.SegundoApellido);
                _comandosql.Parameters.AddWithValue("@fechaNacimientoCliente", param.FechaNacimiento);
                _comandosql.Parameters.AddWithValue("@rfcCliente", param.RFC);
                _comandosql.Parameters.AddWithValue("@correoCliente", param.Correo);
                _comandosql.Parameters.AddWithValue("@fechaModificacionCliente", param.FechaModificacion);

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
        public bool ActualizarReservacion(Reservaciones.Reservacion param)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_ActualizarEstatus";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Estatus", param.estatus);
                _comandosql.Parameters.AddWithValue("@Id_Reservacion", param.Id_Reservacion);
                _comandosql.Parameters.AddWithValue("@fechaCheckIn", param.fehcaCheckIn);
                _comandosql.Parameters.AddWithValue("@fechaCheckOut", param.fechaCheckOut);

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
        public bool borrarReservacion(int Id_Reservacion)
        {
            var msg = "";
            var add = true;
            try
            {
                conectar();
                string qry = "SP_BorrarReserva";
                _comandosql = new SqlCommand(qry, _conexion);
                _comandosql.CommandType = CommandType.StoredProcedure;
                _comandosql.CommandTimeout = 1200;

                _comandosql.Parameters.AddWithValue("@Id_Reservacion", Id_Reservacion);

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
