using DataBase;
using Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class frmArticulos : Form
    {
        private List<Articulo> listArticulo;
        Articulo selected = null;
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string directoryFull;
        string folderImage = ConfigurationManager.AppSettings["Imagenes-Catalago"];

        public frmArticulos()
        {
            InitializeComponent();
        }

        private void loader()
        {
            ArticuloDataBase dataBase = new ArticuloDataBase();
            listArticulo = dataBase.toList();
            
            dgvArticulos.DataSource = listArticulo;

            hideColums();
            loadImage(listArticulo[0].urlImagen);
        }
        private void hideColums()
        {
            dgvArticulos.Columns["Id"].Visible = false;
            dgvArticulos.Columns["codArticulo"].Visible = false;
            dgvArticulos.Columns["urlImagen"].Visible = false;
            dgvArticulos.Columns["descripcion"].Visible = false;
        }
        private bool noSelection(ComboBox cbx)
        {
            if (cbx.SelectedIndex < 0)
            {
                return true;
            }
            return false;
        }

       private bool numberOnly (string cadena)
        {
            foreach (char character in cadena)
            {
                if(!(char.IsNumber(character)))
                {
                    return false;
                }
            }
            return true;
        }

        private void frmArticulos_Load_1(object sender, EventArgs e)
        {
            loader();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");
            cboCampo.Items.Add("Precio");

        }
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
            Articulo select = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            loadImage(select.urlImagen);
            }
        }
        private void loadImage(string image)

        {
            directoryFull = System.IO.Path.Combine(baseDirectory, folderImage);
            try
            {
            pbxArticulo.Load(image);

            }

            catch (Exception)
            {

                pbxArticulo.Load(directoryFull + "imagenVacia.jpg");
              
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgregarArticulo agregarArticulo = new frmAgregarArticulo();
            agregarArticulo.ShowDialog();
            loader();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {    if(dgvArticulos.SelectedRows.Count != 0)
                {
                selected = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                loadImage(directoryFull + "imagenVacia.jpg");
                frmAgregarArticulo modificar = new frmAgregarArticulo(selected);
                modificar.ShowDialog();
                loader();
                }
                else
                {
                MessageBox.Show("Debe seleccionar un articulo para modificar");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            ArticuloDataBase articuloEliminar = new ArticuloDataBase();

                try
                {
                     if (dgvArticulos.SelectedRows.Count != 0)
                    {

                        DialogResult respuesta = MessageBox.Show("¿Realmente quiere eliminar el articulo ?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (respuesta == DialogResult.Yes)
                        {
                        
                            selected = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                            articuloEliminar.eliminar(selected.Id);
                            loader();
                        }
                    }
                     else
                {
                    MessageBox.Show("Debe seleccionar un articulo para eliminar");
                }
                }

                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
         
            
        

        private void btnDescripcion_Click(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null)
            {
                Articulo select = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                MessageBox.Show(select.descripcion);
            }
            else
            {
                MessageBox.Show("No hay ningun articulo seleccionado");
            }
        }

        private void filtrar()
        {
            List<Articulo> listaFiltrada;
            string filtro = txtFiltro.Text;
            if (filtro.Length >= 3)
            {
                listaFiltrada = listArticulo.FindAll(x => x.nombre.ToUpper().Contains(filtro.ToUpper()) || x.categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()));

            }
            else
            {
                listaFiltrada = listArticulo;
            }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltrada;
            hideColums();
        }
        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            ArticuloDataBase articulo = new ArticuloDataBase();
            try
            {
                string campo = "";
                if(noSelection(cboCampo))
                {
                    MessageBox.Show("El campo no puede quedar vacio , seleccione una opcion");
                }
                else
                {
                campo = cboCampo.SelectedItem.ToString();
                string criterio = "";           
                     if(noSelection(cboCriterio))
                    {
                        if( campo != "Precio")
                        {

                            cboCriterio.SelectedItem = "Todos";
                            criterio = "Todos"; 
                        }
                        else
                        {

                            cboCriterio.SelectedItem = "Hasta";
                            criterio = "Hasta";
                        }                  
                        }
                    else
                        {
                                criterio = cboCriterio.SelectedItem.ToString();
                        }
                    if (cboCampo.SelectedItem == "Precio" && !(numberOnly(txtFiltroAvanzado.Text)))
                    {
                            MessageBox.Show("Solo numeros por favor ");           
                    }
                    else if(cboCampo.SelectedItem == "Precio" && string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                    {
                        MessageBox.Show("El filtro no puede quedar vacio para filtrar por precio");
                        loader();
                    }
                    else
                    {
                        string filtro = txtFiltroAvanzado.Text;
                        dgvArticulos.DataSource = articulo.filtrar(campo, criterio, filtro);
                    }
                }
             }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            filtrar();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string campo = cboCampo.SelectedItem.ToString();
            if(campo == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Menores a");
                cboCriterio.Items.Add("Hasta");
                cboCriterio.Items.Add("Mayores a ");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Todos");
                cboCriterio.Items.Add("Empieza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
    }

