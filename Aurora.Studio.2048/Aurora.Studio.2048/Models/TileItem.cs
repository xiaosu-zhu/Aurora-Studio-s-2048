using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aurora.Studio._2048.Core.Game;
using Com.Aurora.Shared.MVVM;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Aurora.Studio._2048.Models
{
    class TileItem : ViewModelBase
    {
        public uint Data { get; set; }
        public SolidColorBrush Color { get; set; }

        public int XOld { get; set; }
        public int YOld { get; set; }

        public int Col { get; set; }
        public int Row { get; set; }
        public bool Changed { get; private set; }

        public Rectangle Rect { get; set; } = new Rectangle
        {
            Height = 106.25,
            Width = 106.25,
            RadiusX = 3,
            RadiusY = 3,
            Margin = new Thickness(7.5),
        };
        public Storyboard Ani { get; set; } = new Storyboard();
        public Storyboard Pop { get; set; } = new Storyboard();
        private CompositeTransform tran = new CompositeTransform();


        public TileItem(uint data, int x, int y)
        {
            Data = data;
            Row = x;
            Col = y;
            Rect.Fill = new SolidColorBrush(Palette.GetColor(data));
            Rect.SetValue(Canvas.ZIndexProperty, 1);
            var point = GridData.GetTransform(x, y);
            tran.TranslateX = point.X;
            tran.TranslateY = point.Y;
            tran.ScaleX = 0;
            tran.ScaleY = 0;
            Rect.RenderTransform = tran;

            var p1 = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                EnableDependentAnimation = true,
            };
            Storyboard.SetTarget(p1, tran);
            Storyboard.SetTargetProperty(p1, "TranslateX");
            var p2 = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                EnableDependentAnimation = true,
            };
            Storyboard.SetTarget(p2, tran);
            Storyboard.SetTargetProperty(p2, "TranslateY");
            var p3 = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                EnableDependentAnimation = true,
                BeginTime = TimeSpan.FromMilliseconds(100)
            };
            Storyboard.SetTarget(p3, tran);
            Storyboard.SetTargetProperty(p3, "ScaleX");
            var p4 = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                EnableDependentAnimation = true,
                BeginTime = TimeSpan.FromMilliseconds(100)
            };
            Storyboard.SetTarget(p4, tran);
            Storyboard.SetTargetProperty(p4, "ScaleY");
            var p5 = new ColorAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                EnableDependentAnimation = true,
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
            };
            Storyboard.SetTarget(p5, Rect);
            Storyboard.SetTargetProperty(p5, "Fill.Color");
            Ani.Children.Add(p1);
            Ani.Children.Add(p2);
            Ani.Children.Add(p5);
            Pop.Children.Add(p3);
            Pop.Children.Add(p4);
        }

        internal void MergeRight(int x, int y)
        {
            Rect.SetValue(Canvas.ZIndexProperty, 0);
            (Ani.Children[2] as ColorAnimation).From = (Rect.Fill as SolidColorBrush).Color;
            (Ani.Children[2] as ColorAnimation).To = (Rect.Fill as SolidColorBrush).Color;
            (Ani.Children[0] as DoubleAnimation).From = tran.TranslateX;
            (Ani.Children[1] as DoubleAnimation).From = tran.TranslateY;
            var point = GridData.GetTransform(x, y);
            (Ani.Children[0] as DoubleAnimation).To = point.X;
            (Ani.Children[1] as DoubleAnimation).To = point.Y;
        }

        public void Update(uint data, int X, int Y)
        {
            if (data != Data)
            {
                Changed = true;
            }
            Data = data;
            Color = new SolidColorBrush(Palette.GetColor(data));
            var point = GridData.GetTransform(X, Y);
            XOld = Row;
            YOld = Col;
            Row = Y;
            Col = X;
            (Ani.Children[0] as DoubleAnimation).From = tran.TranslateX;
            (Ani.Children[1] as DoubleAnimation).From = tran.TranslateY;
            (Ani.Children[2] as ColorAnimation).From = (Rect.Fill as SolidColorBrush).Color;
            (Ani.Children[0] as DoubleAnimation).To = point.X;
            (Ani.Children[1] as DoubleAnimation).To = point.Y;
            (Ani.Children[2] as ColorAnimation).To = Palette.GetColor(data);
        }
    }
}
