using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AAVD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }

    public class Usuarios
    {
        public string Login_usuario;
        public string Login_contrasena;
        public int id_TipoUsuario;
        public int id_BaseDatos;
        
        //Constructor
        public Usuarios() { }

    }
}
