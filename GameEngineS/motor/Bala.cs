using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace GameEngineS.motor
{
    public class Bala
    {
        public vector posicion = null;
        public vector escala = null;
        float velocidad;
        float velDiferencia;

        bool vertical;
        bool xPositivo;
        bool yPositivo;

        public int id;

        private Bitmap sprite = null;

        public Bala(vector posicion, vector escala, float velocidad, float velDiferencia, bool vertical, bool xPositivo, bool yPositivo , int id)
        {
            this.posicion = posicion;
            this.escala = escala;
            this.velocidad = velocidad;
            this.velDiferencia = velDiferencia;
            this.id = id;

            this.vertical = vertical;
            this.xPositivo = xPositivo;
            this.yPositivo = yPositivo;


            Image img = Image.FromFile($"content/sprites/bola.png");
            Bitmap bitmap = new Bitmap(img, (int) escala.x, (int) escala.y);
            sprite = bitmap;

            Motor.agregarBala(this);
            
            
        }

        public void moverBala() 
        {
            //calcular trayectoria 


            //Los if se calculan para saber la direccion de movimiento en una linea recta permanente 
            //asi sabe si tiene que ir arriba o abajo izquierda o derecha permanentemente
            //en direccion al destino aun que se pase de largo

            //los if de vertical es para la compensacion ya que disparando en diagonal
            //haces el calculo para que la bala se mueva en una direccion x en una velocidad
            // y en una direccion y en otra velocidad
            // arriba se calcula la diferencia de velocidad ' variable dif' para que la bala se mueva
            // en la direccion correcta a una velocidad establecida
            //pero si el lado que deberia ser mas corto es mas largo la diferencia lo que hace es multiplicar la velocidad de ese lado y sale mal


            //aun asi todavia queda de que hay que calcular la velocidad mejor por el tema de que la volocidad no es constante en
            //todas las direcciones por que si sumas 2 vectores a un objeto la velocidad final es mayor de la esperada cuanto mas cerrado sea el angulo
            //lo cual habria que hacer calculos para ajustar la fuerza de los vectores segun el angulo para que siempre sea la misma velocidad
            //da igual en que direccion se mueva

            if (vertical)
            {
                if (xPositivo) { this.posicion.x += this.velDiferencia; }
                else { this.posicion.x -= this.velDiferencia; }
                if (yPositivo) { this.posicion.y += velocidad; }
                else { this.posicion.y -= velocidad; }
            }
            if (!vertical)
            {

                if (xPositivo) { this.posicion.x += velocidad; }
                else { this.posicion.x -= velocidad; }
                if (yPositivo) { this.posicion.y += velDiferencia; }
                else { this.posicion.y -= velDiferencia; }
            }

            //terminar el disparo
            if (this.posicion.x < 0 || this.posicion.y < 0 || this.posicion.x > 1000 || this.posicion.y > 1000)
            {
                destruirBala();
            }
        }

        /// <summary>
        /// devuelve el bitmap del sprite
        /// </summary>
        /// <returns></returns>
        public Bitmap getImagen() {
            return this.sprite;
        }

        public void destruirBala() 
        {
            Motor.borrarBala(this);
        }

        internal void pintar(Graphics g)
        {
            g.DrawImage(this.getImagen(),this.posicion.x, this.posicion.y, this.escala.x, this.escala.y);
        }
    }
}
