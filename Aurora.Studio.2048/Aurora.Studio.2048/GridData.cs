using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Aurora.Studio._2048
{
    class GridData
    {
        private static readonly double[] trans = new double[] { 0, 121.25, 242.5, 363.75 };

        public static Point GetTransform(int X, int Y)
        {
            return new Point(trans[Y], trans[X]);
        }
    }
}
