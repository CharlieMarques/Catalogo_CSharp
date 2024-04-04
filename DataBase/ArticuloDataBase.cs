using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace DataBase
{
    public class ArticuloDataBase
    {
        public List<Articulo> toList()
        {
            List<Articulo> list = new List<Articulo>();
             AccesoDatos data = new AccesoDatos();
            try
            {
                data.setQuery("Select A.Id,A.Codigo, A.Nombre,A.Descripcion,Precio, C.Descripcion Categoria, M.Descripcion Marca, A.ImagenUrl, A.IdCategoria, A.IdMarca From ARTICULOS A, CATEGORIAS C, MARCAS M Where A.IdCategoria = C.Id and A.IdMarca = M.Id");
                data.read();
                while (data.Reader.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)data.Reader["Id"];
                    aux.codArticulo = (string)data.Reader["Codigo"];
                    aux.descripcion = (string)data.Reader["Descripcion"];
                    aux.nombre = (string)data.Reader["Nombre"];
                    aux.urlImagen = (string)data.Reader["ImagenUrl"];
                    aux.Precio = decimal.Parse(data.Reader["Precio"].ToString());
                    aux.categoria = new Categoria();
                    aux.categoria.Id = (int)data.Reader["IdCategoria"];
                    aux.categoria.Descripcion = (string)data.Reader["Categoria"];
                    aux.marca = new Marca();
                    aux.marca.Id = (int)data.Reader["IdMarca"];
                    aux.marca.Descripcion = (string)data.Reader["Marca"];
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
        public void agregar(Articulo articulo)
        {
            AccesoDatos dataAgregar = new AccesoDatos();
            try
            {
                dataAgregar.setQuery("Insert into ARTICULOS(Codigo, Nombre, Descripcion, ImagenUrl, Precio, IdMarca, IdCategoria) Values (@Codigo,@Nombre,@Descripcion,@ImagenUrl,@Precio, @IdMarca, @IdCategoria)");
                
                
                dataAgregar.setParameter("@Codigo", articulo.codArticulo);
                dataAgregar.setParameter("@Nombre", articulo.nombre);
                if (articulo.descripcion == null || articulo.descripcion =="")
                {
                    articulo.descripcion = "Sin Descripcion";
                }
                dataAgregar.setParameter("@Descripcion", articulo.descripcion);
                dataAgregar.setParameter("@IdMarca", articulo.marca.Id);
                dataAgregar.setParameter("@IdCategoria", articulo.categoria.Id);
                dataAgregar.setParameter("@ImagenUrl", articulo.urlImagen);
                dataAgregar.setParameter("@Precio", articulo.Precio);
                dataAgregar.runQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                dataAgregar.closeConnection();
            }
        }
            public void modificar(Articulo articulo)
        {
            AccesoDatos dataModificar = new AccesoDatos();
            try
            {
                dataModificar.setQuery("update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @ImagenUrl, Precio = @Precio where Id = @Id");
                dataModificar.setParameter("@Id", articulo.Id);
                dataModificar.setParameter("@Codigo", articulo.codArticulo);
                dataModificar.setParameter("@Nombre", articulo.nombre);
                if (articulo.descripcion == null || articulo.descripcion == "")
                {
                    articulo.descripcion = "Sin Descripcion";
                }
                dataModificar.setParameter("@Descripcion", articulo.descripcion);
                dataModificar.setParameter("@IdMarca", articulo.marca.Id);
                dataModificar.setParameter("@IdCategoria", articulo.categoria.Id);
                dataModificar.setParameter("@ImagenUrl", articulo.urlImagen);
                dataModificar.setParameter("@Precio", articulo.Precio);
                dataModificar.runQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                dataModificar.closeConnection();
            }
        }
        public void eliminar(int Id)
        {
            AccesoDatos data = new AccesoDatos();
            try
            {
                data.setQuery("Delete From ARTICULOS Where id = @Id");
                data.setParameter("@Id", Id);
                data.runQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> list = new List<Articulo>();
            AccesoDatos data = new AccesoDatos();
            try
            {
                string consulta = "Select A.Id,A.Codigo, A.Nombre,A.Descripcion,Precio, C.Descripcion Categoria, M.Descripcion Marca, A.ImagenUrl, A.IdCategoria, A.IdMarca From ARTICULOS A, CATEGORIAS C, MARCAS M Where A.IdCategoria = C.Id and A.IdMarca = M.Id and ";

                switch (campo)
                {
                    case "Precio":
                        switch (criterio)
                        {

                            case "Mayor a":
                                consulta += "Precio > " + filtro;
                                break;
                            case "Menor a":
                                consulta += "Precio < " + filtro;
                                break;
                            case "Hasta":
                                consulta += "Precio <= " + filtro;
                                break;
                            default:
                                consulta += "Precio " + filtro;
                                break;

                        }

                        break;
                    case "Nombre":
                        switch(criterio)
                        {
                            case "Empieza con":
                                consulta += "A.Nombre like '"+ filtro+ "%' ";
                                break;
                            case "Termina con":
                                consulta += "A.nombre like '%" + filtro + "' ";
                                break;
                            case "Contiene":
                                consulta += "A.Nombre like '%" + filtro + "%' ";
                                break;
                            default:
                                consulta += "A.Nombre like '%" + filtro + "%' ";
                                break;
                        }

                        break;
                    case "Descripción":
                        switch (criterio)
                        {
                            case "Empieza con":
                                consulta += "A.Descripcion like '" + filtro + "%' ";
                                break;
                            case "Termina con":
                                consulta += "A.Descripcion like '%" + filtro + "' ";
                                break;
                            case "Contiene":
                                consulta += "A.Descripcion like '%" + filtro + "%' ";
                                break;
                            case "Todos":
                                consulta += "A.Descripcion like '%" + filtro + "%' ";
                                break;
                            default:                               
                                break;
                        }
                        break;
                    default: 

                        break;
                }
                
                data.setQuery(consulta);
                data.read();
                while (data.Reader.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)data.Reader["Id"];
                    aux.codArticulo = (string)data.Reader["Codigo"];
                    aux.descripcion = (string)data.Reader["Descripcion"];
                    aux.nombre = (string)data.Reader["Nombre"];
                    aux.urlImagen = (string)data.Reader["ImagenUrl"];
                    aux.Precio = decimal.Parse(data.Reader["Precio"].ToString());
                    aux.categoria = new Categoria();
                    aux.categoria.Id = (int)data.Reader["IdCategoria"];
                    aux.categoria.Descripcion = (string)data.Reader["Categoria"];
                    aux.marca = new Marca();
                    aux.marca.Id = (int)data.Reader["IdMarca"];
                    aux.marca.Descripcion = (string)data.Reader["Marca"];
                    list.Add(aux);
                }


                return list;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
