using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngineS.motor
{
    public class vector
    {
        public float x;
        public float y;

        public vector() {
            this.x = cero().x;
            this.y = cero().y;
        }
        public vector(float x,float y)
        {
            this.x = x;
            this.y = y;
            
        }

        /// comentarios para cuando se use la funcion en otro lado que sepa que hace
        /// <summary>
        /// Devuelve la coordenada como 0,0
        /// </summary>
        /// <returns></returns>
        public vector cero() {
            return new vector(0,0);
        }
    }
}
