using System;
using System.IO;
using System.Windows.Forms;

namespace SistemaMesaPartes
{
    public partial class FormUsuarios : Form
    {
        private string rutaUsuarios = "usuarios.dat";
        private Usuario[] arregloUsuarios;
        private int totalUsuarios = 0;

        public FormUsuarios()
        {
            InitializeComponent();
            ConfigurarTabla();
            CargarUsuarios();
        }

        private void ConfigurarTabla()
        {
            dgvUsuarios.Columns.Clear();
            dgvUsuarios.Columns.Add("colUsuario", "Usuario (Login)");
            dgvUsuarios.Columns.Add("colClave", "Password");

            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
        }

        private void CargarUsuarios()
        {
            totalUsuarios = 0;
            dgvUsuarios.Rows.Clear();

            if (!File.Exists(rutaUsuarios))
            {
                arregloUsuarios = new Usuario[0];
                return;
            }

            int contador = 0;
            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    br.ReadString();
                    br.ReadString();
                    contador++;
                }
            }

            arregloUsuarios = new Usuario[contador];
            int idx = 0;

            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    Usuario u = new Usuario();
                    u.NombreUsuario = br.ReadString();
                    u.Clave = br.ReadString();
                    arregloUsuarios[idx] = u;
                    idx++;

                    dgvUsuarios.Rows.Add(u.NombreUsuario, u.Clave);
                }
            }

            totalUsuarios = contador;
        }

        // Registrar nuevo usuario desde el panel de administrador
        private void btnRegistrarUsuario_Click(object sender, EventArgs e)
        {
            string nuevoUser = txtNuevoUsuario.Text.Trim();
            string nuevaPass = txtNuevaClave.Text.Trim();

            if (string.IsNullOrWhiteSpace(nuevoUser) || string.IsNullOrWhiteSpace(nuevaPass))
            {
                MessageBox.Show("Ingrese Login y Password.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int i = 0; i < totalUsuarios; i++)
            {
                if (arregloUsuarios[i].NombreUsuario.Equals(nuevoUser, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Ese usuario ya está registrado.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Append, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(nuevoUser);
                bw.Write(nuevaPass);
            }

            MessageBox.Show("Usuario registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtNuevoUsuario.Clear();
            txtNuevaClave.Clear();
            CargarUsuarios();
        }

        // Eliminar usuario seleccionado (con condicional que impide borrar al admin)
        private void btnEliminarUsuario_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un usuario de la tabla para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string usuarioSeleccionado = dgvUsuarios.SelectedRows[0].Cells[0].Value.ToString();

            // CONDICIONAL PEDIDA POR EL PROFESOR: admin no se puede eliminar a sí mismo
            if (usuarioSeleccionado.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Operación denegada: el usuario administrador (admin) no puede eliminarse a sí mismo.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Seguro que desea eliminar al usuario '" + usuarioSeleccionado + "'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            // Reescribir el archivo binario excluyendo al usuario eliminado
            using (FileStream fs = new FileStream(rutaUsuarios, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                for (int i = 0; i < totalUsuarios; i++)
                {
                    if (!arregloUsuarios[i].NombreUsuario.Equals(usuarioSeleccionado, StringComparison.OrdinalIgnoreCase))
                    {
                        bw.Write(arregloUsuarios[i].NombreUsuario);
                        bw.Write(arregloUsuarios[i].Clave);
                    }
                }
            }

            MessageBox.Show("Usuario eliminado correctamente.", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarUsuarios();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}