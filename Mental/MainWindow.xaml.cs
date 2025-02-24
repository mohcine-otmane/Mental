using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Mental
{
    public partial class MainWindow : Window
    {
        private int num1, num2, score = 0;
        private Random random = new Random();
        private DispatcherTimer timer;
        private int timeLeft = 5;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            GenerateNewProblem();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
        }

        private void StartTimer()
        {
            timeLeft = 5;
            TimerText.Text = $"Time Left: {timeLeft}s";
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timeLeft--;

            if (timeLeft > 0)
            {
                TimerText.Text = $"Time Left: {timeLeft}s";
            }
            else
            {
                timer.Stop();
                ShowAnswer();
            }
        }

        private void ShowAnswer()
        {
            ProblemText.Text = $"Answer: {num1 * num2}";
            TimerText.Text = "Next Question in 1s...";

            DispatcherTimer waitTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            waitTimer.Tick += (s, e) =>
            {
                waitTimer.Stop();
                GenerateNewProblem();
            };
            waitTimer.Start();
        }

        private void GenerateNewProblem()
        {
            num1 = random.Next(1, 20);
            num2 = random.Next(1, 20);
            ProblemText.Text = $"{num1} × {num2}";
            AnswerInput.Clear();
            StartTimer();
        }

        private void SubmitAnswer(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(AnswerInput.Text, out int userAnswer) && userAnswer == num1 * num2)
            {
                score++;
                ScoreText.Text = $"Score: {score}";
                timer.Stop();
                GenerateNewProblem();
            }
            else
            {
                MessageBox.Show("Incorrect! Try again.", "Oops!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
