namespace ControleDeEstoqueAPI.Entities
{
    public interface IAuditable
    {
        public DateTime DateTimeInclusion { get; set; }
        public string UserInclusion { get; set; }
        public DateTime DateTimeChange { get; set; }
        public string UserChange { get; set; }
        public bool IsActive { get; set; }
    }
}
