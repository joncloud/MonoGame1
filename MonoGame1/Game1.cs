using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tile;

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

            base.Initialize();
        }

        Random random = new Random(0);
        Rectangle destinationRectangle;
        Vector2 velocity;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tile = Content.Load<Texture2D>("Tile");
            destinationRectangle = new Rectangle(
                0,
                0,
                graphics.PreferredBackBufferWidth / 6,
                graphics.PreferredBackBufferHeight / 6
            );
            velocity = Vector2.UnitX * 5;
            RotateVelocity();
        }

        void RotateVelocity()
        {
            var radians = MathHelper.TwoPi * (float)random.NextDouble();
            velocity = Vector2.Transform(
                velocity, 
                Matrix.CreateRotationZ(radians)
            );
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

            do
            {
                var point = new Vector2(destinationRectangle.X, destinationRectangle.Y);
                point += velocity;

                destinationRectangle.X = (int)point.X;
                destinationRectangle.Y = (int)point.Y;

            } while (ShouldRetry());

            base.Update(gameTime);
        }

        bool ShouldRetry()
        {
            var frame = new Rectangle(
                0,
                0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight
            );
            if (!frame.Contains(destinationRectangle))
            {
                RotateVelocity();
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(tile, destinationRectangle, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
