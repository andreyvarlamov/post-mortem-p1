using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using PostMortem_P1.Core;

namespace PostMortem_P1.Input
{
    public class InputManager
    {
        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState LastKeyboardState { get; private set; }
        public MouseState CurrentMouseState { get; private set; }
        public MouseState LastMouseState { get; private set; }

        private TimeSpan _totalGameTime;

        private TimeSpan _repeatStart;

        public InputManager()
        {
            CurrentKeyboardState = new KeyboardState();
            CurrentMouseState = new MouseState();
            LastMouseState = new MouseState();
        }

        public void Update(GameTime gameTime)
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            LastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            _totalGameTime = gameTime.TotalGameTime;
        }


        #region Mouse Helpers
        public bool IsNewLeftMouseClick(out MouseState mouseState)
        {
            mouseState = CurrentMouseState;
            return (CurrentMouseState.LeftButton == ButtonState.Released && LastMouseState.LeftButton == ButtonState.Pressed);
        }
        public bool IsNewRightMouseClick(out MouseState mouseState)
        {
            mouseState = CurrentMouseState;
            return (CurrentMouseState.RightButton == ButtonState.Released && LastMouseState.RightButton == ButtonState.Pressed);
        }
        public bool IsNewThirdMouseClick(out MouseState mouseState)
        {
            mouseState = CurrentMouseState;
            return (CurrentMouseState.MiddleButton == ButtonState.Pressed && LastMouseState.MiddleButton == ButtonState.Released);
        }
        public bool IsNewMouseScrollUp(out MouseState mouseState)
        {
            mouseState = CurrentMouseState;
            return (CurrentMouseState.ScrollWheelValue > LastMouseState.ScrollWheelValue);
        }
        public bool IsNewMouseScrollDown(out MouseState mouseState)
        {
            mouseState = CurrentMouseState;
            return (CurrentMouseState.ScrollWheelValue < LastMouseState.ScrollWheelValue);
        }
        #endregion

        #region Keyboard Helpers
        public bool IsNewKeyPress(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key) && LastKeyboardState.IsKeyUp(key));
        }
        public bool IsKeyPressed(Keys key)
        {
            return (CurrentKeyboardState.IsKeyDown(key));
        }
        public bool IsDownDelay(Keys key)
        {
            if (CurrentKeyboardState.IsKeyDown(key))
            {
                if (LastKeyboardState.IsKeyUp(key))
                {
                    _repeatStart = _totalGameTime;
                    return true;
                }
                else
                {
                    TimeSpan sinceFirstPressed = _totalGameTime - _repeatStart;
                    if (sinceFirstPressed > TimeSpan.FromMilliseconds(400) && sinceFirstPressed.Milliseconds % 50 == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Player Movement
        public Direction IsMove()
        {
            if (IsDownDelay(Keys.N))
            {
                return Direction.SW;
            }
            else if (IsDownDelay(Keys.J))
            {
                return Direction.S;
            }
            else if (IsDownDelay(Keys.M))
            {
                return Direction.SE;
            }
            else if (IsDownDelay(Keys.H))
            {
                return Direction.W;
            }
            else if (IsDownDelay(Keys.OemPeriod))
            {
                return Direction.Center;
            }
            else if (IsDownDelay(Keys.L))
            {
                return Direction.E;
            }
            else if (IsDownDelay(Keys.Y))
            {
                return Direction.NW;
            }
            else if (IsDownDelay(Keys.K))
            {
                return Direction.N;
            }
            else if (IsDownDelay(Keys.U))
            {
                return Direction.NE;
            }
            else
            {
                return Direction.None;
            }
        }
        #endregion

        #region Camera Control
        public bool IsScrollLeft()
        {
            return IsKeyPressed(Keys.A);
        }
        public bool IsScrollRight()
        {
            return IsKeyPressed(Keys.D);
        }
        public bool IsScrollUp()
        {
            return IsKeyPressed(Keys.W);
        }
        public bool IsScrollDown()
        {
            return IsKeyPressed(Keys.S);
        }
        public bool IsZoomOut()
        {
            return IsNewKeyPress(Keys.Z);
        }
        public bool IsZoomIn()
        {
            return IsNewKeyPress(Keys.C);
        }
        #endregion

        #region Util
        public bool IsSpace()
        {
            return IsNewKeyPress(Keys.Space);
        }
        public bool IsExitGame()
        {
            return IsNewKeyPress(Keys.Escape);
        }
        #endregion
    }
}