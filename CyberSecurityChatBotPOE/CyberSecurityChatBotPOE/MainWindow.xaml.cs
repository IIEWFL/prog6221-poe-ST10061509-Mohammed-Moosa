// --------------------------------------------------------------------
// Cybersecurity Awareness Chatbot - Part 3 (WPF GUI Version)
// Author: Mohammed Moosa (ST10061509)
// Institution: Varsity College
// Module: PROG6221 – Programming 2A
// Year: 2025
//
// Description:
// A WPF-based graphical chatbot application that extends the console version 
// with interactive visual elements. Features include chat bubble UI design, 
// task assistant, memory and sentiment recognition, a cybersecurity quiz, 
// sound greetings, ASCII art, and GUI-based interaction with typing effects.
//
// New Features in Part 3:
// - Fully functional WPF GUI with styled chat bubbles
// - Sentiment and memory retention from console version
// - Quiz system with score tracking and feedback
// - Typing effect using async tasks
// - Sound greeting and dynamic ASCII art rendering
// - Task assistant and activity log tracking
//
// Attribution Notice:
// - Sound playback using System.Media.SoundPlayer:
//   https://learn.microsoft.com/en-us/dotnet/api/system.media.soundplayer
//
// - File reading for ASCII art using System.IO:
//   https://learn.microsoft.com/en-us/dotnet/api/system.io.filereadalltext
//
// - Asynchronous typing effect using async/await and Task.Delay:
//   https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/
//   https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.delay
//
// - WPF GUI elements (StackPanel, Border, ScrollViewer, TextBlock) for chat bubble design:
//   https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/
//   https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/border
//
// - Dynamic UI alignment and styling in WPF:
//   https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/how-to-align-content
//
// - Regex usage for task creation and parsing reminders:
//   https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex
//
// - Button event binding and visibility toggling in XAML/C#:
//   https://learn.microsoft.com/en-us/dotnet/desktop/wpf/advanced/how-to-handle-events
//
// - Quiz structure using custom model classes and object lists:
//   https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/
//   https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1
//
// - General chatbot logic and conversational patterns adapted from AI design concepts:
//   https://github.com/microsoft/BotBuilder-Samples
// --------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CyberSecurityChatBotPOE.Models;

namespace CyberSecurityChatBotPOE
{
    public partial class MainWindow : Window
    {
        private ChatbotEngine chatbotEngine = new ChatbotEngine(); // Chatbot logic handler

        // Quiz tracking fields
        private int currentQuizIndex = 0;
        private int score = 0;
        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeQuiz();      // Load quiz data
            PlayGreetingSound();   // Play welcome audio
            ShowAsciiArt();        // Display ASCII art
        }

        // Plays greeting audio and follows with an introductory message
        private void PlayGreetingSound()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("Assets/greeting.wav");
                player.Load();
                player.Play();

                // Show welcome message after sound
                Task.Delay(2500).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        AddChatBubble("👋 Welcome! I'm your cybersecurity assistant. Ask me anything about staying safe online.", false);
                    });
                });
            }
            catch
            {
                AddChatBubble("⚠️ Audio greeting missing.", false);
            }
        }

        // Displays ASCII art from file
        private void ShowAsciiArt()
        {
            try
            {
                string art = File.ReadAllText("Assets/ascii_art.txt");
                AddChatBubble(art, false);
            }
            catch
            {
                AddChatBubble("⚠️ ASCII art file missing.", false);
            }
        }

        // Handles user message input and displays bot response
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AddChatBubble($"🧑 {input}", true);

            string response = chatbotEngine.GetResponse(input);
            await TypeBotResponseAsync(response);

            UserInput.Clear();
        }

        // Simulates typing effect for bot response
        private async Task TypeBotResponseAsync(string message)
        {
            string buffer = "";
            foreach (char c in message)
            {
                buffer += c;
                await Task.Delay(15);
            }
            AddChatBubble($"🤖 {buffer}", false);
        }

        // Adds chat bubble to the interface
        private void AddChatBubble(string message, bool isUser)
        {
            var bubble = new Border
            {
                Background = isUser ? Brushes.LightGreen : Brushes.LightGray,
                CornerRadius = new CornerRadius(12),
                Padding = new Thickness(10),
                Margin = new Thickness(5),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 600,
                Child = new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 14,
                    Foreground = Brushes.Black
                }
            };

            ChatStackPanel.Children.Add(bubble);
            ChatScrollViewer.ScrollToEnd();
        }

        // Loads the quiz questions from the engine
        private void InitializeQuiz()
        {
            quizQuestions = chatbotEngine.LoadQuizQuestions();
        }

        // Starts or restarts the quiz
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            QuizPanelBorder.Visibility = Visibility.Visible;
            currentQuizIndex = 0;
            score = 0;
            DisplayQuizQuestion();
            StartQuizButton.Visibility = Visibility.Collapsed;
        }

        // Displays a question and options in the quiz
        private void DisplayQuizQuestion()
        {
            if (currentQuizIndex >= quizQuestions.Count)
            {
                ShowFinalScore();
                return;
            }

            var q = quizQuestions[currentQuizIndex];
            QuestionText.Text = q.Question;
            OptionAButton.Content = "A) " + q.OptionA;
            OptionBButton.Content = "B) " + q.OptionB;
            OptionCButton.Content = "C) " + q.OptionC;
            OptionDButton.Content = "D) " + q.OptionD;

            AnswerFeedback.Text = "";
            QuizScoreDisplay.Text = $"Question {currentQuizIndex + 1} of {quizQuestions.Count}";

            OptionAButton.Visibility = Visibility.Visible;
            OptionBButton.Visibility = Visibility.Visible;
            OptionCButton.Visibility = Visibility.Visible;
            OptionDButton.Visibility = Visibility.Visible;
        }

        // Validates selected answer and gives feedback
        private async void EvaluateAnswer(string selectedOption)
        {
            var question = quizQuestions[currentQuizIndex];
            if (selectedOption == question.CorrectOption)
            {
                score++;
                AnswerFeedback.Text = "✅ Correct! " + question.Explanation;
            }
            else
            {
                AnswerFeedback.Text = "❌ Incorrect. " + question.Explanation;
            }

            currentQuizIndex++;

            // Brief pause before next question
            await Task.Delay(2000);
            DisplayQuizQuestion();
        }

        // Answer button click handlers
        private void OptionAButton_Click(object sender, RoutedEventArgs e) => EvaluateAnswer("A");
        private void OptionBButton_Click(object sender, RoutedEventArgs e) => EvaluateAnswer("B");
        private void OptionCButton_Click(object sender, RoutedEventArgs e) => EvaluateAnswer("C");
        private void OptionDButton_Click(object sender, RoutedEventArgs e) => EvaluateAnswer("D");

        // Displays the user's final quiz score
        private void ShowFinalScore()
        {
            QuestionText.Text = $"🎉 Quiz Completed!\nYour score: {score}/{quizQuestions.Count}";

            OptionAButton.Visibility = Visibility.Collapsed;
            OptionBButton.Visibility = Visibility.Collapsed;
            OptionCButton.Visibility = Visibility.Collapsed;
            OptionDButton.Visibility = Visibility.Collapsed;

            AnswerFeedback.Text = score >= 4
                ? "Excellent work! You're a cybersecurity pro! 🎯"
                : "Keep practicing to stay safe online! 💡";

            QuizScoreDisplay.Text = "";
            StartQuizButton.Visibility = Visibility.Visible;
        }

        // Hides the quiz panel
        private void CloseQuizButton_Click(object sender, RoutedEventArgs e)
        {
            QuizPanelBorder.Visibility = Visibility.Collapsed;
        }
    }
}

