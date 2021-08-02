using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PostMortem_P1.Core;
using PostMortem_P1.Graphics;
using PostMortem_P1.Input;
using PostMortem_P1.Menus.MenuActions;

namespace PostMortem_P1.Menus.Overlays
{
    public class TileSelectOverlay : Overlay
    {
        private int _tileX;
        private int _tileY;

        private Tile _currentSelectedTile;

        private MenuActionGetSelectedTile _callerAction;

        public TileSelectOverlay(int originTileX, int originTileY, GraphicsDeviceManager graphics, MenuActionGetSelectedTile callerAction) : base (graphics)
        {
            _tileX = originTileX;
            _tileY = originTileY;

            _currentSelectedTile = Global.WorldMap.CurrentChunkMap[_tileX, _tileY];

            _callerAction = callerAction;
        }

        public override void DrawThroughWorldCamera(SpriteBatch spriteBatch)
        {
            var position = new Vector2(_currentSelectedTile.X * SpriteManager.SpriteSize, _currentSelectedTile.Y * SpriteManager.SpriteSize);
            spriteBatch.Draw(Global.SpriteManager.MapCursor, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Menu);
        }

        public override void ProcessFromInput(InputManager inputManager)
        {
            Direction movedDir = inputManager.IsMove();

            if (movedDir != Direction.None)
            {
                switch (movedDir)
                {
                    case Direction.SW:
                        _tileX--;
                        _tileY++;
                        break;
                    case Direction.S:
                        _tileY++;
                        break;
                    case Direction.SE:
                        _tileX++;
                        _tileY++;
                        break;
                    case Direction.W:
                        _tileX--;
                        break;
                    case Direction.E:
                        _tileX++;
                        break;
                    case Direction.NW:
                        _tileX--;
                        _tileY--;
                        break;
                    case Direction.N:
                        _tileY--;
                        break;
                    case Direction.NE:
                        _tileX++;
                        _tileY--;
                        break;
                }

                _currentSelectedTile = Global.WorldMap.CurrentChunkMap[_tileX, _tileY];
                Global.WorldMap.Camera.CenterOn(_currentSelectedTile);
            }

            if (inputManager.IsNewKeyPress(Keys.Enter))
            {
                _callerAction.SetTile(_currentSelectedTile);
            }
        }
    }
}
