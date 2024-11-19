namespace ControleDeEstoqueAPI.Entities
{
    public class PaymentStatus : Entity<int>
    {
        public string Name { get; set; }
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
