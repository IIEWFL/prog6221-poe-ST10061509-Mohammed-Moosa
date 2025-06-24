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
using System.Text.RegularExpressions;
using CyberSecurityChatBotPOE.Models;

namespace CyberSecurityChatBotPOE
{
    public class ChatbotEngine
    {
        // Properties to store chatbot memory and user interaction history
        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public List<ActivityLog> ActivityLog { get; set; } = new List<ActivityLog>();
        public List<QuizQuestion> QuizQuestions { get; private set; } = new List<QuizQuestion>();

        private string userName = "friend";
        private string userMood = string.Empty;
        private string userInterest = string.Empty;

        // Main method to handle user input and return appropriate responses
        public string GetResponse(string input)
        {
            input = input.ToLower();

            // Memory: Store user's name
            if (input.Contains("my name is"))
            {
                var name = input.Replace("my name is", "").Trim();
                userName = name;
                return $"Nice to meet you, {userName}!";
            }

            // Memory: Store user interests
            if (input.Contains("interested in"))
            {
                userInterest = input.Split("interested in")[1].Trim();
                ActivityLog.Add(new ActivityLog { Description = $"User is interested in {userInterest}" });
                return $"Thanks for sharing! I'll remember you're interested in {userInterest}.";
            }

            // Sentiment handling based on keywords
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("anxious"))
            {
                userMood = "worried";
                return "It’s okay to feel worried. I'm here to help you stay safe online.";
            }
            else if (input.Contains("curious") || input.Contains("interested") || input.Contains("excited"))
            {
                userMood = "curious";
                return "I love your curiosity! Let’s explore cybersecurity together.";
            }
            else if (input.Contains("frustrated") || input.Contains("angry") || input.Contains("confused"))
            {
                userMood = "frustrated";
                return "I'm sorry to hear that. Let's break it down step-by-step together.";
            }

            // Recall stored information
            if (input.Contains("what do you remember") || input.Contains("what do you know about me"))
            {
                return RecallMemory();
            }

            // Task-related keywords
            if (input.Contains("add task") || input.Contains("remind me") || input.Contains("set reminder"))
            {
                return HandleTaskCreation(input);
            }

            if (input.Contains("view tasks") || input.Contains("show tasks"))
            {
                return GetTaskSummary();
            }

            // Show activity log
            if (input.Contains("activity log") || input.Contains("what have you done"))
            {
                return GetActivityLog();
            }

            // Quiz prompt
            if (input.Contains("quiz"))
            {
                return "You can start the quiz now. Just click the 'Start Quiz' button!";
            }

            // Keyword-based quick responses
            if (input.Contains("password"))
                return "Use long, complex passwords with symbols and never reuse passwords across sites.";

            if (input.Contains("phishing"))
                return "Watch for suspicious links or emails asking for login credentials. Verify sources.";

            if (input.Contains("scam") || input.Contains("fraud"))
                return "Scams often involve urgency or rewards. Stay cautious and verify identities.";

            // Fallback response
            return "I'm not sure I understand. Try saying 'add task', 'view tasks', or ask a question like 'what is phishing?'";
        }

        // Recall stored user data
        private string RecallMemory()
        {
            List<string> facts = new List<string>();
            if (!string.IsNullOrWhiteSpace(userName)) facts.Add($"Your name is {userName}.");
            if (!string.IsNullOrWhiteSpace(userInterest)) facts.Add($"You’re interested in {userInterest}.");
            if (!string.IsNullOrWhiteSpace(userMood)) facts.Add($"You’ve felt {userMood} during our chat.");

            if (facts.Count == 0)
                return "I don't remember anything specific yet. Feel free to tell me more about yourself!";

            string response = "Here's what I remember about you:\n" + string.Join("\n", facts);
            return response;
        }

        // Parses task input and creates task object
        private string HandleTaskCreation(string input)
        {
            string title = "";
            string description = "";
            DateTime? reminderDate = null;

            // Extract title using regex
            var match = Regex.Match(input, @"(add task|remind me to|set reminder to)?\s?(.*)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                title = match.Groups[2].Value.Trim();
                description = $"Cybersecurity task: {title}";
            }

            // Extract optional reminder date
            var reminderMatch = Regex.Match(input, @"(\d+)\s+days?", RegexOptions.IgnoreCase);
            if (reminderMatch.Success)
            {
                int days = int.Parse(reminderMatch.Groups[1].Value);
                reminderDate = DateTime.Now.AddDays(days);
            }

            // Create and store the task
            var newTask = new TaskModel
            {
                Title = title,
                Description = description,
                ReminderDate = reminderDate,
                IsCompleted = false
            };

            Tasks.Add(newTask);
            ActivityLog.Add(new ActivityLog
            {
                Description = $"Task added: '{title}' {(reminderDate != null ? $"(Reminder in {reminderDate?.ToShortDateString()})" : "(No reminder)")}"
            });

            string response = $"Task added: \"{title}\".";
            response += reminderDate != null
                ? $" I'll remind you in {reminderDate?.Subtract(DateTime.Now).Days} days."
                : " No reminder set.";

            return response;
        }

        // Summarizes user tasks
        private string GetTaskSummary()
        {
            if (Tasks.Count == 0)
                return "You don't have any tasks yet.";

            string summary = "Here are your tasks:\n";
            int i = 1;
            foreach (var task in Tasks)
            {
                summary += $"{i++}. {task.Title} - {(task.IsCompleted ? "✅ Completed" : "🕓 Pending")}";
                if (task.ReminderDate != null)
                    summary += $" (Remind on {task.ReminderDate?.ToShortDateString()})";
                summary += "\n";
            }

            return summary.Trim();
        }

        // Shows the most recent 5 activities
        private string GetActivityLog()
        {
            if (ActivityLog.Count == 0)
                return "No activity recorded yet.";

            string log = "Here's your recent activity:\n";
            int count = 0;
            for (int i = ActivityLog.Count - 1; i >= 0 && count < 5; i--)
            {
                log += $"{ActivityLog.Count - i}. {ActivityLog[i].Description}\n";
                count++;
            }

            return log.Trim();
        }

        // Loads static quiz questions
        public List<QuizQuestion> LoadQuizQuestions()
        {
            QuizQuestions = new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    OptionA = "Reply with your password",
                    OptionB = "Delete the email",
                    OptionC = "Report the email as phishing",
                    OptionD = "Ignore it",
                    CorrectOption = "C",
                    Explanation = "Reporting phishing emails helps prevent scams."
                },
                new QuizQuestion
                {
                    Question = "Which of the following is a strong password?",
                    OptionA = "123456",
                    OptionB = "Password1",
                    OptionC = "MyDogIsCute@2023",
                    OptionD = "qwerty",
                    CorrectOption = "C",
                    Explanation = "Strong passwords are long, unique, and use special characters."
                },
                new QuizQuestion
                {
                    Question = "What does HTTPS in a URL indicate?",
                    OptionA = "The website is unsecure",
                    OptionB = "The website uses secure encryption",
                    OptionC = "It is a government website",
                    OptionD = "The site is hosted on Google",
                    CorrectOption = "B",
                    Explanation = "HTTPS indicates that communications between you and the site are encrypted."
                },
                new QuizQuestion
                {
                    Question = "Which of the following is an example of phishing?",
                    OptionA = "A bank asking you to confirm login info via email",
                    OptionB = "A website asking you to log in",
                    OptionC = "An app requiring permissions",
                    OptionD = "A news site asking for newsletter signup",
                    CorrectOption = "A",
                    Explanation = "Phishing often impersonates legitimate organizations to steal info."
                },
                new QuizQuestion
                {
                    Question = "What is two-factor authentication (2FA)?",
                    OptionA = "Using two passwords",
                    OptionB = "Verifying identity through two different methods",
                    OptionC = "Resetting password twice",
                    OptionD = "Using two accounts",
                    CorrectOption = "B",
                    Explanation = "2FA requires two different authentication methods for extra security."
                },
                new QuizQuestion
                {
                    Question = "Which device is most vulnerable to cyber attacks?",
                    OptionA = "Smartphone",
                    OptionB = "Tablet",
                    OptionC = "Any device connected to the internet",
                    OptionD = "Desktop only",
                    CorrectOption = "C",
                    Explanation = "Any internet-connected device can be vulnerable without proper security."
                },
                new QuizQuestion
                {
                    Question = "Why should you avoid public Wi-Fi for sensitive transactions?",
                    OptionA = "It's too slow",
                    OptionB = "It costs money",
                    OptionC = "It's insecure and can be intercepted",
                    OptionD = "It drains your battery",
                    CorrectOption = "C",
                    Explanation = "Public Wi-Fi can be exploited by hackers to intercept your data."
                },
                new QuizQuestion
                {
                    Question = "How often should you update your passwords?",
                    OptionA = "Never",
                    OptionB = "Every few months",
                    OptionC = "Only when hacked",
                    OptionD = "Every 10 years",
                    CorrectOption = "B",
                    Explanation = "Regular password changes help prevent long-term breaches."
                },
                new QuizQuestion
                {
                    Question = "What should you do if a site asks for too many permissions?",
                    OptionA = "Grant them all",
                    OptionB = "Install anyway",
                    OptionC = "Review and deny unnecessary ones",
                    OptionD = "Ignore it",
                    CorrectOption = "C",
                    Explanation = "Only grant permissions that are essential to the app’s function."
                },
                new QuizQuestion
                {
                    Question = "What is the main purpose of a firewall?",
                    OptionA = "Speed up internet",
                    OptionB = "Prevent overheating",
                    OptionC = "Block unauthorized access",
                    OptionD = "Connect to new networks",
                    CorrectOption = "C",
                    Explanation = "A firewall protects your system by filtering incoming and outgoing traffic."
                }
            };

            return QuizQuestions;
        }
    }
}
