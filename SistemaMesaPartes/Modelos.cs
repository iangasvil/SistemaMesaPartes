using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaMesaPartes
{
    public struct Usuario
    {
        public string NombreUsuario;
        public string Clave;
    }

    public struct Expediente
    {
        public string Codigo;
        public string Dni;
        public string Nombres;
        public string TipoDocumento;
        public string Asunto;
        public string FechaRegistro;
    }
}
