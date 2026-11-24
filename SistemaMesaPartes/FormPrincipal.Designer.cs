namespace SistemaMesaPartes
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.btnIrRegistro = new System.Windows.Forms.Button();
            this.btnIrGestion = new System.Windows.Forms.Button();
            this.btnCerrarSesion = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(165, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(391, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mesa de Partes Digital - Menú Principal";
            // 
            // btnIrRegistro
            // 
            this.btnIrRegistro.Location = new System.Drawing.Point(286, 123);
            this.btnIrRegistro.Name = "btnIrRegistro";
            this.btnIrRegistro.Size = new System.Drawing.Size(180, 35);
            this.btnIrRegistro.TabIndex = 1;
            this.btnIrRegistro.Text = "Registrar Expediente";
            this.btnIrRegistro.UseVisualStyleBackColor = true;
            this.btnIrRegistro.Click += new System.EventHandler(this.btnIrRegistro_Click);
            // 
            // btnIrGestion
            // 
            this.btnIrGestion.Location = new System.Drawing.Point(286, 197);
            this.btnIrGestion.Name = "btnIrGestion";
            this.btnIrGestion.Size = new System.Drawing.Size(180, 35);
            this.btnIrGestion.TabIndex = 2;
            this.btnIrGestion.Text = "Gestionar y Buscar";
            this.btnIrGestion.UseVisualStyleBackColor = true;
            this.btnIrGestion.Click += new System.EventHandler(this.btnIrGestion_Click);
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.Location = new System.Drawing.Point(286, 260);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Size = new System.Drawing.Size(180, 35);
            this.btnCerrarSesion.TabIndex = 3;
            this.btnCerrarSesion.Text = "Cerrar Sesión";
            this.btnCerrarSesion.UseVisualStyleBackColor = true;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCerrarSesion);
            this.Controls.Add(this.btnIrGestion);
            this.Controls.Add(this.btnIrRegistro);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnIrRegistro;
        private System.Windows.Forms.Button btnIrGestion;
        private System.Windows.Forms.Button btnCerrarSesion;
    }
}