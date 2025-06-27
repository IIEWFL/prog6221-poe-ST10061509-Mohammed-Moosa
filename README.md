🛡️ Cybersecurity Awareness Assistant (Part 3 – WPF GUI)

A C#/.NET 9 WPF-based chatbot that builds on the console version by introducing a clean, interactive GUI to enhance user experience while promoting cybersecurity awareness. This assistant uses dynamic conversations, memory recall, sentiment recognition, and now includes task management, a quiz mini-game, and a visual activity log.

📦 Features (GUI + Console Evolution)

🎧 Voice greeting using .wav audio
🎨 ASCII art splash with custom chat bubbles
🧠 Memory & Recall: Remembers your name, interests, and moods
💬 Natural Conversation: Responds differently with random responses and recognizes cybersecurity keywords
😊 Emotion Detection: Understands and responds to mood (worried, curious, frustrated)
📚 Task Manager: Add, view, and track cybersecurity reminders and tasks
🧾 Activity Log: Keeps a record of chatbot interactions and tasks
🧠 Quiz Mini-Game: 10-question multiple choice quiz with instant feedback
📦 GUI Enhancements: Typing effects, colored chat bubbles, clean layout, scrollable chat

🖥️ How to Run (Part 3 – GUI)

1. Make sure you have .NET 9 SDK installed from https://dotnet.microsoft.com
2. Open the project in Visual Studio 2022+
3. Ensure Assets folder contains:
   • `Assets/greeting.wav`
   • `Assets/ascii_art.txt`
4. Press `F5` or click `Start` in Visual Studio to run

💬 Example Prompts & GUI Interactions

| User Prompt                  | Assistant Response                                      |
| ---------------------------- | ------------------------------------------------------- |
| `my name is Mohammed`            | “Nice to meet you, Mohammed!” (remembers your name)         |
| `I'm interested in phishing` | “Thanks for sharing! I’ll remember you’re interested…” |
| `I feel worried`             | “It’s okay to feel worried. I’m here to help.”          |
| `add task secure email`      | “Task added: secure email.”                             |
| `show tasks`                 | Lists current cybersecurity-related tasks               |
| `what do you remember`       | Summarizes name, interest, mood                         |
| `quiz`                       | Prompts user to click “Start Quiz” button               |

🧠 Quiz Functionality

• 10 multiple-choice questions
• Instant feedback after each answer
• Final score summary with motivation
• Quiz panel can be opened/closed dynamically

🎨 UI Highlights (WPF)

• Typing animation for bot responses
• User chat in green bubbles, bot replies in gray bubbles
• ScrollViewer auto-scrolls to latest messages
• Hidden quiz panel with toggled visibility

📌 Version Notes

✔ Part 1: Basic keyword and memory chatbot (Console)
✔ Part 2: Sentiment handling, memory recall, ASCII UI (Console)
✔ Part 3: Full WPF GUI, chat styling, quiz, task manager, activity log

