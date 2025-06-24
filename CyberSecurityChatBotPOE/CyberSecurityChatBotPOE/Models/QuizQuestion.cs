namespace CyberSecurityChatBotPOE.Models
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectOption { get; set; }  // This is the missing property
        public string Explanation { get; set; }    // Optional: Display explanation after answering
    }
}
