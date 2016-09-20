using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HomeworkTwo
{
    class Player : GameObject
    {
        int levelScore;
        int totalScore;

        public Player(int x, int y, int width, int height):base(x, y, width, height) //takes attributes from the GameObj class 
        {
            levelScore = 0; //initializes both scores as zero
            totalScore = 0;
        }

        public int LevelScore //gets and sets the score for each level
        {
            get { return levelScore; }
            set { levelScore = value; }
        }

        public int TotalScore //gets and sets the score from each level and adds them to total the game
        {
            get { return totalScore; }
            set { totalScore = value; }
        }
    }
}
