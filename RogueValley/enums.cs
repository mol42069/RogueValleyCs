﻿using System;
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
            Ogre = 2,
            Dead = 3, // lets make this the last.
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
            hBar = 0,
            hBg = 1,
        }
        public enum Weapon
        {
            StandartSword = 0,
            Staff = 1,
        }

        public enum UpgradeManager {
            WeaponSelect = 0,
            UpgradeScreen = 1,
        }
        public enum UpgardeScreen {
            damageUP = 0,
            defenceUP = 1,
            reachUP = 2,
            speedUP = 3,
            background = 4,
        }

        public enum Projectile {
            FireBall = 0,
            EplodingBall = 1,
        }

        public enum Menu {
            False = 0,
        }

        public enum MenuSprite {
            StartS,
            Upgrade,
            WeaponC,
            Pause,
            max,
        }
        public enum StartScreenS
        {
            Bg,
            Start,
            Quit,
            Record,
            max,
        }
        public enum UpgradeScreenS
        {
            Bg,
            damage,
            defence,
            reach,
            speed,
            max,
        }
        public enum WeaponChoiceS
        {
            Bg,
            Staff,
            Sword,
            max,
        }
        public enum PauseScreenS{
            Bg,
            Continue,
            Home,
            Quit,
            max
        }

    }
}