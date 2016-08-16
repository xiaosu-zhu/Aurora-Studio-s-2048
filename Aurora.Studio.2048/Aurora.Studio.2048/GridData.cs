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

        public static double GetSize(int length)
        {
            return 70 - (length - 1) * 15;
        }
    }
}
