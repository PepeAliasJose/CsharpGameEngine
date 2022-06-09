using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace GameEngineS.motor
{
    public class Sprite
    {
        public vector posicion = null;
        public vector escala = null;
        public String archivo = "";
        public string nombre = "";
        public int IdFigura;
        public Bitmap sprite = null;

        public Sprite(vector posicion, vector escala,string archivo, string nombre)
        {
            this.posicion = posicion;
            this.escala = escala;
            this.nombre = nombre;
            this.archivo = archivo;

            Image img = Image.FromFile($"content/sprites/{archivo}.png");
            Bitmap bitmap = new Bitmap(img, (int) escala.x, (int) escala.y);
            sprite = bitmap;

            log.informacion($"[Sprite]({this.nombre}) - Se ha creado");
            //Motor.agregarSprite(this);//al crearse se autoañade a la lista de Sprites del motor
            
        }

        public void Destruir()
        {
            log.informacion($"[Sprite]({this.nombre}) - Se ha borrado");
            //Motor.borrarSprite(this);//se borra a si mismo de la lista del motor
        }
    }
}
