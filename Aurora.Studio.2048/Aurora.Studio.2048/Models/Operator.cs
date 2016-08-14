using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Studio._2048.Core.Game;

namespace Aurora.Studio._2048.Models
{
    class Operator
    {
        private Tile[][] grid;
        private Tile[][] temp;

        public List<TileItem> Tiles = new List<TileItem>();

        public void Update(Direction direction)
        {
            Core.Game.Operator.Copy(grid, out temp);
            switch (direction)
            {
                case Direction.Up:
                    Core.Game.Operator.MoveUp(ref grid);
                    CheckUp();
                    break;
                case Direction.Down:
                    Core.Game.Operator.MoveDown(ref grid);
                    CheckDown();
                    break;
                case Direction.Left:
                    Core.Game.Operator.MoveLeft(ref grid);
                    CheckLeft();
                    break;
                case Direction.Right:
                    Core.Game.Operator.MoveRight(ref grid);
                    CheckRight();
                    break;
                default:
                    break;
            }

        }

        private void CheckRight()
        {
            int i = 0, j = 0;
            foreach (var row in grid)
            {
                foreach (var item in row)
                {
                    if (item.Data != 0)
                    {
                        var p = Tiles.Find(x =>
                        {
                            return x.Row == item.Row && x.Col == item.Col;
                        });
                        p.Update(item.data, i, j);
                    }
                    i++;
                }
                i = 0;
                j++;
            }
        }

        private void CheckLeft()
        {
            throw new NotImplementedException();
        }

        private void CheckDown()
        {
            throw new NotImplementedException();
        }

        private void CheckUp()
        {
            throw new NotImplementedException();
        }
    }
}
