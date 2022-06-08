using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngineS.motor;
using System.Drawing;

namespace GameEngineS
{
    class Demo : Ventana
    {
        //base se usa para acceder a los miembros de la clase base desde una clase derivada
        public Demo() : base(new vector(800, 600), "Illo es una demo") { 
        }
        int frame = 0;
        int cont = 1;
        public override void Actualiza()
        {
            Console.WriteLine($"Fotogramas: {frame}");
            frame++;
           
        }

        public override void carga() {

            

        }

        public override void Pinta()
        {
            if (frame % 60 == 0)
            {
                cont++;
                if (cont == 4)
                {
                    cont = 1;
                }
            }
            if (cont == 1)
            {
                fondo = Color.Red;
            }
            if (cont == 2)
            {
                fondo = Color.Green;
            }
            if (cont == 3)
            {
                fondo = Color.Blue;
            }
        }
    }
}
