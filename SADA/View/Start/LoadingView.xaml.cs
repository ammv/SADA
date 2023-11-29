using SADA.Services;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Window = System.Windows.Window;

namespace SADA.View.Start
{
    /// <summary>
    /// Логика взаимодействия для LoadingView.xaml
    /// </summary>
    public partial class LoadingView : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public bool isLoaded = false;
        private RepeatBehavior _repeatBehavior = new RepeatBehavior(1);

        public LoadingView()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(5.5);
            timer.Tick += Timer_Tick;
            animationPath.RepeatBehavior = _repeatBehavior;
        }

        private void AnimationPath_Completed(object sender, EventArgs e)
        {
            if (isLoaded)
            {
                Thread.Sleep(100);
                animationPath.Fill = Application.Current.Resources["PrimaryBlueBrush"] as Brush;

                var timer2 = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(500);

                timer.Tick += (p1, p2) =>
                {
                    IWindowService windowService = App.Current.GetService<IWindowService>();

                    windowService.ShowAndCloseWindow<View.Start.AuthView>(this);
                };

                timer.Start();
            }
            else
            {
                animationPath.IsPlaying = false;
                animationPath.RepeatBehavior = _repeatBehavior;
                animationPath.IsPlaying = true;
            }

            //MessageBox.Show("end");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            isLoaded = true;

            timer.Stop();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}