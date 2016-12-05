using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObstacleLine2D
{
    class cButton
    {
        Texture2D btn;
        Vector2 Position;
        Rectangle btnRect;
        Color color = new Color(255, 255, 255, 255); //Прозрачность

        public Vector2 size;

        public cButton(Texture2D newTexture, GraphicsDevice graphics)
        {
            btn = newTexture;

            size = new Vector2(300, 120); //Размер кнопки
        }
        bool down;
        public bool isCicked;
        public void Update(MouseState mouse)
        {
            btnRect = new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y);
            Rectangle mouseRectangle = new Rectangle(mouse.X, mouse.Y, 1, 1);
            if (mouseRectangle.Intersects(btnRect))
            {
                if (color.A == 255) down = false;
                if (color.A == 0) down = true;
                if (down) color.A += 3; else color.A -= 3;
                if (mouse.LeftButton == ButtonState.Pressed) isCicked = true;
            }
            else if(color.A < 255)
            {
                color.A += 3;
                isCicked = false;
            }
        }

        public void setPosition(Vector2 newPosition)
        {
            Position = newPosition;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(btn, btnRect, color);
        }
    }
}
