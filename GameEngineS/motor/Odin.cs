using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameEngineS.motor
{
    //clase que es parecida a la de terreno se podria hacer por herencia 
    //
    public class Odin
    {
        public vector posicion = null;
        public vector escala = null;
        public string nombre = "";

        public float origenBalaX;
        public float origenBalaY;

        public vector[] hitbox = new vector[4];
        public Sprite sprite = null;
        
        public Odin(vector posicion, vector escala, string nombre, Sprite sprite)
        {
            this.posicion = posicion;
            this.escala = escala;
            this.nombre = nombre;

            origenBalaX = this.posicion.x + 10;
            origenBalaY = this.posicion.y + 10;

            //para las colisiones
            this.hitbox[0] = posicion;
            this.hitbox[1] = new vector(posicion.x + escala.x, posicion.y);
            this.hitbox[2] = new vector(posicion.x, posicion.y + escala.y);
            this.hitbox[3] = escala;

            log.informacion($"[Figura]({this.nombre}) - Se ha creado");
           Motor.agregarOdin(this);//al crearse se autoañade a la lista de figuras del motor
            this.sprite = sprite;
        }

        static bool disparo = false;
        float a; // lado
        float b; // lado
        float dif; // velocidad diferencia 
         

        //maneter direccion y velocidad constantes da igual la direccion de movimiento
        bool xPositivo;
        bool yPositivo;
        float aPositivo;
        float bPositivo;
        bool vertical = false;

        public void dispara(vector destino, float velocidadDisparo) 
        {
            if (!disparo)
            {
                //hay un problema que con los disparos verticales la velocidad se dispara 
                disparo = true;
                float velDiff = calcularDisparo(destino, velocidadDisparo);
                Bala bala = new Bala(new vector((this.posicion.x + 10), (this.posicion.y + 10)), new vector(25, 25), velocidadDisparo, velDiff,
                    vertical, xPositivo,yPositivo,-1);
            }
            else
            {
                disparo = false;
            }
        }

        private float calcularDisparo(vector destino,float velocidadDisparo )
        {
            a = (int)destino.x - (this.posicion.x + 10);
            b = (int)destino.y - (this.posicion.y + 10);


            if (a < 0) { aPositivo = (a * -1); } else { aPositivo = a; };
            if (b < 0) { bPositivo = (b * -1); } else { bPositivo = b; };

            if (bPositivo >= aPositivo) { vertical = true; } else { vertical = false; };

            //segun se dispara en vertical o horizontal se tiene que calcular la diferencia de velocidad distinto
            if (vertical)
            {
                dif = (velocidadDisparo / (b / a));
                if (dif < 0) { dif = dif * -1; }
            }
            if (!vertical)
            {
                dif = (velocidadDisparo / (a / b));
                if (dif < 0) { dif = dif * -1; }
            }

            //los if se explican debajo
            if (destino.x >= origenBalaX)
            {
                xPositivo = true;
            }
            else
            {
                xPositivo = false;
            }
            if (destino.y >= origenBalaY)
            {
                yPositivo = true;
            }
            else
            {
                yPositivo = false;
            }
            Console.WriteLine($"{dif},{a},{b}");
            return dif;
        }

        public bool disparado() {
            return disparo;
        }

        public bool colision(personaje a)
        {
            if (this.posicion.x + this.escala.x > a.posicion.x &&
                this.posicion.x < a.posicion.x + a.escala.x &&
                this.posicion.y + this.escala.y > a.posicion.y &&
                this.posicion.y < a.posicion.y + a.escala.y) 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// pintar el sprite de la figura en un graphic que le pasas como parametro
        /// </summary>
        /// <param name="g"></param>
        public void pintar(Graphics g)
        {
            g.DrawImage(this.sprite.getImagen(), this.posicion.x, this.posicion.y, this.escala.x, this.escala.y);
           
        }

        public void Destruir()
        {
            log.informacion($"[Figura]({this.nombre}) - Se ha borrado");
           Motor.borrarOdin(this);//se borra a si mismo de la lista del motor
        }
    }
}
