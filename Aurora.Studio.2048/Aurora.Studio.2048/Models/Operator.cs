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

        public Operator()
        {
            var m = Core.Game.Operator.New(out grid);
            Tiles.Add(new TileItem(grid[m[0]][m[1]].Data, m[0], m[1]));
            Tiles.Add(new TileItem(grid[m[2]][m[3]].Data, m[2], m[3]));
        }

        public void Update(Direction direction)
        {
            Core.Game.Operator.Copy(grid, out temp);
            switch (direction)
            {
                case Direction.Up:
                    Core.Game.Operator.MoveUp(ref grid);
                    FindTile();
                    MergeUp();
                    break;
                case Direction.Down:
                    Core.Game.Operator.MoveDown(ref grid);
                    FindTile();
                    MergeDown();
                    break;
                case Direction.Left:
                    Core.Game.Operator.MoveLeft(ref grid);
                    FindTile();
                    MergeLeft();
                    break;
                case Direction.Right:
                    Core.Game.Operator.MoveRight(ref grid);
                    FindTile();
                    MergeRight();
                    break;
                default:
                    break;
            }
            Core.Game.Operator.Refresh(ref grid);
        }

        private void FindTile()
        {
            int i = 0, j = 0;
            var find = false;
            foreach (var item in Tiles)
            {
                foreach (var row in grid)
                {
                    foreach (var tile in row)
                    {
                        j++;
                        if (tile.Row == item.Row && tile.Col == item.Col)
                        {
                            item.Update(tile.Data, i, j - 1);
                            find = true;
                            break;
                        }
                    }
                    if (find)
                        break;
                    i++;
                    j = 0;
                }
                i = 0;
                j = 0;
                find = false;
            }
        }

        private void MergeRight()
        {
            var m = from g in Tiles
                    group g by g.Row into p
                    select p;
            foreach (var row in m)
            {
                foreach (var tile in row)
                {
                    if (tile.Data == 0)
                    {
                        var p = row.ToList().Find(x =>
                        {
                            return x.YOld == tile.YOld + 1;
                        });
                        tile.MergeRight(p.Row, p.Col);
                    }
                }
            }
        }

        private void MergeLeft()
        {
            throw new NotImplementedException();
        }

        private void MergeDown()
        {
            throw new NotImplementedException();
        }

        private void MergeUp()
        {
            throw new NotImplementedException();
        }
    }
}
