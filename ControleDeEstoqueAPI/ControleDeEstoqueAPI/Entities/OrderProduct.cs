namespace ControleDeEstoqueAPI.Entities
{
    public class OrderProduct : Entity<int>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }

}
