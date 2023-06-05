using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace IDK
{
    public class Target
    {
        private const int Size = 100;

        private static readonly Random random = new Random();

        private Ellipse target;
        private Canvas canvas;
        private int duration;

        public event EventHandler TargetHit;

        public Target(Canvas canvas, int duration)
        {
            this.canvas = canvas;
            this.duration = duration;

            CreateTarget();
        }

        private void CreateTarget()
        {
            target = new Ellipse();
            target.Width = target.Height = Size;

            // Create a bullseye pattern brush for the target appearance
            RadialGradientBrush brush = new RadialGradientBrush();
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.0));
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.2));
            brush.GradientStops.Add(new GradientStop(Colors.White, 0.3));
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.4));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.6));
            brush.GradientStops.Add(new GradientStop(Colors.Red, 0.7));
            brush.GradientStops.Add(new GradientStop(Colors.Black, 0.8));

            target.Fill = brush;
            target.Stroke = Brushes.Black;
            target.StrokeThickness = 2;

            target.MouseDown += Target_MouseDown;

            Canvas.SetLeft(target, random.Next((int)canvas.Width - Size));
            Canvas.SetTop(target, random.Next((int)canvas.Height - Size));

            canvas.Children.Add(target);
            RemoveTargetAfterDelay();
        }

        private void RemoveTargetAfterDelay()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(duration);
            timer.Tick += (sender, e) =>
            {
                canvas.Children.Remove(target);
                timer.Stop();
            };
            timer.Start();
        }

        private void Target_MouseDown(object sender, MouseButtonEventArgs e)
        {
            canvas.Children.Remove(target);
            OnTargetHit();
        }

        protected virtual void OnTargetHit()
        {
            TargetHit?.Invoke(this, EventArgs.Empty);
        }
    }
}
