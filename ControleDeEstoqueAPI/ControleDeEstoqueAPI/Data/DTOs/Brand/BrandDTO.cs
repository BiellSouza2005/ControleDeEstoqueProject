using System.ComponentModel.DataAnnotations;

namespace ControleDeEstoqueAPI.Data.DTOs.Brand
{
    public class BrandDTO
    {
        public string Name { get; set; }

        public BrandDTO(string name)
        {
            Name = name;
        }
    }


}
