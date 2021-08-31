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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        Random rand;
        Rectangle[] snakebody;
        int score;
        enum direction
        {
            Up,
            Down,
            Left,
            Right
        }
        direction snakeDirection;
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer.Tick += timer_Tick;

            rand = new Random();
            snakebody = new Rectangle[900];
        }
        private void start_Click(object sender, RoutedEventArgs e)
        {
            int rand1, rand2;
            do
            {
                rand1 = rand.Next(0, 30);
                rand2 = rand.Next(0, 30);
            } while (rand1 == 15 && rand2 == 15);
 
            foreach (var x in snakebody)
            {
                Snake.Children.Remove(x);
            }

            for (int i = 0; i < snakebody.Length; i++)
            {
                snakebody[i] = new Rectangle();
             // snakebody[i].Stroke = Brushes.White;
                snakebody[i].Fill = Brushes.White;
             // snakebody[i].HorizontalAlignment = HorizontalAlignment.Left;
             // snakebody[i].VerticalAlignment = VerticalAlignment.Center;
                snakebody[i].Width = 25;
                snakebody[i].Height = 25;
            }
            snakebody[0].SetValue(TopProperty, (double)375);
            snakebody[0].SetValue(LeftProperty, (double)375);
            Snake.Children.Add(snakebody[0]);
            

            score = 0;
            food.SetValue(TopProperty, (double)rand1 * 25);
            food.SetValue(LeftProperty, (double)rand2 * 25);
            food.Visibility = Visibility.Visible;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            moveSnake(snakebody[0], snakeDirection);

            double topProp = (double)snakebody[0].GetValue(TopProperty);
            double leftProp = (double)snakebody[0].GetValue(LeftProperty);

            double food

            if (leftProp < 0 || leftProp > 725 || topProp < 0 || topProp > 725)
            {
                MessageBox.Show("Game Over");
                //this.Close();
            }    

            if(leftProp == food.g)


            
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up && snakeDirection != direction.Down)
            {
                snakeDirection = direction.Up;
            }
            if (e.Key == Key.Down && snakeDirection != direction.Up)
            {
                snakeDirection = direction.Down;
            }
            if (e.Key == Key.Left && snakeDirection != direction.Right)
            {
                snakeDirection = direction.Left;
            }
            if (e.Key == Key.Right && snakeDirection != direction.Left)
            {
                snakeDirection = direction.Right;
            }
        }
        private void moveSnake(Rectangle head, direction dir)
        {
            double topProp = (double)head.GetValue(TopProperty);
            double leftProp = (double)head.GetValue(LeftProperty);
            switch(dir)
            {
                case direction.Up:
                    head.SetValue(TopProperty, topProp - (double)25);
                    break;

                case direction.Down:
                    head.SetValue(TopProperty, topProp + (double)25);
                    break;

                case direction.Left:
                    head.SetValue(LeftProperty, leftProp - (double)25);
                    break;

                case direction.Right:
                    head.SetValue(LeftProperty, leftProp + (double)25);
                    break;
   
                default:
                    break;
            }    
        }
    }
}
