namespace ControleDeEstoqueAPI.Entities
{
    public class Login : Entity<int>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
