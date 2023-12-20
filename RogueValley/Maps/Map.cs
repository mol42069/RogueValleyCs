using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RogueValley.Entities;

namespace RogueValley.Maps
{
    class Map
    {
        public int[] map_position, mapSize, sSize;
        private Texture2D mapSprite;

        public Map(int[] playerPosition, int[] screenSize, Texture2D mapSprite, int[] sSize) {
            this.mapSize = new int[2];
            this.mapSize[0] = 14000;
            this.mapSize[1] = 7000;

            this.sSize = sSize;

            this.map_position = new int[2];
            this.map_position[0] = 0;
            this.map_position[1] = 0;

            this.mapSprite = mapSprite;

        }
        public void Update(Player player) {

            // Update the map X-Axis Position:
            
            if (player.playerPosition[0] <= this.sSize[0] / 2)
            {
                this.map_position[0] = 0;
                player.drawPosition[0] = player.playerPosition[0];

            } else if (this.mapSize[0] - player.playerPosition[0] <= this.sSize[0] / 2) 
            {
                this.map_position[0] = this.sSize[0] - this.mapSize[0];
                player.drawPosition[0] = player.playerPosition[0] - (this.mapSize[0] - this.sSize[0]);
            }
            else 
            {
                this.map_position[0] = (this.sSize[0] / 2) - player.playerPosition[0];
                player.drawPosition[0] = this.sSize[0] / 2;
            }

            // Update the map Y-Axis Position:

            if (player.playerPosition[1] <= this.sSize[1] / 2)
            {
                this.map_position[1] = 0;
                player.drawPosition[1] = player.playerPosition[1];

            }
            else if (this.mapSize[1] - player.playerPosition[1] <= this.sSize[1] / 2)
            {
                this.map_position[1] = this.sSize[1] - this.mapSize[1];
                player.drawPosition[1] = player.playerPosition[1] - (this.mapSize[1] - this.sSize[1]);
            }
            else
            {
                this.map_position[1] = (this.sSize[1] / 2) - player.playerPosition[1];
                player.drawPosition[1] = this.sSize[1] / 2;
            }            
        }
        public Texture2D get_map() {
            // return the map sprite
            return mapSprite;
        }
    }
}
