using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SADA.View.MainMenu.Car.Car
{
    /// <summary>
    /// Логика взаимодействия для PayToCounteragentListView.xaml
    /// </summary>
    public partial class PayToCounteragentListView : UserControl
    {
        public PayToCounteragentListView()
        {
            InitializeComponent();
        }
        private void filterButton_Checked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 300,
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                Duration = TimeSpan.FromSeconds(0.3)
            };
            filterGrid.BeginAnimation(Grid.WidthProperty, animation);
        }
        private void filterButton_Unchecked(object sender, RoutedEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation
            {
                From = 300,
                To = 0,
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                },
                Duration = TimeSpan.FromSeconds(0.3)
            };
            filterGrid.BeginAnimation(Grid.WidthProperty, animation);
        }
    }
}
