using System;
using System.IO;
using System.Windows.Forms;

namespace SistemaMesaPartes
{
    public partial class FormGestion : Form
    {
        private string rutaExpedientes = "expedientes.dat";
        private Expediente[] arregloExpedientes;
        private int totalExpedientes = 0;
        private string usuarioActual;

        public FormGestion()
        {
            InitializeComponent();
            ConfigurarTabla();
            CargarDesdeArchivo();
            MostrarEnTabla(arregloExpedientes, totalExpedientes);
        }

        // Constructor que recibe quién inició sesión para habilitar/bloquear Eliminar
        public FormGestion(string usuarioLogueado) : this()
        {
            usuarioActual = usuarioLogueado;
            btnEliminar.Enabled = (usuarioActual != null && usuarioActual.Equals("admin", StringComparison.OrdinalIgnoreCase));
        }

        private void ConfigurarTabla()
        {
            dgvExpedientes.Columns.Clear();
            dgvExpedientes.Columns.Add("colCodigo", "Código");
            dgvExpedientes.Columns.Add("colDni", "DNI");
            dgvExpedientes.Columns.Add("colNombres", "Nombres");
            dgvExpedientes.Columns.Add("colTipo", "Tipo Documento");
            dgvExpedientes.Columns.Add("colAsunto", "Asunto");
            dgvExpedientes.Columns.Add("colFecha", "Fecha Registro");

            dgvExpedientes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExpedientes.RowHeadersVisible = false; // Oculta el margen vacío de la izquierda

            // Reparte el ancho proporcionalmente según el contenido de cada columna
            dgvExpedientes.Columns["colCodigo"].FillWeight = 65;
            dgvExpedientes.Columns["colDni"].FillWeight = 70;
            dgvExpedientes.Columns["colNombres"].FillWeight = 145;
            dgvExpedientes.Columns["colTipo"].FillWeight = 85;
            dgvExpedientes.Columns["colAsunto"].FillWeight = 220;
            dgvExpedientes.Columns["colFecha"].FillWeight = 80;

            dgvExpedientes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExpedientes.ReadOnly = true;
            dgvExpedientes.AllowUserToAddRows = false;
        }

        private void CargarDesdeArchivo()
        {
            totalExpedientes = 0;

            if (!File.Exists(rutaExpedientes))
            {
                arregloExpedientes = new Expediente[0];
                return;
            }

            int contador = 0;
            using (FileStream fs = new FileStream(rutaExpedientes, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    br.ReadString();
                    br.ReadString();
                    br.ReadString();
                    br.ReadString();
                    br.ReadString();
                    br.ReadString();
                    contador++;
                }
            }

            arregloExpedientes = new Expediente[contador];
            int indice = 0;

            using (FileStream fs = new FileStream(rutaExpedientes, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                while (fs.Position < fs.Length)
                {
                    Expediente exp = new Expediente();
                    exp.Codigo = br.ReadString();
                    exp.Dni = br.ReadString();
                    exp.Nombres = br.ReadString();
                    exp.TipoDocumento = br.ReadString();
                    exp.Asunto = br.ReadString();
                    exp.FechaRegistro = br.ReadString();

                    arregloExpedientes[indice] = exp;
                    indice++;
                }
            }

            totalExpedientes = contador;
        }

        private void MostrarEnTabla(Expediente[] lista, int cantidad)
        {
            dgvExpedientes.Rows.Clear();
            if (lista == null || cantidad == 0) return;

            for (int i = 0; i < cantidad; i++)
            {
                dgvExpedientes.Rows.Add(
                    lista[i].Codigo,
                    lista[i].Dni,
                    lista[i].Nombres,
                    lista[i].TipoDocumento,
                    lista[i].Asunto,
                    lista[i].FechaRegistro
                );
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string criterio = txtBuscar.Text.Trim();

            if (string.IsNullOrWhiteSpace(criterio))
            {
                MessageBox.Show("Ingrese un DNI o Código para buscar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBuscar.Focus();
                return;
            }

            dgvExpedientes.Rows.Clear();
            bool encontrado = false;

            for (int i = 0; i < totalExpedientes; i++)
            {
                bool coincideDni = arregloExpedientes[i].Dni.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0;
                bool coincideCodigo = arregloExpedientes[i].Codigo.IndexOf(criterio, StringComparison.OrdinalIgnoreCase) >= 0;

                if (coincideDni || coincideCodigo)
                {
                    dgvExpedientes.Rows.Add(
                        arregloExpedientes[i].Codigo,
                        arregloExpedientes[i].Dni,
                        arregloExpedientes[i].Nombres,
                        arregloExpedientes[i].TipoDocumento,
                        arregloExpedientes[i].Asunto,
                        arregloExpedientes[i].FechaRegistro
                    );
                    encontrado = true;
                }
            }

            if (!encontrado)
            {
                MessageBox.Show("No se encontraron expedientes con ese DNI o Código.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MostrarEnTabla(arregloExpedientes, totalExpedientes);
            }
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            CargarDesdeArchivo();
            MostrarEnTabla(arregloExpedientes, totalExpedientes);
        }

        private void btnOrdenar_Click(object sender, EventArgs e)
        {
            if (totalExpedientes < 2)
            {
                MessageBox.Show("Se necesitan al menos 2 expedientes para ordenar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            for (int i = 0; i < totalExpedientes - 1; i++)
            {
                for (int j = 0; j < totalExpedientes - 1 - i; j++)
                {
                    if (string.Compare(arregloExpedientes[j].Nombres, arregloExpedientes[j + 1].Nombres, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        Expediente temp = arregloExpedientes[j];
                        arregloExpedientes[j] = arregloExpedientes[j + 1];
                        arregloExpedientes[j + 1] = temp;
                    }
                }
            }

            MostrarEnTabla(arregloExpedientes, totalExpedientes);
            MessageBox.Show("Expedientes ordenados alfabéticamente por Nombre.", "Ordenamiento Burbuja", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Eliminar expediente seleccionado (solo habilitado para admin)
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvExpedientes.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un expediente de la tabla para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string codigoSeleccionado = dgvExpedientes.SelectedRows[0].Cells[0].Value.ToString();

            DialogResult confirmacion = MessageBox.Show("¿Seguro que desea eliminar el expediente " + codigoSeleccionado + "?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmacion != DialogResult.Yes) return;

            using (FileStream fs = new FileStream(rutaExpedientes, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                for (int i = 0; i < totalExpedientes; i++)
                {
                    if (!arregloExpedientes[i].Codigo.Equals(codigoSeleccionado, StringComparison.OrdinalIgnoreCase))
                    {
                        bw.Write(arregloExpedientes[i].Codigo);
                        bw.Write(arregloExpedientes[i].Dni);
                        bw.Write(arregloExpedientes[i].Nombres);
                        bw.Write(arregloExpedientes[i].TipoDocumento);
                        bw.Write(arregloExpedientes[i].Asunto);
                        bw.Write(arregloExpedientes[i].FechaRegistro);
                    }
                }
            }

            MessageBox.Show("Expediente eliminado correctamente.", "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            CargarDesdeArchivo();
            MostrarEnTabla(arregloExpedientes, totalExpedientes);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}