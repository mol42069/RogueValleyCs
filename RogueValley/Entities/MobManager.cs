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
        public void RmList() {
            this.mobList.Clear();
        }
        
        public void Spawn(int[] ammount, Player player) {
            // we spawn an specific ammount of enemies at random positions that arent ont the screen.
            // And we add them to a list.
            for (int i = 0; i < ammount[(int)enums.Entitiy.Zombie]; i++)
            {
                int[] pos = new int[2];
                pos[0] = player.playerPosition[0];
                pos[1] = player.playerPosition[1];

                while (!(!(pos[1] < (player.playerPosition[1] + 1080) && pos[1] > (player.playerPosition[1] - 1080)) || !(pos[0] < (player.playerPosition[0] + 1920) && pos[0] > (player.playerPosition[0] - 1920))))
                {
                    pos[0] = rnd.Next(0, 14000);
                    pos[1] = rnd.Next(0, 7000);
                }
                Zombie zombie = new Zombie(pos);
                zombie.LoadContent(this.sprites[(int)enums.Entitiy.Zombie]);
                this.mobList.Add(zombie);
            }            
        }

        public void Update(Player player) {
            // we go through all enemies in our mobList and Update them.
            //if they are dead we remove them from the list so they basicly dont exist anymore.
            player.target.Clear();
            for (int i = 0; i < this.mobList.Count; i++) {
                if (this.mobList[i].Update(player) <= 0) {
                    this.mobList.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch, Map m) {
            // we go through the enemy list and draw them all.
            for (int i = 0; i < this.mobList.Count; i++)
            {
                this.mobList[i].Draw(_spriteBatch, m);
            }
        }
    }
}
