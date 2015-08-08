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

    public class Projectile
    {
        public Texture2D laser;
        public int Width;
        public int Height;

        public Vector2 Velocity { get; set; }
        public Vector2 Position { get; set; }
        public Boolean Active;



        public Projectile(Texture2D texture, Vector2 initialPosition, Vector2 initialVelocity)
        {
            laser = texture;
            Width = texture.Width;
            Height = texture.Height;
            Position = initialPosition;
            Velocity = initialVelocity;
            Active = true;
        }

        public void Update(GameTime gametime)
        {
            Position += Velocity * (float)gametime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(laser, Position, new Rectangle(0, 0, laser.Width, laser.Height + 20), Color.White);
        }

    }
}