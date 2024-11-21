using ControleDeEstoqueAPI.Entities;

namespace ControleDeEstoqueAPI.Entities
{
    public class Order : Entity<int>
    {
        public Order() 
        { 
            OrderProducts = new List<OrderProduct>();        
        }  

        public DateTime OrderDate { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string OrderPaymentStatus
        {
            get { return Payments != null ? $"{Payments.Where(x => x.PaymentStatus == PaymentStatus.Pago).LongCount()}/{Payments.Count}":"0/0"; }
        }

        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
