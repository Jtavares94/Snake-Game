using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Snake
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Snake snake;
        private Grid grid;
        private GameContentManager contentManager;
        private CollisionManager collisionManager;

        private readonly Vector2 tileSize = new Vector2(32, 32);


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 640;
            graphics.ApplyChanges();
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
            collisionManager = new CollisionManager();
            contentManager = new GameContentManager(new XnaContentManagerAdapter(Content));
            snake = new Snake(contentManager, 4, 4, 4, 1, 0, tileSize);
            grid = new Grid(contentManager, tileSize, GridMap.GetGrid("level1"));
            grid.AddRandomPowerUp(snake.GetPositions());

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
            contentManager.Add<Texture2D>("darkgreen");
            contentManager.Add<Texture2D>("lightblue");
            contentManager.Add<Texture2D>("apple");
            contentManager.Add<Texture2D>("wall");

            // TODO: use this.Content to load your game content here
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

            HandleInput(Keyboard.GetState());

            // TODO: Add your update logic here
            snake.Update(gameTime);
            HandleCollision();
            base.Update(gameTime);
        }

        private void HandleInput(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyState.IsKeyDown(Keys.W))
            {
                snake.ChangeDirection(0, -1);
            }
            else if (keyState.IsKeyDown(Keys.S))
            {
                snake.ChangeDirection(0, 1);
            }
            else if (keyState.IsKeyDown(Keys.A))
            {
                snake.ChangeDirection(-1, 0);
            }
            else if (keyState.IsKeyDown(Keys.D))
            {
                snake.ChangeDirection(1, 0);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            grid.Draw(spriteBatch);
            snake.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleCollision()
        {
            CollisionType collision = collisionManager.Collision(grid, snake);
            if (collision == CollisionType.PowerUp)
            {
                snake.AddTail();
                grid.Clear(snake.X, snake.Y);
                grid.AddRandomPowerUp(snake.GetPositions());
            }
            
            if (collision == CollisionType.Fatal)
            {
                Exit();
            }           
        }
    }
}
