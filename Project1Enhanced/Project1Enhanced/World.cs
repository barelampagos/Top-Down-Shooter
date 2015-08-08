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
    public class World
    {
        protected Game1 game;

        public Player Player1 { get; set; }
        public List<Enemy> enemies;
        public List<PowerUp> powerUps;
        public ProjectileManager Projectiles { get; set; }
        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }
        public Random random;
        public SpriteFont mFont;

        Texture2D powerUptexture;
        float powerUpCD;
        

        Texture2D enemyTexture; 
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;
        Boolean projectile;

        Texture2D explosionTexture;
        List<Animation> explosions;

        Boss boss;
        Boolean bossActive;
        Texture2D bossTexture;
        

        public World(Game1 containingGame, int w, int h)
        {
            game = containingGame;
            Player1 = new Player(this);
            random = new Random();
            Projectiles = new ProjectileManager(this);
            WorldWidth = w;
            WorldHeight = h;

            explosions = new List<Animation>();

            enemies = new List<Enemy>();
            previousSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            projectile = false;

            powerUps = new List<PowerUp>();

            bossActive = false;
        }

        public void LoadContent(ContentManager content)
        {
            Player1.LoadContent(content);
            enemyTexture = content.Load<Texture2D>("Enemy");
            Projectiles.LoadContent(content);
            mFont = content.Load<SpriteFont>("CourierNew");
            explosionTexture = content.Load<Texture2D>("explosion");
            powerUptexture = content.Load<Texture2D>("PowerUp");
            bossTexture = content.Load<Texture2D>("Boss");


        }

        public void UpdateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }
        private void AddExplosion(Vector2 position)
        {
            Animation explosion = new Animation();
            explosion.Initialize(explosionTexture, position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explosion);
        }

        private void UpdateEnemies(GameTime gameTime)
        {

            if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
            {
                previousSpawnTime = gameTime.TotalGameTime;

                // Adds a random number of Enemies from 1 - 3
                int enemyNum = random.Next(1, 3);
                for (int i = 0; i < enemyNum; i++)
                {
                    if (bossActive != true)
                    {
                        AddEnemy();
                    }
                }

            }

            // Update the Enemies
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime);

                if (enemies[i].hit == true)
                {
                    AddExplosion(new Vector2(enemies[i].shipPosition.X + 40, enemies[i].shipPosition.Y + 30));
                    enemies.RemoveAt(i);
                }
            }
        }
        private void AddEnemy()
        {
            //Generates random speed for enemy
            int speed = random.Next(3, 6);

            // Create an enemy
            Enemy enemy = new Enemy();

            enemy.Initialize(this, enemyTexture, new Vector2(random.Next(0, WorldWidth - 70), 0), speed, projectile);

            // Add the enemy to the active enemies list
            enemies.Add(enemy);

            //Projectile enemies spawn at 2000 points
            if (projectile == false && Player1.score > 2000)
            {
                projectile = true;
            }
            else
            {
                projectile = false;
            }
        }

        private void AddBoss()
        {
            boss = new Boss();
            boss.Initialize(this, bossTexture, new Vector2(WorldWidth / 2 - 50, 0));

        }

        private void UpdatePowerup(GameTime gameTime)
        {
            powerUpCD = Math.Max(0, powerUpCD - (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (powerUpCD == 0.0f)
            {
                AddPowerup();
                powerUpCD = 20.0f;
            }

            for (int i = powerUps.Count - 1; i >= 0; i--)
            {
                powerUps[i].Update(gameTime);

                if (powerUps[i].Active == false)
                {
                    Player1.powerUp = true;
                    Player1.powerUpShots = 3;
                    powerUps.RemoveAt(i);
                }
            }
        }
        private void AddPowerup()
        {
            PowerUp p = new PowerUp();
            p.Initialize(this, powerUptexture, new Vector2(random.Next(0, WorldWidth - 70), 0));
            powerUps.Add(p);
        }

        private void UpdateCollision()
        {
            Rectangle playerRectangle;
            Rectangle projectileRectangle;
            Rectangle enemyRectangle;
            Rectangle enemyProjectileRectangle;
            Rectangle powerUpRectangle;
            Rectangle bossRectangle;

            // Only create the rectangle once for the player
            playerRectangle = new Rectangle((int)Player1.shipPosition.X,
            (int)Player1.shipPosition.Y,
            Player1.ship.Width,
            Player1.ship.Height);

            // Collision between the player and the enemies
            for (int i = 0; i < enemies.Count; i++)
            {
                enemyRectangle = new Rectangle((int)enemies[i].shipPosition.X,
                (int)enemies[i].shipPosition.Y,
                enemies[i].ship.Width,
                enemies[i].ship.Height);

                // Determine if the enemy collided with each player
                if (playerRectangle.Intersects(enemyRectangle))
                {
                    // Subtract the health from the player
                    Player1.armor -= 1;

                    //Destroy enemy that is hit
                    enemies[i].hit = true;

                }

            }

            //Player projectiles and Enemy Ships
            for (int i = 0; i < Projectiles.playerProjectiles.Count; i++)
            {

                projectileRectangle = new Rectangle((int)Projectiles.playerProjectiles[i].Position.X -
                Projectiles.playerProjectiles[i].Width / 2, (int)Projectiles.playerProjectiles[i].Position.Y -
                Projectiles.playerProjectiles[i].Height / 2, Projectiles.playerProjectiles[i].Width, Projectiles.playerProjectiles[i].Height);

                for (int j = 0; j < enemies.Count; j++)
                {                   
                    // rectangle2: rectangle for enemy
                    enemyRectangle = new Rectangle((int)enemies[j].shipPosition.X - enemies[j].ship.Width / 2,
                    (int)enemies[j].shipPosition.Y - enemies[j].ship.Height / 2,
                    enemies[j].ship.Width, enemies[j].ship.Height);

                    // Determine if the two objects collided with each other
                    if (projectileRectangle.Intersects(enemyRectangle))
                    {
                        enemies[j].hit = true;
                        Projectiles.playerProjectiles[i].Active = false;
                        Player1.score += 100;
                        Player1.oneUp += 1;
                    }
                }

                // For Boss Collision
                if (bossActive == true)
                {
                    bossRectangle = new Rectangle((int)boss.shipPosition.X - boss.ship.Width / 2,
                    (int)boss.shipPosition.Y - boss.ship.Height / 2,
                    boss.ship.Width, boss.ship.Height);

                    if (projectileRectangle.Intersects(bossRectangle))
                    {
                        boss.health--;
                        Player1.score += 100;
                        Projectiles.playerProjectiles[i].Active = false;
                        AddExplosion(new Vector2(boss.shipPosition.X + 50, boss.shipPosition.Y + 41));
                    }
                }
              
            }

            //Enemy Projectiles and Player
            for (int i = 0; i < Projectiles.enemyProjectiles.Count; i++)
            {
                enemyProjectileRectangle = new Rectangle((int)Projectiles.enemyProjectiles[i].Position.X -
                Projectiles.enemyProjectiles[i].Width / 2, (int)Projectiles.enemyProjectiles[i].Position.Y -
                Projectiles.enemyProjectiles[i].Height / 2, Projectiles.enemyProjectiles[i].Width, Projectiles.enemyProjectiles[i].Height);


                // Determine if the two objects collided with each other
                if (playerRectangle.Intersects(enemyProjectileRectangle))
                {
                    // Subtract the health from the player
                    Player1.armor -= 1;
                    Projectiles.enemyProjectiles[i].Active = false;
                    AddExplosion(new Vector2(Player1.shipPosition.X + 40, Player1.shipPosition.Y + 30));

                }
                
            }

            //Powerup Collision
            for (int i = 0; i < powerUps.Count; i++)
            {

                powerUpRectangle = new Rectangle((int)powerUps[i].position.X -
                powerUps[i].Width / 2, (int)powerUps[i].position.Y -
                powerUps[i].Height / 2, powerUps[i].Width, powerUps[i].Height);


                // Determine if the two objects collided with each other
                if (playerRectangle.Intersects(powerUpRectangle))
                {
                    // Subtract the health from the player
                    Player1.powerUp = true;
                    powerUps[i].Active = false;

                }

            }

        }

        public void Update(GameTime gameTime)
        {
            Projectiles.Update(gameTime);
            Player1.Update(gameTime);
            UpdateCollision();
            UpdateExplosions(gameTime);
            UpdatePowerup(gameTime);
            UpdateEnemies(gameTime);
            
              
            if (Player1.armor <= 0)
            {
                Player1.alive = false;
            }
            //Boss spawns every 3000 points
            if (Player1.score % 3000 == 0 && Player1.score != 0 && bossActive == false)
            {
                bossActive = true;
                AddBoss();
            }

            if (bossActive == true)
            {
                boss.Update(gameTime);
            }

            if (bossActive == true && boss.health <= 0)
            {
                Player1.score += 1000;
                bossActive = false;
            }
          
        } 

        public void Draw(SpriteBatch sb)
        {
            Player1.Draw(sb);
            Projectiles.Draw(sb);

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(sb);
            }
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Draw(sb);
            }
            for (int i = 0; i < powerUps.Count; i++)
            {
                powerUps[i].Draw(sb);
            }

            sb.DrawString(mFont, "Score: " + Player1.score, new Vector2(50, 750), Color.White);
            sb.DrawString(mFont, "Armor: " + Player1.armor, new Vector2(400, 750), Color.White);
            sb.DrawString(mFont, "One Up: " + Player1.oneUp + " / 10", new Vector2(400, 700), Color.White);


            if (bossActive == true)
            {
                boss.Draw(sb);
            }

            if (Player1.powerUp == true)
            {
                sb.DrawString(mFont, "Special Ammo: " + (Player1.powerUpShots), new Vector2(50, 700), Color.White);
            }

        }

        
    }
}