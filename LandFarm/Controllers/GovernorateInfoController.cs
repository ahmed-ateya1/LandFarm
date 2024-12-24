using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.OleDb;
using LandFarm.Dtos;

namespace LandFarm.Controllers
{
    /// <summary>
    /// Controller for managing government information.
    /// </summary>
    [ApiController]
    [Route("api/governments")]
    public class GovernorateInfoController : ControllerBase
    {
        private readonly string _connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\.NET Project\LandFarm\MainDB-App.accdb";

        /// <summary>
        /// Retrieves all governments from the database.
        /// </summary>
        /// <returns>A list of governments.</returns>
        [HttpGet]
        public IActionResult GetAllGovernments()
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "SELECT * FROM Governorate_Info";
                var dataTable = dbContext.ExecuteQuery(query);

                var result = new List<GovernmentInfo>();
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new GovernmentInfo
                    {
                        ID = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0,
                        Name = row["Name"]?.ToString() ?? string.Empty,
                        Type = row["Type"] != DBNull.Value ? Convert.ToInt32(row["Type"]) : 0,
                        Name_En = row["Name_En"]?.ToString() ?? string.Empty
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a government by its ID.
        /// </summary>
        /// <param name="id">The ID of the government to retrieve.</param>
        /// <returns>The government information, if found.</returns>
        [HttpGet("{id}")]
        public IActionResult GetGovernmentById(int id)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "SELECT * FROM Governorate_Info WHERE ID = @ID";

                var parameters = new[] { new OleDbParameter("@ID", id) };
                var dataTable = dbContext.ExecuteQuery(query, parameters);

                if (dataTable.Rows.Count == 0)
                    return NotFound("Government not found.");

                var row = dataTable.Rows[0];
                var government = new GovernmentInfo
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Name = row["Name"]?.ToString() ?? string.Empty,
                    Type = Convert.ToInt32(row["Type"]),
                    Name_En = row["Name_En"]?.ToString() ?? string.Empty
                };

                return Ok(government);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new government to the database.
        /// </summary>
        /// <param name="government">The government information to add.</param>
        /// <returns>A success or error message.</returns>
        [HttpPost]
        public IActionResult AddGovernment(GovernmentInfo government)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = @"INSERT INTO Governorate_Info (Name, Type, Name_En) 
                                 VALUES (@Name, @Type, @Name_En)";

                var parameters = new[]
                {
                    new OleDbParameter("@Name", government.Name ?? string.Empty),
                    new OleDbParameter("@Type", government.Type),
                    new OleDbParameter("@Name_En", government.Name_En ?? string.Empty)
                };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Government added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing government in the database.
        /// </summary>
        /// <param name="id">The ID of the government to update.</param>
        /// <param name="government">The updated government information.</param>
        /// <returns>A success or error message.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateGovernment(int id, GovernmentInfo government)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = @"UPDATE Governorate_Info SET 
                                 Name = @Name, 
                                 Type = @Type, 
                                 Name_En = @Name_En
                                 WHERE ID = @ID";

                var parameters = new[]
                {
                    new OleDbParameter("@Name", government.Name ?? string.Empty),
                    new OleDbParameter("@Type", government.Type),
                    new OleDbParameter("@Name_En", government.Name_En ?? string.Empty),
                    new OleDbParameter("@ID", id)
                };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Government updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a government from the database.
        /// </summary>
        /// <param name="id">The ID of the government to delete.</param>
        /// <returns>A success or error message.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteGovernment(int id)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "DELETE FROM Governorate_Info WHERE ID = @ID";

                var parameters = new[] { new OleDbParameter("@ID", id) };
                dbContext.ExecuteNonQuery(query, parameters);

                return Ok("Government deleted successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
