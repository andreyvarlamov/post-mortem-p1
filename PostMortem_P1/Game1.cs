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
            MapGenerator mapGenerator = new MapGenerator(Global.MapWidth, Global.MapHeight);
            Global.WorldCellMap = mapGenerator.CreateMap();

            Global.SpriteManager = new SpriteManager();
            Global.SpriteManager.LoadContent(Content);

            Cell startingCell = Global.WorldCellMap.GetCell(10, 10) as Cell;
            Global.Player = new Player(Global.SpriteManager.Player, startingCell.X, startingCell.Y);
            Global.Camera.CenterOn(startingCell);

            //AddEnemies(10);

            //Global.CombatManager = new CombatManager(_player, _enemies);

            Global.WorldCellMap.UpdatePlayerFieldOfView();
            Global.GameState = GameStates.PlayerTurn;
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
                if (Global.GameState == GameStates.PlayerTurn)
                {
                    Global.GameState = GameStates.Debugging;
                }
                else if (Global.GameState == GameStates.Debugging)
                {
                    Global.GameState = GameStates.PlayerTurn;
                }
            }
            else
            {
                if (Global.GameState == GameStates.PlayerTurn &&
                    Global.Player.HandleInput(_inputState, Global.WorldCellMap))
                {
                    Global.WorldCellMap.UpdatePlayerFieldOfView();
                    // Center the camera on player when he moves
                    Global.Camera.CenterOn(Global.WorldCellMap.GetCell(Global.Player.X, Global.Player.Y) as Cell);

                    Global.GameState = GameStates.EnemyTurn;
                }
                if (Global.GameState == GameStates.EnemyTurn)
                {
                    //foreach(var enemy in _enemies)
                    //{
                    //    enemy.Update();
                    //}

                    Global.GameState = GameStates.PlayerTurn;
                }
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

            Global.Player.Draw(_spriteBatch, Global.WorldCellMap);

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
