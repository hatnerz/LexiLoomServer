namespace LexiLoom.DTO
{
    public class ModuleGameWordModel
    {
        public string CorrectAnswer { get; set; }

        public IEnumerable<string> AnswerOptions { get; set; } = new List<string>();
    }
}
