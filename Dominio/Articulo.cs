using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {

        public string codArticulo { get; set; }
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        public string descripcion { get; set; }

        public string urlImagen { get; set; }
        [DisplayName("Precio")]
        public decimal Precio {  get; set; }
        [DisplayName("Marca")]
        public Marca marca { get; set; }
        [DisplayName("Categoria")]
        public Categoria categoria { get; set; }
        public int Id {  get; set; }
    }
}
