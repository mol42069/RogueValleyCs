using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueValley.Entities.Player
{
    class Player
    {
        // PLAYER VARIABLES:

        // Player Sprites:

        public Texture2D playerSprite;
        private Texture2D[][] playerAniSprites;
        private Texture2D[][] playerIdleSprites;

        // Player Positional Variables:

        private int playerDirection;
        public int[] playerPosition;

        // Player Animation Variables:

        private int animationCount, animationTimer, animationMaxTime;


        public void Initialize(int[] pos, int animax=8) {
            // PLAYER VARIABLES:

            // Player Positional Variables:
            {
                playerPosition = new int[2];
                playerPosition[0] = pos[0];
                playerPosition[1] = pos[1];
            }
            // Player Animation Variables:
            {
                animationCount = 0;
                animationTimer = 0;
                animationMaxTime = 8;
            }

        }
        public void LoadContent(Texture2D[][] pas, Texture2D[][] pis, Texture2D ps) {

            // Player Sprites:

            playerAniSprites = pas;
            playerIdleSprites = pis;
            playerSprite = ps;


        }
        public void Movement(int direction) {

            // 0:Standing | 1:UP | 2:RIGHT | 3:DOWN | 4:LEFT | -1:STANDING_UP | -2:STANDING_RIGHT | -3:STANDING_DOWN | -4:STANDING_LEFT


            switch (direction) {

                case 0:
                    if (playerDirection > 0)
                    {
                        playerDirection = playerDirection * -1;
                    }
                    break;

                case 1:

                    playerDirection = 1;
                    playerPosition[1]--;
                    break;


                case 2:

                    playerDirection = 2;
                    playerPosition[0]++;
                    break;

                case 3:

                    playerDirection = 3;
                    playerPosition[1]++;
                    break;

                case 4:

                    playerDirection = 4;
                    playerPosition[0]--;
                    break;

                default:
                    break;

            }


        }

        public void update() {

            Animation();            
        
        }

        protected void Animation() {

            if (animationTimer == animationMaxTime)
            {
                animationTimer = 0;
                animationCount++;

                if (animationCount > 5)
                {
                    animationCount = 0;
                }

                playerSprite = playerAniSprites[playerDirection][animationCount];

            }
            animationTimer++;

        }


    }
}
