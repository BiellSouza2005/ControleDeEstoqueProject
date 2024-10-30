namespace ControleDeEstoqueAPI.Models
{
    public class OrderPayment
    {
        public int OrderId { get; set; }
        public int PaymentId { get; set; }

        public Order Order { get; set; }
        public Payment Payment { get; set; }
    }

}
