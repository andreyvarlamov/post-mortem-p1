using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;
using RogueSharp.MapCreation;
using RogueSharp.DiceNotation;

using PostMortem_P1.Core;
using PostMortem_P1.Graphics;
using PostMortem_P1.Systems;
using PostMortem_P1.Input;
using PostMortem_P1.MapGenSchemas;

namespace PostMortem_P1
{
    public class PostMortem : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager _inputManager;

        public PostMortem()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _inputManager = new InputManager();
        }

        protected override void Initialize()
        {
            Global.Camera.ViewportWidth = _graphics.GraphicsDevice.Viewport.Width;
            Global.Camera.ViewportHeight = _graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Global.SpriteManager = new SpriteManager();
            Global.SpriteManager.LoadContent(Content);

            Global.SchedulingSystem = new SchedulingSystem();
            Global.CommandSystem = new CommandSystem();

            var roomsMapGen = new RoomsMapGen(20, 13, 7);
            var cityMapGen = new CityMapGen();
            var wildernessMapGen = new WildernessMapGen(10, 5, 3);
            var roadMapGen = new RoadMapGen(false);
            MapGenerator mapGenerator = new MapGenerator(roadMapGen, Global.MapWidth, Global.MapHeight, 10);
            Global.ChunkMap = mapGenerator.GenerateMap();
        }

        protected override void Update(GameTime gameTime)
        {
            _inputManager.Update(gameTime);

            if (_inputManager.IsExitGame())
            {
                Exit();
            }
            else if (_inputManager.IsSpace())
            {
                Global.Debugging = !Global.Debugging;
            }

            if (Global.CommandSystem.IsPlayerTurn)
            {
                bool didPlayerMove = Global.CommandSystem.MovePlayer(_inputManager.IsMove());
                if (didPlayerMove)
                {
                    Global.Camera.CenterOn(Global.ChunkMap.GetCell(Global.Player.X, Global.Player.Y) as Cell);
                    Global.CommandSystem.EndPlayerTurn();
                }
            }
            else
            {
                Global.CommandSystem.ActivateEnemies();
            }

            Global.Camera.HandleInput(_inputManager);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Global.Camera.TranslationMatrix);

            Global.ChunkMap.Draw(_spriteBatch);

            Global.Player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
