using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Evo_v0._2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Point GameSpace;
        List<Food> Foods = new List<Food>();
        List<Creature> Creatures = new List<Creature>();
        List<Egg> Eggs = new List<Egg>();
        int numberOfFood = 400;
        int numberOfCreatures = 20;
        int foodInterval = 5;
        double foodCountdown = 0;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 900;
            graphics.PreferredBackBufferWidth = 900;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameSpace = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //Generate Starting Food
            for(int i = 0; i < numberOfFood; i++)
            {
                Foods.Add(new Food(GameSpace, GraphicsDevice));
            }
            for(int i = 0; i < numberOfCreatures; i++)
            {
                Creatures.Add(new Creature(GameSpace, GraphicsDevice));
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            
            Window.Title = "Creatures: "+Creatures.Count.ToString() +" Food: " + Foods.Count.ToString()+ " Eggs: "+Eggs.Count.ToString();

            for (int i =Eggs.Count - 1; i >= 0; i--)
            {
                    Eggs[i].UpdateEgg(gameTime, GraphicsDevice, GameSpace, Creatures, Eggs);
            }

            for (int i = Creatures.Count-1;i>=0;i--)
            {
                Creatures[i].UpdateCreature(gameTime, GameSpace, GraphicsDevice, Foods, Creatures, Eggs);
                if (i >= Creatures.Count)
                {
                    i = Creatures.Count;
                }
            }

            foodCountdown += gameTime.ElapsedGameTime.TotalSeconds;
            if (Foods.Count <= numberOfFood && foodCountdown>= foodInterval)
            {
                int counter = Worker.StaticRandom.Instance.Next(25,125);
                for (int i = 0; i <= counter; i++)
                {
                    Foods.Add(new Food(GameSpace, GraphicsDevice));
                    foodCountdown = 0;
                }
                Debug.Print("Foods Added: " + counter.ToString());
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach(Food f in Foods)
            {
                spriteBatch.Draw(f.Texture, f.Position);
            }

            foreach(Egg e in Eggs)
            {
                spriteBatch.Draw(e.Texture, e.Position);
            }

            foreach(Creature c in Creatures)
            {
                spriteBatch.Draw(c.Texture, c.Position);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
