using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObstacleLine2D
{
    class Traffic
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 velocity;

        public bool isVisible = true;

        Random rand = new Random();
        int randX, randY;

        public Traffic(Texture2D newTexture,Vector2 newPosition)
        {
            texture = newTexture;
            position = newPosition;

            randY = rand.Next(0,0);
            randX = rand.Next(-8,-5);

            velocity = new Vector2(randX, randY);
        }

        public void Update(GraphicsDevice graphics)
        {
            position += velocity;

            if (position.Y <= 0 || position.Y >= graphics.Viewport.Height - texture.Height)
                velocity.Y -= velocity.Y;
            if (position.X < 0 - texture.Width)
                isVisible = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
