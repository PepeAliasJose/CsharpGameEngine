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
    
    class DemoTrayectoria: Motor
    {
        personaje jugador;
        

        //jugador
        vector posDisparo = new vector(0, 0);
        vector posPersonaje = new vector(50, 450);
        vector sicePersonaje = new vector(100, 120);

        //bloques del mundo
        vector siceTerreno = new vector(50, 50);
        

        //movimiento del jugador
        bool arriba;
        bool abajo;
        bool derecha;
        bool izquierda;
        bool dispara;

        //variables de disparo
        float velocidadBala = 6f;

        //chocando
        bool colision;
        vector ultimaPosicion = new vector(0,0);
        List<terreno> terrenos = new List<terreno>();

        //aqui las dimensiones del array son comas dentro de los [,]
        //x es suelo
        //p son pinchos
        //l son losas con la mitad de altura 
        //c es un cañon
        string[,] map =
            {
                {"x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","o",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","l",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x" }
            };

        //base se usa para acceder a los miembros de la clase base desde una clase derivada
        public static int ancho = 1600;
        public static int alto = 900;
        public DemoTrayectoria() : base(new vector((float) ancho,(float) alto), "Trayectorias") { 
            //crea una demo con el motor del juego en una ventana de 800/600
        }

        public override void carga()
        {
            int cannons = 0; // id para el cañon y su bala
            //carga mapa simple
            for (int x = 0; x < map.GetLength(0); x++) 
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    
                    if (map[x, y] == "x") 
                    {
                        Sprite img = new Sprite(new vector(10, 10), new vector(180, 246), "suelo", "suelo");


                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        new terreno(new vector(0 + y*50, 0 + x*50), siceTerreno, "suelo", img);
                    }
                    if (map[x, y] == "l")
                    {
                        Sprite img = new Sprite(new vector(10, 10), new vector(180, 246), "suelo", "suelo");

                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        new terreno(new vector(0 + y * 50, 0 + x * 50), new vector(50,20), "suelo", img);
                    }
                    if (map[x, y] == "p")
                    {
                        Sprite img = new Sprite(new vector(10, 10), new vector(180, 246), "pincho", "pincho");

                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        new terreno(new vector((0 + y * 50), (0 + x * 50) + 20) , new vector(50, 50), "pincho", img);
                    }
                    if (map[x, y] == "c")
                    {
                        Sprite img = new Sprite(new vector(10, 10), new vector(180, 246), "cannon", "cannon");

                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        cannon can = new cannon(new vector((0 + y * 50), (0 + x * 50)+10), new vector(50, 40), "cannon", img,cannons);
                        cannons++;
                    }
                    if (map[x, y] == "o")
                    {
                        Sprite img = new Sprite(new vector(10, 10), new vector(180, 246), "cannon", "cannon");

                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        Odin can = new Odin(new vector((0 + y * 50), (0 + x * 50) + 10), new vector(50, 40), "Odin", img);
                       
                    }
                }
            }


            Sprite[] sprites = new Sprite[2];
            sprites[0] = new Sprite(new vector(10, 10), new vector(180, 246), "Hornet_reves", "Jugador_der");
            sprites[1] = new Sprite(new vector(10, 10), new vector(180, 246), "Hornet", "Jugador_izq");

            //crea el personaje que se van a usar en la demo que se mete dentro del motor
            jugador = new personaje(posPersonaje, sicePersonaje, "Jugador",sprites);
            
        }
        
        float x = 0.9f;

        public override void Actualiza()
        {
            //actualiza que hacen las figuras 
            //camara.x = 100;
            //camara.y = 200;

            terrenos = Motor.getTerreno();

            interaccionJugador(terrenos);
            
            

        }

       

        private void interaccionJugador(List<terreno> terrenos)
        {
            setUltimaPosicion();
            if (arriba) { jugador.posicion.y -= 5; }
            colisiones(terrenos);

            setUltimaPosicion();
            if (abajo) { jugador.posicion.y += 5; }
            colisiones(terrenos);

            setUltimaPosicion();
            if (izquierda) { jugador.posicion.x -= 5; }
            colisiones(terrenos);

            setUltimaPosicion();
            if (derecha) { jugador.posicion.x += 5; }
            colisiones(terrenos);
        }

        private void setUltimaPosicion()
        {
            ultimaPosicion.x = jugador.posicion.x;
            ultimaPosicion.y = jugador.posicion.y;
        }

        private void colisiones(List<terreno> terrenos)
        {
            for (int y = 0; y < terrenos.Count; y++)
            {
                if (terrenos[y].nombre == "suelo")
                {
                    colision = terrenos[y].colision(jugador,25,0);
                    if (colision)
                    {
                        //Console.WriteLine("colision");
                        jugador.posicion.x = ultimaPosicion.x;
                        jugador.posicion.y = ultimaPosicion.y;
                    }
                }
                
            }
        }

        int contador_balas = 0;
        int contador_reset = 0;
        public override void Fisicas()
        {
            /*foreach (cannon c in getCannon()) {

                if (dispara && !c.disparado())
                {
                    posDisparo.x = jugador.posicion.x+25;
                    posDisparo.y = jugador.posicion.y+50;
                    c.dispara(posDisparo, velocidadBala);
                }
                if (c.disparado())
                {
                    c.dispara(posDisparo, velocidadBala);
                }
            }*/
            foreach (Odin o in getOdin())
            {
                if (contador_balas >= 50) {
                    contador_reset++;
                }
                if (contador_reset > 100)
                {
                    contador_reset = 0;
                    contador_balas = 0;
                }
                if (dispara && !o.disparado())
                {
                    contador_balas++;
                    posDisparo.x = jugador.posicion.x + 25;
                    posDisparo.y = jugador.posicion.y + 50;
                    o.dispara(posDisparo, velocidadBala);
                }
                if (o.disparado() && contador_balas<50*2)
                {
                    contador_balas++;
                    if (contador_balas % 10 == 0) 
                    {
                        o.dispara(posDisparo, velocidadBala);
                    }
                }
            }
        }

        public override void teclaPulsada(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { arriba = true;  }
            if (e.KeyCode == Keys.S) { abajo = true; }
            if (e.KeyCode == Keys.A) { izquierda = true; jugador.estado = "left"; }
            if (e.KeyCode == Keys.D) { derecha = true; jugador.estado = "right"; }
            if (e.KeyCode == Keys.P) { dispara = true; }
        }
        
        public override void teclaSoltada(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { arriba = false; }
            if (e.KeyCode == Keys.S) { abajo = false; }
            if (e.KeyCode == Keys.A) { izquierda = false; }
            if (e.KeyCode == Keys.D) { derecha = false; }
            if (e.KeyCode == Keys.P) { dispara = false; }
        }
    }
}
