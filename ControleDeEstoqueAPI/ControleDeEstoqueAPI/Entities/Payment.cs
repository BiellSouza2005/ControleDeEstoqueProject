namespace ControleDeEstoqueAPI.Entities
{
    public class Payment : Entity<int>
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }
        public Order Order { get; set; }
        public ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
