using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

using PostMortem_P1.Graphics;

namespace PostMortem_P1.Core
{
    public class WorldCellMap : Map
    {
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cell cell in GetAllCells())
            {
                // If not explored yet, don't render
                if (!cell.IsExplored && Global.GameState != GameStates.Debugging)
                {
                    continue;
                }

                // If explored, but not in fov - gray tint, if in fov - no tint
                Color tint = Color.Gray;
                if (cell.IsInFov || Global.GameState == GameStates.Debugging)
                {
                    tint = Color.White; 
                }

                var position = new Vector2(cell.X * SpriteManager.SpriteSize, cell.Y * SpriteManager.SpriteSize);

                spriteBatch.Draw(GetCellSprite(cell), position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Cells);
            }
        }

        public Texture2D GetCellSprite(Cell cell)
        {
            if (cell.IsWalkable)
            {
                return Global.SpriteManager.Floor;
            }
            else
            {
                return Global.SpriteManager.Wall;
            }
        }
    }
}
