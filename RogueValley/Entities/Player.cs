using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueValley.Entities;
using RogueValley.Maps;
using static RogueValley.enums;
using RogueValley;

namespace RogueValley.Entities
{
    class Player
    {
        // PLAYER VARIABLES:

        public int damage, hp, defence, maxhp, maxtarget, reach;
        public float piercing, sAttackMulit;
        public List<Enemies> target;

        public Weapon weapon;

        // Player Sprites:

        public Texture2D playerSprite;
        public Texture2D[][] playerAniSprites, playerIdleSprites;
        public Texture2D[][][] pAttackSprite, sAttackSprite;

        // Player Positional Variables:

        public int playerDirection, playerLastDir, speed;
        public int[] playerPosition, drawPosition, lastMovement;
        public bool sAttackTrigger;

        // Player Animation Variables:

        public int animationCount, animationTimer, animationMaxTime, immunityFrames, maxImmFrames, AttackCooldown, pAttackTimer, pAttackTimerMax, sAttackTimer, sAttackTimerMax;

        // OTHER:

        Random rnd;
        int random, AttackCooldownMax;

        public Player(int[] pos, int animax = 8, int speed = 20)
        {
            // Player Positional Variables:
            {
                this.playerPosition = new int[2];
                this.playerPosition[0] = pos[0];
                this.playerPosition[1] = pos[1];

                this.drawPosition = new int[2];
                this.drawPosition[0] = pos[0];
                this.drawPosition[1] = pos[1];
            }
            // Player Animation Variables:
            {
                this.animationCount = 0;
                this.animationTimer = 0;
                this.animationMaxTime = 5;

                this.playerLastDir = 1;         // 0 = right | 1 = left
                this.playerDirection = 1;       // 0 = right | 1 = left

                this.AttackCooldown = 0;
                this.AttackCooldownMax = 10;

                this.pAttackTimer = 0;
                this.pAttackTimerMax = 3;

                this.sAttackTimer = 0;
                this.sAttackTimerMax = 5;
            }
            // other Player Variables:
            {
                this.speed = speed;
                this.hp = 100;
                this.maxhp = 100;
                this.defence = 5;
                this.immunityFrames = 0;
                this.maxImmFrames = 20;
                this.piercing = 5.0f;
                this.damage = 100;
                this.sAttackMulit = 2.5f;
                this.sAttackTrigger = false;

                this.reach = 200;

                this.target = new List<Enemies>();
                this.maxtarget = 20;
            }
            // OTHER:
            {
                rnd = new Random();
                this.random = rnd.Next(0, 6);
            }
        }
        public void LoadContent(Texture2D[][] pas, Texture2D[][] pis, Texture2D[][][] pAttack, Texture2D[][][] sAttack)
        {
            // Player Sprites:

            this.playerAniSprites = pas;
            this.playerIdleSprites = pis;

            this.playerSprite = pis[0][0];
            this.pAttackSprite = pAttack;
            this.sAttackSprite = sAttack;
        }
        public void Movement(int[] direction, Map map)
        {
            this.lastMovement = direction;

            if (!(direction[0] == 0 && direction[1] == 0))
            {
                weapon.ResetAnimation();
                /*
                this.pAttackTimer = 0;
                this.sAttackTimer = 0;
                this.AttackCooldown = 0;*/
            }
            if (0 <= (this.playerPosition[0] + (this.speed / 10) * direction[0]) && (this.playerPosition[0] + (this.speed / 10) * direction[0]) <= map.mapSize[0] - 35)
            {
                this.playerPosition[0] += (this.speed / 10) * direction[0];
            }
            if (0 <= (this.playerPosition[1] + (this.speed / 10) * direction[1]) && (this.playerPosition[1] + (this.speed / 10) * direction[1]) <= map.mapSize[1] - 90)
            {
                this.playerPosition[1] += (this.speed / 10) * direction[1];
            }
        }

        public void Update()
        {
            if (weapon.AttackCooldown > 0) {
                weapon.AttackCooldown--;
            }

            this.Animation();
            if (immunityFrames > 0)
            {
                immunityFrames--;
            }
            else
            {
                immunityFrames = 0;
            }
            if (this.target.Count != 0 && this.lastMovement[0] == 0 && this.lastMovement[1] == 0)
            {
                int rCount = 0;
                int lCount = 0;
                List<Enemies> tenemies = new List<Enemies>();

                // We count where the enemies are:

                for (int i = 0; i < this.target.Count; i++)
                {
                    if (this.target[i].position[0] < this.playerPosition[0])
                    {
                        lCount++;
                    }
                    else
                    {
                        rCount++;
                    }
                }
                // then we add the enemies wich are on the side of the player with the most enemies to an List:
                // if they are equal we just add them on the last direction into the list.
                if (rCount < lCount)
                {
                    this.playerDirection = 1;
                    for (int i = 0; i < this.target.Count; i++)
                    {
                        if (this.target[i].position[0] < this.playerPosition[0])
                        {
                            tenemies.Add(this.target[i]);
                        }
                    }
                }
                else if (rCount > lCount)
                {
                    this.playerDirection = 0;
                    for (int i = 0; i < this.target.Count; i++)
                    {
                        if (this.target[i].position[0] > this.playerPosition[0])
                        {
                            tenemies.Add(this.target[i]);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.target.Count; i++)
                    {
                        if (this.playerDirection == 0)
                        {
                            if (this.target[i].position[0] > this.playerPosition[0])
                            {
                                tenemies.Add(this.target[i]);
                            }
                        }
                        else
                        {
                            if (this.target[i].position[0] < this.playerPosition[0])
                            {
                                tenemies.Add(this.target[i]);
                            }
                        }
                    }
                }
                // we attack the previous generated List with either primary or secondary wich we choose randomly:
                if (!this.sAttackTrigger)
                {
                    if(tenemies.Count != 0)
                        this.PrimaryAttack(tenemies);
                }
                else
                {
                    if (tenemies.Count != 0)
                        this.SecondAttack(tenemies);
                }
                // after all we clear our list so we dont attack non-existend enemies:
                tenemies.Clear();          
            }
        }

        protected void Animation()
        {
            if (!(this.lastMovement[0] == 0 && this.lastMovement[1] == 0 && this.target.Count != 0 && this.AttackCooldown == 0)) {
            
                this.animationTimer++;
                if(this.animationTimer == this.animationMaxTime)
                {
                    this.animationTimer = 0;
                    this.animationCount++;
                }
                if (this.lastMovement[0] != 0 || this.lastMovement[1] != 0)
                {
                    if (this.animationCount > 5)
                    {
                        this.animationCount = 0;
                    }
                    if (this.lastMovement[0] != 0)
                    {
                        if (this.lastMovement[0] == 1)
                        {
                            this.playerDirection = 0;
                            this.playerLastDir = 0;
                        }
                        else
                        {
                            this.playerDirection = 1;
                            this.playerLastDir = 1;
                        }
                    }
                    else
                    {
                        this.playerDirection = this.playerLastDir;
                    }
                    this.playerSprite = this.playerAniSprites[this.playerDirection][this.animationCount];
                }
                else
                {
                    if (this.animationCount > 3)
                    {
                        this.animationCount = 0;
                    }
                    this.playerSprite = this.playerIdleSprites[this.playerDirection][this.animationCount];
                }
            }
        }

        public void PrimaryAttack(List<Enemies> e)
        {
            weapon.PrimaryAttack(e, this);

            /*
            if (this.AttackCooldown == 0)
            {
                this.pAttackTimer++;
                if (this.pAttackTimer >= this.pAttackTimerMax * (this.pAttackSprite[this.playerDirection].Length - 1))
                {
                    this.pAttackTimer = 0;
                    // we want to attack all enemies in the list e:
                    // but maximum this.maxtarget ammount.
                    if (e.Count < this.maxtarget)
                    {
                        for (int i = 0; i < e.Count; i++)
                        {
                            e[i].TakeDamage(this.damage, this.piercing);
                        }
                    }
                    else {
                        for (int i = 0; i < this.maxtarget; i++)
                        {
                            e[i].TakeDamage(this.damage, this.piercing);
                        }
                    }
                    this.random = rnd.Next(0, 6);
                    this.AttackCooldown = this.AttackCooldownMax;
                }
                if (this.pAttackTimer % this.pAttackTimerMax == 0)
                {
                    this.playerSprite = this.pAttackSprite[this.playerDirection][(int)(this.pAttackTimer / this.pAttackTimerMax)];
                }
                return;
            }
            this.AttackCooldown--;
            */
        }

        public void SecondAttack(List<Enemies> e)
        {
            weapon.SecondaryAttack(e, this);


            /*
            if (this.AttackCooldown == 0)
            {
                this.sAttackTimer++;
                if (this.sAttackTimer >= this.sAttackTimerMax * (this.sAttackSprite[this.playerDirection].Length - 1))
                {
                    this.sAttackTimer = 0;
                    // we want to attack all enemies in the list e:
                    // but maximum this.maxtarget ammount.
                    if (e.Count < this.maxtarget)
                    {
                        for (int i = 0; i < e.Count; i++)
                        {
                            e[i].TakeDamage((int)((float)this.damage * this.sAttackMulit), this.piercing);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < this.maxtarget; i++)
                        {
                            e[i].TakeDamage((int)((float)this.damage * this.sAttackMulit), this.piercing);
                        }
                    }
                    this.random = rnd.Next(0, 6);
                    this.AttackCooldown = this.AttackCooldownMax;
                }
                if (this.sAttackTimer % this.sAttackTimerMax == 0)
                {
                    this.playerSprite = this.sAttackSprite[this.playerDirection][(int)(this.sAttackTimer / this.sAttackTimerMax)];
                }
                return;
            }
            this.AttackCooldown--;
            */
        }

        public void TakeDamage(int damage, int piercing)
        {
            // we need immunity frames so we dont die instantly when a lot of enemies attack the player at once.
            if (this.immunityFrames == 0)
            {
                this.immunityFrames = this.maxImmFrames;
                // we calculate if the enemy pierces the players defence if not we calculate the new damage so the
                // enemy does less damage.
                if (piercing < this.defence)
                {
                    this.hp -= (int)((float)damage * ((float)((float)piercing/(float)this.defence) + 0.2f));
                }
                else
                {
                    this.hp -= damage;
                }
                if (this.hp <= 0){
                    this.GameOver();
                }
            }
        }

        public void GameOver()
        {
            return;
        }
    }
}
