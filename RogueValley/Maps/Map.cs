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
        public int[] map_position, mapSize;
        private Texture2D mapSprite;
        public Map(int[] playerPosition, int[] screenSize, Texture2D mapSprite) {
            this.mapSize = new int[2];
            this.mapSize[0] = 2230;
            this.mapSize[1] = 1440;
            this.map_position = new int[2];
            this.map_position[0] = -100;
            this.map_position[1] = -100;
            this.mapSprite = mapSprite;

        }
        public void moveMap(Player player) {

            

        
        }
        public Texture2D get_map() {
        
            return mapSprite;
        }
    }
}
