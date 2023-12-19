using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueValley.Maps;

namespace RogueValley.Entities
{
    class MobManager
    {
        private Texture2D[][][][] sprites;
        private List<Enemies> mobList;
        private Random rnd;
        public MobManager(Player player) {
            this.mobList = new List<Enemies>();

            rnd = new Random();
            
            

        }

        public void LoadContent(Texture2D[][][][] sprites) {
            this.sprites = sprites;        
        }
        private int GetPosX(int x, Player player) {

            x = rnd.Next(0, 14000);
            if (x == player.playerPosition[0])
                x = this.GetPosX(x, player);

            return x;
        }
        private int GetPosY(int y, Player player)
        {
            y = rnd.Next(0, 7000);
            if (y == player.playerPosition[1])
                y = this.GetPosX(y, player);

            return y;
        }

        public void Spawn(int[] ammount, Player player) {
            int[] pos = new int[2];
            this.mobList = new List<Enemies>();
            for (int i = 0; i < ammount[(int)enums.Entitiy.Zombie]; i++) {

                pos[0] = this.GetPosX(0, player);
                pos[1] = this.GetPosY(0, player);


                Zombie zombie = new Zombie(pos);
                zombie.LoadContent(this.sprites[(int)enums.Entitiy.Zombie]);
                this.mobList.Add(zombie);
            }        
        }

        public void Update(Player player) {

            for (int i = 0; i < this.mobList.Count; i++) {
                this.mobList[i].Update(player);
            }
        }

        public void Draw(SpriteBatch _spriteBatch, Map m) {
            for (int i = 0; i < this.mobList.Count; i++)
            {
                this.mobList[i].Draw(_spriteBatch, m);
            }
        }
    }
}
