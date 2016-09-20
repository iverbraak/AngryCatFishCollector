using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HomeworkTwo
{
    class GameObject
    {
        private Texture2D texture;
        private Rectangle rect;

        public Texture2D Texture //the texture of the game obj, in this game's case, its image
        {
            get { return texture; }
            set { texture = value; }
        }

        public Rectangle Rect //gets the properties of the rectangle such as the x and y
        {
            get { return rect; }
            set { rect = value; }
        }

        public GameObject(int x, int y, int width, int height) //param constructor that accepts x,y,width,height and creates a rectangle with those values
        {
            rect = new Rectangle(x, y, width, height);
        }

        public virtual void Draw(SpriteBatch spr) //draws the actual spritebatch 
        {
            spr.Draw(texture, rect, Color.White);
        }
    }
}
