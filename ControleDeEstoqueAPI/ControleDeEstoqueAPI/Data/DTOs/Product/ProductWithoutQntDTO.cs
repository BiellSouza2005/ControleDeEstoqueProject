namespace ControleDeEstoqueAPI.Data.DTOs.Product
{
    public class ProductWhithoutQntDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
    }
}
