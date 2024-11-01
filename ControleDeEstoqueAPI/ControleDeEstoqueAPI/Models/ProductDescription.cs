namespace ControleDeEstoqueAPI.Models
{
    public class ProductDescription
    {
        public int DescriptionID { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }

        public Product Product { get; set; }
    }
}
