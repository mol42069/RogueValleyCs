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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Numerics;

namespace RogueValley.Entities
{
    class Player
    {
        // PLAYER VARIABLES:

        public int damage, hp, defence, maxhp, maxtarget, regeneration, reach;
        public float piercing, sAttackMulit;
        public List<Enemies> target, mobList;
        public List<Projectiles> projectiles;

        public Weapon weapon;

        // Player Sprites:

        public Texture2D playerSprite;
        public Texture2D[][] playerAniSprites, playerIdleSprites;
        public Texture2D[][][] pAttackSprite, sAttackSprite;

        // Player Positional Variables:

        public int playerDirection, playerLastDir, speed;
        public int[] playerPosition, drawPosition, lastMovement, targetPos;
        public bool sAttackTrigger;

        // Player Animation Variables:

        public int animationCount, animationTimer, animationMaxTime, immunityFrames, maxImmFrames, AttackCooldown, pAttackTimer, pAttackTimerMax, sAttackTimer, sAttackTimerMax;
        // OTHER:
        public Rectangle playerRec;
        Random rnd;
        int random, damageTaken, damagePos_x, damagePos_y;
        bool damageAni;

        List<PlayerUI> damageIndicatorList;

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

                this.playerRec = new Rectangle(pos[0], pos[1], 100, 100);

    }
            // Player Animation Variables:
            {
                this.animationCount = 0;
                this.animationTimer = 0;
                this.animationMaxTime = 5;

                this.playerLastDir = 1;         // 0 = right | 1 = left
                this.playerDirection = 1;       // 0 = right | 1 = left

                this.AttackCooldown = 0;

                this.pAttackTimer = 0;
                this.pAttackTimerMax = 3;

                this.sAttackTimer = 0;
                this.sAttackTimerMax = 5;

                this.damageIndicatorList = new List<PlayerUI>();
                this.reach = 0;

            }
            // other Player Variables:
            {
                this.speed = speed;
                this.hp = 100;
                this.maxhp = 100;
                this.defence = 5;
                this.immunityFrames = 00;
                this.maxImmFrames = 30;
                this.piercing = 5.0f;
                this.damage = 100;
                this.sAttackMulit = 2.5f;
                this.sAttackTrigger = false;
                this.damageTaken = 0;

                this.damageAni = false;

                this.regeneration = 25;


                this.target = new List<Enemies>();
                this.maxtarget = 20;

                this.projectiles = new List<Projectiles>();
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
        {//
            this.lastMovement = direction;
            
            if (0 <= (this.playerPosition[0] + (this.speed / 10) * direction[0]) && (this.playerPosition[0] + (this.speed / 10) * direction[0]) <= map.mapSize[0] - 35)
            {
                this.playerPosition[0] += (this.speed / 10) * direction[0];
            }
            if (0 <= (this.playerPosition[1] + (this.speed / 10) * direction[1]) && (this.playerPosition[1] + (this.speed / 10) * direction[1]) <= map.mapSize[1] - 90)
            {
                this.playerPosition[1] += (this.speed / 10) * direction[1];
            }
            if (this.weapon is null) return;

            if (!(direction[0] == 0 && direction[1] == 0))
            {
                weapon.ResetAnimation();
                
                this.pAttackTimer = 0;
                this.sAttackTimer = 0;
                //this.AttackCooldown = 0;
            }

        }

        private void PlayerDrawPos(Map map) {
            this.drawPosition[0] = map.CalcdrawPosX(this.playerPosition[0]);
            this.drawPosition[1] = map.CalcdrawPosY(this.playerPosition[1]);
        }

        public void Update(Map map)
        {
            this.PlayerDrawPos(map);

            this.playerRec.X = this.playerPosition[0];
            this.playerRec.Y = this.playerPosition[1];

            if (this.reach != 0) this.reach = this.weapon.Update(this.reach);

            if (this.projectiles.Count > 0) {
                for (int i = 0; i < this.projectiles.Count; i++) {
                    if (this.projectiles[i].UpdatePlayer(this, this.mobList)) {
                        this.projectiles.RemoveAt(i);
                    }
                }
            }

            if (immunityFrames > 0)
            {
                immunityFrames--;
            }
            else
            {
                immunityFrames = 0;
            }

            if (this.lastMovement[0] == 0 && this.lastMovement[1] == 0)
            {
                List<Enemies> tenemies = new List<Enemies>();

                // we attack the closest enemy and change the direction of the player:
                if (!this.sAttackTrigger)
                {
                    if (this.mobList is not null)
                    {
                        if (this.mobList.Count != 0)
                        {
                            if (this.mobList[0] is not null && this.mobList[0].distance <= this.weapon.reach)
                            {
                                this.playerDirection = (int)enums.Direction.RIGHT;
                                if (this.mobList[0].position[0] < this.playerPosition[0])
                                    this.playerDirection = (int)enums.Direction.LEFT;
                                this.PrimaryAttack();
                            }
                        }
                    }
                }
                else
                {
                    this.SecondAttack(this.mobList, map);
                }
            }
            else 
            {
                this.sAttackTrigger = false;
                this.Animation();
                this.targetPos = null;
            }
            if (this.target.Count == 0 && (!this.sAttackTrigger && this.weapon is not Staff) || (this.weapon is Staff && this.targetPos is null)) {
                this.Animation();
            }
            if (this.weapon is null) return;
            if (this.weapon.AttackCooldown > 0)
            {
                this.weapon.AttackCooldown--;
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

        public void PrimaryAttack()
        {
            if (weapon is Staff) {
                int[] targetPos = this.mobList[0].targetPosition;
                weapon.PrimaryAttack(this.mobList, this, targetPos);
            }
            else if (weapon is StandartSword){
                weapon.PrimaryAttack(this.mobList[0], this);
            }
        }

        public void SecondAttack(List<Enemies> e, Map map)
        {
            if (weapon is Staff)
            {
                if (this.targetPos is null)
                {
                    this.targetPos = new int[2];
                    var mouseState = Mouse.GetState();
                    Point mousePos = new Point(mouseState.X, mouseState.Y);
                    this.targetPos[0] = mousePos.X - map.map_position[0];
                    this.targetPos[1] = mousePos.Y - map.map_position[1];
                }
                weapon.SecondaryAttack(e, this, targetPos);
            }
            else if (weapon is StandartSword)
            {
                weapon.SecondaryAttack(e, this);
            }
        }

        public void TakeDamage(int damage, int piercing)
        {
            // we need immunity frames so we dont die instantly when a lot of enemies attack the player at once.
            if (this.immunityFrames == 0)
            {
                this.immunityFrames = this.maxImmFrames;
                this.damageAni = true;
                // we calculate if the enemy pierces the players defence if not we calculate the new damage so the
                // enemy does less damage.
                if (piercing < this.defence)
                {
                    this.hp -= (int)((float)damage * ((float)((float)piercing/(float)this.defence) + 0.2f));
                    this.damageIndicatorList.Add(new PlayerUI((int)((float)damage * ((float)((float)piercing / (float)this.defence) + 0.2f))));
                }
                else
                {
                    this.hp -= damage;
                    this.damageIndicatorList.Add(new PlayerUI(damage));
                }
                if (this.hp <= 0){
                    this.hp = 0;
                    this.GameOver();
                }
            }
        }
        public void DrawDamage(SpriteBatch _spriteBatch, SpriteFont font) {

            if (this.damageIndicatorList.Count != 0) {
                for (int i = 0; i < this.damageIndicatorList.Count; i++) {
                    if (this.damageIndicatorList[i].DrawDamage(_spriteBatch, font, this)) {
                        this.damageIndicatorList.RemoveAt(i);
                    }
                }            
            }
        }
        public void GameOver()
        {
            this.damageIndicatorList.Clear();
            return;
        }
    }
}
