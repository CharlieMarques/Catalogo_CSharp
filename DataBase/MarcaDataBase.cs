using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace DataBase
{
    public class MarcaDataBase
    {
        public List<Marca> toList()
        {
            List<Marca> list = new List<Marca>();
            AccesoDatos data = new AccesoDatos();

            try
            {
                data.setQuery("Select Id, Descripcion From MARCAS");
                data.read();
                while(data.Reader.Read())

                {
                    Marca aux = new Marca();
                    aux.Id = (int)data.Reader["Id"];
                    aux.Descripcion = (string)data.Reader["Descripcion"];
                    list.Add(aux);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                data.closeConnection();
            }
        }
    }
}
