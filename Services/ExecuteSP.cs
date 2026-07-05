using Microsoft.Data.SqlClient;
using System.Data;

namespace HotelProject.Services
{
	public class ExecuteSP
	{
		private readonly IConfiguration _configuration;
		public ExecuteSP(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task<DataTable> ExecuteSPAsync(string spName, params SqlParameter[] parameters)
		{
			using var conn = new SqlConnection(
				_configuration.GetConnectionString("DefaultConnection"));
			try
			{
				var dt = new DataTable();

				using var cmd = new SqlCommand(spName, conn);
				cmd.CommandType = CommandType.StoredProcedure;

				if (parameters != null)
					cmd.Parameters.AddRange(parameters);

				await conn.OpenAsync();

				using var reader = await cmd.ExecuteReaderAsync();
				dt.Load(reader);

				return dt;
			}
			finally
			{
				if (conn.State != ConnectionState.Closed)
					await conn.CloseAsync();
			}
		}
	}
}
