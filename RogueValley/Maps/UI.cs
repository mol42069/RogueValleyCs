using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using RogueValley.Entities;
using System.Numerics;
using Microsoft.Xna.Framework.Input;

namespace RogueValley.Maps
{
    class UI
    {
        public Texture2D[] sprites;

        private Rectangle sButtonRec;
        private SpriteFont font;

        private int[] bgPos, startBPos, startBSize, hBarSizeMax, hBgSize, hBgPos;
        private int hBarSize;
        public UI() {
            // StartScreen Size and Position Variables
            {
                this.bgPos = new int[2];
                this.bgPos[0] = 0;
                this.bgPos[1] = 0;

                this.startBSize = new int[2];
                this.startBSize[0] = 350;
                this.startBSize[1] = 100;

                this.startBPos = new int[2];
                this.startBPos[0] = (1920 / 2) - (this.startBSize[0] / 2);
                this.startBPos[1] = (1080 / 2) - (this.startBSize[1] / 2);
            }
            // Ingame Size and Position Variables for UI
            {
                this.hBarSizeMax = new int[2];
                this.hBarSizeMax[0] = 490;
                this.hBarSizeMax[1] = 40;

                this.hBarSize = this.hBarSizeMax[0];

                this.hBgSize = new int[2];
                this.hBgSize[0] = 500;
                this.hBgSize[1] = 50;

                this.hBgPos = new int[2];
                this.hBgPos[0] = 10;
                this.hBgPos[1] = 980;


            }
            // Rectangles for Buttons:
            {
                this.sButtonRec = new Rectangle(this.startBPos[0], this.startBPos[1], this.startBSize[0], this.startBSize[1]);
            }
        }

        public void LoadContent(Texture2D[] sprites, SpriteFont font) {
            this.sprites = sprites;
            this.font = font;
        }
        public void InGameUpdate(Player player) {
            // we make the red bar in the health bar smaller if we take damage.
            this.hBarSize = (int)((float)this.hBarSizeMax[0] * ((float)player.hp / (float)player.maxhp));
            if (this.hBarSize < 0) {
                this.hBarSize = 0;
            }
        }

        public int Click(Point mousePos) {
            // we check if the start button on the start-screen is clicked.
            if (this.sButtonRec.Contains(mousePos))
            {
                return 2;
            }
            else {
                return 0;
            }
        }

        public void DrawStartScreen(SpriteBatch _spriteBatch) {
            // we draw all ui elements on the start-screen.
            _spriteBatch.Draw(this.sprites[(int)enums.StartScreen.bg], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.StartScreen.sButton], this.sButtonRec, Color.White);        
        }

        public void DrawInGameUI(SpriteBatch _spriteBatch, Player player, MobManager mm, int score) {
            // we draw all ui elements in-game.
            String s = "WAVE: " + mm.wave.ToString() + " | ENEMIES: " + mm.mobList.Count.ToString();

            _spriteBatch.DrawString(this.font, s, new Microsoft.Xna.Framework.Vector2(10, 10), Color.White);
            String scoreS = "SCORE: " + score.ToString();
            _spriteBatch.DrawString(this.font, scoreS, new Microsoft.Xna.Framework.Vector2(10, 40), Color.White);


            _spriteBatch.Draw(this.sprites[(int)enums.UI.hBg], new Rectangle(this.hBgPos[0], this.hBgPos[1], this.hBgSize[0], this.hBgSize[1]), Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.UI.hBar], new Rectangle(this.hBgPos[0] + 5, this.hBgPos[1] + 5, this.hBarSize, this.hBarSizeMax[1]), Color.White);
            _spriteBatch.DrawString(this.font, player.hp.ToString(), new Microsoft.Xna.Framework.Vector2(this.hBgPos[0] + (this.hBgSize[0]/2) - 10, this.hBgPos[1] + (this.hBgSize[1] / 2) - 10), Color.White);
            
        }
    }

    class EnemyUI {

        protected int enemyHBarSize;
        protected Texture2D[] sprites;
        public EnemyUI() {
        }
        public void LoadContent(Texture2D[] sprites)
        {
            this.sprites = sprites;
        }

        public void EnemyHealthbarUpdate(Enemies enemy)
        {
            this.enemyHBarSize = (int)((float)(enemy.spriteSize[0] - 4) * ((float)enemy.hp / (float)enemy.maxhp));
        }


        public SpriteBatch EnemyHealthBarDraw(SpriteBatch _spriteBatch, Map m, Enemies enemy)
        {
            _spriteBatch.Draw(this.sprites[(int)enums.UI.hBg], new Rectangle(enemy.drawPosition[0], enemy.drawPosition[1] - 13, enemy.spriteSize[0], 10), Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.UI.hBar], new Rectangle(enemy.drawPosition[0] + 2, enemy.drawPosition[1] - 12, this.enemyHBarSize, 8), Color.White);

            return _spriteBatch;
        }


    }
    class PlayerUI {

        private int damageDrawTimer, damageDrawTimerMax, damageDrawChange_y, damageDrawChange_x, damagePos_x, damagePos_y, damageDrawChangeMax_y, damageTaken;

        public PlayerUI(int damageTaken) {
            this.damageTaken = damageTaken;

            this.damageDrawChangeMax_y = 20;
            this.damageDrawChange_y = 0;
            this.damageDrawChange_x = 0;
            this.damageDrawTimer = 0;
            this.damageDrawTimerMax = 2;

            this.damagePos_x = 0;
            this.damagePos_y = 0;
        }

        public bool DrawDamage(SpriteBatch _spriteBatch, SpriteFont font, Player player)
        {
            // TODO: THE WRITING HAS TO BE WIGGLING UPWARDS MAYBE WE CHANGE THE X DIRECTION USING SINUS OR COSINUS AND GO UP CONTINUOSLY...

                this.damageDrawTimer++;
                if (this.damageDrawTimer == this.damageDrawTimerMax)
                {
                    this.damageDrawTimer = 0;
                    this.damageDrawChange_y++;
                }
                int a = this.damageDrawChange_y * 9;
                double b = (a * (Math.PI)) / 180;

                this.damageDrawChange_x = (int)(Math.Sin(b) * 10) + 40;

                this.damagePos_x = player.drawPosition[0] + this.damageDrawChange_x;
                this.damagePos_y = player.drawPosition[1] - this.damageDrawChange_y - 10;

                String s = this.damageTaken.ToString();
                _spriteBatch.DrawString(font, s, new Microsoft.Xna.Framework.Vector2(this.damagePos_x, this.damagePos_y), Color.Red);

                if (this.damageDrawChange_y == this.damageDrawChangeMax_y)
                {
                    return true;
                }
            return false;
        }
    }

    class UpgradeUI {

        private int[] spriteSize;
        private int[][] pos;

        private Texture2D[] sprites;
        private Rectangle[] ButtonRecs;

        public UpgradeUI() {

            this.spriteSize = new int[2] { 300, 300 };

            this.pos = new int[4][];
            this.pos[0] = new int[2] { 100, 500 };
            this.pos[1] = new int[2] { this.pos[0][0] + this.spriteSize[0] + 150, this.pos[0][1] };
            this.pos[2] = new int[2] { this.pos[1][0] + this.spriteSize[0] + 150, this.pos[1][1] };
            this.pos[3] = new int[2] { this.pos[2][0] + this.spriteSize[0] + 150, this.pos[2][1] };

            this.ButtonRecs = new Rectangle[4];
            this.ButtonRecs[(int)enums.UpgardeScreen.damageUP] = new Rectangle(this.pos[(int)enums.UpgardeScreen.damageUP][0], this.pos[(int)enums.UpgardeScreen.damageUP][1], this.spriteSize[0], this.spriteSize[1]);
            this.ButtonRecs[(int)enums.UpgardeScreen.defenceUP] = new Rectangle(this.pos[(int)enums.UpgardeScreen.defenceUP][0], this.pos[(int)enums.UpgardeScreen.defenceUP][1], this.spriteSize[0], this.spriteSize[1]);
            this.ButtonRecs[(int)enums.UpgardeScreen.reachUP] = new Rectangle(this.pos[(int)enums.UpgardeScreen.reachUP][0], this.pos[(int)enums.UpgardeScreen.reachUP][1], this.spriteSize[0], this.spriteSize[1]);
            this.ButtonRecs[(int)enums.UpgardeScreen.speedUP] = new Rectangle(this.pos[(int)enums.UpgardeScreen.speedUP][0], this.pos[(int)enums.UpgardeScreen.speedUP][1], this.spriteSize[0], this.spriteSize[1]);

        }

        public void LoadContent(Texture2D[] sprites) {
            this.sprites = sprites;        
        }

        public bool Update(Player player, Game1 g1) {
            var mouseState = Mouse.GetState();

            if (g1.clicked) {
                Point mousePos = new Point(mouseState.X, mouseState.Y);

                for (int i = 0; i < this.ButtonRecs.Length; i++) {
                    if (this.ButtonRecs[i].Contains(mousePos)) {
                        switch (i){
                        
                            case (int)enums.UpgardeScreen.damageUP:
                                player.damage += 20;
                                break;

                            case (int)enums.UpgardeScreen.defenceUP:
                                player.defence += 2;
                                break;

                            case (int)enums.UpgardeScreen.reachUP:
                                player.reach += 20;
                                break;

                            case (int)enums.UpgardeScreen.speedUP:
                                player.speed += 5;
                                break;

                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.sprites[(int)enums.UpgardeScreen.background], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.UpgardeScreen.damageUP],this.ButtonRecs[(int)enums.UpgardeScreen.damageUP],Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.UpgardeScreen.defenceUP],this.ButtonRecs[(int)enums.UpgardeScreen.defenceUP],Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.UpgardeScreen.reachUP],this.ButtonRecs[(int)enums.UpgardeScreen.reachUP],Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.UpgardeScreen.speedUP],this.ButtonRecs[(int)enums.UpgardeScreen.speedUP],Color.White);
        }    
    }
}
