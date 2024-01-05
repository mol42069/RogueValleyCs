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

        public int damage, hp, defence, maxhp, maxtarget, reach, regeneration;
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
        public int damageDrawChange_x, damageDrawChangeMax_x, damageDrawChange_y;
        // OTHER:

        Random rnd;
        int random, damageTaken;
        bool damageAni;

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

                this.pAttackTimer = 0;
                this.pAttackTimerMax = 3;

                this.sAttackTimer = 0;
                this.sAttackTimerMax = 5;

                this.damageDrawChange_x = 20;
                this.damageDrawChangeMax_x = -10;
                this.damageDrawChange_y = 20;


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

                this.reach = 200;

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
        {
            this.lastMovement = direction;
            if (this.weapon is null) return;

            if (!(direction[0] == 0 && direction[1] == 0))
            {
                weapon.ResetAnimation();
                
                this.pAttackTimer = 0;
                this.sAttackTimer = 0;
                //this.AttackCooldown = 0;
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

        public void Update(Map map)
        {
            if (projectiles.Count > 0) {
                for (int i = 0; i < projectiles.Count; i++) {
                    if (projectiles[i].UpdatePlayer(this, this.mobList)) {
                        projectiles.RemoveAt(i);
                    }
                }
            }
            if (this.weapon is null) return;
            if (this.weapon.AttackCooldown > 0) {
                this.weapon.AttackCooldown--;
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
                int rCount = 0;
                int lCount = 0;
                List<Enemies> tenemies = new List<Enemies>();

                // We count where the enemies are:

                for (int i = 0; i < this.target.Count; i++)
                {
                    if (this.target[i].targetPosition[0] < this.playerPosition[0])
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
                        if (this.target[i].targetPosition[0] < this.playerPosition[0])
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
                        if (this.target[i].targetPosition[0] > this.playerPosition[0])
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
                            if (this.target[i].targetPosition[0] > this.playerPosition[0])
                            {
                                tenemies.Add(this.target[i]);
                            }
                        }
                        else
                        {
                            if (this.target[i].targetPosition[0] < this.playerPosition[0])
                            {
                                tenemies.Add(this.target[i]);
                            }
                        }
                    }
                }
                // we attack the previous generated List with either primary or secondary wich we choose randomly:
                if (!this.sAttackTrigger)
                {
                    if (tenemies.Count != 0)
                        this.PrimaryAttack(tenemies);
                }
                else
                {
                    if (this.weapon is not Staff)
                    {
                        if (tenemies.Count != 0)
                            this.SecondAttack(tenemies, map);
                    }
                    else {
                        this.SecondAttack(this.mobList, map);
                    }
                }
                // after all we clear our list so we dont attack non-existend enemies:
                tenemies.Clear();
            }
            else 
            {
                this.Animation();
                this.targetPos = null;
            }
            if (this.target.Count == 0 && (!this.sAttackTrigger && this.weapon is not Staff) || (this.weapon is Staff && this.targetPos is null)) {
                this.Animation();
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

        protected int[] getTargetPos() {
            int[] targetPos = new int[2];

            int distance = 1000;
            int finalIdx = 0;
            for (int i = 0; i < this.target.Count; i++) {

                int x = this.target[i].position[0] - this.playerPosition[0];
                if (x < 0) x = -x;
                
                int y = this.target[i].position[1] - this.playerPosition[1];
                if (y < 0) y = -y;

                if (x + y < distance) { 
                    distance = x + y;
                    finalIdx = i;
                }

            }
            targetPos = this.target[finalIdx].targetPosition;
            return targetPos;
        }

        public void PrimaryAttack(List<Enemies> e)
        {

            if (weapon is Staff) {
                int[] targetPos = new int[2];
                targetPos = this.getTargetPos();
                weapon.PrimaryAttack(e, this, targetPos);
            }
            else if (weapon is StandartSword){
                weapon.PrimaryAttack(e, this);
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
                    this.damageTaken = (int)((float)damage * ((float)((float)piercing / (float)this.defence) + 0.2f));
                }
                else
                {
                    this.hp -= damage;
                    this.damageTaken = damage;
                }
                if (this.hp <= 0){
                    this.hp = 0;
                    this.GameOver();
                }
            }
        }

        public SpriteBatch DrawDamage(SpriteBatch _spriteBatch, SpriteFont font) {

            // TODO: THE WRITING HAS TO BE WIGGLING UPWARDS MAYBE WE CHANGE THE X DIRECTION USING SINUS OR COSINUS AND GO UP CONTINUOSLY...

            _spriteBatch.DrawString(font, this.damageTaken.ToString(), new Microsoft.Xna.Framework.Vector2());


            return _spriteBatch;
        }

        public void GameOver()
        {
            return;
        }
    }
}
