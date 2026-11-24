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
        }

        private bool ValidarCampos(string usuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(clave))
            {
                MessageBox.Show("Por favor, complete usuario y contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (clave.Length < 4)
            {
                MessageBox.Show("La contraseña debe tener al menos 4 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool UsuarioExiste(string usuarioBuscado)
        {
            if (!File.Exists(rutaUsuarios)) return false;

            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    Usuario u = new Usuario();
                    u.NombreUsuario = br.ReadString();
                    u.Clave = br.ReadString();

                    if (u.NombreUsuario.Equals(usuarioBuscado, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();

            if (!ValidarCampos(usuario, clave)) return;

            if (UsuarioExiste(usuario))
            {
                MessageBox.Show("Ese nombre de usuario ya está registrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Append, FileAccess.Write))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    Usuario nuevo = new Usuario();
                    nuevo.NombreUsuario = usuario;
                    nuevo.Clave = clave;

                    bw.Write(nuevo.NombreUsuario);
                    bw.Write(nuevo.Clave);
                }

                MessageBox.Show("¡Usuario registrado con éxito! Ahora dale clic a Ingresar.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text.Trim();

            if (!ValidarCampos(usuario, clave)) return;

            if (!File.Exists(rutaUsuarios))
            {
                MessageBox.Show("No hay usuarios registrados aún. Regístrese primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool accesoConcedido = false;

            try
            {
                using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        Usuario u = new Usuario();
                        u.NombreUsuario = br.ReadString();
                        u.Clave = br.ReadString();

                        if (u.NombreUsuario.Equals(usuario, StringComparison.OrdinalIgnoreCase) && u.Clave == clave)
                        {
                            accesoConcedido = true;
                            break;
                        }
                    }
                }

                if (accesoConcedido)
                {
                    MessageBox.Show("Bienvenido(a) al sistema, " + usuario, "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FormPrincipal menu = new FormPrincipal();
                    menu.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}