using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using PostMortem_P1.Core;
using PostMortem_P1.Input;
using PostMortem_P1.Models;

namespace PostMortem_P1.Menus.Overlays
{
    public class MenuInspectTile : Overlay
    {
        private List<ColoredParagraph> _coloredParagraphs;

        private int _viewCursor;

        private int _horizontalOffset;
        private int _verticalOffset;
        private int _lineHeight;
        private int _maxLines;

        private bool _canScroll;

        public InspectTileModel InspectTileModel { get; set; }

        public MenuInspectTile(int pxWidth, int pxHeight, InspectTileModel inspectTileModel, GraphicsDeviceManager graphics) : base(graphics)
        {
            PxWidth = pxWidth;
            PxHeight = pxHeight;

            PxX = this.viewportWidth / 2 - PxWidth / 2;
            PxY = this.viewportHeight / 2 - PxHeight / 2;

            this.canvas = CreateStaticCanvas();

            InspectTileModel = inspectTileModel;

            _viewCursor = 0;

            _horizontalOffset = 25;
            _verticalOffset = 25;
            _lineHeight = 20;
            _maxLines = (this.canvas.Height - _verticalOffset * 2) / _lineHeight;

            UpdateText();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawDecoration(spriteBatch);
            DrawContent(spriteBatch);
        }

        public void UpdateText()
        {
            _coloredParagraphs = new List<ColoredParagraph>();

            ColoredParagraph title = new ColoredString("INSPECTING TILE", Color.White) + new ColoredString($"", Color.White);
            ColoredParagraph spacer = new ColoredParagraph();

            _coloredParagraphs.Add(title);
            _coloredParagraphs.Add(spacer);


            if (InspectTileModel.Tile != null)
            {
                ColoredParagraph tile = new ColoredString("TILE:", Color.Lavender) + new ColoredString($"", Color.White);
                List<ColoredString> modifierStrings = new List<ColoredString>();
                if (InspectTileModel.Tile.IsAir)
                {
                    modifierStrings.Add(new ColoredString("A", Color.BlueViolet));
                }
                if (InspectTileModel.Tile.IsWalkable)
                {
                    modifierStrings.Add(new ColoredString("W", Color.GreenYellow));
                }
                if (InspectTileModel.Tile.IsTransparent)
                {
                    modifierStrings.Add(new ColoredString("T", Color.White));
                }
                if (InspectTileModel.Tile.IsExplored)
                {
                    modifierStrings.Add(new ColoredString("E", Color.Orange));
                }
                ColoredParagraph modifiers = new ColoredParagraph(modifierStrings);

                _coloredParagraphs.Add(tile);
                _coloredParagraphs.Add(modifiers);

                if (InspectTileModel.Tile.Floor != null)
                {
                    ColoredParagraph floor = new ColoredString("FLOOR:", Color.White) + new ColoredString("", Color.White);
                    ColoredParagraph floorID = new ColoredString($"ID: {InspectTileModel.Tile.Floor.ID}", Color.White) + new ColoredString("", Color.White);
                    ColoredParagraph floorName = new ColoredString($"Name: {InspectTileModel.Tile.Floor.Name}", Color.White) + new ColoredString("", Color.White);

                    _coloredParagraphs.Add(floor);
                    _coloredParagraphs.Add(floorID);
                    _coloredParagraphs.Add(floorName);
                }

                if (InspectTileModel.Tile.Block != null)
                {
                    ColoredParagraph block = new ColoredString("BLOCK:", Color.White) + new ColoredString("", Color.White);
                    ColoredParagraph blockID = new ColoredString($"ID: {InspectTileModel.Tile.Block.ID}", Color.White) + new ColoredString("", Color.White);
                    ColoredParagraph blockName = new ColoredString($"Name: {InspectTileModel.Tile.Block.Name}", Color.White) + new ColoredString("", Color.White);
                    List<ColoredString> blockModifierStrings = new List<ColoredString>();
                    if (InspectTileModel.Tile.Block.IsAir)
                    {
                        blockModifierStrings.Add(new ColoredString("A", Color.BlueViolet));
                    }
                    if (InspectTileModel.Tile.Block.IsWalkable)
                    {
                        blockModifierStrings.Add(new ColoredString("W", Color.GreenYellow));
                    }
                    if (InspectTileModel.Tile.IsTransparent)
                    {
                        blockModifierStrings.Add(new ColoredString("T", Color.White));
                    }
                    ColoredParagraph blockModifiers = new ColoredParagraph(blockModifierStrings);


                    _coloredParagraphs.Add(block);
                    _coloredParagraphs.Add(blockID);
                    _coloredParagraphs.Add(blockName);
                    _coloredParagraphs.Add(blockModifiers);
                }

                if (InspectTileModel.Tile.ItemPickup != null)
                {
                    ColoredParagraph itemPickup = new ColoredString("ITEM PICKUP:", Color.White) + new ColoredString("", Color.White);
                    ColoredParagraph pickupInventory = new ColoredString("INVENTORY:", Color.White) + new ColoredString("", Color.White);

                    _coloredParagraphs.Add(itemPickup);
                    _coloredParagraphs.Add(pickupInventory);

                    var items = InspectTileModel.Tile.ItemPickup.Inventory.Items;

                    foreach (string item in items)
                    {
                        ColoredParagraph itemParagraph = new ColoredString($"{item}", Color.White) + new ColoredString("", Color.White);

                        _coloredParagraphs.Add(itemParagraph);
                    }
                }
            }

            if (InspectTileModel.Actor != null)
            {
                _coloredParagraphs.Add(spacer);

                ColoredParagraph actor = new ColoredString("ACTOR:", Color.Lavender) + new ColoredString("", Color.White);
                ColoredParagraph position = new ColoredString($"POS: X={InspectTileModel.Actor.X} Y={InspectTileModel.Actor.Y}", Color.White) + new ColoredString("", Color.White);
                ColoredParagraph awareness = new ColoredString($"AWARENESS: {InspectTileModel.Actor.Awareness}", Color.White) + new ColoredString("", Color.White);
                ColoredParagraph name = new ColoredString($"NAME: {InspectTileModel.Actor.Name}", Color.White) + new ColoredString("", Color.White);
                ColoredParagraph speed = new ColoredString($"SPEED: {InspectTileModel.Actor.Speed}", Color.White) + new ColoredString("", Color.White);
                ColoredParagraph armorClass = new ColoredString($"ARMOR CLASS: {InspectTileModel.Actor.ArmorClass}", Color.White) + new ColoredString("", Color.White);
                ColoredParagraph attackBonus = new ColoredString($"ARMOR CLASS: {InspectTileModel.Actor.ArmorClass}", Color.White) + new ColoredString("", Color.White);
                ColoredParagraph damage = new ColoredString($"DAMAGE: {InspectTileModel.Actor.Damage}", Color.White) + new ColoredString("", Color.White);

                float healthPercent = 0.0f;

                int health = InspectTileModel.Actor.Health;
                int maxHealth = InspectTileModel.Actor.MaxHealth;
                if (maxHealth != 0)
                {
                    healthPercent = (float)health / maxHealth;
                }

                Color healthColor;
                if (healthPercent > 0.9f)
                {
                    healthColor = Color.Green;
                }
                else if (healthPercent  > 0.25f && healthPercent <= 0.9f)
                {
                    healthColor = Color.Yellow;
                }
                else
                {
                    healthColor = Color.Red;
                }
                ColoredParagraph hp = new ColoredString("HP:", Color.White) + new ColoredString($"{InspectTileModel.Actor.Health}/{InspectTileModel.Actor.MaxHealth}", healthColor);

                _coloredParagraphs.Add(actor);
                _coloredParagraphs.Add(position);
                _coloredParagraphs.Add(awareness);
                _coloredParagraphs.Add(name);
                _coloredParagraphs.Add(speed);
                _coloredParagraphs.Add(armorClass);
                _coloredParagraphs.Add(attackBonus);
                _coloredParagraphs.Add(damage);
                _coloredParagraphs.Add(hp);

                int? disposition = InspectTileModel.Actor.Disposition;

                if (disposition.HasValue)
                {
                    Color dispositionColor = Color.GreenYellow;

                    if (disposition.Value < 0)
                    {
                        dispositionColor = Color.Red;
                    }

                    ColoredParagraph dispositionPar = new ColoredString($"DISPOTION:", Color.White) + new ColoredString($"{disposition.Value}", dispositionColor);

                    _coloredParagraphs.Add(dispositionPar);
                }

                ColoredParagraph actorInventory = new ColoredString("INVENTORY:", Color.White) + new ColoredString("", Color.White);

                _coloredParagraphs.Add(actorInventory);

                var items = InspectTileModel.Actor.Inventory.Items;

                foreach (string item in items)
                {
                    ColoredParagraph itemParagraph = new ColoredString($"{item}", Color.White) + new ColoredString("", Color.White);

                    _coloredParagraphs.Add(itemParagraph);
                }
            }

            if (_coloredParagraphs.Count > _maxLines)
            {
                _canScroll = true;
            }
        }

        private void DrawContent(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _maxLines; i++)
            {
                Vector2 colParPosition = new Vector2(PxX + _horizontalOffset, PxY + _verticalOffset + i * _lineHeight);
                _coloredParagraphs[i + _viewCursor].DrawParagraph(spriteBatch, Global.FontManager.MainFont, colParPosition, LayerDepth.MenuText);
            }
        }

        private void DrawDecoration(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(PxX, PxY);
            spriteBatch.Draw(this.canvas, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Menu); ;
        }
        public override void ProcessFromInput(InputManager inputManager)
        {
            if (_canScroll)
            {
                switch (inputManager.IsMove())
                {
                    case Direction.S:
                        _viewCursor++;
                        break;
                    case Direction.N:
                        _viewCursor--;
                        break;
                }

                if (_viewCursor > _coloredParagraphs.Count - _maxLines)
                {
                    _viewCursor = _coloredParagraphs.Count - _maxLines;
                }
                else if (_viewCursor < 0)
                {
                    _viewCursor = 0;
                }
            }
        }
    }
}
