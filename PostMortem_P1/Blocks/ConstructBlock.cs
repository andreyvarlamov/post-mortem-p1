using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PostMortem_P1.Core;

namespace PostMortem_P1.Blocks
{
    public class ConstructBlock : WakeableBlock
    {
        public int TurnsTillBuilt { get; set; }
        public Block ChangeInto { get; set; }
        public bool ReadyToBeChanged { get; set; }
        public ConstructBlock(int blockID, Block changeInto, int x, int y) :
            base(blockID, GetConstructName(changeInto), GetConstructSprite(changeInto),
                true, true, true, null, null, x, y)
        {
            TurnsTillBuilt = changeInto.BuildTime.Value;
            ChangeInto = changeInto;
            ReadyToBeChanged = false;
        }

        public override long Time
        {
            get
            {
                if (!ReadyToBeChanged)
                {
                    return TurnsTillBuilt;
                }
                else
                {
                    return 0;
                }
            }
        }

        public override bool WakeUp(long ticksReloadedAt)
        {
            Debug.WriteLine($"ConstructBlock.WakeUp. {ticksReloadedAt} - {TicksUnloadedAt} ");
            if ((ticksReloadedAt - TicksUnloadedAt) >= (long) TurnsTillBuilt * 100)
            {
                PerformAction();
                return true;
            }
            else
            {
                TurnsTillBuilt -= (int)((ticksReloadedAt - TicksUnloadedAt) / 100);
                return false;
            }
        }

        public override bool WillWakeUp
        {
            get
            {
                return !ReadyToBeChanged;
            }
        }

        public override void PerformAction()
        {
            ReadyToBeChanged = true;
        }

        public static Texture2D GetConstructSprite(Block block)
        {
            Texture2D sprite = new Texture2D(Global.GraphicsDevice, block.Sprite.Width, block.Sprite.Height);

            Color[] data = new Color[sprite.Width * sprite.Height];
            block.Sprite.GetData<Color>(data);
            for (int i = 0; i < data.Length; i++)
            {
                int rDec = 50;
                int gDec = 50;
                int bInc = 50;
                int aDec = 100;

                int newR = data[i].R - rDec;
                int newG = data[i].G - gDec;
                int newB = data[i].B + bInc;
                int newA = data[i].A - aDec;

                if (newR < 0)
                {
                    data[i].R = 0;
                }
                else
                {
                    data[i].R = (byte)newR;
                }

                if (newG < 0)
                {
                    data[i].G = 0;
                }
                else
                {
                    data[i].G = (byte)newG;
                }

                if (newB > 255)
                {
                    data[i].B = 255;
                }
                else
                {
                    data[i].B = (byte)newB;
                }

                if (newA < 0)
                {
                    data[i].A = 0;
                }
                else
                {
                    data[i].A = (byte)newA;
                }
            }
            sprite.SetData<Color>(data);

            return sprite;
        }

        public static string GetConstructName(Block block)
        {
            return "Constructing " + block.Name;
        }
    }
}
