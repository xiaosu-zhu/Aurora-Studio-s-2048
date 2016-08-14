using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Studio._2048.Core.Game
{
    public struct Tile : IEquatable<Tile>
    {
        public uint Data;
        public int Row;
        public int Col;

        public bool Equals(Tile other)
        {
            return (Data == other.Data && Row == other.Row && Col == other.Col);
        }

        public override string ToString()
        {
            return "Data = " + Data + ", Row = " + Row + ", Col = " + Col;
        }
    }
}
