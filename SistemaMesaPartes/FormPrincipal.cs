using System;
using System.Windows.Forms;

namespace SistemaMesaPartes
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnIrRegistro_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Registro en construcción.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnIrGestion_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo de Gestión en construcción.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            FormLogin login = new FormLogin();
            login.Show();
            this.Close();
        }
    }
}