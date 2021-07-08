using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;
using RogueSharp.MapCreation;
using RogueSharp.DiceNotation;

using PostMortem_P1.Core;
using PostMortem_P1.Graphics;
using PostMortem_P1.Systems;

namespace PostMortem_P1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputState _inputState;

        //private List<Enemy> _enemies = new List<Enemy>();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _inputState = new InputState();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Global.Camera.ViewportWidth = _graphics.GraphicsDevice.Viewport.Width;
            Global.Camera.ViewportHeight = _graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Global.SpriteManager = new SpriteManager();
            Global.SpriteManager.LoadContent(Content);

            Global.SchedulingSystem = new SchedulingSystem();
            Global.CommandSystem = new CommandSystem();

            MapGenerator mapGenerator = new MapGenerator(Global.MapWidth, Global.MapHeight, 20, 13, 7);
            Global.WorldCellMap = mapGenerator.CreateMap();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            _inputState.Update();

            if (_inputState.IsExitGame())
            {
                Exit();
            }
            else if (_inputState.IsSpace())
            {
                Global.Debugging = !Global.Debugging;
            }

            bool didPlayerMove = false;
            if (Global.CommandSystem.IsPlayerTurn)
            {
                didPlayerMove = Global.CommandSystem.MovePlayer(_inputState.IsMove());
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

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Global.Camera.TranslationMatrix);

            Global.WorldCellMap.Draw(_spriteBatch);

            Global.Player.Draw(_spriteBatch);

            //foreach (var enemy in _enemies)
            //{
            //    if (_map.IsInFov(enemy.X, enemy.Y) || Global.GameState == GameStates.Debugging)
            //    {
            //        enemy.Draw(_spriteBatch);
            //    }
            //}

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        //private void AddEnemies(int num)
        //{
        //    for (int i = 0; i < num; i++)
        //    {
        //        Cell enemyCell = GetRandomEmptyCell();
        //        var pathFromEnemy = new PathToPlayer(_player, _map, Content.Load<Texture2D>("White"));
        //        var enemy = new Enemy(_map, pathFromEnemy)
        //        {
        //            X = enemyCell.X,
        //            Y = enemyCell.Y,
        //            Sprite = Content.Load<Texture2D>("Hound"),
        //            ArmorClass = 10,
        //            AttackBonus = 0,
        //            Damage = Dice.Parse("d3"),
        //            Health = 10,
        //            Name = $"Enemy {i + 1}"
        //        };
        //        _enemies.Add(enemy);
        //    }
        //}

        //private Cell GetRandomEmptyCell()
        //{

        //    while (true)
        //    {
        //        int x = Global.Random.Next(49);
        //        int y = Global.Random.Next(29);
        //        if (_map.IsWalkable(x, y))
        //        {
        //            return _map.GetCell(x, y) as Cell;
        //        }
        //    }
        //}

    }
}
