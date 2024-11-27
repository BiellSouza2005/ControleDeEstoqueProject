using ControleDeEstoqueAPI.Data.DTOs.OrderProduct;
using ControleDeEstoqueAPI.Data.DTOs.Payment;
using ControleDeEstoqueAPI.Entities;

namespace ControleDeEstoqueAPI.Data.DTOs.Order
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int ClientId { get; set; }

        public ICollection<OrderProductDTO> OrderItems { get; set; }
        public ICollection<PaymentDTO> OrderPayments { get; set; }

    }
}
