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
    class window : Form {
       
    }

    //abstract indica que una clase está diseñada como clase base de otras clases, no para crear instancias por sí misma
    public abstract class Ventana
    {
        private vector size = new vector(512, 512); ///tamaño de la pantalla
        private string titulo = "Illo que guapo"; // titulo del form
        private window cuadro = null; // form pero cambiandole el nombre

        private Thread bucleTiempo = null;//tiempo para el bucle

        public Color fondo = Color.Green;

        public Ventana(vector size, string titulo) { 
            this.size = size;
            this.titulo = titulo;

            cuadro = new window();//se quita que la ventana sea null
            cuadro.Size = new Size((int) this.size.x, (int) this.size.y);//tamaño de la ventana que creamos antes
            cuadro.Text = this.titulo; //se pone el titulo

            cuadro.Paint += Render;

            bucleTiempo = new Thread(bucle);//iniciar el bucle
            bucleTiempo.Start();

            Application.Run(cuadro);
        }

        void bucle() {
            carga();//carga el contenido
            while (bucleTiempo.IsAlive) { //bucle que actualiza la ventana cada 16 ms
                try
                {
                    Pinta();//pinta
                    cuadro.BeginInvoke((MethodInvoker)delegate { cuadro.Refresh(); });
                    Actualiza();//actualiza
                    Thread.Sleep(16);
                }
                catch 
                {
                    Console.WriteLine("cargando...");
                }
            }
        }

        private void Render(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(fondo);
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
    }
}
