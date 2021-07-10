using Microsoft.Xna.Framework;

using RogueSharp;

using Rectangle = Microsoft.Xna.Framework.Rectangle;

using PostMortem_P1.Graphics;
using PostMortem_P1.Input;

namespace PostMortem_P1.Core
{
    public class Camera
    {
        public Vector2 Position { get; private set; }
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }

        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        public Vector2 ViewPortCenter
        {
            get
            {
                return new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);
            }
        }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation((int)-Position.X, (int)-Position.Y, 0) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(ViewPortCenter, 0));
            }
        }
        public Camera(int viewportWidth, int viewportHeight)
        {
            Zoom = 1.0f;

            ViewportWidth = viewportWidth;
            ViewportHeight = viewportHeight;
        }

        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.25f)
            {
                Zoom = 0.25f;
            }
        }

        public void MoveCamera(Vector2 cameraMovement, bool clampToMap = false)
        {
            Vector2 newPosition = Position + cameraMovement;

            if (clampToMap)
            {
                Position = MapClampedPosition(newPosition);
            }
            else
            {
                Position = newPosition;
            }
        }

        public Rectangle ViewportWorldBoundary()
        {
            Vector2 viewPortCorner = ScreenToWorld(new Vector2(0, 0));
            Vector2 viewPortBottomCorner = ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

            return new Rectangle((int)viewPortCorner.X,
                (int)viewPortCorner.Y,
                (int)(viewPortBottomCorner.X - viewPortCorner.X),
                (int)(viewPortBottomCorner.Y = viewPortCorner.Y));
        }

        /// <summary>
        /// Center the camera on a pixel position
        /// </summary>
        /// <param name="cell"></param>
        public void CenterOn(Vector2 position)
        {
            Position = position;
        }

        /// <summary>
        /// Center the camera on a cell
        /// </summary>
        /// <param name="cell"></param>
        public void CenterOn(Cell cell)
        {
            Position = CenteredPosition(cell, true);
        }

        /// <summary>
        /// Find the centered camera pixel position over a cell
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="clampToMap"></param>
        /// <returns></returns>
        private Vector2 CenteredPosition(Cell cell, bool clampToMap = false)
        {
            var cameraPosition = new Vector2(cell.X * SpriteManager.SpriteSize, cell.Y * SpriteManager.SpriteSize);
            var cameraCenteredOnTilePosition =
                new Vector2(cameraPosition.X + SpriteManager.SpriteSize / 2, cameraPosition.Y + SpriteManager.SpriteSize / 2);
            if (clampToMap)
            {
                return MapClampedPosition(cameraCenteredOnTilePosition);
            }

            return cameraCenteredOnTilePosition;
        }

        /// <summary>
        /// Clamp the position, so that camera doesn't go outside of viewable map
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector2 MapClampedPosition(Vector2 position)
        {
            var cameraMax = new Vector2(Global.MapWidth * SpriteManager.SpriteSize - (ViewportWidth / Zoom / 2),
                Global.MapHeight * SpriteManager.SpriteSize - (ViewportHeight / Zoom / 2));

            return Vector2.Clamp(position, new Vector2(ViewportWidth / Zoom / 2, ViewportHeight / Zoom / 2), cameraMax);
        }

        /// <summary>
        /// Convert world coordinates to screen coordinates
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        /// <summary>
        /// Convert screen coordinates to world coordinates
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
        }

        public void HandleInput(InputManager inputManager)
        {
            Vector2 cameraMovement = Vector2.Zero;

            if (inputManager.IsScrollLeft())
            {
                cameraMovement.X = -1;
            }
            else if (inputManager.IsScrollRight())
            {
                cameraMovement.X = 1;
            }

            if (inputManager.IsScrollUp())
            {
                cameraMovement.Y = -1;
            }
            else if (inputManager.IsScrollDown())
            {
                cameraMovement.Y = 1;
            }

            if (inputManager.IsZoomIn())
            {
                AdjustZoom(0.25f);
            }
            else if (inputManager.IsZoomOut())
            {
                AdjustZoom(-0.25f);
            }

            // Normalize so diagonal direction is equal speed to orthogonal
            if (cameraMovement != Vector2.Zero)
            {
                cameraMovement.Normalize();
            }

            // Scale out movement to move 25 pixels per second
            cameraMovement *= 25f;

            MoveCamera(cameraMovement, true);
        }


    }
}
