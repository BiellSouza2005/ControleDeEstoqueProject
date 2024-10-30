namespace ControleDeEstoqueAPI.Models
{
    public class PaymentStatus
    {
        public int PaymentStatusId { get; set; }
        public string Name { get; set; }

        public ICollection<Payment> Payments { get; set; }
    }
}
