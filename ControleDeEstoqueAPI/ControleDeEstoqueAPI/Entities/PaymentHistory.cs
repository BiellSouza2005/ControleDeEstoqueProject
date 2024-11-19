namespace ControleDeEstoqueAPI.Entities
{
    public class PaymentHistory : Entity<int>
    {
        public int PaymentId { get; set; }
        public decimal PreviousAmount { get; set; }
        public decimal NewAmount { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }

        public Payment Payment { get; set; }
    }
}
