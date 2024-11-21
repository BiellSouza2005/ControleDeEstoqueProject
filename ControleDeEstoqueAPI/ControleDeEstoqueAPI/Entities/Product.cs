using ControleDeEstoqueAPI.Entities;

namespace ControleDeEstoqueAPI.Entities
{
    public class Product : Entity<int>
    {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public int BrandId { get; set; }
            public int ProductTypeId { get; set; }
            public DateTime DateTimeInclusion { get; set; }
            public string UserInclusion { get; set; }
            public DateTime DateTimeChange { get; set; }
            public string UserChange { get; set; }
            public bool IsActive { get; set; }

            public Brand Brand { get; set; }
            public ProductType ProductType { get; set; }
            public ProductDescription ProductDescription { get; set; }
    }
}
