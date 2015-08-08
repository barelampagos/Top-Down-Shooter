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
    public class PowerUp
    {
        public World world;
        public Texture2D powerUptexture;
        public Vector2 position;
        public Boolean Active;
        public int Width;
        public int Height;

        public void Initialize(World world, Texture2D texture, Vector2 position)
        {
            this.world = world;
            this.position = position;
            Active = true;
            powerUptexture = texture;
            Width = texture.Width;
            Height = texture.Height;
        }


        public void Update(GameTime gameTime)
        {
            
            position.Y += 3;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(powerUptexture, position, Color.White);

        }

    }
}