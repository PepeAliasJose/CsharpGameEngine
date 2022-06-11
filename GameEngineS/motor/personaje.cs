using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GameEngineS.motor
{
    public class personaje
    {
        public vector posicion = null;
        public vector escala = null;
        public string nombre = "";

        //propiedades para el juego
        public string estado = "right";
        public int vida = 100;

        public vector[] hitbox = new vector[4]; 
        public Sprite[] sprites = new Sprite[2];

        

        /// <summary>
        /// Una figura aqui es un objeto basico del motor con hitbox y vida 
        /// a esta figura se le enlazan una serie de sprites que es la imagen que representa
        /// a la figura en cualquier estado que se especifique.
        /// 
        /// Por ahora como testing solo esta el esatdo de vivo o muerto si la 
        /// vida baja de 0.
        /// </summary>
        /// <param name="posicion"></param>
        /// <param name="escala"></param>
        /// <param name="nombre"></param>
        /// <param name="sprites"></param>
        public personaje (vector posicion, vector escala, string nombre, Sprite[] sprites)
        {
            this.posicion = posicion;
            this.escala = escala;
            this.nombre = nombre;

            //para las colisiones
            this.hitbox[0] = posicion;
            this.hitbox[1] = new vector(posicion.x+escala.x, posicion.y);
            this.hitbox[2] = new vector(posicion.x , posicion.y+escala.y);
            this.hitbox[3] = escala;

            log.informacion($"[Figura]({this.nombre}) - Se ha creado");
            Motor.agregarFigura(this);//al crearse se autoañade a la lista de figuras del motor
            this.sprites = sprites;
        }

        //saber si esta figura esta chocando con los elementos de alrededor
        public bool colision(personaje a) 
        {

            return false;
        }

        /// <summary>
        /// pintar el sprite de la figura en un graphic que le pasas como parametro
        /// </summary>
        /// <param name="g"></param>
        public void pintar(Graphics g) 
        {
            //una barra de vida

            g.FillRectangle(new SolidBrush(Color.Red),100,100,vida*5,20);
            g.FillRectangle(new SolidBrush(Color.Green), 600, 100, 2, 20);
            g.DrawString("VIDA",new Font("Calibri",20,FontStyle.Bold) ,new SolidBrush(Color.Red),610,93);

            //sprite de la direccion
            if (estado == "right")
            {
                g.DrawImage(this.sprites[0].getImagen(), this.posicion.x, this.posicion.y, this.escala.x, this.escala.y);
            }
            if(estado == "left") 
            {
                g.DrawImage(this.sprites[1].getImagen(), this.posicion.x, this.posicion.y, this.escala.x, this.escala.y);
            }
        }

        

        public void Destruir() {
            log.informacion($"[Figura]({this.nombre}) - Se ha borrado");
            Motor.borrarFigura(this);//se borra a si mismo de la lista del motor
        }
    }
}
