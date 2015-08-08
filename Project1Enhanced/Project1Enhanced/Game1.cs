using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project1Enhanced
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch mSpriteBatch;
        World mWorld;
        SpriteFont mFont;
        SpriteFont titleFont;

        Boolean menu;

        Texture2D background;
        Vector2 screenPosition;
        Vector2 origin;
        Vector2 textureSize;


        public int Width { get; set; }
        public int Height { get; set; }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Width = 600;
            graphics.PreferredBackBufferHeight = Height = 800;
            Content.RootDirectory = "Content";

            menu = true;
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            mWorld = new World(this, Width, Height);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            mSpriteBatch = new SpriteBatch(GraphicsDevice);
            //background = Content.Load<Texture2D>("background");
            mFont = Content.Load<SpriteFont>("CourierNew");
            titleFont = Content.Load<SpriteFont>("titleFont");

            background = Content.Load<Texture2D>("spacebackground");

            origin = new Vector2(background.Width / 2, 0);
            screenPosition = new Vector2(Width / 2, Height / 2);
            textureSize = new Vector2(0, background.Height);

            mWorld.LoadContent(Content);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            KeyboardState keyboard = Keyboard.GetState();

            // Allows the game to exit
            if (keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            if (menu == true)
            {
                if (keyboard.IsKeyDown(Keys.Space))
                {
                    menu = false;
                }
            }
            else
            {
                mWorld.Update(gameTime);
            }

            //test
            screenPosition.Y += 2;
            screenPosition.Y = screenPosition.Y % background.Height;

            base.Update(gameTime);
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            mSpriteBatch.Begin();
            
            //Moving Background
            if (screenPosition.Y < Height)
            {
                mSpriteBatch.Draw(background, screenPosition, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }
            mSpriteBatch.Draw(background, screenPosition - textureSize, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);

            //Menu System
            if (menu == true)
            {
                mSpriteBatch.DrawString(titleFont, "Starfighter X", new Vector2(Width/2 - 150, 200), Color.White);
                mSpriteBatch.DrawString(mFont, "Press Space to Play", new Vector2(Width / 2 - 130, 600), Color.White);

            }

            else if (mWorld.Player1.alive == true)
            {
                mWorld.Draw(mSpriteBatch);
            }
            else
            {
                mSpriteBatch.DrawString(titleFont, "GAME OVER", new Vector2(200, 100), Color.White);

                mSpriteBatch.DrawString(mFont, "Your Score: " + mWorld.Player1.score, new Vector2(200, Height - 150), Color.White);

                mSpriteBatch.DrawString(mFont, "Exit game: Escape", new Vector2(180, Height - 100), Color.White);

            }


            

            mSpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}