﻿using Microsoft.Xna.Framework.Graphics;
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
        private Texture2D[] sprites;

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
            this.hBarSize = (int)((float)this.hBarSizeMax[0] * ((float)player.hp / (float)player.maxhp));
            if (this.hBarSize < 0) {
                this.hBarSize = 0;
            }
        }

        public int Click(Point mousePos) {

            if (this.sButtonRec.Contains(mousePos))
            {
                return 1;
            }
            else {
                return 0;
            }
        }

        public void DrawStartScreen(SpriteBatch _spriteBatch) {
            _spriteBatch.Draw(this.sprites[(int)enums.StartScreen.bg], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.StartScreen.sButton], this.sButtonRec, Color.White);        
        }

        public void DrawInGameUI(SpriteBatch _spriteBatch, Player player) {
            _spriteBatch.Draw(this.sprites[(int)enums.UI.hBg], new Rectangle(this.hBgPos[0], this.hBgPos[1], this.hBgSize[0], this.hBgSize[1]), Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.UI.hBar], new Rectangle(this.hBgPos[0] + 5, this.hBgPos[1] + 5, this.hBarSize, this.hBarSizeMax[1]), Color.White);
            _spriteBatch.DrawString(font, player.hp.ToString(), new Microsoft.Xna.Framework.Vector2(this.hBgPos[0] + (this.hBgSize[0]/2) - 10, this.hBgPos[1] + (this.hBgSize[1] / 2) - 10), Color.White);
        }

    }
}