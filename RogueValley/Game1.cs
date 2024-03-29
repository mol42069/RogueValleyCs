﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using RogueValley.Entities;
using RogueValley.Maps;

using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using static RogueValley.enums;
using UI = RogueValley.Maps.UI;

namespace RogueValley
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D bg;

        private Player player;
        private Map bgSprite;
        private MobManager mobManager;
        private UI ui;

        private int[] movement, bgSize, screenSize;

        private System.Diagnostics.Stopwatch watch;

        public bool clicked, past_clicked;

        public int gameState, score;
        SpriteFont font;
        private UpgradeManager upgradeManager, awUpgradeManager;

        private MenuScreen menu;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = false;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            movement = new int[2]; // 0=X-Axis | 1=y-Axis
            this.screenSize = new int[]{ 1920, 1080 };
            menu = new MenuScreen(this.screenSize);

            this.gameState = 0;
            this.bgSize = new int[] {7000, 4000};
            ui = new UI();

            int[] tempPos = new int[2];

            tempPos[0] = 400;
            tempPos[1] = 400;

            player = new Player(tempPos, 8, 100);

            watch = System.Diagnostics.Stopwatch.StartNew();

            mobManager = new MobManager(this.player, this.bgSize);

            this.past_clicked = false;
            this.clicked = false;

            upgradeManager = new WeaponSelecScreen();
            this.awUpgradeManager = new UpgradeManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D[][] AniSprites, deathSprites;
            Texture2D[][][] pAtckSprites, sAtckSprites;

            Texture2D[][] IdleSprites;
            Texture2D[][][][] sprites = new Texture2D[4][][][];

            bg = Content.Load<Texture2D>("Background/grass");
            bgSprite = new Map(bg, this.screenSize, this.bgSize);

            // Load death Sprites:
            {
                deathSprites = new Texture2D[2][];
                deathSprites[0] = new Texture2D[6];

                for (int j = 0; j < 6; j++)
                {
                    string name = "Entity/Enemies/Dead/DeathAnimation/" + j.ToString();

                    if (name != null)
                    {
                        deathSprites[0][j] = Content.Load<Texture2D>(name);
                    }
                }
                Texture2D[][][] dead = new Texture2D[1][][];
                dead[0] = new Texture2D[1][];
                dead[0][0] = new Texture2D[1];
                String death = "Entity/Enemies/Dead/DeathSprite/Skull";
                dead[0][0][0] = Content.Load<Texture2D>(death);

                deathSprites[1] = new Texture2D[7];

                for (int j = 0; j < 7; j++)
                {
                    string name = "Entity/Enemies/Dead/DeleteDeadAnimation/" + j.ToString();

                    if (name != null)
                    {
                        deathSprites[1][j] = Content.Load<Texture2D>(name);
                    }
                }
                sprites[(int)enums.Entity.Zombie] = new Texture2D[5][][];
                sprites[(int)enums.Entity.Zombie][(int)enums.Movement.DEAD] = deathSprites;
                sprites[(int)enums.Entity.Dead] = new Texture2D[3][][];
                sprites[(int)enums.Entity.Dead] = dead;
            }
            // Load the Player Sprites:
            {
                AniSprites = new Texture2D[2][];

                for (int i = 0; i < 2; i++)
                {
                    AniSprites[i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Player/move/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Player/move/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            AniSprites[i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                IdleSprites = new Texture2D[2][];

                for (int i = 0; i < 2; i++)
                {
                    IdleSprites[i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {

                            case 0:
                                name = "Entity/Player/idle/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Player/idle/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            IdleSprites[i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                pAtckSprites = new Texture2D[2][][];
                pAtckSprites[0] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    pAtckSprites[0][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {

                            case 0:
                                name = "Entity/Player/pAttack/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Player/pAttack/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            pAtckSprites[0][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                pAtckSprites[1] = pAtckSprites[0];

                sAtckSprites = new Texture2D[2][][];
                sAtckSprites[0] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    sAtckSprites[0][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Player/sAttack/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Player/sAttack/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            sAtckSprites[0][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                sAtckSprites[1] = sAtckSprites[0];
                player.LoadContent(AniSprites, IdleSprites, pAtckSprites, sAtckSprites);
            }
            // Load the Zombie Sprites:
            {
                Texture2D[][][] zombieSprites = new Texture2D[5][][];

                // idleSprites:
                zombieSprites[(int)enums.Movement.IDLE] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    zombieSprites[(int)enums.Movement.IDLE][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Zombie/idle/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Zombie/idle/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            zombieSprites[(int)enums.Movement.IDLE][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                // movSprites
                zombieSprites[(int)enums.Movement.MOVE] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    zombieSprites[(int)enums.Movement.MOVE][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Zombie/move/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Zombie/move/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            zombieSprites[(int)enums.Movement.MOVE][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                // pAttackSprite
                zombieSprites[(int)enums.Movement.PATTACK] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    zombieSprites[(int)enums.Movement.PATTACK][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Zombie/pAttack/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Zombie/pAttack/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            zombieSprites[(int)enums.Movement.PATTACK][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                //sAttackSprite
                zombieSprites[(int)enums.Movement.SATTACK] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    zombieSprites[(int)enums.Movement.SATTACK][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Zombie/sAttack/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Zombie/sAttack/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            zombieSprites[(int)enums.Movement.SATTACK][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }

                zombieSprites[4] = deathSprites;
                sprites[(int)enums.Entity.Zombie] = zombieSprites;
            }
            // Load Mage Sprites:
            {
                Texture2D[][][] MageSprites = new Texture2D[6][][];
                // idleSprites:
                MageSprites[(int)enums.Movement.IDLE] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    MageSprites[(int)enums.Movement.IDLE][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Mage/idle/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Mage/idle/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            MageSprites[(int)enums.Movement.IDLE][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                // movSprites
                MageSprites[(int)enums.Movement.MOVE] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    MageSprites[(int)enums.Movement.MOVE][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Mage/move/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Mage/move/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            MageSprites[(int)enums.Movement.MOVE][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                // pAttackSprite
                MageSprites[(int)enums.Movement.PATTACK] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    MageSprites[(int)enums.Movement.PATTACK][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Mage/pAttack/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Mage/pAttack/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            MageSprites[(int)enums.Movement.PATTACK][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                //sAttackSprite
                MageSprites[(int)enums.Movement.SATTACK] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    MageSprites[(int)enums.Movement.SATTACK][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Mage/sAttack/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Mage/sAttack/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            MageSprites[(int)enums.Movement.SATTACK][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                // Projectile:
                MageSprites[(int)enums.Movement.PROJECTILE] = new Texture2D[5][];
                for (int i = 0; i < 4; i++)
                {
                    MageSprites[(int)enums.Movement.PROJECTILE][i] = new Texture2D[3];

                    for (int j = 0; j < 3; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case (int)enums.Direction.RIGHT:
                                name = "Entity/Enemies/Mage/Projectile/Right/" + j.ToString();
                                break;
                            case (int)enums.Direction.LEFT:
                                name = "Entity/Enemies/Mage/Projectile/Left/" + j.ToString();
                                break;
                            case (int)enums.Direction.UP:
                                name = "Entity/Enemies/Mage/Projectile/Up/" + j.ToString();
                                break;
                            case (int)enums.Direction.DOWN:
                                name = "Entity/Enemies/Mage/Projectile/Down/" + j.ToString();
                                break;
                            default:
                                break;
                        }
                        if (name != null)
                        {
                            MageSprites[(int)enums.Movement.PROJECTILE][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                MageSprites[(int)enums.Movement.PROJECTILE][(int)enums.Direction.EXP] = new Texture2D[7];
                for (int i = 0; i < 7; i++) {
                    string name = "Entity/Enemies/Mage/Projectile/Final/" + i.ToString();

                    MageSprites[(int)enums.Movement.PROJECTILE][(int)enums.Direction.EXP][i] = Content.Load<Texture2D>(name);
                }


                MageSprites[(int)enums.Movement.DEAD] = deathSprites;

                sprites[(int)enums.Entity.Mage] = MageSprites;
            }
            // Load Ogre Sprites:
            {
                Texture2D[][][] OgreSprites = new Texture2D[6][][];
                // idleSprites:
                OgreSprites[(int)enums.Movement.IDLE] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    OgreSprites[(int)enums.Movement.IDLE][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Ogre/idle/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Ogre/idle/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            OgreSprites[(int)enums.Movement.IDLE][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                // movSprites
                OgreSprites[(int)enums.Movement.MOVE] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    OgreSprites[(int)enums.Movement.MOVE][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Ogre/move/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Ogre/move/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            OgreSprites[(int)enums.Movement.MOVE][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                // pAttackSprite
                OgreSprites[(int)enums.Movement.PATTACK] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    OgreSprites[(int)enums.Movement.PATTACK][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Ogre/pAttack/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Ogre/pAttack/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            OgreSprites[(int)enums.Movement.PATTACK][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                //sAttackSprite
                OgreSprites[(int)enums.Movement.SATTACK] = new Texture2D[2][];
                for (int i = 0; i < 2; i++)
                {
                    OgreSprites[(int)enums.Movement.SATTACK][i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case 0:
                                name = "Entity/Enemies/Ogre/sAttack/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Entity/Enemies/Ogre/sAttack/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            OgreSprites[(int)enums.Movement.SATTACK][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                OgreSprites[(int)enums.Movement.DEAD] = deathSprites;
                sprites[(int)enums.Entity.Ogre] = OgreSprites;
            }
            // Load Other Stuff:
            {
                Texture2D[][][] StaffProjSprites = new Texture2D[2][][];
                StaffProjSprites[(int)enums.Projectile.FireBall] = new Texture2D[5][];
                for (int i = 0; i < 4; i++)
                {
                    StaffProjSprites[(int)enums.Projectile.FireBall][i] = new Texture2D[3];

                    for (int j = 0; j < 3; j++)
                    {
                        string name = null;

                        switch (i)
                        {
                            case (int)enums.Direction.RIGHT:
                                name = "Entity/Player/Projectiles/FireBall/Animation/RIGHT/" + j.ToString();
                                break;
                            case (int)enums.Direction.LEFT:
                                name = "Entity/Player/Projectiles/FireBall/Animation/Left/" + j.ToString();
                                break;
                            case (int)enums.Direction.UP:
                                name = "Entity/Player/Projectiles/FireBall/Animation/Up/" + j.ToString();
                                break;
                            case (int)enums.Direction.DOWN:
                                name = "Entity/Player/Projectiles/FireBall/Animation/Down/" + j.ToString();
                                break;
                            default:
                                break;
                        }
                        if (name != null)
                        {
                            StaffProjSprites[(int)enums.Projectile.FireBall][i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
                StaffProjSprites[(int)enums.Projectile.FireBall][(int)enums.Direction.EXP] = new Texture2D[7];
                for (int i = 0; i < 7; i++)
                {
                    string name = "Entity/Player/Projectiles/FireBall/Finale/" + i.ToString();

                    StaffProjSprites[(int)enums.Projectile.FireBall][(int)enums.Direction.EXP][i] = Content.Load<Texture2D>(name);
                }
                StaffProjSprites[(int)enums.Projectile.EplodingBall] = new Texture2D[2][];
                StaffProjSprites[(int)enums.Projectile.EplodingBall][0] = new Texture2D[1];
                StaffProjSprites[(int)enums.Projectile.EplodingBall][0][0] = Content.Load<Texture2D>("Entity/Player/Projectiles/ExplodingBall/Animation/0");

                StaffProjSprites[(int)enums.Projectile.EplodingBall][1] = new Texture2D[7];
                for (int i = 0; i < 7; i++)
                {
                    string name = "Entity/Player/Projectiles/ExplodingBall/Finale/" + i.ToString();

                    StaffProjSprites[(int)enums.Projectile.EplodingBall][1][i] = Content.Load<Texture2D>(name);
                }
            


            // Load the UI Sprites:
            
                Texture2D[] textures = new Texture2D[2];
                textures[(int)enums.UI.hBar] = Content.Load<Texture2D>("Utility/HealthBar/HBarHealth");
                textures[(int)enums.UI.hBg] = Content.Load<Texture2D>("Utility/HealthBar/HBarBg");

                Texture2D[][][] menuSprites = new Texture2D[(int)enums.MenuSprite.max][][];

                // START-SCREEN-SPRITES:

                menuSprites[(int)enums.MenuSprite.StartS] = new Texture2D[(int)enums.StartScreenS.max][];

                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Bg] = new Texture2D[1];
                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Bg][0] = Content.Load<Texture2D>("Utility/Menus/StartScreen/Background/Bg");

                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Start] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Start][0] = Content.Load<Texture2D>("Utility/Menus/StartScreen/Start/Norm");
                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Start][1] = Content.Load<Texture2D>("Utility/Menus/StartScreen/Start/Hover");

                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Quit] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Quit][0] = Content.Load<Texture2D>("Utility/Menus/StartScreen/Quit/Norm");
                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Quit][1] = Content.Load<Texture2D>("Utility/Menus/StartScreen/Quit/Hover");

                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Record] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Record][0] = Content.Load<Texture2D>("Utility/Menus/StartScreen/Record/Norm");
                menuSprites[(int)enums.MenuSprite.StartS][(int)enums.StartScreenS.Record][1] = Content.Load<Texture2D>("Utility/Menus/StartScreen/Record/Hover");

                // UPGRADE-SCREEN-SPRITES:

                menuSprites[(int)enums.MenuSprite.Upgrade] = new Texture2D[(int)enums.UpgradeScreenS.max][];
                // Background:
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.Bg] = new Texture2D[1];
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.Bg][0] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/Background/Bg");
                // damage:
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.damage] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.damage][0] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/damage/Norm");
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.damage][1] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/damage/Hover");
                // defence:
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.defence] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.defence][0] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/defence/Norm");
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.defence][1] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/defence/Hover");
                // reach:
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.reach] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.reach][0] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/reach/Norm");
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.reach][1] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/reach/Hover");
                // speed:
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.speed] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.speed][0] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/speed/Norm");
                menuSprites[(int)enums.MenuSprite.Upgrade][(int)enums.UpgradeScreenS.speed][1] = Content.Load<Texture2D>("Utility/Menus/UpgradeScreen/speed/Hover");

                // WEAPON-CHOICE-SCREEN-SPRITES:

                menuSprites[(int)enums.MenuSprite.WeaponC] = new Texture2D[(int)enums.WeaponChoiceS.max][];
                // Background:
                menuSprites[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Bg] = new Texture2D[1];
                menuSprites[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Bg][0] = Content.Load<Texture2D>("Utility/Menus/WeaponSelectScreen/Background/Bg");
                // Staff:
                menuSprites[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Staff] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Staff][0] = Content.Load<Texture2D>("Utility/Menus/WeaponSelectScreen/Staff/Norm");
                menuSprites[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Staff][1] = Content.Load<Texture2D>("Utility/Menus/WeaponSelectScreen/Staff/Hover");
                // Sword:
                menuSprites[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Sword] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Sword][0] = Content.Load<Texture2D>("Utility/Menus/WeaponSelectScreen/Sword/Norm");
                menuSprites[(int)enums.MenuSprite.WeaponC][(int)enums.WeaponChoiceS.Sword][1] = Content.Load<Texture2D>("Utility/Menus/WeaponSelectScreen/Sword/Hover");

                // PAUSE-SCREEN-SPRITES:

                menuSprites[(int)enums.MenuSprite.Pause] = new Texture2D[(int)enums.PauseScreenS.max][];
                // Background:
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Bg] = new Texture2D[1];
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Bg][0] = Content.Load<Texture2D>("Utility/Menus/PauseScreen/Background/Bg");
                // damage:
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Continue] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Continue][0] = Content.Load<Texture2D>("Utility/Menus/PauseScreen/Continue/Norm");
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Continue][1] = Content.Load<Texture2D>("Utility/Menus/PauseScreen/Continue/Hover");
                // defence:
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Home] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Home][0] = Content.Load<Texture2D>("Utility/Menus/PauseScreen/Home/Norm");
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Home][1] = Content.Load<Texture2D>("Utility/Menus/PauseScreen/Home/Hover");
                // reach:
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Quit] = new Texture2D[2];
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Quit][0] = Content.Load<Texture2D>("Utility/Menus/PauseScreen/Quit/Norm");
                menuSprites[(int)enums.MenuSprite.Pause][(int)enums.PauseScreenS.Quit][1] = Content.Load<Texture2D>("Utility/Menus/PauseScreen/Quit/Hover");

                Texture2D hoverSprite = Content.Load<Texture2D>("Utility/Menus/HoverEff");

                this.font = Content.Load<SpriteFont>("Font/gameFont");
                this.menu.LoadConent(menuSprites, StaffProjSprites, this.font, hoverSprite);
                ui.LoadContent(textures, font);

                SpriteFont timerFont = Content.Load<SpriteFont>("Font/timerFont");

            this.mobManager.LoadContent(sprites, textures, timerFont);

            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                Exit();

            // here we catch a mouseclick:

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Released && this.past_clicked)
            {
                this.clicked = true;
                this.past_clicked = false;
            }
            else {
                this.clicked = false;
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                this.past_clicked = true;
            }


            // we want to switch between gameStates for start-screen, in-Game or Game-Over-Screen etc.
            InGameKeyHandler();

            switch (this.gameState) {
                case 1:                     // ingame Updates:
                    InGameUpdate();
                    break;

                default:
                    this.menu.player = this.player;
                    this.gameState = this.menu.Update(this.gameState, this.movement);
                    if(this.gameState == 1)
                        this.mobManager.afterWaveUp = 2;
                    this.player = this.menu.player;
                    break;
            }
            base.Update(gameTime);

        }

        protected void InGameUpdate() {
            // here is everything we update if we are in Game.
            if (this.player.hp <= 0 || this.player.weapon is null)
            {
                this.gameState = 0;

                // Delete the Enemies on death:

                this.mobManager.RmList();
                this.player.target.Clear();
                this.mobManager.wave = 0;
                this.mobManager.ammount = 10;
                this.player.weapon = null;
            }            
            this.player.Movement(movement, bgSprite);
            this.player.Update(this.bgSprite);
            this.ui.InGameUpdate(this.player);
            this.mobManager.Update(this.player, this, this.upgradeManager);

            bgSprite.Update(this.player);
        }
        protected void StartScreen() {
            // here is everything we update if we are on the Start Screen.
            var mouseState = Mouse.GetState();
            if (this.clicked) 
            {
                Point mousePos = new Point(mouseState.X, mouseState.Y);
                this.gameState = ui.Click(mousePos);

                // Reset Player:

                this.mobManager.ammount = 10;
                this.mobManager.wave = 0;
                this.mobManager.RmList();

                this.player.weapon = null;

                this.player.hp = 100;
                this.player.playerPosition[0] = 500;
                this.player.playerPosition[1] = 500;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            // we want to switch between gameStates for start-screen, in-Game or Game-Over-Screen etc.
            
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            switch (this.gameState) {
                case 1:
                    InGameDraw();
                    break;

                default:
                    this.menu.Draw(_spriteBatch, this.gameState);
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void InGameDraw() {
            // here is everything we update if we are in Game.
            _spriteBatch.Draw(this.bgSprite.get_map(), new Rectangle(this.bgSprite.map_position[0], this.bgSprite.map_position[1], this.bgSprite.mapSize[0], this.bgSprite.mapSize[1]), Color.White);

            // TODO: Draw Particles
            // TODO: Draw Enemies
            this.mobManager.Draw(_spriteBatch, this.bgSprite);
            if (this.player.projectiles.Count > 0)
            {
                for (int i = 0; i < this.player.projectiles.Count; i++)
                {
                    this.player.projectiles[i].Draw(_spriteBatch, this.bgSprite);
                }
            }            
            _spriteBatch.Draw(this.player.playerSprite, new Rectangle(this.player.drawPosition[0], this.player.drawPosition[1], 100, 100), Color.White);
            
            this.player.DrawDamage(_spriteBatch, this.font);
            
            this.ui.DrawInGameUI(_spriteBatch, this.player, this.mobManager, this.score);
        }
        protected void StartScreenDraw() {
            // here is everything we update if we are on the Start Screen.
            ui.DrawStartScreen(_spriteBatch);        
        }

        protected void InGameKeyHandler()
        {
            // we poll the keyboard.
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                this.gameState = 0;
            }
            if (state.IsKeyDown(Keys.A) && !(state.IsKeyDown(Keys.D)))
            {
                this.movement[0] = -1;
            }
            else if (state.IsKeyDown(Keys.D) && !(state.IsKeyDown(Keys.A)))
            {
                this.movement[0] = 1;
            }
            else
            {
                this.movement[0] = 0;
            }
            if (state.IsKeyDown(Keys.W) && !(state.IsKeyDown(Keys.S)))
            {
                this.movement[1] = -1;
            }
            else if (state.IsKeyDown(Keys.S) && !(state.IsKeyDown(Keys.W)))
            {
                this.movement[1] = 1;
            }
            else
            {
                this.movement[1] = 0;
            }
            var mouseState = Mouse.GetState();
            if (this.clicked)            
                this.player.sAttackTrigger = true;
        }
    }
}