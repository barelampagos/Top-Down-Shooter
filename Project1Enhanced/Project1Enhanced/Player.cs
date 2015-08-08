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
    public class Player
    {
        public Texture2D ship;
        private World mWorld;
        float shotCooldown;
        public Vector2 shipPosition;
        public int armor;
        public Boolean alive;
        public int score;
        public Boolean powerUp;
        public int powerUpShots;
        public int oneUp;
        Vector2 gunPosition;

        KeyboardState keyboard;

        //Constructor
        public Player(World game)
        {
            mWorld = game;
            shotCooldown = 0.0f;
            shipPosition = new Vector2(270, 600);
            armor = 3;
            alive = true;
            score = 0;
            powerUp = false;
            oneUp = 0;
            keyboard = Keyboard.GetState();
            gunPosition = new Vector2(22.0f, 25.0f);
        }



        public void LoadContent(ContentManager content)
        {
            ship = content.Load<Texture2D>("Spaceship");

        }


        public void Update(GameTime gameTime)
        {

            shotCooldown = Math.Max(0, shotCooldown - (float)gameTime.ElapsedGameTime.TotalSeconds);

            KeyboardState keyboard = Keyboard.GetState();

            //Controls ship with Arrow Keys
            if (keyboard.IsKeyDown(Keys.Left) && shipPosition.X > 0)
            {
                shipPosition.X -= 12;
            }
            else if (keyboard.IsKeyDown(Keys.Right) && shipPosition.X < mWorld.WorldWidth - 70)
            {
                shipPosition.X += 12;
            }
            else if (keyboard.IsKeyDown(Keys.Up) && shipPosition.Y > mWorld.WorldHeight / 2)
            {
                shipPosition.Y -= 12;
            }
            else if (keyboard.IsKeyDown(Keys.Down) && shipPosition.Y < mWorld.WorldHeight - 100)
            {
                shipPosition.Y += 12;
            }

            //Spacebar = shoots projectile
            if (keyboard.IsKeyDown(Keys.Space) && shotCooldown == 0 && powerUp == false)
            {
                shotCooldown = 0.75f;
                mWorld.Projectiles.CreateProjectile(new Vector2(shipPosition.X + gunPosition.X, shipPosition.Y - gunPosition.Y), new Vector2(0, -600));
               
            }
            else if (keyboard.IsKeyDown(Keys.Space) && shotCooldown == 0 && powerUp == true)
            {
                powerUpShots--;
                shotCooldown = 0.25f;

                mWorld.Projectiles.CreateProjectile(new Vector2(shipPosition.X + gunPosition.X, shipPosition.Y - gunPosition.Y), new Vector2(0, -650));
                mWorld.Projectiles.CreateProjectile(new Vector2(shipPosition.X + gunPosition.X - 20, shipPosition.Y - gunPosition.Y), new Vector2(-40, -600));
                mWorld.Projectiles.CreateProjectile(new Vector2(shipPosition.X + gunPosition.X + 20, shipPosition.Y - gunPosition.Y), new Vector2(40, -600));

                if (powerUpShots == 0)
                {
                    powerUp = false;
                    powerUpShots = 3;
                }

            }
            if (oneUp >= 10)
            {
                armor += 1;
                oneUp = 0;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ship, shipPosition, Color.White);
        }

    }
}