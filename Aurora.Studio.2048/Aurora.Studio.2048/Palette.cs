using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace Aurora.Studio._2048
{
    public static class Palette
    {
        private static readonly Color[] colors = new Color[] { Color.FromArgb(255, 0xee, 0xe4, 0xda), Color.FromArgb(255, 0xed, 0xe0, 0xc8), Color.FromArgb(255, 0xf2, 0xb1, 0x79), Color.FromArgb(255, 0xf5, 0x95, 0x63), Color.FromArgb(255, 0xf6, 0x7c, 0x5f), Color.FromArgb(255, 0xf6, 0x5e, 0x3b), Color.FromArgb(255, 0xed, 0xcf, 0x72), Color.FromArgb(255, 0xed, 0xcc, 0x61), Color.FromArgb(255, 0xed, 0xc8, 0x50), Color.FromArgb(255, 0xed, 0xc5, 0x3f), Color.FromArgb(255, 0xed, 0xc2, 0x2e), Color.FromArgb(255, 0x3c, 0x3a, 0x32), };
        private static readonly Color[] colorsDark = new Color[] { Color.FromArgb(255, 0x11, 0x1b, 0x25), Color.FromArgb(255, 0x12, 0x1f, 0x37), Color.FromArgb(255, 0x0d, 0x4e, 0x86), Color.FromArgb(255, 0x0a, 0x6a, 0x9c), Color.FromArgb(255, 0x09, 0x83, 0xa0), Color.FromArgb(255, 0x09, 0xa1, 0xc4), Color.FromArgb(255, 0x12, 0x30, 0x8d), Color.FromArgb(255, 0x12, 0x33, 0x9e), Color.FromArgb(255, 0x12, 0x37, 0xaf), Color.FromArgb(255, 0x12, 0x3a, 0xc0), Color.FromArgb(255, 0x12, 0x3d, 0xd1), Color.FromArgb(255, 0xc3, 0xc5, 0xcd), };


        public static Color GetColor(uint data, ElementTheme theme)
        {
            if (theme == ElementTheme.Dark)
            {
                switch (data)
                {
                    case 2: return colorsDark[0];
                    case 4: return colorsDark[1];
                    case 8: return colorsDark[2];
                    case 16: return colorsDark[3];
                    case 32: return colorsDark[4];
                    case 64: return colorsDark[5];
                    case 128: return colorsDark[6];
                    case 256: return colorsDark[7];
                    case 512: return colorsDark[8];
                    case 1024: return colorsDark[9];
                    case 2048: return colorsDark[10];
                    default: return colorsDark[11];
                }
            }
            switch (data)
            {
                case 2: return colors[0];
                case 4: return colors[1];
                case 8: return colors[2];
                case 16: return colors[3];
                case 32: return colors[4];
                case 64: return colors[5];
                case 128: return colors[6];
                case 256: return colors[7];
                case 512: return colors[8];
                case 1024: return colors[9];
                case 2048: return colors[10];
                default: return colors[11];
            }
        }

    }
}
