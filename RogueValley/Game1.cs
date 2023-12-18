using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using RogueValley.Entities;
using RogueValley.Maps;

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
        private Zombie z;



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

            int[] tempPos = new int[2];

            tempPos[0] = 100;
            tempPos[1] = 100;

            z = new Zombie(tempPos);

            tempPos[0] = 200;
            tempPos[1] = 200;

            player = new Player(tempPos, 8, 100);



            base.Initialize();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D playerSprite;
            Texture2D[][] playerAniSprites;
            Texture2D[][] playerIdleSprites;

            bg = Content.Load<Texture2D>("Background/grass");
            int[] screenSize = {1920, 1080};
            bgSprite = new Map(player.playerPosition, screenSize, bg, screenSize);

            playerSprite = Content.Load<Texture2D>("Entity/Player");

            // Load all Player-Walking-Animation Variables into an Array:
            {
                playerAniSprites = new Texture2D[2][];

                for (int i = 0; i < 2; i++)
                {

                    playerAniSprites[i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {

                        string name = null;

                        switch (i)
                        {

                            case 0:
                                name = "Animations/Player/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Animations/Player/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }

                        if (name != null)
                        {
                            Console.WriteLine(name);
                            playerAniSprites[i][j] = Content.Load<Texture2D>(name);
                        }

                    }

                }
            }
            // Load all Player-Idle-Animation Variables into an Array:
            {
                playerIdleSprites = new Texture2D[2][];

                for (int i = 0; i < 2; i++)
                {

                    playerIdleSprites[i] = new Texture2D[6];

                    for (int j = 0; j < 4; j++)
                    {

                        string name = null;

                        switch (i)
                        {

                            case 0:
                                name = "Animations/IdlePlayer/Right/" + j.ToString();
                                break;

                            case 1:
                                name = "Animations/IdlePlayer/Left/" + j.ToString();
                                break;

                            default:
                                break;
                        }
                        if (name != null)
                        {
                            Console.WriteLine(name);
                            playerIdleSprites[i][j] = Content.Load<Texture2D>(name);
                        }
                    }
                }
            }
            // Send the Sprites to the player Class:
            {

                player.LoadContent(playerAniSprites, playerIdleSprites, playerSprite);
                z.LoadContent(playerAniSprites, playerIdleSprites);

            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyHandler();

            player.Movement(movement, bgSprite);
            player.Update();

            z.Update();

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
            z.Draw(_spriteBatch);

            _spriteBatch.Draw(player.playerSprite, new Rectangle(player.drawPosition[0], player.drawPosition[1], 40, 80), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        protected void KeyHandler() {

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape)) {
                Exit();
            }
            if (state.IsKeyDown(Keys.A)&& !(state.IsKeyDown(Keys.D)))
            {
                movement[0] = -1;
            }
            else if (state.IsKeyDown(Keys.D) && !(state.IsKeyDown(Keys.A)))
            {
                movement[0] = 1;
            }
            else {
                movement[0] = 0;
            }
            if (state.IsKeyDown(Keys.W) && !(state.IsKeyDown(Keys.S)))
            {
                movement[1] = -1;
            }
            else if (state.IsKeyDown(Keys.S) && !(state.IsKeyDown(Keys.W)))
            {
                movement[1] = 1;
            }
            else {
                movement[1] = 0;
            }


        }
    }
}