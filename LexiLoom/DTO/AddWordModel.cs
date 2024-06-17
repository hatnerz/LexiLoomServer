namespace LexiLoom.DTO
{
    public class AddWordModel
    {
        public string Name { get; set; }

        public int UserId { get; set; }

        public IEnumerable<NewTranslationModel> Translations { get; set; }
    }
}
