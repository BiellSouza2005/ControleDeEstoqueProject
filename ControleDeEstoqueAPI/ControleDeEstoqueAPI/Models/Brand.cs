namespace ControleDeEstoqueAPI.Models
{
    using System.Collections.Generic;

    public class Brand
    {
        public int BrandId { get; set; }
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}
