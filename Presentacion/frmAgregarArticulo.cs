using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using DataBase;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace Presentacion
{
    public partial class frmAgregarArticulo : Form
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string folderImage = ConfigurationManager.AppSettings["Imagenes-Catalago"];
        string directoryFull;

        private Articulo articulo = null;
        private OpenFileDialog image = null;


        public frmAgregarArticulo()
        {
            InitializeComponent();
         directoryFull = System.IO.Path.Combine(baseDirectory, folderImage);
        }
        public frmAgregarArticulo(Articulo articulo)
        {
         directoryFull = System.IO.Path.Combine(baseDirectory, folderImage);
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";

        }
        private bool numberOnly(string cadena)
        { int floatCount = 0;
            foreach (char character in cadena)
            {
                if (!(char.IsNumber(character) || character ==',' || character == '.'))
                {
                    MessageBox.Show("Solo numeros para el campo de precio");
                    return false;
                }
                if(character == '.' || character == ',')
                {
                    floatCount++;
                    if(floatCount > 1)
                    {
                        MessageBox.Show("No colocar mas de un punto decimal");
                        return false;
                    }
                }
            }
            return true;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {           
            ArticuloDataBase data = new ArticuloDataBase();
            try
            {
                if (articulo == null)
                {
                    articulo = new Articulo();
                }
                string precio;
                articulo.codArticulo = tbCodArticulo.Text;
                articulo.nombre = tbNombre.Text;
                articulo.descripcion = tbDescripcion.Text;
                articulo.urlImagen = tbImagen.Text;
                articulo.marca = (Marca)cbxMarca.SelectedItem;
                articulo.categoria = (Categoria)cbxCategoria.SelectedItem;

                 if(string.IsNullOrEmpty(articulo.codArticulo)||string.IsNullOrEmpty(articulo.nombre) || string.IsNullOrEmpty(tbPrecio.Text))
                { 
                    MessageBox.Show("Los campos con * no pueden quedar vacios");
                }
               else if (!(numberOnly(tbPrecio.Text)))
                {
                   // MessageBox.Show("Solo numeros por favor");
                }
                else
                {
                    precio = tbPrecio.Text;
                    precio = precio.Replace('.', ',');
                    articulo.Precio = decimal.Parse(precio);
                    if (articulo.Id != 0)
                    {

                        if (image != null && !(tbImagen.Text.ToUpper().Contains("HTTP")))
                        {
                            existingFile();
                        }
                        data.modificar(articulo);
                        MessageBox.Show("Modificado con Exito");
                    }
                    else
                    {
                        if (image != null && !(tbImagen.Text.ToUpper().Contains("HTTP")))
                        {
                            existingFile();
                        }
                        data.agregar(articulo);
                        MessageBox.Show("Agregado con Exito");
                    }              
                Close();
                } 

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAgregarArticulo_Load(object sender, EventArgs e)
        {
            MarcaDataBase marca = new MarcaDataBase();
            CategoriaDataBase categoria = new CategoriaDataBase();

            try
            {
                cbxMarca.DataSource = marca.toList();
                cbxMarca.ValueMember = "id";
                cbxMarca.DisplayMember = "Descripcion";
                cbxCategoria.DataSource = categoria.toList();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";
                if (articulo != null)
                {
                    tbCodArticulo.Text = articulo.codArticulo;
                    tbDescripcion.Text = articulo.descripcion;
                    tbImagen.Text = articulo.urlImagen;
                    tbNombre.Text = articulo.nombre;
                    loadImage(articulo.urlImagen);
                    tbPrecio.Text = articulo.Precio.ToString();
                    cbxMarca.SelectedValue = articulo.marca.Id;
                    cbxCategoria.SelectedValue = articulo.categoria.Id;


                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        private void tbImagen_Leave(object sender, EventArgs e)
        {
            loadImage(tbImagen.Text);
        }
        private void loadImage(string image)
        {
            try
            {
                pbxAgregar.Load(image);

            }
            catch (Exception)
            {

                pbxAgregar.Load(directoryFull + "imagenVacia.jpg");
            }

        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
                image = new OpenFileDialog();

                image.Filter = "jpg|*.jpg;|png|*.png";

                if (image.ShowDialog() == DialogResult.OK)
                {
                    directoryFull = System.IO.Path.Combine(baseDirectory, folderImage);
                    tbImagen.Text = directoryFull + image.SafeFileName;
                    loadImage(image.FileName);

                    //existingFile();
                }
                


         }
        private void existingFile()
        {
                 if (!(File.Exists(image.SafeFileName)))
                {
                    DialogResult result = MessageBox.Show("El nombre del archivo ya existe desea reemplazarlo?", "Reemplazar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {                  
                    File.Delete(directoryFull + image.SafeFileName);
                    File.Copy(image.FileName, directoryFull + image.SafeFileName);
                }
                }
                 else
                {
                    File.Copy(image.FileName, directoryFull + image.SafeFileName);
                }

        }
        
    }
}
