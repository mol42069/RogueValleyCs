using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace RogueValley
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

// PLAYER VARIABLES:

        // Player Sprites:

        private Texture2D playerSprite;
        private Texture2D[][] playerAniSprites;
        private Texture2D[] playerIdleSprites;

        // Player Positional Variables:

        private int playerDirection;
        private int[] playerPosition;

        // Player Animation Variables:

        private int animationCount, animationTimer, animationMaxTime;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            // PLAYER VARIABLES:

            // Player Positional Variables:
            {
                playerPosition = new int[2];
                playerPosition[0] = 100;
                playerPosition[1] = 200;
            }
            // Player Animation Variables:
            {
                animationCount = 0;
                animationTimer = 0;
                animationMaxTime = 8;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

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
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Player-Movement:
            {
                //playerPosition[0]--;
                //playerPosition[1]--;
            }
            // Player-Animation:
            {
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

            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.Draw(playerSprite, new Rectangle(playerPosition[0], playerPosition[1], 20, 40), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}