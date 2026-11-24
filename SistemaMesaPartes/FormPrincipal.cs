using System;
using System.Windows.Forms;

namespace SistemaMesaPartes
{
    public partial class FormPrincipal : Form
    {
        private string usuarioActual;

        public FormPrincipal()
        {
            InitializeComponent();
        }

        public FormPrincipal(string usuarioLogueado)
        {
            InitializeComponent();
            usuarioActual = usuarioLogueado;
            ConfigurarPermisos();
        }

        // Condicional: solo el administrador tiene habilitada la gestión de usuarios
        private void ConfigurarPermisos()
        {
            if (usuarioActual != null && usuarioActual.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                btnUsuarios.Enabled = true;
            }
            else
            {
                btnUsuarios.Enabled = false;
            }
        }

        private void btnIrRegistro_Click(object sender, EventArgs e)
        {
            FormRegistro registro = new FormRegistro();
            registro.ShowDialog();
        }

        private void btnIrGestion_Click(object sender, EventArgs e)
        {
            FormGestion gestion = new FormGestion(usuarioActual);
            gestion.ShowDialog();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            FormUsuarios gestionUsuarios = new FormUsuarios();
            gestionUsuarios.ShowDialog();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}