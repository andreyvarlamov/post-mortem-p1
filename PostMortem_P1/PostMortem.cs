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

namespace PostMortem_P1
{
    public class PostMortem : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputState _inputState;

        public PostMortem()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _inputState = new InputState();
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

            MapGenerator mapGenerator = new MapGenerator(Global.MapWidth, Global.MapHeight, 20, 13, 7);
            Global.WorldCellMap = mapGenerator.CreateMap();
        }

        protected override void Update(GameTime gameTime)
        {
            _inputState.Update();

            if (_inputState.IsExitGame())
            {
                Exit();
            }
            else if (_inputState.IsSpace())
            {
                Global.Debugging = !Global.Debugging;
            }

            if (Global.CommandSystem.IsPlayerTurn)
            {
                bool didPlayerMove = Global.CommandSystem.MovePlayer(_inputState.IsMove());
                if (didPlayerMove)
                {
                    Global.Camera.CenterOn(Global.WorldCellMap.GetCell(Global.Player.X, Global.Player.Y) as Cell);
                    Global.CommandSystem.EndPlayerTurn();
                }
            }
            else
            {
                Global.CommandSystem.ActivateEnemies();
            }

            Global.Camera.HandleInput(_inputState);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Global.Camera.TranslationMatrix);

            Global.WorldCellMap.Draw(_spriteBatch);

            Global.Player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}