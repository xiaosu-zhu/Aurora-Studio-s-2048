using System;
using System.Diagnostics;
using Com.Aurora.Shared.MVVM;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace Aurora.Studio._2048.Models
{
    [DebuggerDisplay("Data = {Data}, Row = {Row}, Col = {Col}")]
    class TileItem : ViewModelBase
    {
        private static readonly Color black = Color.FromArgb(255, 0x77, 0x6e, 0x65);
        private static readonly Color white = Color.FromArgb(255, 0xf9, 0xf6, 0xf2);

        public uint Data { get; set; }
        public SolidColorBrush BG { get; set; }

        public int XOld { get; set; }
        public int YOld { get; set; }

        public int Col { get; set; }
        public int Row { get; set; }

        public Rectangle tang { get; set; } = new Rectangle
        {
            Height = 106.25,
            Width = 106.25,
            RadiusX = 3,
            RadiusY = 3,
            Margin = new Thickness(7.5),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        public Grid Rect { get; set; } = new Grid
        {
        };
        public TextBlock text { get; set; } = new TextBlock
        {
            FontFamily = new FontFamily("/Assets/ClearSans-Bold.ttf#Clear Sans"),
            Foreground = new SolidColorBrush(black),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        public Storyboard Ani { get; set; } = new Storyboard();
        public Storyboard Pop { get; set; } = new Storyboard();
        public bool IsMerged { get; internal set; }
        public bool IsDisappeared { get; internal set; }

        private CompositeTransform tran = new CompositeTransform();


        public TileItem(uint data, int x, int y)
        {
            Data = data;
            Row = x;
            Col = y;
            XOld = Row;
            YOld = Col;
            tang.Fill = new SolidColorBrush(Palette.GetColor(data));
            Rect.SetValue(Canvas.ZIndexProperty, 1);
            var point = GridData.GetTransform(x, y);
            tran.TranslateX = point.X;
            tran.TranslateY = point.Y;
            tran.ScaleX = 0;
            tran.ScaleY = 0;
            Rect.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            Rect.RenderTransform = tran;
            text.Text = data.ToString();
            text.FontSize = GridData.GetSize(text.Text.Length);
            Rect.Children.Add(tang);
            Rect.Children.Add(text);
            if (data > 5)
            {
                (text.Foreground as SolidColorBrush).Color = white;
            }

            var p1 = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
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
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                EnableDependentAnimation = true,
            };
            Storyboard.SetTarget(p2, tran);
            Storyboard.SetTargetProperty(p2, "TranslateY");
            var p3 = new DoubleAnimationUsingKeyFrames
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EnableDependentAnimation = true,
                BeginTime = TimeSpan.FromMilliseconds(100),
            };
            var k1 = new EasingDoubleKeyFrame
            {
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100)),
                Value = 1.25
            };
            var k2 = new EasingDoubleKeyFrame
            {
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseIn
                },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200)),
                Value = 1
            };
            var k0 = new DiscreteDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)),
                Value = 0
            };
            p3.KeyFrames.Add(k0);
            p3.KeyFrames.Add(k1);
            p3.KeyFrames.Add(k2);
            Storyboard.SetTarget(p3, tran);
            Storyboard.SetTargetProperty(p3, "ScaleX");
            var p4 = new DoubleAnimationUsingKeyFrames
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EnableDependentAnimation = true,
                BeginTime = TimeSpan.FromMilliseconds(100),
            };
            var k4 = new EasingDoubleKeyFrame
            {
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100)),
                Value = 1.25
            };
            var k5 = new EasingDoubleKeyFrame
            {
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseIn
                },
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200)),
                Value = 1
            };
            var k3 = new DiscreteDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)),
                Value = 0
            };
            p4.KeyFrames.Add(k3);
            p4.KeyFrames.Add(k4);
            p4.KeyFrames.Add(k5);
            Storyboard.SetTarget(p4, tran);
            Storyboard.SetTargetProperty(p4, "ScaleY");
            var p5 = new ColorAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(200)),
                EnableDependentAnimation = true,
                EasingFunction = new QuinticEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                BeginTime = TimeSpan.FromMilliseconds(100)
            };
            Storyboard.SetTarget(p5, tang);
            Storyboard.SetTargetProperty(p5, "(Rect.Fill).(SolidColorBrush.Color)");
            Ani.Children.Add(p1);
            Ani.Children.Add(p2);
            Pop.Children.Add(p3);
            Pop.Children.Add(p4);
            Pop.Children.Add(p5);
        }

        internal void Refresh()
        {
            IsDisappeared = false;
            IsMerged = false;
            XOld = Row;
            YOld = Col;
            Rect.SetValue(Canvas.ZIndexProperty, 1);
            (Ani.Children[0] as DoubleAnimation).From = tran.TranslateX;
            (Ani.Children[1] as DoubleAnimation).From = tran.TranslateY;
        }

        internal void Merge()
        {
            IsMerged = true;
        }

        internal void Disappear()
        {
            IsDisappeared = true;
        }

        public void Update(uint data, int X, int Y)
        {
            if (data != Data)
            {
                (Pop.Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[0].Value = 1;
                (Pop.Children[1] as DoubleAnimationUsingKeyFrames).KeyFrames[0].Value = 1;
                (Pop.Children[2] as ColorAnimation).From = (tang.Fill as SolidColorBrush).Color;
                (Pop.Children[2] as ColorAnimation).To = Palette.GetColor(data);
            }

            if (data == 0)
            {
                Rect.SetValue(Canvas.ZIndexProperty, 0);
            }
            else
            {
                text.Text = data.ToString();
                text.FontSize = GridData.GetSize(text.Text.Length);
                if (data > 5)
                {
                    (text.Foreground as SolidColorBrush).Color = white;
                }
            }
            Data = data;
            var point = GridData.GetTransform(X, Y);
            XOld = Row;
            YOld = Col;
            Row = X;
            Col = Y;
            (Ani.Children[0] as DoubleAnimation).From = tran.TranslateX;
            (Ani.Children[1] as DoubleAnimation).From = tran.TranslateY;
            (Ani.Children[0] as DoubleAnimation).To = point.X;
            (Ani.Children[1] as DoubleAnimation).To = point.Y;
        }
    }
}
