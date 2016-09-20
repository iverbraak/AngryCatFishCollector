using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace HomeworkTwo
{
    class Collectible : GameObject
    {
        private bool activation;
        public Collectible(int x, int y, int width, int height):base(x, y, width, height) //param constructor that takes base from the GameObj class
        {
            activation = true; 
        }

        public bool Activation //property that can tell if the collectible can be collided with
        {
            get { return activation; }
            set { activation = value; }
        }
        public bool CheckCollision(GameObject gameObj) //checks if the player collides with the collectibles
        {
            if(activation == false) //if the collectible isnt active, it cannot be collided with
            {
                return false;
            }
            else if(this.Rect.Intersects(gameObj.Rect)) //if the player can intersect with the collectibles, then the collectible is activated
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Draw(SpriteBatch spr) //draws the spritebatch if the collectible has been activated
        {
            if (activation == true)
            {
              base.Draw(spr);
            }

        }
    }
}
