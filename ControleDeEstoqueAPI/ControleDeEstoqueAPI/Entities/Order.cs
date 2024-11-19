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
        public int PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Client Client { get; set; }
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        //public ICollection<OrderPayment> OrderPayments { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
