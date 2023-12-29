using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueValley
{
    public class enums
    {

        public enum SpawnRate 
        {
            Zombie = 90,
            Mage = 10,
        }
        public enum Entity
        {
            Zombie = 0,
            Mage = 1,

            Dead = 2, // lets make this the last.
        }
        public enum Direction
        {
            RIGHT = 0,
            LEFT = 1,
            UP = 2,
            DOWN = 3,
            EXP = 4
        }
        public enum Movement
        {
            IDLE = 0,
            MOVE = 1,
            PATTACK = 2,
            SATTACK = 3,
            DEAD = 4,
            PROJECTILE = 5
        }
        public enum StartScreen 
        {
            bg = 0,
            sButton = 1,
        }
        public enum UI 
        {
            hBar = 2,
            hBg = 3,
        }
        public enum Weapon
        {
            StandartSword = 0,
            Staff = 1,
        }

        public enum UpgradeManager {
            WeaponSelect = 0,

        }

    }
}