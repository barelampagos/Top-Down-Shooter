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
    public class Enemy
    {
        public World world;
        public Texture2D ship;
        public Vector2 shipPosition;
        public Boolean hit;
        public int speed;
        public Boolean projectile;
        public Boolean side;
        public Random r;

        float shotCooldown;

        public void Initialize(World world, Texture2D texture, Vector2 position, int speed, Boolean proj)
        {
            this.world = world;
            shipPosition = position;
            hit = false;
            ship = texture;
            this.speed = speed;
            projectile = proj;
            r = new Random();

            if (r.Next(0, 100) > 50)
            {
                side = true;
            }
            else
            {
                side = false;
            }
        }


        public void Update(GameTime gameTime)
        {
            shotCooldown = Math.Max(0, shotCooldown - (float)gameTime.ElapsedGameTime.TotalSeconds);
           
            //Handles shooting enemies
            if (projectile == true)
            {
                if (shotCooldown == 0.0) { 
                    world.Projectiles.CreateEnemyProjectile(new Vector2(shipPosition.X + 15, shipPosition.Y + 40), new Vector2(0, 400));
                    shotCooldown = 2.0f;
                }

                if (side == true)
                {
                    shipPosition.X += 2.0f;
                    shipPosition.Y += 3.0f;


                    if (shipPosition.X > world.WorldWidth - 70) 
                    {
                        side = false;

                    }
                }
                else if (side == false)
                {
                    shipPosition.X -= 2.0f;
                    shipPosition.Y += 3.0f;

                    if (shipPosition.X <= 0)
                    {
                        side = true;
                    }
                }
            }
            else
            {
                //Ramming enemy moves faster
                shipPosition.Y += speed + 2;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (projectile != true)
            {
                spriteBatch.Draw(ship, shipPosition, Color.White);
            }
            else
            {
                spriteBatch.Draw(ship, shipPosition, Color.MediumTurquoise);

            }

        }

    }
}