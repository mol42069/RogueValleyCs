﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using RogueValley.Entities;
using RogueValley.Maps;

using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace RogueValley
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D bg;

        private Player player;
        private Map bgSprite;

        private int[] movement;

        private int fps;
        private System.Diagnostics.Stopwatch watch;

        //private int gameState;

        private MobManager mobManager;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            movement = new int[2]; // 0=X-Axis | 1=y-Axis

            //this.gameState = 1;

            int[] tempPos = new int[2];

            tempPos[0] = 400;
            tempPos[1] = 400;

            player = new Player(tempPos, 8, 100);

            this.fps = 60;
            watch = System.Diagnostics.Stopwatch.StartNew();

            mobManager = new MobManager(this.player);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D[][] AniSprites;
            Texture2D[][] IdleSprites;
            Texture2D[][][][] sprites = new Texture2D[1][][][];



            bg = Content.Load<Texture2D>("Background/grass");
            int[] screenSize = {1920, 1080};
            bgSprite = new Map(player.playerPosition, screenSize, bg, screenSize);

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
                player.LoadContent(AniSprites, IdleSprites);
            }

            // Load the Zombie Sprites:
            {
                Texture2D[][][] zombieSprites = new Texture2D[4][][];

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

                sprites[(int)enums.Entitiy.Zombie] = zombieSprites;
            }

            this.mobManager.LoadContent(sprites);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (player.hp <= 0) {
                Exit();
            }
            KeyHandler();

            // watch.Stop();

            long lost = (1 / this.fps) * 1000 - watch.ElapsedMilliseconds;

            if (lost >= 0 ) {
            
                Thread.Sleep((int)lost);
                watch = System.Diagnostics.Stopwatch.StartNew();
            }

            // watch = System.Diagnostics.Stopwatch.StartNew();

            player.Movement(movement, bgSprite);
            player.Update();

            this.mobManager.Update(this.player);

            bgSprite.Update(player);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(bgSprite.get_map(), new Rectangle(bgSprite.map_position[0], bgSprite.map_position[1], bgSprite.mapSize[0], bgSprite.mapSize[1]), Color.White);

            // TODO: Draw Particles
            // TODO: Draw Enemies
            this.mobManager.Draw(_spriteBatch, this.bgSprite);

            _spriteBatch.Draw(player.playerSprite, new Rectangle(player.drawPosition[0], player.drawPosition[1], 100, 100), Color.White);

            SpriteFont font = Content.Load<SpriteFont>("Font/gameFont");


            _spriteBatch.DrawString(font, player.hp.ToString(), new Vector2(10, 10), Color.Red);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        protected void KeyHandler() {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape)) {
                Exit();
            }
            if (state.IsKeyDown(Keys.A)&& !(state.IsKeyDown(Keys.D))){
                movement[0] = -1;
            }
            else if (state.IsKeyDown(Keys.D) && !(state.IsKeyDown(Keys.A))){
                movement[0] = 1;
            }
            else {
                movement[0] = 0;
            }
            if (state.IsKeyDown(Keys.W) && !(state.IsKeyDown(Keys.S))){
                movement[1] = -1;
            }
            else if (state.IsKeyDown(Keys.S) && !(state.IsKeyDown(Keys.W))){
                movement[1] = 1;
            }
            else {
                movement[1] = 0;
            }
            if (state.IsKeyDown(Keys.L)) {
                int[] a = new int[2];
                a[(int)enums.Entitiy.Zombie] = 10;
                mobManager.Spawn(a, player);
            }
        }
    }
}