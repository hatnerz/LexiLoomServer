namespace LexiLoom.DTO
{
    public class NewModuleModel
    {
        public string Name { get; set; }
        
        public string? Description { get; set; }

        public int UserId { get; set; }

        public IEnumerable<AddWordModel> Words { get; set; } = new List<AddWordModel>();
    }
}
