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

namespace RogueValley.Maps
{
    class UI
    {
        public Texture2D[] sprites;

        private Rectangle sButtonRec;
        private SpriteFont font;

        private int[] bgPos, startBPos, startBSize, hBarSizeMax, hBgSize, hBgPos;
        private int hBarSize, enemyHBarSize;
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

        public void DrawInGameUI(SpriteBatch _spriteBatch, Player player, MobManager mm) {
            // we draw all ui elements in-game.
            String s = "WAVE: " + mm.wave.ToString() + " | ENEMIES: " + mm.mobList.Count.ToString();

            _spriteBatch.DrawString(this.font, s, new Microsoft.Xna.Framework.Vector2(10, 10), Color.White);

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
}
