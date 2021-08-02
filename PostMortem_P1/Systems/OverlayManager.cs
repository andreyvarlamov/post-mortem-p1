using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PostMortem_P1.Core;
using PostMortem_P1.Input;
using PostMortem_P1.Menus;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Systems
{
    public class OverlayManager
    {
        public Overlay CurrentOverlay { get; private set; }

        private Stack<Overlay> _previousOverlays;

        private GraphicsDeviceManager _graphics;

        public OverlayManager(GraphicsDeviceManager graphics)
        {
            SetCurrentOverlayAndReset(null);

            _graphics = graphics;
        }

        public void SetCurrentOverlay(Overlay overlay)
        {
            if (CurrentOverlay != null)
            {
                _previousOverlays.Push(CurrentOverlay);
            }

            CurrentOverlay = overlay;
        }

        public void SetCurrentOverlayAndReset(Overlay overlay)
        {
            _previousOverlays = new Stack<Overlay>();
            CurrentOverlay = overlay;
        }

        public void ProcessFromInput(InputManager inputManager)
        {
            if (inputManager.IsNewKeyPress(Keys.Back))
            {
                CurrentOverlay = _previousOverlays.Pop();
                Global.WorldMap.Camera.CenterOn(Global.WorldMap.CurrentChunkMap[Global.WorldMap.Player.X, Global.WorldMap.Player.Y]);
            }

            if (inputManager.IsNewKeyPress(Keys.Escape))
            {
                ReturnToGame();
                return;
            }

            CurrentOverlay.ProcessFromInput(inputManager);
        }

        public void ReturnToGame()
        {
            Global.GameMode = GameMode.Game;
            CurrentOverlay = null;
            _previousOverlays = new Stack<Overlay>();
            Global.WorldMap.Camera.CenterOn(Global.WorldMap.CurrentChunkMap[Global.WorldMap.Player.X, Global.WorldMap.Player.Y]);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            CurrentOverlay.Draw(spriteBatch);
        }

        public void DrawThroughWorldCamera(SpriteBatch spriteBatch)
        {
            CurrentOverlay.DrawThroughWorldCamera(spriteBatch);
        }
    }
}
