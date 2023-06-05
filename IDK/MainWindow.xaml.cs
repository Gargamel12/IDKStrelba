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



namespace IDK
{
    public partial class MainWindow : Window
    {
        private const int CanvasWidth = 1920;
        private const int CanvasHeight = 1080;
        private const int TargetDuration = 1000; // milliseconds
        private const int ScoreIncrement = 20;
        private int[] TrophyScoreThresholds = { 200, 400, 600 };
        private const string TrophyImagePath = "mini_trophy.jpg"; // Path to mini trophy image

        private Random random = new Random();
        private DispatcherTimer timer;
        private int score;
        private List<Image> trophies = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeCanvas();
            score = 0;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(2000);
            timer.Tick += Timer_Tick;
            scoreLabel.Foreground = Brushes.White;
            scoreLabel.Background = Brushes.Black;
        }

        private void InitializeCanvas()
        {
            canvas.Width = CanvasWidth;
            canvas.Height = CanvasHeight;
            canvas.Cursor = Cursors.Cross;
            canvas.MouseDown += Canvas_MouseDown;
            ImageBrush back = new ImageBrush();
            back.ImageSource = new BitmapImage(new Uri("dust2.jpg", UriKind.RelativeOrAbsolute));
            canvas.Background = back;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CreateTarget();
        }

        private void CreateTarget()
        {
            Target target = new Target(canvas, TargetDuration);
            target.TargetHit += Target_TargetHit;
        }

        private void Target_TargetHit(object sender, EventArgs e)
        {
            score += ScoreIncrement;
            scoreLabel.Content = "Score: " + score;

            if (CheckTrophyScoreThreshold(score))
            {
                DisplayMiniTrophy();
            }

            var uri = new Uri(@"Gun_Shot.wav", UriKind.RelativeOrAbsolute);
            var player = new MediaPlayer();

            player.Open(uri);
            player.Play();
        }

        private bool CheckTrophyScoreThreshold(int score)
        {
            foreach (int threshold in TrophyScoreThresholds)
            {
                if (score == threshold)
                {
                    return true;
                }
            }
            return false;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!timer.IsEnabled)
            {
                timer.Start();
            }
        }

        private void DisplayMiniTrophy()
        {
            Image trophy = new Image();
            trophy.Source = new ImageSourceConverter().ConvertFromString(TrophyImagePath) as ImageSource;
            trophy.Width = trophy.Height = 30;

            trophy.VerticalAlignment = VerticalAlignment.Top;
            trophy.HorizontalAlignment = HorizontalAlignment.Left;
            trophy.Margin = new Thickness(50);

            var uri = new Uri(@"Cink_Cink.mp3", UriKind.RelativeOrAbsolute);
            var player = new MediaPlayer();

            player.Open(uri);
            player.Play();

            trophies.Add(trophy);
            canvas.Children.Add(trophy);
        }
    }
}
