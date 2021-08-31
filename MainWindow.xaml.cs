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
    public class Position : Window
    {
        public double x, y;
        public Position()
        {

        }
        Position(double a, double b)
        {
            this.x = a;
            this.y = b;
        }
        public Position GetFoodPosition(double x, double y, List<Rectangle> snakebody)
        {
            Random rand = new Random();
            double newX = (double)rand.Next(30) * 25;
            double newY = (double)rand.Next(30) * 25;

            foreach (Rectangle piece in snakebody)
            {
                if ((double)piece.GetValue(TopProperty) == newY && (double)piece.GetValue(LeftProperty) == newX)
                    return GetFoodPosition(x, y, snakebody);
            }
            return new Position(newX, newY);
        }
    }
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        Random rand;
        List<Rectangle> snakebody;
        int score;
        enum direction
        {
            Up,
            Down,
            Left,
            Right,
            Nowhere
        }
        direction snakeDirection = direction.Nowhere;
        bool foodEaten = false;
        double lastX, lastY;
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += timer_Tick;
            snakebody = new List<Rectangle>();

            rand = new Random();
        }
        private void start_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            snakeDirection = direction.Nowhere;
            int rand1, rand2;
            do
            {
                rand1 = rand.Next(30);
                rand2 = rand.Next(30);
            } while (rand1 == 15 && rand2 == 15);

            foreach (var x in snakebody)
            {
                Snake.Children.Remove(x);
            }
            snakebody.Clear();

            snakebody.Add(new Rectangle());
            snakebody[0].Fill = Brushes.GreenYellow;
            snakebody[0].Width = 25;
            snakebody[0].Height = 25;
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
            scoreTextBox.Text = "Score: " + score;
            for (int i = snakebody.Count() - 1; i > 0; i--)
            {
                snakebody[i].SetValue(TopProperty, snakebody[i - 1].GetValue(TopProperty));
                snakebody[i].SetValue(LeftProperty, snakebody[i - 1].GetValue(LeftProperty));
            }

            moveSnake(snakebody[0], snakeDirection);

            if(foodEaten)
            {
                growSnake(lastX, lastY, snakebody);
                foodEaten = false;
                string temp = "";

             /* foreach(var x in snakebody)
                {
                    temp += "x: " + x.GetValue(LeftProperty) + "y: " + x.GetValue(TopProperty) + "\n";
                }
                MessageBox.Show(temp);
             */
            }

            double headY = (double)snakebody[0].GetValue(TopProperty);
            double headX = (double)snakebody[0].GetValue(LeftProperty);

            double foodTopProp = (double)food.GetValue(TopProperty);
            double foodLeftProp = (double)food.GetValue(LeftProperty);

            if (headX < 0 || headX > 725 || headY < 0 || headY > 725)
            {
                MessageBox.Show("Game Over, your score: " + score);
                timer.Stop();
            }
            if (headX == foodLeftProp && headY == foodTopProp)
            {
                score++;

                foodEaten = true;
                lastX = (double)snakebody[snakebody.Count() - 1].GetValue(LeftProperty);
                lastY = (double)snakebody[snakebody.Count() - 1].GetValue(TopProperty);

                Position position = new Position().GetFoodPosition(rand.Next(30) * 25, rand.Next(30) * 25, snakebody);
                Canvas.SetLeft(food, position.x);
                Canvas.SetTop(food, position.y);
            }
            foreach(var piece in snakebody)
            {
                if (headX == (double)piece.GetValue(LeftProperty)   && 
                    headY == (double)piece.GetValue(TopProperty)    && 
                    snakebody.Count() > 1                           &&
                    piece != snakebody[0])
                {
                    MessageBox.Show("Game Over, your score: " + score);
                    timer.Stop();
                }
            }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && snakeDirection != direction.Down)
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
            double headY = (double)head.GetValue(TopProperty);
            double headX = (double)head.GetValue(LeftProperty);
            switch (dir)
            {
                case direction.Up:
                    head.SetValue(TopProperty, headY - (double)25);
                    break;

                case direction.Down:
                    head.SetValue(TopProperty, headY + (double)25);
                    break;

                case direction.Left:
                    head.SetValue(LeftProperty, headX - (double)25);
                    break;

                case direction.Right:
                    head.SetValue(LeftProperty, headX + (double)25);
                    break;

                default:
                    break;
            }
        }
        private void growSnake(double x, double y, List<Rectangle> snakebody)
        {
            Rectangle temp = new Rectangle();

            temp.Fill = Brushes.YellowGreen;
            temp.Width = 25;
            temp.Height = 25;
            temp.SetValue(TopProperty, lastY);
            temp.SetValue(LeftProperty, lastX);

            snakebody.Add(temp);

            Snake.Children.Add(snakebody[snakebody.Count() - 1]);
        }
    }
}
