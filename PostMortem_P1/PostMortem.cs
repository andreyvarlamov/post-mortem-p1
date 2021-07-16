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
using PostMortem_P1.Menus;

namespace PostMortem_P1
{
    public class PostMortem : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Menu _menu;


        public PostMortem()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Global.InputManager = new InputManager();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Global.SpriteManager = new SpriteManager();
            Global.SpriteManager.LoadContent(Content);

            Camera camera = new Camera(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);

            Global.WorldMap = WorldGenerator.GenerateWorld(Global.WorldWidth, Global.WorldHeight, camera);
            Global.WorldMap.SpawnPlayerInWorld(2, 0);

            _menu = new Menu(500, 400, null, _graphics);
        }

        protected override void Update(GameTime gameTime)
        {
            Global.InputManager.Update(gameTime);

            if (Global.InputManager.IsExitGame())
            {
                Exit();
            }
            else if (Global.InputManager.IsSpace())
            {
                Global.Debugging = !Global.Debugging;
            }

            Global.WorldMap.Camera.HandleInput(Global.InputManager);

            Global.WorldMap.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Global.WorldMap.Camera.TranslationMatrix);

            Global.WorldMap.Draw(_spriteBatch);

            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            _menu.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
