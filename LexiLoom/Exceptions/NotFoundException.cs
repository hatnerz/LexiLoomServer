namespace LexiLoom.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName) : base($"{entityName} not found") 
        { }
        
        public NotFoundException(string entityName, int id) : base ($"{entityName} with id {id} not found") 
        { }

        public NotFoundException(string entityName, string fieldName) : base($"{entityName} with this {fieldName} not found")
        { }

        public NotFoundException(string entityName, string fieldName, object fieldValue) : base($"{entityName} with {fieldName} {fieldValue} not found")
        { }
    }
}
