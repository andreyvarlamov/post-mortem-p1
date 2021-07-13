using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

using PostMortem_P1.Core;
using PostMortem_P1.Graphics;

namespace PostMortem_P1.Core
{
    public class LayerMap : Map<Tile>
    {
        public float _layerDepth;
        
        public LayerMap(float layerDepth) : base()
        {
            _layerDepth = layerDepth;
        }

        public void Draw(SpriteBatch spriteBatch, FieldOfView<Tile> playerFov)
        {
            foreach (Tile tile in GetAllCells())
            {
                if (!tile.IsAir)
                {
                    // If not explored yet, don't render
                    if (!tile.IsExplored && !Global.Debugging)
                    {
                        continue;
                    }

                    // If explored, but not in fov - gray tint, if in fov - no tint
                    Color tint = Color.Gray;
                    if (playerFov.IsInFov(tile.X, tile.Y) || Global.Debugging)
                    {
                        tint = Color.White;
                    }

                    var position = new Vector2(tile.X * SpriteManager.SpriteSize, tile.Y * SpriteManager.SpriteSize);

                    spriteBatch.Draw(tile.Sprite, position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, _layerDepth);
                }
            }
        }

        public void InitializeTiles(Texture2D baseSprite, bool isWalkable, bool isTransparent)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var tile = GetCell(x, y);
                    tile.SetSprite(baseSprite);
                    tile.IsWalkable = isWalkable;
                    tile.IsTransparent = isTransparent;
                }
            }
        }

        public void InitializeAsAir()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    this[x, y].SetAir(true);
                }
            }
        }
    }
}
