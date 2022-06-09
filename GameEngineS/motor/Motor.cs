using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///Cosas para pintar en pantalla
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace GameEngineS.motor
{
      class window : Form
     {
        public window() 
        { 
            this.DoubleBuffered = true; 
        }
     }
    //abstract indica que una clase está diseñada como clase base de otras clases, no para crear instancias por sí misma
    public abstract class Motor
    {


        private vector size = new vector(512, 512); ///tamaño de la pantalla
        private string titulo = "Illo que guapo"; // titulo del form
        private window cuadro = null; // form pero cambiandole el nombre

        //Thread para el tiempo
        private Thread bucleTiempo = null;//tiempo para el bucle

        //Listas de contenido 
        private static List<figura> AllFiguras = new List<figura>();
        private static List<Sprite> AllSprites = new List<Sprite>();

        //fondo por defecto
        public Color fondo = Color.Blue;

        public vector camara = new vector().cero();




        public Motor(vector size, string titulo) {
            log.informacion("El juego esta empezando...");
            this.size = size;
            this.titulo = titulo;

            cuadro = new window();//se quita que la ventana sea null
            cuadro.Size = new Size((int) this.size.x, (int) this.size.y);//tamaño de la ventana que creamos antes
            cuadro.Text = this.titulo; //se pone el titulo

            //ajustar la ventana 
            //no se redimensiona
            cuadro.FormBorderStyle = FormBorderStyle.FixedSingle;
            cuadro.MaximizeBox = false;
            cuadro.MinimizeBox = false;
            //posicion de aparicion
            cuadro.StartPosition = FormStartPosition.Manual;
            cuadro.Location = new Point(100, 50);  


            cuadro.Paint += Render;
            cuadro.KeyDown += Cuadro_KeyDown;
            cuadro.KeyUp += Cuadro_KeyUp;

            bucleTiempo = new Thread(BucleMotor);//iniciar el bucle
            bucleTiempo.Start();

            Application.Run(cuadro);
        }

        
        private void Cuadro_KeyUp(object sender, KeyEventArgs e)
        {
            teclaSoltada(e);
        }

        /// <summary>
        /// entrada de tecla pulsada 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cuadro_KeyDown(object sender, KeyEventArgs e)
        {
            teclaPulsada(e);
        }




        /// <summary>
        /// añade una figura a la lista de figuras
        /// </summary>
        /// <param name="figura"></param>
        public static void agregarFigura(figura figura) {
            AllFiguras.Add(figura);
        }
        /// <summary>
        /// quita una figura a la lista de figuras
        /// </summary>
        /// <param name="figura"></param>
        public static void borrarFigura(figura figura)
        {
            AllFiguras.Remove(figura);
        }


        /// <summary>
        /// añade un sprite a la lista de sprites
        /// </summary>
        /// <param name="sprite"></param>
        public static void agregarSprite(Sprite sprite)
        {
            AllSprites.Add(sprite);
        }
        /// <summary>
        /// quita un sprite de la lista de sprites
        /// </summary>
        /// <param name="sprite"></param>
        public static void borrarSprite(Sprite sprite)
        {
            AllSprites.Remove(sprite);
        }




        /// <summary>
        /// bucle principal del motor
        /// </summary>
        void BucleMotor() {
            carga();//carga el contenido
            while (bucleTiempo.IsAlive) { //bucle que actualiza la ventana cada 16 ms
                if (cuadro.IsDisposed) //si se cierra la ventana
                {
                    bucleTiempo.Abort(); //aborta el thread y se cierra el proceso
                }

                try
                {

                    Actualiza();//actualiza
                    Pinta();//pinta
                    cuadro.BeginInvoke((MethodInvoker)delegate {cuadro.Refresh(); });
                    Thread.Sleep(8);

                }
                catch 
                {
                    log.advertencia("El bucle del motor esta haciendo cosas raras");
                }
            }
        }




        /// <summary>
        /// Renderiza el contenido de la ventana
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Render(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(fondo);
            g.TranslateTransform(camara.x, camara.y);
            //foreach (figura figura in AllFiguras) 
            //{
            //    //g.FillRectangle(new SolidBrush(Color.Red), figura.posicion.x, figura.posicion.y, figura.escala.x, figura.escala.y);
            //    figura.pintar(g);
            //}

            for (int x = 0; x < AllFiguras.Count; x++) {
                AllFiguras[x].pintar(g);
            }

            
        }




        public abstract void carga();
        /// <summary>
        /// Actualiza las fisicas del juego
        /// </summary>
        public abstract void Actualiza();
        /// <summary>
        /// Pinta las cosas en la pantalla
        /// </summary>
        public abstract void Pinta();


        /// <summary>
        /// saber que tecla se pulsa que se guarda en el motor.
        /// Luego esta tecla se manda al juego para que haga lo que sea
        /// </summary>
        /// <param name="e"></param>
        public abstract void teclaPulsada(KeyEventArgs e);
        public abstract void teclaSoltada(KeyEventArgs e);
    }
}
