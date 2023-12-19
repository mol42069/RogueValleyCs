using System;
using System.Net;
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
        protected int pAttackTimer, pAttackTimerMax, AttackCooldown, sAttackTimerMax, sAttackTimer;
        protected float sAttackMult;
        protected int[] drawPosition, spriteSize, lastMove;
        protected int[] mov;
        public int[] position;
        protected Texture2D[][] movSprites, idleSprites, pAttackSprite, sAttackSprite;
        protected Texture2D sprite;
        protected Random rnd;

        public void Init()
        {

            this.drawPosition = new int[2];

            this.aniCount = 0;
            this.aniTimer = 0;
            this.aniTimerMax = 5;
            this.spriteSize = new int[2];
            this.spriteSize[0] = 100;
            this.spriteSize[1] = 100;

            rnd = new Random();
        }

        public void LoadContent(Texture2D[][][] sprites)
        {

            this.movSprites = sprites[(int)enums.Movement.MOVE];
            this.idleSprites = sprites[(int)enums.Movement.IDLE];
            this.pAttackSprite = sprites[(int)enums.Movement.PATTACK];
            this.sAttackSprite = sprites[(int)enums.Movement.SATTACK];
            this.sprite = idleSprites[0][0];

        }

        private int[] CalcdrawPos(Map m)
        {
            int[] drawPos = new int[2];

            drawPos[0] = this.position[0] + m.map_position[0];
            drawPos[1] = this.position[1] + m.map_position[1];

            return drawPos;
        }

        public SpriteBatch Draw(SpriteBatch _spriteBatch, Map m)
        {

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
                if (this.aniCount > 5)
                {
                    this.aniCount = 0;
                }
                this.sprite = this.idleSprites[this.entityDir][this.aniCount];
            }
        }

        public virtual Player Update(Player player)
        {
            return player;
        }

        protected virtual Player Ai(Player player)
        {
            return player;
        }

        public void TakeDamage(int damage, float piercing)
        {

        }
        public virtual Player PrimaryAttack(Player player)
        {
            return player;
        }

        public void PrimaryAttackAni()
        {

        }

        public virtual Player SecondaryAttack(Player player)
        {
            return player;
        }

    }
    class Zombie : Enemies
    {

        int random;

        public Zombie(int[] pos)
        {


            base.pAttackTimer = 0;
            base.pAttackTimerMax = 5;

            base.sAttackTimer = 0;
            base.sAttackTimerMax = 10;

            base.lastMove = new int[2] { 0, 0 };

            base.hp = 100;
            base.damage = 10;
            base.sAttackMult = 1.3f;

            base.position = pos;
            base.defence = 10;
            base.reach = 100;
            base.speed = 5;
            base.piercing = 5;

            base.Init();

            this.random = rnd.Next(0, 6);
        }

        public override Player Update(Player player)
        {

            this.Ai(player);
            Console.WriteLine(this.drawPosition);
            Console.WriteLine("\n");
            Console.WriteLine(this.position);
            return player;
        }

        protected override Player Ai(Player player)
        {

            mov = new int[2];
            int x = 0;
            int y = 0;

            mov[0] = (int)(((float)player.playerPosition[0]) - base.position[0]);
            mov[1] = (int)(((float)player.playerPosition[1]) - base.position[1]);

            if (mov[0] < 0)
            {
                x = mov[0] * -1;
            }
            else
            {
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

                if (random != 0)
                {
                    this.PrimaryAttack(player);
                }
                else
                {
                    this.SecondaryAttack(player);
                }
            }
            else
            {
                base.pAttackTimer = 0;
                base.lastMove[0] = (int)(((float)mov[0] / (float)n) * base.speed);
                base.lastMove[1] = (int)(((float)mov[1] / (float)n) * base.speed);
                base.position[0] += base.lastMove[0];
                base.position[1] += base.lastMove[1];

                base.Animation();
            }
            if (base.pAttackTimer == 0 && base.sAttackTimer == 0)
            {
                base.Animation();
            }
            return player;
        }
        public override Player PrimaryAttack(Player player)
        {

            if (base.AttackCooldown == 0)
            {
                base.pAttackTimer++;
                if (base.pAttackTimer >= base.pAttackTimerMax * (base.pAttackSprite[base.lastDir].Length - 1))
                {
                    base.pAttackTimer = 0;
                    player.TakeDamage(base.damage, base.piercing);
                    this.random = rnd.Next(0, 6);
                    base.AttackCooldown = 20;
                }
                if (base.pAttackTimer % base.pAttackTimerMax == 0)
                {
                    base.sprite = base.pAttackSprite[base.lastDir][(int)(base.pAttackTimer / base.pAttackTimerMax)];
                }
                return player;
            }
            base.AttackCooldown--;
            return player;
        }

        public override Player SecondaryAttack(Player player)
        {
            if (base.AttackCooldown == 0)
            {
                base.sAttackTimer++;
                if (base.sAttackTimer >= base.sAttackTimerMax * (base.sAttackSprite[base.lastDir].Length - 1))
                {
                    base.sAttackTimer = 0;
                    player.TakeDamage((int)(base.damage * base.sAttackMult), base.piercing);
                    this.random = rnd.Next(0, 6);
                    base.AttackCooldown = 20;
                }
                if (base.sAttackTimer % base.sAttackTimerMax == 0)
                {
                    base.sprite = base.sAttackSprite[base.lastDir][(int)(base.sAttackTimer / base.sAttackTimerMax)];
                }
                return player;
            }
            base.AttackCooldown--;
            return player;
        }
    }
}
