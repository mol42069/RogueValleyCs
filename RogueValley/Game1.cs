using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using RogueValley.Entities;

namespace RogueValley
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;

        private int[] movement;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            player = new Player();

            movement = new int[2]; // 0=X-Axis | 1=y-Axis

            int[] tempPos = new int[2];

            tempPos[0] = 100;
            tempPos[1] = 100;

            player.Initialize(tempPos);

            base.Initialize();


        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D playerSprite;
            Texture2D[][] playerAniSprites;
            Texture2D[][] playerIdleSprites;

            playerSprite = Content.Load<Texture2D>("Entity/Player");

            // Load all Player-Walking-Animation Variables into an Array:
            {
                playerAniSprites = new Texture2D[4][];

                for (int i = 0; i < 4; i++)
                {

                    playerAniSprites[i] = new Texture2D[6];

                    for (int j = 0; j < 6; j++)
                    {

                        string name = null;

                        switch (i)
                        {

                            case 0:
                                name = "Animations/Player/Down/" + j.ToString();
                                break;

                            case 1:
                                name = "Animations/Player/Up/" + j.ToString();
                                break;

                            case 2:
                                name = "Animations/Player/Left/" + j.ToString();
                                break;

                            case 3:
                                name = "Animations/Player/Right/" + j.ToString();
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
                playerIdleSprites = new Texture2D[4][];

                for (int i = 0; i < 4; i++)
                {

                    playerIdleSprites[i] = new Texture2D[6];

                    for (int j = 0; j < 4; j++)
                    {

                        string name = null;

                        switch (i)
                        {

                            case 0:
                                name = "Animations/IdlePlayer/Down/" + j.ToString();
                                break;

                            case 1:
                                name = "Animations/IdlePlayer/Up/" + j.ToString();
                                break;

                            case 2:
                                name = "Animations/IdlePlayer/Left/" + j.ToString();
                                break;

                            case 3:
                                name = "Animations/IdlePlayer/Right/" + j.ToString();
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

            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyHandler();

            player.Movement(movement);
            player.Update();

            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(player.playerSprite, new Rectangle(player.playerPosition[0], player.playerPosition[1], 20, 40), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        protected void KeyHandler() {

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Escape)) {
                Exit();
            }
            if (state.IsKeyDown(Keys.A))
            {
                movement[0] = -1;
            }
            else if (state.IsKeyDown(Keys.D))
            {
                movement[0] = 1;
            }
            else {
                movement[0] = 0;
            }
            if (state.IsKeyDown(Keys.W))
            {
                movement[1] = -1;
            }
            else if (state.IsKeyDown(Keys.S))
            {
                movement[1] = 1;
            }
            else {
                movement[1] = 0;
            }


        }
    }
}