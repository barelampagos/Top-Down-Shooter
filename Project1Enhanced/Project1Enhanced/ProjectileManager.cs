using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project1Enhanced
{
    public class ProjectileManager
    {
        private World mWorld;
        public List<Projectile> playerProjectiles;
        private Texture2D playerLaser;
        
        //
        public List<Projectile> enemyProjectiles;
        private Texture2D enemyLaser;



        public ProjectileManager(World containingWorld)
        {
            mWorld = containingWorld;
            playerProjectiles = new List<Projectile>();
            
            //
            enemyProjectiles = new List<Projectile>();
        }

        public void LoadContent(ContentManager content)
        {
            playerLaser = content.Load<Texture2D>("Laserbeam");
            enemyLaser = content.Load<Texture2D>("EnemyLaserbeam");
        }


        public void CreateProjectile(Vector2 position, Vector2 velocity)
        {
            playerProjectiles.Add(new Projectile(playerLaser, position, velocity));
        }

        //
        public void CreateEnemyProjectile(Vector2 position, Vector2 velocity)
        {
            enemyProjectiles.Add(new Projectile(enemyLaser, position, velocity));
        }

        public void Update(GameTime gameTime)
        {
            List<Projectile> toDeletePlayer = null;
            List<Projectile> toDeleteEnemy = null;

            foreach (Projectile p in playerProjectiles)
            {
                p.Update(gameTime);
                if (p.Position.X < 0 || p.Position.X > mWorld.WorldWidth || p.Position.Y < 0 || p.Position.Y > mWorld.WorldHeight || p.Active == false)
                {
                    if (toDeletePlayer == null)
                    {
                        toDeletePlayer = new List<Projectile>();
                    }
                    toDeletePlayer.Add(p);
                }
            }

            if (toDeletePlayer != null)
            {
                foreach (Projectile p in toDeletePlayer)
                {
                    playerProjectiles.Remove(p);
                }
            }


            //
            foreach (Projectile p in enemyProjectiles)
            {
                p.Update(gameTime);
                if (p.Position.X < 0 || p.Position.X > mWorld.WorldWidth || p.Position.Y < 0 || p.Position.Y > mWorld.WorldHeight || p.Active == false)
                {
                    if (toDeleteEnemy == null)
                    {
                        toDeleteEnemy = new List<Projectile>();
                    }
                    toDeleteEnemy.Add(p);
                }
            }

            if (toDeleteEnemy != null)
            {
                foreach (Projectile p in toDeleteEnemy)
                {
                    enemyProjectiles.Remove(p);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Projectile p in playerProjectiles)
            {
                p.Draw(sb);
            }

            foreach (Projectile p in enemyProjectiles)
            {
                p.Draw(sb);
            }

        }

    }
}