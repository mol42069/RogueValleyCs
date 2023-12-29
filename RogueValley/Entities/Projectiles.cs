using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueValley.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RogueValley.Entities
{
    class Projectiles
    {
        protected Texture2D[][] sprites;
        protected Texture2D sprite;
        protected int aniCount, aniTimer, aniTimerMax, speed, direction, reach, playerReach, aniHitCount, damage, piercing;
        protected int[] position, drawPosition, spriteSize, finalPos, finalSize;

        public Projectiles() {
        
        
        }

        public virtual bool Update(Player player) {        
            return false;
        }

        private int[] CalcdrawPos(Map m)
        {
            // we calculate the enemy based on the player / map position

            int[] drawPos = new int[2];

            drawPos[0] = this.position[0] + m.map_position[0];
            drawPos[1] = this.position[1] + m.map_position[1];

            return drawPos;
        }

        public void Draw(SpriteBatch _spriteBatch, Map m) {
            this.drawPosition = CalcdrawPos(m);
            if (this.sprite != null)
                _spriteBatch.Draw(this.sprite, new Rectangle(this.drawPosition[0], this.drawPosition[1], this.spriteSize[0], this.spriteSize[1]), Color.White);

        }

        protected void Animation()
        {
            this.aniTimer++;
            if (this.aniTimer == this.aniTimerMax)
            {
                this.aniTimer = 0;
                this.aniCount++;
            }
            if (this.aniCount > this.sprites[this.direction].Length - 1) {
                this.aniCount = 0;
            }
            this.sprite = this.sprites[this.direction][this.aniCount];
        }
        protected virtual bool ExplosionAnimation()
        {
            this.aniTimer++;
            if (this.aniTimer == this.aniTimerMax)
            {
                this.aniTimer = 0;
                this.aniHitCount++;
            }
            if (this.aniHitCount > this.sprites[(int)enums.Direction.EXP].Length - 1)
            {
                return true;
            }
            this.sprite = this.sprites[(int)enums.Direction.EXP][this.aniHitCount];
            return false;
        }
        public virtual bool UpdatePlayer(Player player, List<Enemies> enemy) {

            return false;
        }

    }

    class FlameBall : Projectiles {

        public FlameBall(Texture2D[][] sprites, int[] pos, int[] finalPos, int damage, int piercing) {
            base.sprites = sprites;

            base.spriteSize = new int[2];
            base.spriteSize[0] = 40;
            base.spriteSize[1] = 40;

            base.finalSize = new int[2];
            base.finalSize[0] = 100;
            base.finalSize[1] = 100;

            base.position = pos;
            base.finalPos = finalPos;

            base.speed = 15;
            base.reach = 50;

            base.piercing = piercing;
            base.damage = damage;

            base.aniTimerMax = 5;
        }

        public override bool Update(Player player) {

            if ((base.finalPos[0] + base.reach > base.position[0] && base.finalPos[0] - base.reach < base.position[0] && base.finalPos[1] + base.reach > base.position[1] && base.finalPos[1] - base.reach < base.position[1]) || base.aniHitCount != 0)
            {

                base.spriteSize = base.finalSize;

                if (base.ExplosionAnimation()) {
                    return true;
                }

                if ((player.playerPosition[0] + base.reach > base.position[0] && player.playerPosition[0] - base.reach < base.position[0] && player.playerPosition[1] + base.reach > base.position[1] && player.playerPosition[1] - base.reach < base.position[1]))
                    player.TakeDamage(base.damage, base.piercing);


                return false;
            }
            else
            {
                int[] mov = new int[2];
                int x = 0;
                int y = 0;

                mov[0] = (int)(((float)base.finalPos[0]) - base.position[0]);
                mov[1] = (int)(((float)base.finalPos[1]) - base.position[1]);

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

                if (x < 0)
                {
                    if (y < 0)
                    {
                        if (x < y)
                        {
                            base.direction = (int)enums.Direction.LEFT;
                        }
                        else
                        {
                            base.direction = (int)enums.Direction.UP;
                        }
                    }
                    else
                    {
                        if (x < -y)
                        {
                            base.direction = (int)enums.Direction.LEFT;
                        }
                        else
                        {
                            base.direction = (int)enums.Direction.DOWN;
                        }
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        if (-x < y)
                        {
                            base.direction = (int)enums.Direction.RIGHT;
                        }
                        else
                        {
                            base.direction = (int)enums.Direction.UP;
                        }
                    }
                    else
                    {
                        if (-x < -y)
                        {
                            base.direction = (int)enums.Direction.RIGHT;
                        }
                        else
                        {
                            base.direction = (int)enums.Direction.DOWN;
                        }
                    }
                }

                int n = x + y;

                base.position[0] += (int)(((float)mov[0] / (float)n) * base.speed);
                base.position[1] += (int)(((float)mov[1] / (float)n) * base.speed);

                base.Animation();
                return false;
            }
        }
    }

        class PlayerFireBall : Projectiles
        {
            public PlayerFireBall(Texture2D[][] sprites, int[] pos, int[] finalPos, int damage, int piercing)
            {
                base.sprites = sprites;

                base.spriteSize = new int[2];
                base.spriteSize[0] = 40;
                base.spriteSize[1] = 40;

                base.finalSize = new int[2];
                base.finalSize[0] = 100;
                base.finalSize[1] = 100;

                base.position = pos;
                base.finalPos = finalPos;

                base.speed = 15;
                base.reach = 50;
            

                base.piercing = piercing;
                base.damage = damage;

                base.aniTimerMax = 5;
            }





            public override bool UpdatePlayer(Player player, List<Enemies> enemy)
            {
                if ((base.finalPos[0] + base.reach > base.position[0] && base.finalPos[0] - base.reach < base.position[0] && base.finalPos[1] + base.reach > base.position[1] && base.finalPos[1] - base.reach < base.position[1]) || base.aniHitCount != 0)
                {

                    base.spriteSize = base.finalSize;

                    if (base.ExplosionAnimation())
                    {
                        return true;
                    }

                    for (int i = 0; i < enemy.Count; i++)
                    {

                        if ((enemy[i].position[0] + base.reach > base.position[0] && enemy[i].position[0] - base.reach < base.position[0] && enemy[i].position[1] + base.reach > base.position[1] && enemy[i].position[1] - base.reach < base.position[1]))
                            enemy[i].TakeDamage(base.damage, base.piercing);
                    }
                    return false;
                }
                else
                {
                    int[] mov = new int[2];
                    int x = 0;
                    int y = 0;

                    mov[0] = (int)(((float)base.finalPos[0]) - base.position[0]);
                    mov[1] = (int)(((float)base.finalPos[1]) - base.position[1]);

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

                    if (x < 0)
                    {
                        if (y < 0)
                        {
                            if (x < y)
                            {
                                base.direction = (int)enums.Direction.LEFT;
                            }
                            else
                            {
                                base.direction = (int)enums.Direction.UP;
                            }
                        }
                        else
                        {
                            if (x < -y)
                            {
                                base.direction = (int)enums.Direction.LEFT;
                            }
                            else
                            {
                                base.direction = (int)enums.Direction.DOWN;
                            }
                        }
                    }
                    else
                    {
                        if (y < 0)
                        {
                            if (-x < y)
                            {
                                base.direction = (int)enums.Direction.RIGHT;
                            }
                            else
                            {
                                base.direction = (int)enums.Direction.UP;
                            }
                        }
                        else
                        {
                            if (-x < -y)
                            {
                                base.direction = (int)enums.Direction.RIGHT;
                            }
                            else
                            {
                                base.direction = (int)enums.Direction.DOWN;
                            }
                        }
                    }

                    int n = x + y;

                    base.position[0] += (int)(((float)mov[0] / (float)n) * base.speed);
                    base.position[1] += (int)(((float)mov[1] / (float)n) * base.speed);

                    base.Animation();
                    return false;
                }
            }
        }

    class PlayerExplodingBall : Projectiles
    {

        protected int[] finalExpPos;
        public PlayerExplodingBall(Texture2D[][] sprites, int[] pos, int[] finalPos, int damage, int piercing)
        {
            base.sprites = sprites;
            base.sprite = base.sprites[0][0];
            base.spriteSize = new int[2];
            base.spriteSize[0] = 40;
            base.spriteSize[1] = 40;

            base.finalSize = new int[2];
            base.finalSize[0] = 300;
            base.finalSize[1] = 300;

            base.position = pos;
            base.finalPos = finalPos;

            this.finalExpPos = new int[2];
            this.finalExpPos[0] = finalPos[0] - (base.finalSize[0] / 2);
            this.finalExpPos[1] = finalPos[1] - (base.finalSize[1] / 2);

            base.speed = 15;
            base.reach = 250;


            base.piercing = piercing;
            base.damage = damage;

            base.aniTimerMax = 5;
        }
        protected bool ExploAnimation()
        {
            base.aniTimer++;
            if (base.aniTimer == base.aniTimerMax)
            {
                base.aniTimer = 0;
                base.aniHitCount++;
            }
            if (base.aniHitCount > base.sprites[1].Length - 1)
            {
                return true;
            }
            base.sprite = base.sprites[1][base.aniHitCount];
            return false;
        }



        public override bool UpdatePlayer(Player player, List<Enemies> enemy)
        {
            if ((base.finalPos[0] + base.reach > base.position[0] && base.finalPos[0] - base.reach < base.position[0] && base.finalPos[1] + base.reach > base.position[1] && base.finalPos[1] - base.reach < base.position[1]) || base.aniHitCount != 0)
            {

                base.spriteSize = base.finalSize;
                /*
                base.position[0] = base.position[0] - (base.finalSize[0] / 2);
                base.position[1] = base.position[1] - (base.finalSize[1] / 2);
                */
                if (this.ExploAnimation())
                {
                    return true;
                }

                for (int i = 0; i < enemy.Count; i++)
                {

                    if ((enemy[i].position[0] + base.reach > base.position[0] && enemy[i].position[0] - base.reach < base.position[0] && enemy[i].position[1] + base.reach > base.position[1] && enemy[i].position[1] - base.reach < base.position[1]))
                        enemy[i].TakeDamage(base.damage, base.piercing);
                }
                return false;
            }
            else
            {
                int[] mov = new int[2];
                int x = 0;
                int y = 0;

                mov[0] = (int)(((float)base.finalPos[0]) - base.position[0]);
                mov[1] = (int)(((float)base.finalPos[1]) - base.position[1]);

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

                if (x < 0)
                {
                    if (y < 0)
                    {
                        if (x < y)
                        {
                            base.direction = (int)enums.Direction.LEFT;
                        }
                        else
                        {
                            base.direction = (int)enums.Direction.UP;
                        }
                    }
                    else
                    {
                        if (x < -y)
                        {
                            base.direction = (int)enums.Direction.LEFT;
                        }
                        else
                        {
                            base.direction = (int)enums.Direction.DOWN;
                        }
                    }
                }
                else
                {
                    if (y < 0)
                    {
                        if (-x < y)
                        {
                            base.direction = (int)enums.Direction.RIGHT;
                        }
                        else
                        {
                            base.direction = (int)enums.Direction.UP;
                        }
                    }
                    else
                    {
                        if (-x < -y)
                        {
                            base.direction = (int)enums.Direction.RIGHT;
                        }
                        else
                        {
                            base.direction = (int)enums.Direction.DOWN;
                        }
                    }
                }

                int n = x + y;

                base.position[0] += (int)(((float)mov[0] / (float)n) * base.speed);
                base.position[1] += (int)(((float)mov[1] / (float)n) * base.speed);
                return false;
            }
        }
    }

}
