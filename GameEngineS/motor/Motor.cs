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
        private static List<personaje> AllFiguras = new List<personaje>();
        private static List<terreno> AllTerreno = new List<terreno>();
        private static List<cannon> todosCannon = new List<cannon>();
        private static List<Odin> allOdin = new List<Odin>();
        private static List<Bala> balas = new List<Bala>(); 

        //fondo por defecto
        public Color fondo = Color.Blue;
        

        //camara para desplazamiento
        public vector camara = new vector().cero();

        /// <summary>
        /// 
        ///  -- ELEMENTOS DEL MOTOR --
        /// 
        /// + Personaje con vida +
        /// + Terreno +
        /// + Entrada de teclado +
        /// + Comprobacion dentro del objeto para comprobar colisiones +
        /// + Funcion que devuelve los objetos dentro de un area especificada para luego hacer
        ///   cualquier tipo de calculos con elementos cercanos a un objeto para ahorrar recursos
        /// + cañones que disparan 1 sola bala
        /// + balas
        /// + (EN PROCESO) metralleta 
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="titulo"></param>
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
                    Fisicas();
                    cuadro.BeginInvoke((MethodInvoker)delegate {cuadro.Refresh(); });
                    Thread.Sleep(1);

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

            //pintas el terreno
            for (int y = 0; y < AllTerreno.Count; y++)
            {
                AllTerreno[y].pintar(g);
            }
            for (int x = 0; x < todosCannon.Count; x++)
            {
                todosCannon[x].pintar(g);
            }
            for (int x = 0; x < allOdin.Count; x++)
            {
                allOdin[x].pintar(g);
            }
            for (int x = 0; x < balas.Count; x++) 
            { 
                balas[x].pintar(g);
                balas[x].moverBala();
            }


            //pinta el personaje luego pa que quede por encima
            for (int x = 0; x < AllFiguras.Count; x++) {
                AllFiguras[x].pintar(g);
            }


           

        }


       

        // personaje
        
        /// <summary>
        /// añade una figura a la lista de figuras
        /// </summary>
        /// <param name="figura"></param>
        public static void agregarFigura(personaje figura)
        {
            AllFiguras.Add(figura);
        }
        /// <summary>
        /// quita una figura a la lista de figuras
        /// </summary>
        /// <param name="figura"></param>
        public static void borrarFigura(personaje figura)
        {
            AllFiguras.Remove(figura);
        }

        // terrenos 

        /// <summary>
        /// añade un sprite a la lista de sprites
        /// </summary>
        /// <param name="sprite"></param>
        public static void agregarTerreno(terreno t)
        {
            AllTerreno.Add(t);
        }
        /// <summary>
        /// quita un sprite de la lista de sprites
        /// </summary>
        /// <param name="sprite"></param>
        public static void borrarTerrno(terreno t)
        {
            AllTerreno.Remove(t);
        }
        /// <summary>
        /// devuelve los objetos alrededor del area del jugador que sean terreno
        /// asi puedes hacer calculos con los objetos a su alrededor que interactuen con el no 
        /// con todo el mapa
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Sx"></param>
        /// <param name="Sy"></param>
        /// <returns></returns>
        public static List<terreno> getTerreno()
        {
            //int x, int y, int Sx, int Sy
            /*List<terreno> ret = new List<terreno>();

            for (int a = 0; a < AllTerreno.Count; a++)
            {
                if (x + Sx >= AllTerreno[a].posicion.x &&
                x <= AllTerreno[a].posicion.x + AllTerreno[a].escala.x &&
                y + Sy >= AllTerreno[a].posicion.y &&
                y <= AllTerreno[a].posicion.y + AllTerreno[a].escala.y)
                {
                    ret.Add(AllTerreno[a]);
                }
                
            }*/

            return AllTerreno;
        }


        //cañones
        internal static void agregarCannon(cannon cannon)
        {
            todosCannon.Add(cannon);
        }
        internal static void borrarCannon(cannon cannon)
        {
            todosCannon.Remove(cannon);
        }
        public static List<cannon> getCannon()
        {
            return todosCannon;
        }

        //balas
        internal static void agregarBala(Bala b)
        {
            balas.Add(b);
        }
        internal static void borrarBala(Bala b)
        {
            balas.Remove(b);
        }
        internal static bool existeBala(int id)
        {
            for (int x = 0; x < balas.Count; x++)
            {
                if (balas[x].id == id)
                {
                    //Console.WriteLine($"Bala con id: {id} existe");
                    return true;
                }
            }
            return false;
            //Console.WriteLine($"Bala con id: {id} NO existe");
        }
       


        //odin
        internal static void agregarOdin(Odin o)
        {
            allOdin.Add(o);
        }
        internal static void borrarOdin(Odin o)
        {
            allOdin.Remove(o);
        }
        public static List<Odin> getOdin() {
        return allOdin;
        }


        //entrada teclado
        /// <summary>
        /// entrada de tecla pulsada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cuadro_KeyUp(object sender, KeyEventArgs e)
        {
            teclaSoltada(e);
        }
        /// <summary>
        /// entrada de tecla soltada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cuadro_KeyDown(object sender, KeyEventArgs e)
        {
            teclaPulsada(e);
        }

        //metodos abstractos
        public abstract void carga();
        /// <summary>
        /// Actualiza las fisicas del juego
        /// </summary>
        public abstract void Actualiza();
        /// <summary>
        /// Pinta las cosas en la pantalla
        /// </summary>
        public abstract void Fisicas();


        /// <summary>
        /// saber que tecla se pulsa que se guarda en el motor.
        /// Luego esta tecla se manda al juego para que haga lo que sea
        /// </summary>
        /// <param name="e"></param>
        public abstract void teclaPulsada(KeyEventArgs e);
        public abstract void teclaSoltada(KeyEventArgs e);
        
    }
}
