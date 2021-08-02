using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PostMortem_P1.Graphics;

namespace PostMortem_P1.Core
{
    public class ItemPickup : Block
    {
        public Inventory Inventory { get; set; }

        private RenderTarget2D _canvas;

        public override Texture2D Sprite
        {
            get
            {
                return _canvas;
            }
            set
            {
            }
        }

        private bool _toUpdateCanvas;

        public ItemPickup(int blockID, bool isAir, bool isWalkable, bool isTransparent) : base(blockID, "Item Pickup", null, isAir, isWalkable, isTransparent, null)
        {
            Inventory = new Inventory();

            _canvas = new RenderTarget2D(Global.GraphicsDevice, SpriteManager.SpriteSize, SpriteManager.SpriteSize);

            _toUpdateCanvas = true;
        }

        public void UpdateCanvas(SpriteBatch spriteBatch)
        {
            if (_toUpdateCanvas)
            {
                spriteBatch.End();

                Global.GraphicsDevice.SetRenderTarget(_canvas);
                Global.GraphicsDevice.Clear(Color.Transparent);

                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

                spriteBatch.Draw(Inventory.Items[0].Sprite, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(Global.SpriteManager.ItemPickup, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
                if (Inventory.Items.Count > 1)
                {
                    spriteBatch.Draw(Global.SpriteManager.ItemPickupMultiple, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0f);
                }

                _toUpdateCanvas = false;

                spriteBatch.End();

                Global.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.Begin();
            }
        }

        public void AddItem(Item item)
        {
            Inventory.Items.Add(item);
            _toUpdateCanvas = true;
        }

        /// <summary>
        /// Will return true/false depending if the last item in the stack was removed
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool RemoveItem(Item item)
        {
            Inventory.Items.Remove(item);
            _toUpdateCanvas = true;
            return Inventory.Items.Count == 0;
        }
   
    }
}
