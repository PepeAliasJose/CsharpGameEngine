using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngineS.motor;
using System.Drawing;
using System.Windows.Forms;

namespace GameEngineS
{
    class Demo : Motor
    {
        figura jugador;
        figura mapa;

        //movimiento del jugador
        bool arriba;
        bool abajo;
        bool derecha;
        bool izquierda;


        //aqui las dimensiones del array son comas dentro de los []
        string[,] map =
            {
                {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
                {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
                {".",".",".",".",".",".",".",".",".",".",".",".","x",".",".",".",".",".",".","." },
                {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
                {".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
                {".",".",".",".",".",".",".",".",".",".","x",".","x",".","x",".",".",".",".","." },
                {".",".",".","x",".","x",".",".",".",".",".",".",".",".",".",".",".",".",".","." },
                {".",".","x","x",".","x","x",".",".",".",".",".",".",".",".",".",".",".",".","." },
                {"x","x","x","x",".","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x" }
            };

        //base se usa para acceder a los miembros de la clase base desde una clase derivada
        public static int ancho = 1600;
        public static int alto = 900;
        public Demo() : base(new vector((float) ancho,(float) alto), "Illo es una demo") { 
            //crea una demo con el motor del juego en una ventana de 800/600
        }

        public override void carga()
        {
            
            //carga mapa simple
            for (int x = 0; x < map.GetLength(0); x++) 
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    
                    if (map[x, y] == "x") 
                    {
                        Sprite[] spriteMap = new Sprite[2];
                        spriteMap[0] = new Sprite(new vector(10, 10), new vector(180, 246), "suelo", "suelo");
                        spriteMap[1] = new Sprite(new vector(10, 10), new vector(180, 246), "suelo", "suelo2");

                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        mapa = new figura(new vector(0 + y*50, 0 + x*50), new vector(50, 50), "Suelo", spriteMap);
                    }
                }
            }


            Sprite[] sprites = new Sprite[2];
            sprites[0] = new Sprite(new vector(10, 10), new vector(180, 246), "player", "Jugador_vivo");
            sprites[1] = new Sprite(new vector(10, 10), new vector(180, 246), "player_dead", "Jugador_muerto");

            //crea las figuras que se van a usar en la demo que se meten dentro del motor
            jugador = new figura(new vector(10, 10), new vector(70, 100), "Jugador",sprites);
            //foreach (vector vector in jugador.hitbox) { 
            //    Console.WriteLine($"{vector.x},{vector.y}");
            //}
            

        }
        
        float x = 0.9f;

        public override void Actualiza()
        {
            //actualiza que hacen las figuras 
            camara.y = 350;



        }

       

        public override void Pinta()
        {
            if (arriba) { jugador.posicion.y -= 3.5f; }
            if (abajo) { jugador.posicion.y += 3.5f; }
            if (izquierda) { jugador.posicion.x -= 3.5f; }
            if (derecha) { jugador.posicion.x += 3.5f; }
        }

        public override void teclaPulsada(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { arriba = true;  }
            if (e.KeyCode == Keys.S) { abajo = true; }
            if (e.KeyCode == Keys.A) { izquierda = true; }
            if (e.KeyCode == Keys.D) { derecha = true; }
        }

        public override void teclaSoltada(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { arriba = false; }
            if (e.KeyCode == Keys.S) { abajo = false; }
            if (e.KeyCode == Keys.A) { izquierda = false; }
            if (e.KeyCode == Keys.D) { derecha = false; }
        }
    }
}
