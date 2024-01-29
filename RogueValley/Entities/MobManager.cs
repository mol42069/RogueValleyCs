using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueValley.Maps;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace RogueValley.Entities
{
    class MobManager
    {
        private Texture2D[][][][] sprites;
        private Texture2D[] hBarSprites;
        private SpriteFont font;
        public List<Enemies> mobList, deadList, drawList;
        private Random rnd;
        public Stopwatch timer;

        private int[] spawnRate, bgSize;
        public int ammount, wave, maxRandom, afterWaveUp, spawnTimer, spawnTimerMax, spawnTimerRnd, spawnAmmount, roundTimerMax, roundTimer;

        public MobManager(Player player, int[] bgSize) {

            this.mobList = new List<Enemies>();
            this.deadList = new List<Enemies>();
            this.drawList = new List<Enemies>();

            this.rnd = new Random();
            this.timer = new Stopwatch();

            this.afterWaveUp = 0;

            this.ammount = 10;
            this.wave = 0;
            this.maxRandom = 100;

            this.spawnTimer = 0;
            this.spawnTimerMax = 100;
            this.spawnAmmount = 2;
            this.roundTimerMax = 10;
            this.roundTimer = this.roundTimerMax;

            this.spawnRate = new int[2];
            this.bgSize = bgSize;
        }

        public void LoadContent(Texture2D[][][][] sprites, Texture2D[] hBarSprites, SpriteFont font) {
            this.sprites = sprites;
            this.hBarSprites = hBarSprites;
            this.font = font;
        }
        public void RmList() {
            this.mobList.Clear();
            this.deadList.Clear();
        }
        protected void Spawn(Player player) {
            for (int i = 0; i < this.spawnAmmount; i++)
            {

                int[] pos = new int[2];
                pos[0] = player.playerPosition[0];
                pos[1] = player.playerPosition[1];

                this.spawnTimerRnd = rnd.Next(10) - 5;


                while (!(!(pos[1] < (player.playerPosition[1] + 1080) && pos[1] > (player.playerPosition[1] - 1080)) || !(pos[0] < (player.playerPosition[0] + 1920) && pos[0] > (player.playerPosition[0] - 1920))))
                {
                    pos[0] = rnd.Next(0, this.bgSize[0]);
                    pos[1] = rnd.Next(0, this.bgSize[1]);
                }



                int random = rnd.Next(0, 100);
                switch (this.wave)
                {
                    case 1:

                        if (random < 80)
                        {
                            Zombie zombie = new Zombie(pos);
                            zombie.LoadContent(this.sprites[(int)enums.Entity.Zombie], this.hBarSprites);
                            this.mobList.Add(zombie);

                        }
                        else if (random >= 80)
                        {
                            if (random >= 95)
                            {

                                Ogre ogre = new Ogre(pos);
                                ogre.LoadContent(this.sprites[(int)enums.Entity.Ogre], this.hBarSprites);
                                this.mobList.Add(ogre);

                            }
                            else
                            {
                                // here we spawn other stuff for example Mages
                                Mage mage = new Mage(pos);
                                mage.LoadContent(this.sprites[(int)enums.Entity.Mage], this.hBarSprites);
                                mage.LoadProjectile(this.sprites[(int)enums.Entity.Mage][(int)enums.Movement.PROJECTILE]);
                                this.mobList.Add(mage);
                            }
                        }
                        break;

                    default:
                        if (random < 80 - this.wave * 2)
                        {
                            Zombie zombie = new Zombie(pos);
                            zombie.LoadContent(this.sprites[(int)enums.Entity.Zombie], this.hBarSprites);
                            this.mobList.Add(zombie);

                        }
                        else if (random >= 80 - this.wave * 2)
                        {
                            if (random >= 95 - this.wave)
                            {

                                Ogre ogre = new Ogre(pos);
                                ogre.LoadContent(this.sprites[(int)enums.Entity.Ogre], this.hBarSprites);
                                this.mobList.Add(ogre);

                            }
                            else
                            {
                                // here we spawn other stuff for example Mages
                                Mage mage = new Mage(pos);
                                mage.LoadContent(this.sprites[(int)enums.Entity.Mage], this.hBarSprites);
                                mage.LoadProjectile(this.sprites[(int)enums.Entity.Mage][(int)enums.Movement.PROJECTILE]);
                                this.mobList.Add(mage);
                            }
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

        // WE CHANGE THE ORDER OF THE MOBLIST VIA QUICKSORT SO THAT THE CLOSEST ENEMIES ARE FIRST:

        private Enemies[] swap(Enemies[] arr, int i, int j) {

            Enemies temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;

            return arr;
        }

        private int partition(Enemies[] arr, int low, int high) {

            int pivot = arr[high].distance;

            int i = (low - 1);

            for (int j = low; j <= high; j++) {

                if (arr[j].distance < pivot) {
                    i++;
                    arr = this.swap(arr, i, j);
                }
            }
            arr = this.swap(arr, i + 1, high);
            return (i + 1);
        }

        private void quickSort(Enemies[] arr, int low, int high) {

            if (low < high) {

                int pi = this.partition(arr, low, high);

                quickSort(arr, low, pi - 1);
                quickSort(arr, pi + 1, high);
            }

        }

        private void MobSorter() {

            Enemies[] Arr = new Enemies[this.mobList.Count];
            for (int i = 0; i < this.mobList.Count; i++) {
                Arr[i] = this.mobList[i];
            }
            this.quickSort(Arr, 0, this.mobList.Count - 1);

            this.mobList.Clear();

            for (int i = 0; i < Arr.Length; i++)
            {
                this.mobList.Add(Arr[i]);
            }
        }


        // WE SORT ENEMIES SO WE CAN DRAW THE LOWER ONES LATER THAN THE UPPER-ONES:
        private int partitionDraw(Enemies[] arr, int low, int high) {
            int pivot = arr[high].position[1] + arr[high].spriteSize[1] - 20;

            int i = (low - 1);

            for (int j = low; j <= high; j++)
            {

                if (arr[j].position[1] + arr[j].spriteSize[1] - 20 < pivot)
                {
                    i++;
                    arr = this.swap(arr, i, j);
                }
            }
            arr = this.swap(arr, i + 1, high);
            return (i + 1);
        }

        private void quickSortDraw(Enemies[] arr, int low, int high) {
            if (low < high) {
                int pi = this.partitionDraw(arr, low, high);

                this.quickSortDraw(arr, low, pi - 1);
                this.quickSortDraw(arr, pi + 1, high);
            }
        }

        private void MobSorterDraw() {
            Enemies[] Arr = new Enemies[this.mobList.Count];
            for (int i = 0; i < this.mobList.Count; i++) {
                Arr[i] = this.mobList[i];
            }

            this.quickSort(Arr, 0, this.mobList.Count - 1);
            this.drawList.Clear();
            for (int i = 0; i < this.mobList.Count; i++) 
            {
                this.drawList.Add(Arr[i]);
            }

        }

        public void Update(Player player, Game1 g1, UpgradeManager upgradeManager) {
            // we go through all enemies in our mobList and Update them.
            //if they are dead we remove them from the list so they basicly dont exist anymore.            
            player.target.Clear();
            if (this.wave == 0) {

                this.wave++;
                //this.maxRandom *= this.wave;
                this.ammount = 10 * this.wave;
                this.timer.Start();
            }
            if (this.roundTimer != 0)
            {
                this.spawnTimer++;

                if (this.spawnTimer >= this.spawnTimerMax + this.spawnTimerRnd)
                {
                    this.Spawn(player);
                    this.spawnTimer = 0;
                }
                if (this.timer.Elapsed.Seconds == 1)
                {
                    this.roundTimer--;
                    this.timer.Stop();
                    this.timer.Reset();
                    this.timer.Start();
                }

                for (int i = 0; i < this.mobList.Count; i++)
                {
                    if (this.mobList[i].Update(player) == -1)
                    {
                        this.deadList.Add(new Dead(this.mobList[i].position, this.sprites[(int)enums.Entity.Dead][0][0][0], this.sprites[(int)enums.Entity.Zombie][(int)enums.Movement.DEAD], this.mobList[i].spriteSize));
                        switch (this.mobList[i]) {

                            case Ogre:
                                g1.score += 10;
                                break;

                            case Mage:
                                g1.score += 5;
                                break;

                            case Zombie:
                                g1.score += 1;
                                break;
                        }
                        this.mobList.RemoveAt(i);
                    }
                }
                if (this.mobList.Count > 1)
                {
                    this.MobSorter();
                    //this.MobSorterDraw();
                }
                this.afterWaveUp = 0;
            }
            else
            {
                if (DeleteDead())
                {
                    this.mobList.Clear();
                    this.deadList.Clear();

                    if (this.afterWaveUp == 0) { 
                        this.afterWaveUp = 1;
                        g1.gameState = 3;
                    }
                    if (this.afterWaveUp == 2)
                    {
                        this.roundTimer = roundTimerMax;
                        player.hp += player.regeneration;

                        if (player.hp > player.maxhp) player.hp = player.maxhp;

                        this.wave++;
                        //this.maxRandom *= this.wave;
                        this.ammount = 10 * this.wave;
                        this.spawnTimer = 0;
                        if (this.spawnTimerMax * 0.8 < 40)
                        {
                            this.spawnAmmount += this.rnd.Next(3);
                        }
                        else {
                            this.spawnTimerMax = (int)(this.spawnTimerMax * 0.8);
                        }

                        this.timer.Stop();
                        this.timer.Reset();
                        this.timer.Start();

                        this.afterWaveUp = 0;
                    }
                }
            }
            player.mobList = this.mobList;
        }

        public void Draw(SpriteBatch _spriteBatch, Map m) {
            // we go through the enemy list and draw them all.

            //for (int i = 0; i < this.drawList.Count; i++)
            //    this.drawList[i].Draw(_spriteBatch, m);

            this.MobSorterDraw();

            for (int i = 0; i < this.deadList.Count; i++) {
                this.deadList[i].Draw(_spriteBatch, m);
            }
            for (int i = this.drawList.Count - 1; i >= 0 ; i--)
            {
                this.drawList[i].Draw(_spriteBatch, m);
            }
            _spriteBatch.DrawString(this.font, this.roundTimer.ToString(), new Microsoft.Xna.Framework.Vector2(900, 50), Color.White);

        }
    }
}
