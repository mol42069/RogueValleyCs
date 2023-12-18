using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueValley.Entities;

namespace RogueValley.Entities
{
    class Enemies
    {

        protected int hp, defence, damage, speed, aniCount, aniTimer, aniTimerMax, entityDir, lastDir;
        protected int[] position, drawPosition, spriteSize, lastMove;
        protected Texture2D[][] movSprites, idleSprites;
        protected Texture2D sprite;

        public void Init() { 
        
            this.aniCount = 0;
            this.aniTimer = 0;
            this.aniTimerMax = 5;
            this.spriteSize = new int[2];
            this.spriteSize[0] = 40;
            this.spriteSize[1] = 80;
            this.drawPosition = position;

        }

        public void LoadContent(Texture2D[][] movSprites, Texture2D[][] idleSprites) {

            this.movSprites = movSprites;
            this.idleSprites = idleSprites;
            this.sprite = idleSprites[0][0];
        
        }
        public SpriteBatch Draw(SpriteBatch _spriteBatch) {

            //this.Animation();
            this.drawPosition = position;
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
                    if (this.lastMove[0] == 1)
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

        public void TakeDamage(int damage, float piercing) {
        


        }
        public virtual void PrimaryAttack(Player player)
        {


        }

        public virtual void SecondaryAttack(Player player)
        {



        }

    }
    class Zombie : Enemies {
    
        public Zombie(int[] pos) {

            base.Init();

            base.hp = 100;
            base.damage = 10;
            base.position = pos;
            base.defence = 10;
            base.speed = 10;

        }

        public void Update() {

            base.position[0]++;

        }

        public override void PrimaryAttack(Player player) {
            
            
            }

        public override void SecondaryAttack(Player player)
        {


        }



    }
}
