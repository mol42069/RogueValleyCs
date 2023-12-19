using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueValley
{
    public class enums
    {
        public enum Entitiy {
            Player = 10,
            Zombie = 0,
        }
        public enum Direction {
            RIGHT = 0,
            LEFT = 1,
            UP = 2,
            DOWN = 3,
        }
        public enum Movement {
            IDLE = 0,
            MOVE = 1,
            PATTACK = 2,
            SATTACK = 3,
        }
    }
}
