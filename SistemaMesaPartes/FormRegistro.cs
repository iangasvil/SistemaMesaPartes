using System;
using System.IO;
using System.Windows.Forms;

namespace SistemaMesaPartes
{
    public partial class FormRegistro : Form
    {
        private string rutaExpedientes = "expedientes.dat";

        public FormRegistro()
        {
            InitializeComponent();
            CargarTiposDocumento();
            PrepararCodigoAutomatico();
        }

        private void PrepararCodigoAutomatico()
        {
            txtCodigo.ReadOnly = true;
            txtCodigo.Text = GenerarSiguienteCodigo();
        }

        private string GenerarSiguienteCodigo()
        {
            int maximoNumero = 0;

            if (File.Exists(rutaExpedientes))
            {
                using (FileStream fs = new FileStream(rutaExpedientes, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (fs.Position < fs.Length)
                    {
                        string cod = br.ReadString(); // Codigo
                        br.ReadString(); // Dni
                        br.ReadString(); // Nombres
                        br.ReadString(); // TipoDocumento
                        br.ReadString(); // Asunto
                        br.ReadString(); // FechaRegistro

                        string soloNumero = cod.Replace("EXP-", "");
                        int num;
                        if (int.TryParse(soloNumero, out num) && num > maximoNumero)
                        {
                            maximoNumero = num;
                        }
                    }
                }
            }

            return "EXP-" + (maximoNumero + 1).ToString("D3");
        }

        private void CargarTiposDocumento()
        {
            cboTipoDoc.Items.Clear();
            cboTipoDoc.Items.Add("Solicitud");
            cboTipoDoc.Items.Add("Oficio");
            cboTipoDoc.Items.Add("Carta");
            cboTipoDoc.Items.Add("Informe");
            cboTipoDoc.Items.Add("Reclamo");
            cboTipoDoc.SelectedIndex = 0;
        }

        private bool ValidarDni(string dni)
        {
            if (dni.Length != 8)
            {
                return false;
            }

            foreach (char c in dni)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text.Trim();
            string dni = txtDni.Text.Trim();
            string nombres = txtNombres.Text.Trim();
            string tipoDoc = cboTipoDoc.SelectedItem.ToString();
            string asunto = txtAsunto.Text.Trim();

            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(nombres) || string.IsNullOrWhiteSpace(asunto))
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarDni(dni))
            {
                MessageBox.Show("El DNI debe contener exactamente 8 dígitos numéricos.", "DNI Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDni.Focus();
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(rutaExpedientes, FileMode.Append, FileAccess.Write))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    Expediente nuevo = new Expediente();
                    nuevo.Codigo = codigo;
                    nuevo.Dni = dni;
                    nuevo.Nombres = nombres;
                    nuevo.TipoDocumento = tipoDoc;
                    nuevo.Asunto = asunto;
                    nuevo.FechaRegistro = DateTime.Now.ToString("dd/MM/yyyy");

                    bw.Write(nuevo.Codigo);
                    bw.Write(nuevo.Dni);
                    bw.Write(nuevo.Nombres);
                    bw.Write(nuevo.TipoDocumento);
                    bw.Write(nuevo.Asunto);
                    bw.Write(nuevo.FechaRegistro);
                }

                MessageBox.Show("Expediente " + codigo + " guardado correctamente.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el expediente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            txtCodigo.Text = GenerarSiguienteCodigo();
            txtDni.Clear();
            txtNombres.Clear();
            cboTipoDoc.SelectedIndex = 0;
            txtAsunto.Clear();
            txtDni.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}