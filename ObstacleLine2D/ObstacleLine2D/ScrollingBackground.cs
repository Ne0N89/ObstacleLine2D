using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObstacleLine2D
{
    class Backgrounds
    {
        public Texture2D texture;
        public Rectangle rect;

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
        class Scrolling : Backgrounds
        {
            public Scrolling(Texture2D newTexture, Rectangle newRect)
            {
                texture = newTexture;
                rect = newRect;
            }
            public void Update()
            {
                rect.X -= 3;
            }
        }
}
