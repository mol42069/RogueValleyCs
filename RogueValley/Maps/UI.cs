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

namespace RogueValley.Maps
{
    class UI
    {
        private Texture2D[] sprites;

        private Rectangle sButtonRec;

        private int[] bgPos, startBPos, startBSize;
        public UI() { 
            bgPos = new int[2];
            bgPos[0] = 0;
            bgPos[1] = 0;

            startBSize = new int[2];
            startBSize[0] = 350;
            startBSize[1] = 100;

            startBPos = new int[2];
            startBPos[0] = (1920 / 2) - (startBSize[0] / 2);
            startBPos[1] = (1080 / 2) - (startBSize[1] / 2);

            this.sButtonRec = new Rectangle(this.startBPos[0], this.startBPos[1], this.startBSize[0], this.startBSize[1]);

        }

        public void LoadContent(Texture2D[] sprites) {
            this.sprites = sprites;        
        }
        public void Update() {



            return;
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

        public void Draw(SpriteBatch _spriteBatch) {
            _spriteBatch.Draw(this.sprites[(int)enums.StartScreen.bg], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(this.sprites[(int)enums.StartScreen.sButton], this.sButtonRec, Color.White);        
        }

    }
}
