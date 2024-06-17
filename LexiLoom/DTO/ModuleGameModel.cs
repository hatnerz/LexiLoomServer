namespace LexiLoom.DTO
{
    public class ModuleGameModel
    {
        public int WordsCount { get; set; }

        public int OptionVariants { get; set; }

        public IEnumerable<ModuleGameWordModel> Words { get; set; } = new List<ModuleGameWordModel>();
    }
}
