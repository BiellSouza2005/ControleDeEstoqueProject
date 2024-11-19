namespace ControleDeEstoqueAPI.Entities
{
    public class Entity<T> : IAuditable
    {
        public T Id { get; set; }
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }
    }
}
