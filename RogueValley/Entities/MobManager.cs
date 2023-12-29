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
        public List<Enemies> mobList, deadList;
        private Random rnd;

        private int[] spawnRate, bgSize;
        public int ammount, wave, maxRandom;

        public MobManager(Player player, int[] bgSize) {
            this.mobList = new List<Enemies>();
            this.deadList = new List<Enemies>();
            this.rnd = new Random();

            this.ammount = 10;
            this.wave = 0;
            this.maxRandom = 100;

            this.spawnRate = new int[2];
            this.bgSize = bgSize;
        }

        public void LoadContent(Texture2D[][][][] sprites) {
            this.sprites = sprites;
        }
        public void RmList() {
            this.mobList.Clear();
            this.deadList.Clear();
        }
        
        public void Spawn(Player player) {
            // we spawn an specific ammount of enemies at random positions that arent ont the screen.
            // And we add them to a list.
            for (int i = 0; i < this.ammount; i++)
            {
                int[] pos = new int[2];
                pos[0] = player.playerPosition[0];
                pos[1] = player.playerPosition[1];

                while (!(!(pos[1] < (player.playerPosition[1] + 1080) && pos[1] > (player.playerPosition[1] - 1080)) || !(pos[0] < (player.playerPosition[0] + 1920) && pos[0] > (player.playerPosition[0] - 1920))))
                {
                    pos[0] = rnd.Next(0, this.bgSize[0]);
                    pos[1] = rnd.Next(0, this.bgSize[1]);
                }

                int random = rnd.Next(0, 100);
                switch (this.wave) {
                    case 1:
                        
                        if (random < 90)
                        {
                            Zombie zombie = new Zombie(pos);
                            zombie.LoadContent(this.sprites[(int)enums.Entity.Zombie]);
                            this.mobList.Add(zombie);
                        }
                        else if (random >= 90)
                        {
                            // here we spawn other stuff for example Mages
                            Mage mage = new Mage(pos);
                            mage.LoadContent(this.sprites[(int)enums.Entity.Mage]);
                            mage.LoadProjectile(this.sprites[(int)enums.Entity.Mage][(int)enums.Movement.PROJECTILE]);
                            this.mobList.Add(mage);
                        }
                        break;

                    default:
                        if (random < 90)
                        {
                            Zombie zombie = new Zombie(pos);
                            zombie.LoadContent(this.sprites[(int)enums.Entity.Zombie]);
                            this.mobList.Add(zombie);
                        }
                        else if (random >= 90)
                        {
                            // here we spawn other stuff for example Mages
                            Mage mage = new Mage(pos);
                            mage.LoadContent(this.sprites[(int)enums.Entity.Mage]);
                            mage.LoadProjectile(this.sprites[(int)enums.Entity.Mage][(int)enums.Movement.PROJECTILE]);
                            this.mobList.Add(mage);
                        }
                        break;
                }
            }            
        }

        protected bool DeleteDead() {
            for (int i = 0; i < this.deadList.Count; i++) {
                if (this.deadList[i].DeleteDead())
                    this.deadList.RemoveAt(i);
            }
            if (this.deadList.Count == 0)
                return true;
            return false;
        }

        public void Update(Player player) {
            // we go through all enemies in our mobList and Update them.
            //if they are dead we remove them from the list so they basicly dont exist anymore.
            player.target.Clear();
            if (this.mobList.Count != 0)
            {
                for (int i = 0; i < this.mobList.Count; i++)
                {
                    if (this.mobList[i].Update(player) == -1)
                    {
                        this.deadList.Add(new Dead(this.mobList[i].position, this.sprites[(int)enums.Entity.Dead][0][0][0], this.sprites[(int)enums.Entity.Zombie][(int)enums.Movement.DEAD]));
                        this.mobList.RemoveAt(i);
                    }
                }
            }
            else
            {
                if (DeleteDead())
                {

                    this.mobList.Clear();

                    this.wave++;
                    //this.maxRandom *= this.wave;
                    this.ammount = this.ammount * this.wave;

                    this.deadList.Clear();
                    this.Spawn(player);
                }
            }
        }

        public void Draw(SpriteBatch _spriteBatch, Map m) {
            // we go through the enemy list and draw them all.

            for (int i = 0; i < this.deadList.Count; i++) {
                this.deadList[i].Draw(_spriteBatch, m);
            }
            for (int i = 0; i < this.mobList.Count; i++)
            {
                this.mobList[i].Draw(_spriteBatch, m);
            }
           
        }
    }
}
