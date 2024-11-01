namespace ControleDeEstoqueAPI.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ClientId { get; set; }
        public int PaymentStatusId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public Client Client { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
        public ICollection<OrderPayment> OrderPayments { get; set; }
    }
}
