namespace ControleDeEstoqueAPI.Entities
{
    public enum PaymentStatus 
    { 
        Pendente,
        Vencido,
        Pago
    }
    public class Payment : Entity<int>
    {
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }
        public int OrderId { get; set; }
    }
}
