namespace ControleDeEstoqueAPI.Models
{
    public class Product
    {
            public int ProductId { get; set; }
            public string Name { get; set; }

            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public int BrandId { get; set; }
            public int ProductTypeId { get; set; }

            public Brand Brand { get; set; }
            public ProductType ProductType { get; set; }
            public ICollection<OrderProduct> OrderProducts { get; set; }
            public ProductDescription ProductDescription { get; set; }
    }
}
