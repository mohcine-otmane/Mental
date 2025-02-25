using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Mental
{
    public partial class MainWindow : Window
    {
        private int num1, num2, score = 0, streak = 0, difficulty = 1;
        private Random random = new Random();
        private DispatcherTimer timer;
        private int timeLeft = 5;
        private bool answered = false;

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
            answered = false;
            TimerText.Text = $"{timeLeft}s";
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            timeLeft--;

            if (timeLeft > 0)
            {
                TimerText.Text = $"{timeLeft}s";
            }
            else
            {
                timer.Stop();
                if (!answered)
                {
                    HandleIncorrect();
                }
                GenerateNewProblem();
            }
        }

        private void GenerateNewProblem()
        {
            int min = (int)Math.Pow(10, difficulty - 1);
            int max = (int)Math.Pow(10, difficulty) - 1;

            num1 = random.Next(min, max);
            num2 = random.Next(min, max);

            ProblemText.Text = $"{num1} × {num2}";
            AnswerInput.Clear();
            StartTimer();
        }

        private void SubmitAnswer(object sender, RoutedEventArgs e)
        {
            if (answered) return; // Prevent multiple answers

            if (int.TryParse(AnswerInput.Text, out int userAnswer) && userAnswer == num1 * num2)
            {
                score++;
                streak++;
                ScoreText.Text = $"Score: {score}";
                StreakText.Text = $"Streak: {streak}";

                if (streak == 5)
                {
                    difficulty++;
                    streak = 0;
                    DifficultyText.Text = $"Level: {difficulty}";
                }

                answered = true;
                timer.Stop();
                GenerateNewProblem();
            }
            else
            {
                HandleIncorrect();
            }
        }

        private void HandleIncorrect()
        {
            streak = 0;
            difficulty = Math.Max(1, difficulty - 1);
            StreakText.Text = "Streak: 0";
            DifficultyText.Text = $"Level: {difficulty}";
            answered = true;
        }

        private void AnswerInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SubmitAnswer(null, null);
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
