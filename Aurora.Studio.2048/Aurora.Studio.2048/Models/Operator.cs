using System;
using System.Collections.Generic;
using System.Linq;
using Com.Aurora.Shared.Helpers;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Aurora.Studio._2048.Models
{
    class Operator
    {
        public event EventHandler GameOverEvent;
        public event EventHandler ScoreAdd;
        public event EventHandler GameEndEvent;

        public List<TileItem> Tiles = new List<TileItem>();

        public uint Score { get; set; } = 0;
        public bool Endless { get; internal set; }

        public Operator(uint[] data, uint score, ElementTheme theme, bool ignoreEnd)
        {
            int i = 0;
            Score = score;
            Endless = ignoreEnd;
            foreach (var item in data)
            {
                if (item != 0u)
                {
                    Tiles.Add(new TileItem(item, i / 4, i % 4, theme));
                }
                i++;
            }
            if (Tiles.Count == 0)
            {
                Tiles.Clear();
                var a = Tools.Random.Next(4);
                var b = Tools.Random.Next(4);
                Tiles.Add(new TileItem(Tools.RandomBool(95) ? 2u : 4u, a, b, theme));
                int c, d;
                do
                {
                    c = Tools.Random.Next(4);
                    d = Tools.Random.Next(4);
                } while (c == a && d == b);
                Tiles.Add(new TileItem(Tools.RandomBool(95) ? 2u : 4u, c, d, theme));
                Score = 0;
            }
        }

        internal void Place(Grid ground)
        {
            ground.Children.Clear();
            foreach (var item in Tiles)
            {
                ground.Children.Add(item.Rect);
                item.Pop.Begin();
            }
        }

        internal bool IsNewGame()
        {
            return Tiles.Count == 2;
        }

        public bool Update(Direction direction, ElementTheme theme)
        {
            switch (direction)
            {
                case Direction.Up:
                    return MoveUp(theme);
                case Direction.Down:
                    return MoveDown(theme);
                case Direction.Left:
                    return MoveLeft(theme);
                case Direction.Right:
                    return MoveRight(theme);
                default: return false;
            }
        }

        internal void Clear(Grid ground)
        {
            ground.Children.Clear();
        }

        internal static Direction? GetDirection(VirtualKey virtualKey)
        {
            if (virtualKey == VirtualKey.Up || virtualKey == VirtualKey.W)
            {
                return Direction.Up;
            }
            if (virtualKey == VirtualKey.Down || virtualKey == VirtualKey.S)
            {
                return Direction.Down;
            }
            if (virtualKey == VirtualKey.Left || virtualKey == VirtualKey.A)
            {
                return Direction.Left;
            }
            if (virtualKey == VirtualKey.Right || virtualKey == VirtualKey.D)
            {
                return Direction.Right;
            }
            return null;
        }

        internal void Play(Grid ground, bool p, ElementTheme theme)
        {
            ground.Children.Clear();
            foreach (var item in Tiles)
            {
                ground.Children.Add(item.Rect);
                item.Ani.Begin();
                if (item.IsMerged)
                {
                    item.Pop.Begin();
                }
            }
            Refresh(ground, p, theme);
        }

        private void Refresh(Grid ground, bool b, ElementTheme theme)
        {
            for (int i = Tiles.Count - 1; i > -1; i--)
            {
                if (Tiles[i].Data == 0)
                {
                    Tiles.Remove(Tiles[i]);
                    continue;
                }
                if (Tiles[i].Data == 2048 && !Endless)
                {
                    OnGameEnd();
                }
                Tiles[i].Refresh();
            }
            if (Tiles.Count == 16)
            {
                var p = new Tile[][] { new Tile[] { new Tile { Data = 0, Row = 0, Col = 0 }, new Tile { Data = 0, Row = 0, Col = 1 }, new Tile { Data = 0, Row = 0, Col = 2 }, new Tile { Data = 0, Row = 0, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 1, Col = 0 }, new Tile { Data = 0, Row = 1, Col = 1 }, new Tile { Data = 0, Row = 1, Col = 2 }, new Tile { Data = 0, Row = 1, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 2, Col = 0 }, new Tile { Data = 0, Row = 2, Col = 1 }, new Tile { Data = 0, Row = 2, Col = 2 }, new Tile { Data = 0, Row = 2, Col = 3 } }, new Tile[] { new Tile { Data = 0, Row = 3, Col = 0 }, new Tile { Data = 0, Row = 3, Col = 1 }, new Tile { Data = 0, Row = 3, Col = 2 }, new Tile { Data = 0, Row = 3, Col = 3 } } };
                foreach (var item in Tiles)
                {
                    p[item.Row][item.Col].Data = item.Data;
                }
                if (GridHelper.MoveDown(p) || GridHelper.MoveLeft(p) || GridHelper.MoveRight(p) || GridHelper.MoveUp(p))
                {

                }
                else
                {
                    OnGameOver();
                }
            }
            if (b)
            {
                if (Tiles.Count < 16)
                {
                    int p;
                    do
                    {
                        p = Tools.Random.Next(16);
                    } while (Tiles.Exists(x =>
                    {
                        return x.Row * 4 + x.Col == p;
                    }));
                    var til = new TileItem(Tools.RandomBool(95) ? 2u : 4u, p / 4, p % 4, theme);
                    Tiles.Add(til);
                    ground.Children.Add(til.Rect);
                    til.Pop.Begin();
                }

            }
        }

        internal Direction? GetDirection(Point In, Point Out)
        {
            var x = Out.X - In.X;
            var y = Out.Y - In.Y;
            if (Math.Abs(x) > 15 || Math.Abs(y) > 15)
            {
                var angle = (float)Math.Atan2(y, x);
                angle = Tools.RadiansToDegrees(angle);
                if (angle < 30 && angle > -30)
                {
                    return Direction.Right;
                }
                else if (angle > -120 && angle < -60)
                {
                    return Direction.Up;
                }
                else if (angle < -150 || angle > 150)
                {
                    return Direction.Left;
                }
                else if (angle > 60 && angle < 120)
                {
                    return Direction.Down;
                }
            }
            return null;
        }

        private void OnGameEnd()
        {
            GameEndEvent?.Invoke(this, new EventArgs());
        }

        private void OnGameOver()
        {
            GameOverEvent?.Invoke(this, new EventArgs());
        }

        private bool MoveRight(ElementTheme theme)
        {
            var g = from p in Tiles
                    group p by p.Row into m
                    select m;
            bool b = false;
            foreach (var item in g)
            {
                var k = from l in item
                        orderby l.Col descending
                        select l;

                var n = k.ToList();

                for (int i = 0; i < n.Count - 1; i++)
                {
                    if (n[i].Data == n[i + 1].Data && !n[i].IsMerged && !n[i].IsDisappeared)
                    {
                        n[i].Disappear();
                        n[i + 1].Merge();
                    }
                }
                var j = 3;
                foreach (var tile in n)
                {
                    if (tile.IsDisappeared)
                    {
                        b = true;
                        continue;
                    }
                    if (tile.Col != j)
                        b = true;
                    tile.Col = j;
                    j--;
                }
                if (b)
                {
                    for (j = n.Count - 1; j > -1; j--)
                    {
                        if (n[j].IsDisappeared)
                        {
                            n[j].Update(0, n[j + 1].Row, n[j + 1].Col, theme);
                        }
                        else if (n[j].IsMerged)
                        {
                            n[j].Update(n[j].Data * 2, n[j].Row, n[j].Col, theme);
                            Score += n[j].Data;
                            OnScoreAdd();
                        }
                        else
                        {
                            n[j].Update(n[j].Data, n[j].Row, n[j].Col, theme);
                        }
                    }
                }
            }
            return b;
        }

        internal void ChangeTheme(ElementTheme dark)
        {
            foreach (var item in Tiles)
            {
                item.ChangeTheme(dark);
            }
        }

        private bool MoveLeft(ElementTheme theme)
        {
            var g = from p in Tiles
                    group p by p.Row into m
                    select m;
            bool b = false;
            foreach (var item in g)
            {
                var k = from l in item
                        orderby l.Col ascending
                        select l;

                var n = k.ToList();

                for (int i = 0; i < n.Count - 1; i++)
                {
                    if (n[i].Data == n[i + 1].Data && !n[i].IsMerged && !n[i].IsDisappeared)
                    {
                        n[i].Disappear();
                        n[i + 1].Merge();
                    }
                }
                var j = 0;
                foreach (var tile in n)
                {
                    if (tile.IsDisappeared)
                    {
                        b = true;
                        continue;
                    }
                    if (tile.Col != j)
                        b = true;
                    tile.Col = j;
                    j++;
                }
                if (b)
                {
                    for (j = n.Count - 1; j > -1; j--)
                    {
                        if (n[j].IsDisappeared)
                        {
                            n[j].Update(0, n[j + 1].Row, n[j + 1].Col, theme);
                        }
                        else if (n[j].IsMerged)
                        {
                            n[j].Update(n[j].Data * 2, n[j].Row, n[j].Col, theme);
                            Score += n[j].Data;
                            OnScoreAdd();
                        }
                        else
                        {
                            n[j].Update(n[j].Data, n[j].Row, n[j].Col, theme);
                        }
                    }
                }
            }
            return b;
        }

        private bool MoveDown(ElementTheme theme)
        {
            var g = from p in Tiles
                    group p by p.Col into m
                    select m;
            bool b = false;
            foreach (var item in g)
            {
                var k = from l in item
                        orderby l.Row descending
                        select l;

                var n = k.ToList();

                for (int i = 0; i < n.Count - 1; i++)
                {
                    if (n[i].Data == n[i + 1].Data && !n[i].IsMerged && !n[i].IsDisappeared)
                    {
                        n[i].Disappear();
                        n[i + 1].Merge();
                    }
                }
                var j = 3;
                foreach (var tile in n)
                {
                    if (tile.IsDisappeared)
                    {
                        b = true;
                        continue;
                    }
                    if (tile.Row != j)
                        b = true;
                    tile.Row = j;
                    j--;
                }
                if (b)
                {
                    for (j = n.Count - 1; j > -1; j--)
                    {
                        if (n[j].IsDisappeared)
                        {
                            n[j].Update(0, n[j + 1].Row, n[j + 1].Col, theme);
                        }
                        else if (n[j].IsMerged)
                        {
                            n[j].Update(n[j].Data * 2, n[j].Row, n[j].Col, theme);
                            Score += n[j].Data;
                            OnScoreAdd();
                        }
                        else
                        {
                            n[j].Update(n[j].Data, n[j].Row, n[j].Col, theme);
                        }
                    }
                }
            }
            return b;
        }

        private bool MoveUp(ElementTheme theme)
        {
            var g = from p in Tiles
                    group p by p.Col into m
                    select m;
            bool b = false;
            foreach (var item in g)
            {
                var k = from l in item
                        orderby l.Row ascending
                        select l;

                var n = k.ToList();

                for (int i = 0; i < n.Count - 1; i++)
                {
                    if (n[i].Data == n[i + 1].Data && !n[i].IsMerged && !n[i].IsDisappeared)
                    {
                        n[i].Disappear();
                        n[i + 1].Merge();
                    }
                }
                var j = 0;
                foreach (var tile in n)
                {
                    if (tile.IsDisappeared)
                    {
                        b = true;
                        continue;
                    }
                    if (tile.Row != j)
                        b = true;
                    tile.Row = j;
                    j++;
                }
                if (b)
                {
                    for (j = n.Count - 1; j > -1; j--)
                    {
                        if (n[j].IsDisappeared)
                        {
                            n[j].Update(0, n[j + 1].Row, n[j + 1].Col, theme);
                        }
                        else if (n[j].IsMerged)
                        {
                            n[j].Update(n[j].Data * 2, n[j].Row, n[j].Col, theme);
                            Score += n[j].Data;
                            OnScoreAdd();
                        }
                        else
                        {
                            n[j].Update(n[j].Data, n[j].Row, n[j].Col, theme);
                        }
                    }
                }
            }
            return b;
        }

        private void OnScoreAdd()
        {
            ScoreAdd?.Invoke(this, new EventArgs());
        }
    }
}
