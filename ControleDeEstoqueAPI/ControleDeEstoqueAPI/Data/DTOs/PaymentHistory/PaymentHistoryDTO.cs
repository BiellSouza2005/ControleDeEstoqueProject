namespace ControleDeEstoqueAPI.Data.DTOs.PaymentHistory
{
    public class PaymentHistoryDTO
    {
        public int PaymentId { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal NewAmount { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
