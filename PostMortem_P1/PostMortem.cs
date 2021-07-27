﻿using System.Collections.Generic;

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
using PostMortem_P1.Menus.Overlays;
using PostMortem_P1.Menus.MenuActions;

namespace PostMortem_P1
{
    public class PostMortem : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public PostMortem()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Global.InputManager = new InputManager();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Global.GraphicsDevice = _graphics.GraphicsDevice;

            Global.SpriteManager = new SpriteManager();
            Global.SpriteManager.LoadContent(Content);

            Global.FontManager = new FontManager();
            Global.FontManager.LoadContent(Content);

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            Global.OverlayManager = new OverlayManager(_graphics);
            Global.Hud = new Hud(_graphics, 20, true);

            Camera camera = new Camera(_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height, Global.Hud);

            Global.WorldMap = WorldGenerator.GenerateWorld(Global.WorldWidth, Global.WorldHeight, camera);
            Global.WorldMap.SpawnPlayerInWorld(2, 0);
        }

        private void ToggleGameMode()
        {
            if (Global.GameMode == GameMode.Game)
            {
                Global.GameMode = GameMode.Overlay;

                List<MenuItem> menuItems = new List<MenuItem>();

                MenuActionSetBlock getSelectedTileSetBlock = new MenuActionSetBlock(_graphics);
                MenuActionGetSelectedTile setBlock = new MenuActionGetSelectedTile(_graphics, getSelectedTileSetBlock);
                menuItems.Add(new MenuItem("Set Block", setBlock));

                MenuActionRemoveBlock getSelectedTileRemoveBlock = new MenuActionRemoveBlock(_graphics);
                MenuActionGetSelectedTile removeBlock = new MenuActionGetSelectedTile(_graphics, getSelectedTileRemoveBlock);
                menuItems.Add(new MenuItem("Remove Block", removeBlock));

                MenuActionReplaceFloor getSelectedTileReplaceFloor = new MenuActionReplaceFloor(_graphics);
                MenuActionGetSelectedTile replaceFloor = new MenuActionGetSelectedTile(_graphics, getSelectedTileReplaceFloor);
                menuItems.Add(new MenuItem("Replace Floor", replaceFloor));

                MenuOverlay menu = new MenuOverlay(300, 400, menuItems, _graphics);
                Global.OverlayManager.SetCurrentOverlayAndReset(menu);
            }
            else
            {
                Global.GameMode = GameMode.Game;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // Updated at an fps
            Global.InputManager.Update(gameTime);

            if (Global.InputManager.IsExitGame())
            {
                Exit();
            }
            else if (Global.InputManager.IsSpace())
            {
                ToggleGameMode();
            }

            if (Global.GameMode == GameMode.Game)
            {
                // Update at an fps
                Global.WorldMap.Camera.HandleInput(Global.InputManager);
                Global.WorldMap.Update();
            }
            else if (Global.GameMode == GameMode.Overlay)
            {
                Global.OverlayManager.ProcessFromInput(Global.InputManager);
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Global.WorldMap.Camera.TranslationMatrix);

            Global.WorldMap.Draw(_spriteBatch);

            if (Global.GameMode == GameMode.Overlay)
            {
                Global.OverlayManager.DrawThroughWorldCamera(_spriteBatch);
            }

            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            Global.Hud.Draw(_spriteBatch);
            if (Global.GameMode == GameMode.Overlay)
            {
                Global.OverlayManager.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
