using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PostMortem_P1.Core;
using System;
using System.Collections.Generic;

namespace PostMortem_P1.Menus.Overlays
{
    public class Hud : Overlay
    {
        private List<ColoredParagraph> _coloredParagraphs;

        private int _maxHealth;
        private int _health;
        private int _chunkX;
        private int _chunkY;
        private int _worldX;
        private int _worldY;
        private float _turns;
        private DateTime _dateTime;

        private List<Item> _items;

        public Hud(GraphicsDeviceManager graphics, int widthPercent, bool isRightSide) : base(graphics)
        {
            PxWidth = (int)(this.viewportWidth * (widthPercent / 100.0f));
            PxHeight = this.viewportHeight;

            if (isRightSide)
            {
                PxX = this.viewportWidth - PxWidth;
            }

            this.canvas = CreateRectangle(PxWidth, PxHeight, Color.Black, this.graphics);

            _maxHealth = 0;
            _health = 0;
            _chunkX = 0;
            _chunkY = 0;
            _worldX = 0;
            _worldY = 0;
            _turns = 0.0f;
            _dateTime = new DateTime();

            _items = new List<Item>();

            UpdateText();
        }

        public void UpdateText()
        {
            _coloredParagraphs = new List<ColoredParagraph>();

            float healthPercent = 0.0f;
            if (_maxHealth != 0)
            {
                healthPercent = _health / (float)_maxHealth;
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

            ColoredParagraph healthColPar = new ColoredString("HEALTH:", Color.White) + new ColoredString($"{_health}/{_maxHealth}", healthColor) + new ColoredString("...", Color.Red);

            ColoredParagraph spacer = new ColoredParagraph();
            ColoredParagraph chunkPosition = new ColoredString($"CHUNK: X={_chunkX} Y={_chunkY}", Color.White) + new ColoredString("", Color.White);
            ColoredParagraph worldPosition = new ColoredString($"WORLD: X={_worldX} Y={_worldY}", Color.White) + new ColoredString("", Color.White);

            ColoredParagraph turns = new ColoredString($"TURNS: {_turns.ToString("0.00")}", Color.White) + new ColoredString("", Color.White);
            ColoredParagraph dateTime = new ColoredString($"DATE: {_dateTime.ToString("G")}", Color.White) + new ColoredString("", Color.White);

            _coloredParagraphs.Add(healthColPar);
            _coloredParagraphs.Add(spacer);

            _coloredParagraphs.Add(chunkPosition);
            _coloredParagraphs.Add(worldPosition);
            _coloredParagraphs.Add(spacer);

            _coloredParagraphs.Add(turns);
            _coloredParagraphs.Add(dateTime);
            _coloredParagraphs.Add(spacer);

            _coloredParagraphs.Add(new ColoredString("PLAYER ITEMS: ", Color.Blue) + new ColoredString("", Color.White));

            for (int i = 0; i < _items.Count; i++)
            {
                _coloredParagraphs.Add(new ColoredString(_items[i].Name, Color.White) + new ColoredString("", Color.White));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(PxX, PxY);
            spriteBatch.Draw(this.canvas, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Hud);

            int horizontalOffset = 10;
            int verticalOffset = 10;
            int lineHeight = 20;

            for (int i = 0; i < _coloredParagraphs.Count; i++)
            {
                Vector2 colParPosition = new Vector2(PxX + horizontalOffset, verticalOffset + i * lineHeight);
                _coloredParagraphs[i].DrawParagraph(spriteBatch, Global.FontManager.MainFont, colParPosition, LayerDepth.Hud);
            }
        }

        public void SetMaxHealth(int maxHealth)
        {
            _maxHealth = maxHealth;
            UpdateText();
        }

        public void SetHealth(int health)
        {
            _health = health;
            UpdateText();
        }

        public void SetChunkX(int chunkX)
        {
            _chunkX = chunkX;
            UpdateText();
        }

        public void SetChunkY(int chunkY)
        {
            _chunkY = chunkY;
            UpdateText();
        }

        public void SetWorldX(int worldX)
        {
            _worldX = worldX;
            UpdateText();
        }

        public void SetWorldY(int worldY)
        {
            _worldY = worldY;
            UpdateText();
        }

        public void SetTurns(float turns)
        {
            _turns = turns;
            UpdateText();
        }

        public void SetDateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
            UpdateText();
        }

        public void SetItems(List<Item> items)
        {
            _items = items;
            UpdateText();
        }
    }
}
