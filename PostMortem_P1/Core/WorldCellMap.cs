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

                spriteBatch.Draw(getCellSprite(cell), position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Cells);
            }
        }

        private Texture2D getCellSprite(Cell cell)
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

        public void UpdatePlayerFieldOfView()
        {
            Player player = Global.Player;
            ComputeFov(player.X, player.Y, Global.Player.Awareness, true);
            foreach (Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }
    }
}
