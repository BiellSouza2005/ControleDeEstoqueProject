using ControleDeEstoqueAPI.Data.DTOs.OrderProduct;
using ControleDeEstoqueAPI.Data.DTOs.Payment;
using ControleDeEstoqueAPI.Data.DTOs.PaymentHistory;
using ControleDeEstoqueAPI.Entities;

namespace ControleDeEstoqueAPI.Data.DTOs.Order
{
    public class OrderDTO
    {
        public DateTime OrderDate { get; set; }
        public int ClientId { get; set; }

        public ICollection<OrderProductDTO> OrderItems { get; set; }

        public ICollection<PaymentDTO> OrderPayments { get; set; }

        public ICollection<PaymentHistoryDTO> PaymentsHistory { get; set; }
    }
}
