namespace ControleDeEstoqueAPI.Data.DTOs.Order
{
    public class OrderDTO
    {
        public DateTime OrderDate { get; set; }
        public int ClientId { get; set; }
        public int PaymentStatusId { get; set; }
    }
}
