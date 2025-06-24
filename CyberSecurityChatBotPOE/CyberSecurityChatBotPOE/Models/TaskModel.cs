using System;

namespace CyberSecurityChatBotPOE.Models
{
    public class TaskModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
