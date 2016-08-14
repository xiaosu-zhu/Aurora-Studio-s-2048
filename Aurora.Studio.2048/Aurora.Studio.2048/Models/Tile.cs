using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Studio._2048.Core.Game;
using Com.Aurora.Shared.MVVM;
using Windows.UI.Xaml.Media;

namespace Aurora.Studio._2048.Models
{
    class TileItem : ViewModelBase
    {
        public uint Data { get; set; }
        public SolidColorBrush Color { get; set; }

        public int XOffset { get; set; }
        public int YOffset { get; set; }

        public int Col { get; set; }
        public int Row { get; set; }
        public bool Changed { get; private set; }

        public void Update(uint data, int X, int Y)
        {
            if (data != Data)
            {
                Changed = true;
            }
            Data = data;
            Color = new SolidColorBrush(Palette.GetColor(data));
            XOffset = X - Col;
            YOffset = Y - Row;
            Row = Y;
            Col = X;
        }

    }
}
