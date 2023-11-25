using HandyControl.Controls;
using SADA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;
using Window = System.Windows.Window;

namespace SADA.View.Start
{
    /// <summary>
    /// Логика взаимодействия для LoadingView.xaml
    /// </summary>
    public partial class LoadingView : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public bool isLoaded = false;
        private RepeatBehavior _repeatBehavior = new RepeatBehavior(1);
        public LoadingView()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(5.5);
            timer.Tick += Timer_Tick;
            animationPath.RepeatBehavior = _repeatBehavior;
        }

        private async void AnimationPath_Completed(object sender, EventArgs e)
        {
            if(isLoaded)
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
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
