using System.Windows.Forms;

namespace AAVD
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Login_Usuario = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Login_Contrasena = new System.Windows.Forms.TextBox();
            this.Login_Continuar = new System.Windows.Forms.Button();
            this.Login_SQL = new System.Windows.Forms.RadioButton();
            this.Login_CQL = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lexend", 20F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label1.Location = new System.Drawing.Point(76, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 43);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bienvenido!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lexend", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label2.Location = new System.Drawing.Point(104, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Iniciar Sesion";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Lexend", 10F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label3.Location = new System.Drawing.Point(68, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "Usuario:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Login_Usuario
            // 
            this.Login_Usuario.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Login_Usuario.Location = new System.Drawing.Point(88, 46);
            this.Login_Usuario.Name = "Login_Usuario";
            this.Login_Usuario.Size = new System.Drawing.Size(120, 20);
            this.Login_Usuario.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Lexend", 10F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label4.Location = new System.Drawing.Point(68, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "Contraseña:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Login_Contrasena
            // 
            this.Login_Contrasena.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Login_Contrasena.Location = new System.Drawing.Point(88, 103);
            this.Login_Contrasena.Name = "Login_Contrasena";
            this.Login_Contrasena.Size = new System.Drawing.Size(120, 20);
            this.Login_Contrasena.TabIndex = 5;
            this.Login_Contrasena.UseSystemPasswordChar = true;
            // 
            // Login_Continuar
            // 
            this.Login_Continuar.BackColor = System.Drawing.Color.MidnightBlue;
            this.Login_Continuar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Login_Continuar.Font = new System.Drawing.Font("Lexend", 7F, System.Drawing.FontStyle.Bold);
            this.Login_Continuar.ForeColor = System.Drawing.SystemColors.Window;
            this.Login_Continuar.Location = new System.Drawing.Point(129, 304);
            this.Login_Continuar.Name = "Login_Continuar";
            this.Login_Continuar.Size = new System.Drawing.Size(75, 23);
            this.Login_Continuar.TabIndex = 6;
            this.Login_Continuar.Text = "Continuar";
            this.Login_Continuar.UseVisualStyleBackColor = false;
            // 
            // Login_SQL
            // 
            this.Login_SQL.AutoSize = true;
            this.Login_SQL.Font = new System.Drawing.Font("Lexend", 10F, System.Drawing.FontStyle.Bold);
            this.Login_SQL.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Login_SQL.Location = new System.Drawing.Point(84, 385);
            this.Login_SQL.Name = "Login_SQL";
            this.Login_SQL.Size = new System.Drawing.Size(57, 26);
            this.Login_SQL.TabIndex = 8;
            this.Login_SQL.TabStop = true;
            this.Login_SQL.Text = "SQL";
            this.Login_SQL.UseVisualStyleBackColor = true;
            // 
            // Login_CQL
            // 
            this.Login_CQL.AutoSize = true;
            this.Login_CQL.Font = new System.Drawing.Font("Lexend", 10F, System.Drawing.FontStyle.Bold);
            this.Login_CQL.ForeColor = System.Drawing.Color.MidnightBlue;
            this.Login_CQL.Location = new System.Drawing.Point(197, 385);
            this.Login_CQL.Name = "Login_CQL";
            this.Login_CQL.Size = new System.Drawing.Size(58, 26);
            this.Login_CQL.TabIndex = 9;
            this.Login_CQL.TabStop = true;
            this.Login_CQL.Text = "CQL";
            this.Login_CQL.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Lexend", 10F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.MidnightBlue;
            this.label5.Location = new System.Drawing.Point(78, 360);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(177, 22);
            this.label5.TabIndex = 7;
            this.label5.Text = "Elige una base de datos";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(350, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(450, 450);
            this.panel2.TabIndex = 10;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.Window;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 450);
            this.panel3.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Login_Usuario);
            this.groupBox1.Controls.Add(this.Login_Contrasena);
            this.groupBox1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.groupBox1.Location = new System.Drawing.Point(49, 145);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 134);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos de inicio";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Lexend", 19F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.SystemColors.Window;
            this.label6.Location = new System.Drawing.Point(91, 360);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(282, 41);
            this.label6.TabIndex = 12;
            this.label6.Text = "HOTEL TIGRE AZUL";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::AAVD.Properties.Resources.logo_blanco;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(98, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(270, 254);
            this.panel1.TabIndex = 11;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::AAVD.Properties.Resources.lock_azul;
            this.pictureBox3.Location = new System.Drawing.Point(18, 82);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(44, 41);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 12;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AAVD.Properties.Resources.user_azul;
            this.pictureBox2.Location = new System.Drawing.Point(18, 25);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(44, 41);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Login_CQL);
            this.Controls.Add(this.Login_SQL);
            this.Controls.Add(this.Login_Continuar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Inicio de sesion";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Login_Usuario;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Login_Contrasena;
        private System.Windows.Forms.Button Login_Continuar;
        private System.Windows.Forms.RadioButton Login_SQL;
        private System.Windows.Forms.RadioButton Login_CQL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private Panel panel1;
        private Panel panel3;
        private GroupBox groupBox1;
        private Label label6;
    }
}

