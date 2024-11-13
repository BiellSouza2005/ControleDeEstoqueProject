namespace ControleDeEstoqueAPI.Entities
{
    public class PaymentHistory
    {
        public int PaymentHistoryId { get; set; }
        public int PaymentId { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal NewAmount { get; set; }
        public DateTime ModificationDate { get; set; }

        public Payment Payment { get; set; }
    }
}
