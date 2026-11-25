using System;
using System.IO;
using System.Windows.Forms;

namespace SistemaMesaPartes
{
    public partial class FormLogin : Form
    {
        private string rutaUsuarios = "usuarios.dat";

        public FormLogin()
        {
            InitializeComponent();
            AsegurarAdminPorDefecto();
        }

        private void AsegurarAdminPorDefecto()
        {
            if (UsuarioExiste("admin")) return;

            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Append, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write("admin");
                bw.Write("admin123");
            }
        }

        private bool UsuarioExiste(string usuarioBuscado)
        {
            if (!File.Exists(rutaUsuarios)) return false;

            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    string u = br.ReadString();
                    string c = br.ReadString();
                    if (u.Equals(usuarioBuscado, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Ingrese usuario y contraseña.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!File.Exists(rutaUsuarios))
            {
                MessageBox.Show("No hay usuarios registrados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool accesoConcedido = false;
            string usuarioReal = "";

            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    string u = br.ReadString();
                    string c = br.ReadString();

                    if (u.Equals(usuario, StringComparison.OrdinalIgnoreCase) && c == clave)
                    {
                        accesoConcedido = true;
                        usuarioReal = u;
                        break;
                    }
                }
            }

            if (accesoConcedido)
            {
                this.Hide();
                FormPrincipal principal = new FormPrincipal(usuarioReal);
                principal.ShowDialog();
                txtClave.Clear();
                this.Show();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();

            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Complete usuario y contraseña para registrarse.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UsuarioExiste(usuario))
            {
                MessageBox.Show("Ese nombre de usuario ya existe.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Append, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(usuario);
                bw.Write(clave);
            }

            MessageBox.Show("Usuario registrado correctamente. Ahora puede ingresar.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}