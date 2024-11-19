using Microsoft.Data.SqlClient;

namespace ControleDeEstoqueAPI.Services
{
    public interface IDataService
    {
        public string ConnectionString { get; set; }

        public SqlDataReader ReturnDataReader();
    }
}
