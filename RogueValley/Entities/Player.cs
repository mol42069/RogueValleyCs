using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueValley.Entities;
using RogueValley.Maps;
namespace RogueValley.Entities
{
    class Player
    {
        // PLAYER VARIABLES:

        // Player Sprites:

        public Texture2D playerSprite;
        private Texture2D[][] playerAniSprites;
        private Texture2D[][] playerIdleSprites;

        // Player Positional Variables:

        private int playerDirection, speed;
        public int[] playerPosition, drawPosition;

        // Player Animation Variables:

        private int animationCount, animationTimer, animationMaxTime;


        public Player(int[] pos, int animax=8, int speed=10) {
            // PLAYER VARIABLES:

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
                this.animationMaxTime = 8;
            }
            // other Player Variables:
            {
                this.speed = speed;
            }

        }
        public void LoadContent(Texture2D[][] pas, Texture2D[][] pis, Texture2D ps) {

            // Player Sprites:

            this.playerAniSprites = pas;
            this.playerIdleSprites = pis;
            this.playerSprite = ps;


        }
        public void Movement(int[] direction, Map map) {
            if (0 <= (this.playerPosition[0] + (this.speed / 10) * direction[0]) && (this.playerPosition[0] + (this.speed / 10) * direction[0]) <= map.mapSize[0] - 35)
            {
                this.playerPosition[0] += (this.speed / 10) * direction[0];
            }
            if (0 <= (this.playerPosition[1] + (this.speed / 10) * direction[1]) && (this.playerPosition[1] + (this.speed / 10) * direction[1]) <= map.mapSize[1] - 90)
            {
                this.playerPosition[1] += (this.speed / 10) * direction[1];
            }

        }

        public void Update() {

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

        public Enemies PrimaryAttack(Enemies enemie) {


            return enemie;
        }

        public Enemies SecondAttack(Enemies enemie) {



            return enemie;        
        }
        public void TakeDamage()
        {



        }


    }
}
