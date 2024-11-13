namespace ControleDeEstoqueAPI.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        public ICollection<OrderPayment> OrderPayments { get; set; }
        public ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
