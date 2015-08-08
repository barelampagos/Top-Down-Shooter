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
    class Boss
    {
        public World world;
        public Texture2D ship;
        public Vector2 shipPosition;
        public int health;
        public Boolean side;
        public Boolean top;


        float shotCooldown;


        public void Initialize(World world, Texture2D texture, Vector2 position)
        {
            this.world = world;
            shipPosition = position;
            ship = texture;

            health = 5;

            side = true;
            top = true;

            
        }


        public void Update(GameTime gameTime)
        {

            if (top == true)
            {
                shipPosition.Y += 2.0f;

                if (shipPosition.Y == 300)
                {
                    top = false;
                }
            }
            else if (top == false)
            {
                shipPosition.Y -= 2.0f;

                if (shipPosition.Y == 0)
                {
                    top = true;
                }
            }


            shotCooldown = Math.Max(0, shotCooldown - (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (shotCooldown == 0.0)
            {
                world.Projectiles.CreateEnemyProjectile(new Vector2(shipPosition.X + 25, shipPosition.Y + 41), new Vector2(0, 500));
                world.Projectiles.CreateEnemyProjectile(new Vector2(shipPosition.X + 25, shipPosition.Y + 41), new Vector2(-45, 500));
                world.Projectiles.CreateEnemyProjectile(new Vector2(shipPosition.X + 50, shipPosition.Y + 41), new Vector2(0, 500));
                world.Projectiles.CreateEnemyProjectile(new Vector2(shipPosition.X + 75, shipPosition.Y + 41), new Vector2(45, 500));
                world.Projectiles.CreateEnemyProjectile(new Vector2(shipPosition.X + 75, shipPosition.Y + 41), new Vector2(0, 500));

                shotCooldown = 1.0f;
            }

            if (side == true)
            {
                shipPosition.X += 4.0f;

                if (shipPosition.X > world.WorldWidth - 70)
                {
                    side = false;

                }
            }
            else if (side == false)
            {
                shipPosition.X -= 4.0f;

                if (shipPosition.X <= 0)
                {
                    side = true;

                }

            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ship, shipPosition, Color.White);

        }

    }
}
