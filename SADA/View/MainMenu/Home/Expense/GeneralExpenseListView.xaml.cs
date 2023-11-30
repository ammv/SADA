using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SADA.View.MainMenu.Home.Expense
{
    /// <summary>
    /// Логика взаимодействия для GeneralExpenseView.xaml
    /// </summary>
    public partial class GeneralExpenseListView : UserControl
    {
        public GeneralExpenseListView()
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