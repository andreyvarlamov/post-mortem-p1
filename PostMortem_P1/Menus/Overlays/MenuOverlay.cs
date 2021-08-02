using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PostMortem_P1.Core;
using PostMortem_P1.Input;
using PostMortem_P1.Menus.MenuActions;

namespace PostMortem_P1.Menus.Overlays
{
    public class MenuOverlay : Overlay
    {
        public List<MenuItem> MenuItems { get; set; }

        private int _selection;

        private bool _isActionable;

        private MenuActionGet _callerAction;

        public int Selection
        {
            get
            {
                return _selection;
            }
            set
            {
                if (value <= -1)
                {
                    _selection = MenuItems.Count - 1;
                }
                else if (value >= MenuItems.Count)
                {
                    _selection = 0;
                }
                else
                {
                    _selection = value;
                }
            }
        }

        public MenuOverlay(int pxWidth, int pxHeight, List<MenuItem> menuItems, bool isActionable, MenuActionGet callerAction, GraphicsDeviceManager graphics) : base(graphics)
        {
            PxWidth = pxWidth;
            PxHeight = pxHeight;

            PxX = this.viewportWidth / 2 - PxWidth / 2;
            PxY = this.viewportHeight / 2 - PxHeight / 2;

            this.canvas = CreateStaticCanvas();

            MenuItems = menuItems;

            //_selection = 0;
            Selection = 0;

            _isActionable = isActionable;
            _callerAction = callerAction;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawDecoration(spriteBatch);
            DrawContent(spriteBatch);
        }

        private void DrawContent(SpriteBatch spriteBatch)
        {
            int horizontalOffset = 25;
            int verticalOffset = 25;
            int lineHeight = 20;

            if (MenuItems.Count == 0)
            {
                // Display empty menu message
                string msg = "Empty menu";
                Vector2 msgSize = Global.FontManager.MainFont.MeasureString(msg);
                int msgWidth = (int)msgSize.X;
                int msgHeight = (int)msgSize.Y;

                Vector2 msgPosition = new Vector2(PxX + horizontalOffset, PxY + verticalOffset - msgHeight / 2);
                Vector2 msgCursorPosition = new Vector2(PxX + horizontalOffset - 3, PxY + verticalOffset - msgHeight / 2 - 1);

                var selectionRect = CreateRectangle(msgWidth + 3, msgHeight + 1, Color.Salmon, this.graphics);

                spriteBatch.Draw(selectionRect, msgCursorPosition, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.MenuSelect);
                spriteBatch.DrawString(Global.FontManager.MainFont, msg, msgPosition, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.MenuText);
            }

            for (int i = 0; i < MenuItems.Count; i++)
            {
                Vector2 textSize = Global.FontManager.MainFont.MeasureString(MenuItems[i].Text);
                int textWidth = (int) textSize.X;
                int textHeight = (int) textSize.Y;

                Vector2 textPosition = new Vector2(PxX + horizontalOffset, PxY + verticalOffset + lineHeight * i - textHeight / 2);
                if (i == Selection)
                {

                    Vector2 cursorPosition = new Vector2(PxX + horizontalOffset - 3, PxY + verticalOffset + lineHeight * i - textHeight / 2 - 1);

                    var selectionRect = CreateRectangle(textWidth + 3, textHeight + 1, Color.White, this.graphics);

                    spriteBatch.Draw(selectionRect, cursorPosition, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.MenuSelect);
                    spriteBatch.DrawString(Global.FontManager.MainFont, MenuItems[i].Text, textPosition, Color.Black, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.MenuText);
                }
                else
                {
                    spriteBatch.DrawString(Global.FontManager.MainFont, MenuItems[i].Text, textPosition, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.MenuText);
                }
            }

        }

        private void DrawDecoration(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(PxX, PxY);
            //spriteBatch.Draw(_canvas, position, Color.White);
            spriteBatch.Draw(this.canvas, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Menu);
        }

        public override void ProcessFromInput(InputManager inputManager)
        {
            switch (inputManager.IsMove())
            {
                case Direction.S:
                    Selection++;
                    break;
                case Direction.N:
                    Selection--;
                    break;
            }

            if (inputManager.IsNewKeyPress(Keys.Enter) && MenuItems.Count != 0)
            {
                if (_isActionable)
                {
                    MenuItems[Selection].MenuAction.Do();
                }
                else
                {
                    _callerAction.SetSelectedIndex(Selection);
                }
            }
        }
    }
}
