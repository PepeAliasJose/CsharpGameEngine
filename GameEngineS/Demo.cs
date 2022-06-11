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
    /// <summary>
    /// Nivel basico para probar el motor
    /// 
    /// + Personaje con vida +
    /// + Terreno +
    /// + Entrada de teclado +
    /// + Comprobacion dentro del objeto para comprobar colisiones +
    /// + Funcion que devuelve los objetos alrededor de un area dada que se usa para comprobar
    ///   colision con elementos del terreno cercanos al personaje en lugar de con todos los objetos
    ///   del juego asi se ahorra tiempo de procesador por que no vas a comprobar si tu personaje
    ///   choca con un cuadro que esta en el otro lado de la pantalla +
    /// 
    /// Propio de la demo
    /// 
    /// + Fisicas basicas de gravedad
    /// + Salto del personaje
    /// + Comprobacion basica de colision con objetos
    /// + Distinto comportamiento de colision para quitar vida al personaje
    /// 
    /// </summary>
    class Demo : Motor
    {
        personaje jugador;
        terreno mapa;
        //jugador
        vector posInicial = new vector(50, 150);
        vector posPersonaje = new vector(50, 150);
        vector sicePersonaje = new vector(50, 100);

        //bloques del mundo
        vector siceTerreno = new vector(50, 50);
        

        //movimiento del jugador
        bool arriba;
        bool abajo;
        bool derecha;
        bool izquierda;
        bool salto;

        //chocando
        bool colision;
        vector ultimaPosicion = new vector(0,0);
        List<terreno> terrenos = new List<terreno>();

        //aqui las dimensiones del array son comas dentro de los [,]
        string[,] map =
            {
                {"x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".","p",".",".",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".","l",".","l",".","l",".",".",".",".",".",".",".",".","x" },
                {"x",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".",".","x" },
                {"x","x","x","x","x","x","x","x","x","p","p","p","p","p","p","p","p","p","x","x","x","x","x","x","x" },
                {"x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x","x" }
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
                        Sprite img = new Sprite(new vector(10, 10), new vector(180, 246), "suelo", "suelo");


                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        mapa = new terreno(new vector(0 + y*50, 0 + x*50), siceTerreno, "suelo", img);
                    }
                    if (map[x, y] == "l")
                    {
                        Sprite img = new Sprite(new vector(10, 10), new vector(180, 246), "suelo", "suelo");

                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        mapa = new terreno(new vector(0 + y * 50, 0 + x * 50), new vector(50,20), "suelo", img);
                    }
                    if (map[x, y] == "p")
                    {
                        Sprite img = new Sprite(new vector(10, 10), new vector(180, 246), "pincho", "pincho");

                        //crea las figuras que se van a usar en la demo que se meten dentro del motor
                        mapa = new terreno(new vector((0 + y * 50), (0 + x * 50) + 20) , new vector(50, 50), "pincho", img);
                    }
                }
            }


            Sprite[] sprites = new Sprite[2];
            sprites[0] = new Sprite(new vector(10, 10), new vector(180, 246), "player", "Jugador_vivo");
            sprites[1] = new Sprite(new vector(10, 10), new vector(180, 246), "player_dead", "Jugador_muerto");

            //crea el personaje que se van a usar en la demo que se mete dentro del motor
            jugador = new personaje(posPersonaje, sicePersonaje, "Jugador",sprites);
            
        }
        
        float x = 0.9f;

        public override void Actualiza()
        {
            //actualiza que hacen las figuras 
            //camara.x = 100;
            //camara.y = 200;

            terrenos = Motor.getTerreno((int)jugador.posicion.x - 100, (int)jugador.posicion.y - 100,
                (int) (jugador.posicion.x + jugador.escala.x + 100), (int) (jugador.posicion.y + jugador.escala.y + 100));

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
                    colision = terrenos[y].colision(jugador);
                    if (colision)
                    {
                        haCaido = true;
                        //Console.WriteLine("colision");
                        jugador.posicion.x = ultimaPosicion.x;
                        jugador.posicion.y = ultimaPosicion.y;
                    }
                }
                if (terrenos[y].nombre == "pincho")
                {
                    colision = terrenos[y].colision(jugador);
                    if (colision)
                    {
                        jugador.posicion.x = posInicial.x;
                        jugador.posicion.y = posInicial.y;
                        jugador.vida -= 10;
                    }
                }
            }
        }

        //variables globales para el salto
        int cont = 0;
        bool salto2 = false;
        int velSalto = 20;
        int velGravedad = 5;
        bool haCaido = true;
        bool activarSalto = true;
        bool soltado = false;//especifico para que salte solo cuando se pulse el boton
        
        public override void Fisicas()
        {
            //salto de prueba

            //cuando pulse espacio salta y activa otra variable para que el salto 
            //no sea permanente en vertical por mantener pulsado
            if (salto && activarSalto) 
            {
                salto2 = true;
            }
            
            if (salto2 && haCaido && activarSalto) 
            {
                
                cont++;

                setUltimaPosicion();
                jugador.posicion.y -= velSalto;
                colisiones(terrenos);

                if (cont % 5 == 0) //cuanto mas alto mas lento va
                { 
                    velSalto -= 5;
                }

                if (cont > 40)//reset de variables
                {
                    cont = 0;
                    if (!soltado) 
                    {
                        activarSalto = false;
                    }
                    haCaido = false;
                    velSalto = 20;
                    salto2 = false;
                }
            }
            else
            {
                
                //gravedad de prueba
                setUltimaPosicion();
                jugador.posicion.y += velGravedad;
                colisiones(terrenos);
                setUltimaPosicion();
                jugador.posicion.y += velGravedad;
                colisiones(terrenos);

                
            }

           
            
            
        }

        public override void teclaPulsada(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { arriba = true;  }
            if (e.KeyCode == Keys.S) { abajo = true; }
            if (e.KeyCode == Keys.A) { izquierda = true; jugador.estado = "left"; }
            if (e.KeyCode == Keys.D) { derecha = true; jugador.estado = "right"; }
            if (e.KeyCode == Keys.Space) { salto = true; soltado = false; }
        }
        
        public override void teclaSoltada(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { arriba = false; }
            if (e.KeyCode == Keys.S) { abajo = false; }
            if (e.KeyCode == Keys.A) { izquierda = false; }
            if (e.KeyCode == Keys.D) { derecha = false; }
            if (e.KeyCode == Keys.Space) { salto = false;
                activarSalto = true;
                soltado = true;//saber si ha soltado la tecla
                //false la variable que controla el salto para que no sea infinito
            }
        }
    }
}
