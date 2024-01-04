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

        protected int hp, defence, damage, speed, aniCount, aniTimer, aniTimerMax, entityDir, lastDir, reach, piercing, targetId;
        protected int pAttackTimer, pAttackTimerMax, AttackCooldown, sAttackTimerMax, sAttackTimer;

        protected float sAttackMult;

        protected int[] spriteSize, lastMove;
        protected int[] mov;
        public int[] position, drawPosition;
        protected List<Projectiles> projectilesList;

        protected Texture2D[][] movSprites, idleSprites, pAttackSprite, sAttackSprite, deathAnimation;
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

            this.targetId = -1;

            rnd = new Random();
        }

        public void LoadContent(Texture2D[][][] sprites)
        {
            this.movSprites = sprites[(int)enums.Movement.MOVE];
            this.idleSprites = sprites[(int)enums.Movement.IDLE];
            this.pAttackSprite = sprites[(int)enums.Movement.PATTACK];
            this.sAttackSprite = sprites[(int)enums.Movement.SATTACK];
            this.deathAnimation = sprites[(int)enums.Movement.DEAD];


            this.sprite = idleSprites[0][0];
        }

        private int[] CalcdrawPos(Map m)
        {
            // we calculate the enemy based on the player / map position

            int[] drawPos = new int[2];

            drawPos[0] = this.position[0] + m.map_position[0];
            drawPos[1] = this.position[1] + m.map_position[1];

            return drawPos;
        }

        public SpriteBatch Draw(SpriteBatch _spriteBatch, Map m)
        {
            // we draw the enemy.
            this.drawPosition = CalcdrawPos(m);
            _spriteBatch.Draw(this.sprite, new Rectangle(this.drawPosition[0], this.drawPosition[1], this.spriteSize[0], this.spriteSize[1]), Color.White);

            if (this.projectilesList != null)
            {
                for (int i = 0; i < this.projectilesList.Count; i++)
                {
                    this.projectilesList[i].Draw(_spriteBatch, m);
                }
            }

            return _spriteBatch;
        }

        protected void DeathAnimation() {
            this.aniTimer++;

            if (this.aniTimer == this.aniTimerMax)
            {
                this.aniTimer = 0;
                this.aniCount++;
            }
            if (this.aniCount < 6)
            {
                this.sprite = this.deathAnimation[0][this.aniCount];
            }
        }

        public virtual bool DeleteDead()
        {
            return false;
        }

        protected void Animation()
        {
            // we do the same as with the player see there
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

        public virtual int Update(Player player)
        {
            return 0;
        }

        protected virtual Player Ai(Player player)
        {
            return player;
        }

        public void TakeDamage(int damage, float piercing)
        {
            // same as in the player class check there for explanation.
            if (piercing < this.defence)
            {
                this.hp -= (int)((float)damage * ((float)piercing / (float)this.defence));
            }
            else
            {
                this.hp -= damage;
            }
            this.aniCount = 0;
            if (this.hp < 0) {
                this.hp = 0;
            }
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
            base.defence = 1;
            base.reach = 100;
            base.speed = 7;
            base.piercing = 0;

            base.Init();

            this.random = rnd.Next(0, 6);
        }

        public override int Update(Player player)
        {
            // we just call the ai func. and return the entity hp.
            if (base.hp == 0)
            {
                if (base.aniCount != 6)
                {
                    base.DeathAnimation();
                    return 0;
                }
                else {
                    return -1;
                }
            }
            else
            {
                this.Ai(player);
                return base.hp;
            }
        }

        protected override Player Ai(Player player)
        {
            // we move the entity towards the player until it is in reach. Then we tell the Zombie to attack as well as when this zombie is in reach of
            // the player we add this to the target list.
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

            if (n <= player.weapon.reach && n >= -player.weapon.reach) 
            {
                this.targetId = player.target.Count;
                player.target.Add(this);
            }

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
                if (base.pAttackTimer != 0 || base.sAttackTimer != 0)
                {
                    this.random = base.rnd.Next(0, 6);
                }
                base.pAttackTimer = 0;
                base.sAttackTimer = 0;

                mov[0] += rnd.Next(-100, 101);
                mov[1] += rnd.Next(-100, 101);

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
            // we attack the player every so often. Same as in player basicly but this only attacks the player
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
        // we attack the player every so often. Same as in player basicly but this only attacks the player
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

    class Mage : Enemies 
    {
        int random;
        Texture2D[][] projectiles;
        

        public Mage(int[] pos)
        {
            base.pAttackTimer = 0;
            base.pAttackTimerMax = 5;

            base.sAttackTimer = 0;
            base.sAttackTimerMax = 10;

            base.lastMove = new int[2] { 0, 0 };

            base.hp = 100;
            base.damage = 15;
            base.sAttackMult = 1.5f;

            base.position = pos;
            base.defence = 6;
            base.reach = 500;
            base.speed = 5;
            base.piercing = 8;
            base.AttackCooldown = 50;

            base.Init();

            base.projectilesList = new List<Projectiles>();

            this.random = rnd.Next(0, 6);
        }

        public void LoadProjectile(Texture2D[][] projectiles) {
            this.projectiles = projectiles;
        }

        public override int Update(Player player)
        {
            // we just call the ai func. and return the entity hp.

            for (int i = 0; i < base.projectilesList.Count; i++) {
                if (base.projectilesList[i].Update(player)) {
                    base.projectilesList.RemoveAt(i);
                }
            }
            if (base.hp == 0)
            {
                if (base.aniCount != 6)
                {
                    base.DeathAnimation();
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                this.Ai(player);
                return base.hp;
            }
        }

        protected override Player Ai(Player player)
        {
            // we move the entity towards the player until it is in reach. Then we tell the Zombie to attack as well as when this zombie is in reach of
            // the player we add this to the target list.
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

            if (n <= player.weapon.reach && n >= -player.weapon.reach)
            {
                this.targetId = player.target.Count;
                player.target.Add(this);
            }

            if (n <= base.reach && n >= -base.reach)
            {
                base.lastMove[0] = 0;
                base.lastMove[1] = 0;

                this.PrimaryAttack(player);
            }
            else
            {
                if (base.pAttackTimer != 0 || base.sAttackTimer != 0)
                {
                    this.random = base.rnd.Next(0, 6);
                }
                base.pAttackTimer = 0;
                base.sAttackTimer = 0;

                mov[0] += rnd.Next(-100, 101);
                mov[1] += rnd.Next(-100, 101);

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
            // we attack the player every so often. Same as in player basicly but this only attacks the player
            if (base.AttackCooldown == 0)
            {
                base.pAttackTimer++;
                if (base.pAttackTimer >= base.pAttackTimerMax * (base.pAttackSprite[base.lastDir].Length - 1))
                {
                    base.pAttackTimer = 0;
                    int[] tempPos = (int[])player.playerPosition.Clone(); 

                    projectilesList.Add(new FlameBall(this.projectiles, (int[])base.position.Clone(), tempPos, base.damage, base.piercing));
                    this.random = rnd.Next(0, 6);
                    base.AttackCooldown = 50;
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
        // we attack the player every so often. Same as in player basicly but this only attacks the player
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

    class Dead : Enemies {


        public Dead(int[] pos, Texture2D sprite, Texture2D[][]deathAnimation)
        {
            base.Init();
            base.position = pos;
            base.sprite = sprite;
            base.deathAnimation = deathAnimation;
        }

        public override int Update(Player player) {
            return 0;
        }

        public override bool DeleteDead() {
            this.aniTimer++;

            if (this.aniTimer == this.aniTimerMax)
            {
                this.aniTimer = 0;
                this.aniCount++;
            }
            if (this.aniCount < 7)
            {
                this.sprite = this.deathAnimation[1][this.aniCount];
            }
            else {
                return true;
            }


            return false;
        }

    }
}
