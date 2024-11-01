namespace ControleDeEstoqueAPI.Data.DTOs.OrderProduct
{
    public class OrderProductDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
