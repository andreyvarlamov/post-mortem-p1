using Microsoft.Xna.Framework.Input;

using PostMortem_P1.Core;

namespace PostMortem_P1
{
    public class InputState
    {
        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState LastKeyboardState { get; private set; }
        public MouseState CurrentMouseState { get; private set; }
        public MouseState LastMouseState { get; private set; }

        public InputState()
        {
            CurrentKeyboardState = new KeyboardState();
            CurrentMouseState = new MouseState();
            LastMouseState = new MouseState();
        }

        public void Update()
        {
            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
            LastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
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
        #endregion

        #region Player Movement
        public bool IsDirN()
        {
            return IsNewKeyPress(Keys.K);
        }
        public bool IsDirS()
        {
            return IsNewKeyPress(Keys.J);
        }
        public bool IsDirW()
        {
            return IsNewKeyPress(Keys.H);
        }
        public bool IsDirE()
        {
            return IsNewKeyPress(Keys.L);
        }
        public bool IsDirNW()
        {
            return IsNewKeyPress(Keys.Y);
        }
        public bool IsDirNE()
        {
            return IsNewKeyPress(Keys.U);
        }
        public bool IsDirSW()
        {
            return IsNewKeyPress(Keys.N);
        }
        public bool IsDirSE()
        {
            return IsNewKeyPress(Keys.M);
        }

        public eDirection IsMove()
        {
            if (IsDirSW())
            {
                return eDirection.SW;
            }
            else if (IsDirS())
            {
                return eDirection.S;
            }
            else if (IsDirSE())
            {
                return eDirection.SE;
            }
            else if (IsDirW())
            {
                return eDirection.W;
            }
            else if (IsSkip())
            {
                return eDirection.Center;
            }
            else if (IsDirE())
            {
                return eDirection.E;
            }
            else if (IsDirNW())
            {
                return eDirection.NW;
            }
            else if (IsDirN())
            {
                return eDirection.N;
            }
            else if (IsDirNE())
            {
                return eDirection.NE;
            }
            else
            {
                return eDirection.None;
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
            return IsKeyPressed(Keys.Z);
        }
        public bool IsZoomIn()
        {
            return IsKeyPressed(Keys.C);
        }
        #endregion

        #region Util
        public bool IsSkip()
        {
            return IsNewKeyPress(Keys.OemPeriod);
        }

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