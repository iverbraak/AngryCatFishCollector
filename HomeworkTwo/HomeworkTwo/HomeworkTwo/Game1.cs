using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace HomeworkTwo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private enum GameState { Menu, Game, Gameover}
        private GameState gameState;
        private SpriteFont font;
        private Texture2D playerImage;
        private Texture2D collectibleImage;
        private List<Collectible> collectibles;
        private KeyboardState kbState;
        private KeyboardState previousKbState;
        private Player player;
        private int level;
        private double timer;
        


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(10, 10, 75, 100); 
            collectibles = new List<Collectible>(); //initializes the player, collectible list, and the gamestate
            gameState = GameState.Menu;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("SpriteFont1"); //loads in the spritefont
            playerImage = Content.Load<Texture2D>("me tho"); //loads in the player's image
            collectibleImage = Content.Load<Texture2D>("fish"); //loads in the collectible's image
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch(gameState)
            {
                case GameState.Menu: //checks to see if the gamestate is in the menu state

                    if(SingleKeyPress(Keys.Enter) == true) //if the player presses enter, they will switch to the game state
                    {
                        ResetGame();
                        gameState = GameState.Game;
                    }
                    break;

                case GameState.Game: //checks to see if the gamestate is in the game state
                    previousKbState = kbState; //sets the keyboard state
                    kbState = Keyboard.GetState(); //gets the new keyboard state

                    timer = timer - gameTime.ElapsedGameTime.TotalSeconds; //sets the timer 

                    KeyboardControl(); //calls the method that allows the player to control the keyboard

                    ScreenWrap(player); //calls the method that wraps the player to the screen

                    for(int i=0; i < collectibles.Count; i++) //loops to check if a collision has occured
                    {
                        if(collectibles[i].CheckCollision(player) == true)  //if a collision has occured, adds to the score, and sets the collectible at the index to inactive
                        {
                            collectibles[i].Activation = false;
                            player.LevelScore += 10;
                            player.TotalScore += 10;
                        }

                        if(timer <= 0) //when the timer hits zero it will reset the game
                        {
                            gameState = GameState.Gameover;
                        }

                        if(player.LevelScore/10 >= collectibles.Count) //when the player collects all collectibles it will go to the next level
                        {
                            NextLevel();
                        }
                    }

                    break;

                case GameState.Gameover: //sets the game to game over, allows the user to press enter to go back to the main menu

                    if(SingleKeyPress(Keys.Enter) == true)
                    {
                        gameState = GameState.Menu;
                    }

                    break;

                default: 

                    break;
            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Menu: //draws the menu for the game, the title and the instructions

                    spriteBatch.DrawString(font, "Angry Cat Adventure", new Vector2(0, 0), Color.HotPink);
                    spriteBatch.DrawString(font, "Press enter to start", new Vector2(0, 20), Color.HotPink);


                    break;

                case GameState.Game:
                    player.Draw(spriteBatch);

                    foreach(Collectible c in collectibles) //draws each collectible to the screen
                    {
                        c.Draw(spriteBatch);
                    }
                    spriteBatch.DrawString(font, "Lvl: " + level, new Vector2(0, 0), Color.HotPink); //displays the current lvl of the game
                    spriteBatch.DrawString(font, "Lvl Score: " + player.LevelScore, new Vector2(0, 40), Color.HotPink); //the score for the individual lvl
                    spriteBatch.DrawString(font, "Time: " + string.Format("{0:0.00}", timer), new Vector2(0, 20), Color.HotPink); //timer
                    
                    break;

                case GameState.Gameover: //draws the gameover screen
                    spriteBatch.DrawString(font, "G A M E   O V E R . Score: " + player.TotalScore, new Vector2(0, 0), Color.HotPink);
                    spriteBatch.DrawString(font, "Press enter to go back to menu", new Vector2(0, 20), Color.HotPink);

                    break;

                default:

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// transitions the game to the next level
        /// </summary>
        public void NextLevel()
        {
            Random rng = new Random(); //rng to create a random number of collectibles
            level++; //goeas to the next level
            timer = 10; //timer will count down from 10
            player.LevelScore = 0; 
            player.Rect = new Rectangle(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2, player.Rect.Width, player.Rect.Height);
            player.Texture = playerImage; //sets the player's texture to the image
            collectibles.Clear(); //clears all collectibles from the list

            for(int i=0; i < 5 * level; i++) //adds a random number of collectibles to the level
            {
                Collectible newCol = new Collectible(rng.Next(0, GraphicsDevice.Viewport.Width - 75), rng.Next(0, GraphicsDevice.Viewport.Height - 50), 75, 50);
                newCol.Texture = collectibleImage;
                collectibles.Add(newCol);
            }
        }
        /// <summary>
        /// resets the game
        /// </summary>
        public void ResetGame() //resets the game
        {
            level = 0;
            player.TotalScore = 0;
            NextLevel();
        }

        //takes the user's keyboard input
        public void KeyboardControl()
        {
            Keyboard.GetState();

            //if the up arrow key is pressed
            if (kbState.IsKeyDown(Keys.Up))
            {
                player.Rect = new Rectangle(player.Rect.X, player.Rect.Y - 3, player.Rect.Width, player.Rect.Height);
            }
            //if the down arrow key is pressed
            if (kbState.IsKeyDown(Keys.Down))
            {
                player.Rect = new Rectangle(player.Rect.X, player.Rect.Y + 3, player.Rect.Width, player.Rect.Height);
            }
            //if the right arrow key is pressed
            if (kbState.IsKeyDown(Keys.Right))
            {
                player.Rect = new Rectangle(player.Rect.X + 3, player.Rect.Y, player.Rect.Width, player.Rect.Height);
            }
            //if the left arrow key is pressed
            if (kbState.IsKeyDown(Keys.Left))
            {
                player.Rect = new Rectangle(player.Rect.X - 3, player.Rect.Y, player.Rect.Width, player.Rect.Height);
            }
        }

        private void ScreenWrap(GameObject go)
        {
            if (go.Rect.X > GraphicsDevice.Viewport.Width) //if the player goes off screen to the right it will wrap around
            {
                go.Rect = new Rectangle(-go.Rect.Width, go.Rect.Y, go.Rect.Width, go.Rect.Height);
            }
            else if (go.Rect.X < -go.Rect.Width) //wraps the player around the screen if it goes off to the left
            {
                go.Rect = new Rectangle(GraphicsDevice.Viewport.Width, go.Rect.Y, go.Rect.Width, go.Rect.Height);
            }
            else if (go.Rect.Y > GraphicsDevice.Viewport.Height) //wraps the player around if it goes off the top
            {
                go.Rect = new Rectangle(go.Rect.X, -go.Rect.Height, go.Rect.Width, go.Rect.Height);
            }
            else if(go.Rect.Y < -go.Rect.Height) //wraps the player around if it goes off the bottom
            {
                go.Rect = new Rectangle(go.Rect.X, GraphicsDevice.Viewport.Height, go.Rect.Width, go.Rect.Height);
            }
        }

        public bool SingleKeyPress(Keys key) //checks to see if a key has been pressed once
        {
            kbState = Keyboard.GetState();
            if(kbState.IsKeyDown(key) && previousKbState.IsKeyUp(key))
            {
                previousKbState = kbState;
                return true;  
            }
            else
            {
                previousKbState = kbState;
                return false;
            }
        }
    }
}
