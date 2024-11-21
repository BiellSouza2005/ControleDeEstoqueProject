namespace ControleDeEstoqueAPI.Entities
{
    public class ProductDescription : Entity<int>
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }

    }
}
