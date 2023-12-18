using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueValley.Entities;
using RogueValley.Maps;

namespace RogueValley.Entities
{
    class Enemies
    {

        protected int hp, defence, damage, speed, aniCount, aniTimer, aniTimerMax, entityDir, lastDir, reach, piercing;
        protected int pAttackTimer, pAttackTimerMax;
        protected int[] drawPosition, spriteSize, lastMove;
        protected int[] position, mov;
        protected Texture2D[][] movSprites, idleSprites, pAttackSprite, sAttackSprite;
        protected Texture2D sprite;

        public void Init() {

            this.position[0] = 100;
            this.position[1] = 200;

            this.drawPosition = new int[2];

            this.aniCount = 0;
            this.aniTimer = 0;
            this.aniTimerMax = 5;
            this.spriteSize = new int[2];
            this.spriteSize[0] = 40;
            this.spriteSize[1] = 80;
        }

        public void LoadContent(Texture2D[][] movSprites, Texture2D[][] idleSprites, Texture2D[][] pAttackSprite, Texture2D[][] sAttackSprite) {

            this.movSprites = movSprites;
            this.idleSprites = idleSprites;
            this.pAttackSprite = pAttackSprite;
            this.sAttackSprite = sAttackSprite;
            this.sprite = idleSprites[0][0];

        }

        private int[] CalcdrawPos(Map m) {
            int[] drawPos = new int[2];

            drawPos[0] = this.position[0] + m.map_position[0];
            drawPos[1] = this.position[1] + m.map_position[1];

            return drawPos;
        }

        public SpriteBatch Draw(SpriteBatch _spriteBatch, Map m) {

            this.drawPosition = CalcdrawPos(m);
            _spriteBatch.Draw(this.sprite, new Rectangle(this.drawPosition[0], this.drawPosition[1], this.spriteSize[0], this.spriteSize[1]), Color.White);

            return _spriteBatch;
        }

        protected void Animation()
        {



            this.aniTimer++;

            if (this.aniTimer == this.aniTimerMax)
            {
                this.aniTimer = 0;
                this.aniCount++;
            }

            if (this.lastMove[0] != 0 || this.lastMove[1] != 0)
            {
                if (this.aniCount > 5)
                {
                    this.aniCount = 0;
                }

                if (this.lastMove[0] != 0)
                {
                    if (this.lastMove[0] > 0)
                    {
                        this.entityDir = 0;
                        this.lastDir = 0;
                    }
                    else
                    {
                        this.entityDir = 1;
                        this.lastDir = 1;
                    }
                }
                else
                {
                    this.entityDir = this.lastDir;
                }
                this.sprite = this.movSprites[this.entityDir][this.aniCount];
            }
            else
            {
                if (this.aniCount > 3)
                {
                    this.aniCount = 0;
                }
                this.sprite = this.idleSprites[this.entityDir][this.aniCount];
            }


        }

        protected virtual Player Ai(Player player){
            return player;
        }

        public void TakeDamage(int damage, float piercing) {

        }
        public virtual Player PrimaryAttack(Player player)
        {
            return player;
        }

        public void PrimaryAttackAni() {
                   
        }

        public virtual void SecondaryAttack(Player player)
        {

        }

    }
    class Zombie : Enemies {

        public Zombie(int[] pos) {


            base.pAttackTimer = 0;
            base.pAttackTimerMax = 5;

            base.lastMove = new int[2] {0, 0 };

            base.hp = 100;
            base.damage = 10;
            base.position = pos;
            base.defence = 10;
            base.reach = 100;
            base.speed = 5;
            base.piercing = 5;

            base.Init();
        }

        public Player Update(Player player) {

            this.Ai(player);
            Console.WriteLine(this.drawPosition);
            Console.WriteLine("\n");
            Console.WriteLine(this.position);
            return player;
        }

        protected override Player Ai(Player player) {

            mov = new int[2];
            int x = 0;
            int y = 0;

            mov[0] = (int)(((float)player.playerPosition[0]) - base.position[0]);
            mov[1] = (int)(((float)player.playerPosition[1]) - base.position[1]);

            if (mov[0] < 0)
            {
                x = mov[0] * -1;
            }
            else {
                x = mov[0];
            }

            if (mov[1] < 0)
            {
                y = mov[1] * -1;
            }
            else
            {
                y = mov[1];
            }

            int n = x + y;
            if (n <= base.reach && n >= -base.reach)
            {
                base.lastMove[0] = 0;
                base.lastMove[1] = 0;
                this.PrimaryAttack(player);
            }
            else {
                base.pAttackTimer = 0;
                base.lastMove[0] = (int)(((float)mov[0] / (float)n) * base.speed);
                base.lastMove[1] = (int)(((float)mov[1] / (float)n) * base.speed);
                base.position[0] += base.lastMove[0];
                base.position[1] += base.lastMove[1];
            }
            base.Animation();
            return player;
        }

        public override Player PrimaryAttack(Player player) {
            base.pAttackTimer++;

            if (base.pAttackTimer >= base.pAttackTimerMax * (base.pAttackSprite[base.lastDir].Length - 1)) {
                base.pAttackTimer = 0;
                base.sprite = base.movSprites[0][0];
                player.TakeDamage(base.damage, base.piercing);
            }

            if (base.pAttackTimer % base.pAttackTimerMax == 0) {
                base.sprite = base.pAttackSprite[base.lastDir][(int)(base.pAttackTimer / base.pAttackTimerMax)];
            }

            

            return player;
        }
                
        public override void SecondaryAttack(Player player)
        {

        }
    }
}
