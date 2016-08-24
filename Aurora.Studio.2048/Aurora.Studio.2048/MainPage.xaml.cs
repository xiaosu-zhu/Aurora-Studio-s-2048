using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Aurora.Studio._2048.Models;
using Windows.Foundation;
using Windows.System.Threading;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Aurora.Studio._2048
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private Operator op;
        private Settings s = Settings.Get();
        private bool isGamePaused;
        private bool isGameEnd;
        private Point point;

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
            Window.Current.CoreWindow.Closed += CoreWindow_Closed;
        }

        private void CoreWindow_Closed(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.CoreWindowEventArgs args)
        {
            s.Save();
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (isGamePaused)
            {
                if (args.VirtualKey == Windows.System.VirtualKey.Enter)
                {
                    Restart();

                }
                return;
            }
            if (isGameEnd)
            {
                if (args.VirtualKey == Windows.System.VirtualKey.Enter)
                {
                    Continue();

                }
                return;
            }
            var direction = Operator.GetDirection(args.VirtualKey);
            Execute(direction);
        }

        private void Execute(Direction? direction)
        {
            if (direction == null)
                return;
            var p = op.Update((Direction)direction, RequestedTheme);
            op.Play(Ground, p, RequestedTheme);
        }

        private void Continue()
        {
            s.IgnoreGameEnd = true;
            EndlessText.Opacity = 1;
            isGameEnd = false;
            isGamePaused = false;
            StartAni.Begin();
            Save();
        }

        private void Restart()
        {
            op.Clear(Ground);
            isGameEnd = false;
            isGamePaused = false;
            EndlessText.Opacity = 0;
            if (op != null)
            {
                op.GameEndEvent -= Op_GameEndEvent;
                op.GameOverEvent -= Op_GameOverEvent;
                op.ScoreAdd -= Op_ScoreAdd;
            }
            op = new Operator(new uint[] { }, 0, RequestedTheme);
            op.GameEndEvent += Op_GameEndEvent;
            op.GameOverEvent += Op_GameOverEvent;
            op.ScoreAdd += Op_ScoreAdd;
            StartAni.Begin();
            s.IgnoreGameEnd = false;
            Save();
        }

        private void StartAni_Completed(object sender, object e)
        {
            s.Score = op.Score;
            Score.Text = op.Score.ToString();
            isGamePaused = false;
            op.Place(Ground);
            Save();
        }

        private void Save()
        {
            var task = ThreadPool.RunAsync((x) =>
            {
                s.WriteData(op.Tiles);
                s.Save();
            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Score.Text = s.Score.ToString();
            Best.Text = s.HighScore.ToString();
            if (op != null)
            {
                op.GameEndEvent -= Op_GameEndEvent;
                op.GameOverEvent -= Op_GameOverEvent;
                op.ScoreAdd -= Op_ScoreAdd;
            }
            op = new Operator(s.Data, s.Score, RequestedTheme);
            op.GameEndEvent += Op_GameEndEvent;
            op.GameOverEvent += Op_GameOverEvent;
            op.ScoreAdd += Op_ScoreAdd;
            if (s.IsDarkMode)
            {
                ThemeButton.Content = "Dark";
                s.IsDarkMode = true;
                s.Save();
                RequestedTheme = ElementTheme.Dark;
                op.ChangeTheme(ElementTheme.Dark);
                App.ChangeStatusBar(ElementTheme.Dark);
            }
            else
            {
                ThemeButton.Content = "Light";
                s.IsDarkMode = false;
                s.Save();
                RequestedTheme = ElementTheme.Light;
                op.ChangeTheme(ElementTheme.Light);
                App.ChangeStatusBar(ElementTheme.Light);
            }
            if (!s.IgnoreGameEnd && op.IsNewGame())
            {
                op.GameEndEvent += Op_GameEndEvent;
            }
            if (s.IgnoreGameEnd)
            {
                EndlessText.Opacity = 1;
            }
            op.Place(Ground);
        }

        private void Op_ScoreAdd(object sender, System.EventArgs e)
        {
            Score.Text = op.Score.ToString();
            s.Score = op.Score;
            if (op.Score > s.HighScore)
            {
                s.HighScore = op.Score;
                Best.Text = op.Score.ToString();
            }
            Save();
        }

        private void Op_GameEndEvent(object sender, System.EventArgs e)
        {
            EndButton.Click -= Button_Click;
            EndText.Text = "You Win!";
            EndButton.Content = "Continue";
            isGameEnd = true;
            Save();
            EndButton.Click += EndButton_Click;
            EndAni.Begin();
        }

        private void Op_GameOverEvent(object sender, System.EventArgs e)
        {
            EndButton.Click -= EndButton_Click;
            EndText.Text = "Game Over!";
            EndButton.Content = "Restart";
            isGamePaused = true;
            EndButton.Click += Button_Click;
            EndAni.Begin();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            s.Save();
        }

        private void EndButton_Click(object sender, RoutedEventArgs e)
        {
            Continue();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://publisher/?name=Aurora-Studio"));
        }

        private void Page_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            point = e.GetCurrentPoint(this).Position;
        }

        private void Page_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            e.Handled = true;
            var p = e.GetCurrentPoint(this).Position;
            var d = op.GetDirection(point, p);
            Execute(d);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/aurora-lzzp/Aurora-Studio-s-2048"));
        }

        private uint click_Count = 0;
        private ThreadPoolTimer clickTimer;

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void TextBlock_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (click_Count >= 5 && clickTimer != null)
            {
                clickTimer.Cancel();
                clickTimer = null;
                click_Count = 0;
                var p = new MessageDialog("我是谁，我在哪，晚上吃啥？", "喵喵喵？");
                await p.ShowAsync();
                return;
            }
            click_Count++;
            if (clickTimer == null)
            {
                clickTimer = ThreadPoolTimer.CreateTimer(x =>
                {
                    click_Count = 0;
                }, TimeSpan.FromSeconds(5));
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            if (((string)b.Content) == "Light")
            {
                b.Content = "Dark";
                s.IsDarkMode = true;
                s.Save();
                RequestedTheme = ElementTheme.Dark;
                op.ChangeTheme(ElementTheme.Dark);
                App.ChangeStatusBar(ElementTheme.Dark);
            }
            else if (((string)b.Content) == "Dark")
            {
                b.Content = "Light";
                s.IsDarkMode = false;
                s.Save();
                RequestedTheme = ElementTheme.Light;
                op.ChangeTheme(ElementTheme.Light);
                App.ChangeStatusBar(ElementTheme.Light);
            }
        }
    }
}
