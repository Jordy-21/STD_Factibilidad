using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace Factibilidad.Presentacion
{
    public partial class MapasGoogle : Form
    {
        GMarkerGoogle marker;
        GMapOverlay markerOverlay;
        DataTable dt;

        int filaSelecccionada = 0;
        double LatInicial = -12.2125;
        double LngInicial = -76.9369;

        public MapasGoogle()
        {
            InitializeComponent();
        }

        private void MapasGoogle_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add(new DataColumn("Descripcion", typeof(String)));
            dt.Columns.Add(new DataColumn("Lat", typeof(double)));
            dt.Columns.Add(new DataColumn("Long", typeof(double)));

            //Insertando datos al dt para mostrar en la lista 
            dt.Rows.Add("Ubicación 1", LatInicial, LngInicial);
            dataGridView1.DataSource = dt;

            //Desactivar las columnas de lat y long
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(LatInicial, LngInicial);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 9;
            gMapControl1.AutoScroll = true;

            //MArcador
            markerOverlay = new GMapOverlay("Marcador");
            marker = new GMarkerGoogle(new PointLatLng(LatInicial, LngInicial), GMarkerGoogleType.green);
            markerOverlay.Markers.Add(marker); //Agregamos al mapa

            //Agregamos un tooltip de texto a los marcadores.
            marker.ToolTipMode = MarkerTooltipMode.Always;
            marker.ToolTipText = String.Format("Ubicación: \n Latitud:{0} \n L", LatInicial, LngInicial);

            //Agregamos el mapa y el map control
            gMapControl1.Overlays.Add(markerOverlay);
        }

        private void SeleccionarRegistro(object sender, DataGridViewCellMouseEventArgs e)
        {
            filaSelecccionada = e.RowIndex;//Fila Seleccionada
            //Recuperamos los datos del grid y los asifnamos a los textbox
            txtDescripcion.Text = dataGridView1.Rows[filaSelecccionada].Cells[0].Value.ToString();
            txtLatitud.Text = dataGridView1.Rows[filaSelecccionada].Cells[1].Value.ToString();
            txtLongitud.Text = dataGridView1.Rows[filaSelecccionada].Cells[2].Value.ToString();

            //se asignaa los valores del grid al marcador
            marker.Position = new PointLatLng(Convert.ToDouble(txtLatitud.Text), Convert.ToDouble(txtLongitud.Text));
            //Se posiciona el foco del mapa en ese punto
            gMapControl1.Position = marker.Position;
        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //se obtiene los datos de lat y lng del mapa usa presiono
            double lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            double lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;

            //Se posicionan en el txt de la latitud y longitud
            txtLatitud.Text = lat.ToString();
            txtLongitud.Text = lng.ToString();
            //Creamos el marcador para moverlo al lugar indicado
            marker.Position = new PointLatLng(lat, lng);
            //Tambien se agrega el mensaje al marcador(tooltip)
            marker.ToolTipText = String.Format("Ubicación: \n Latitud: {0} \n Longitud:{1}", lat, lng);

        }

        private void btnAgregarM_Click(object sender, EventArgs e)
        {
            dt.Rows.Add(txtDescripcion.Text,txtLatitud.Text,txtLongitud.Text);//agregar a la tabla
            //procedimiento para ingresar a una base de datos
        }

        private void btnEliminarM_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(filaSelecccionada); //remover de la tabla
            //procedimeinto para eliminar de una base de datos
        }
    }
}
