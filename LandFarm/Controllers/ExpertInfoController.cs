using LandFarm;
using LandFarm.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.OleDb;

namespace LandFarm.Controllers
{
    /// <summary>
    /// Controller for managing expert information.
    /// </summary>
    [ApiController]
    [Route("api/experts")]
    public class ExpertInfoController : ControllerBase
    {
        private readonly string _connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\.NET Project\LandFarm\MainDB-App.accdb";

        /// <summary>
        /// Retrieves all experts from the database.
        /// </summary>
        /// <returns>A list of experts.</returns>
        [HttpGet]
        public IActionResult GetAllExperts()
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "SELECT * FROM Expert_Info";
                var dataTable = dbContext.ExecuteQuery(query);

                var experts = dataTable.AsEnumerable().Select(row => new ExpertInfo
                {
                    ID = row.Field<int?>("ID") ?? 0,
                    ExpertNameAr = row.Field<string>("ExpertNameAr") ?? string.Empty,
                    ExpertNameEn = row.Field<string>("ExpertNameEn") ?? string.Empty,
                    Type = row.Field<string>("Type") ?? string.Empty,
                    Mail_text = row.Field<string>("Mail_text") ?? string.Empty,
                    Address = row.Field<string>("Address") ?? string.Empty,
                    Gov_Code = row.Field<int?>("Gov_COD") ?? 0,
                    Education_DegreeCOD = row.Field<int?>("Education_DegreeCOD") ?? 0,
                    Specialist_COD = row.Field<int?>("Specialist_COD") ?? 0
                }).ToList();

                return Ok(experts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new expert to the database.
        /// </summary>
        /// <param name="expert">The expert information to add.</param>
        /// <returns>A success or error message.</returns>
        [HttpPost]
        public IActionResult AddExpert(ExpertInfo expert)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = @"INSERT INTO Expert_Info 
                                 (ExpertNameAr, ExpertNameEn, Type, Mail_text, Address, Gov_COD, Education_DegreeCOD, Specialist_COD) 
                                 VALUES (@ExpertNameAr, @ExpertNameEn, @Type, @Mail_text, @Address, @Gov_COD, @Education_DegreeCOD, @Specialist_COD)";

                var parameters = new[]
                {
                    new OleDbParameter("@ExpertNameAr", expert.ExpertNameAr ?? string.Empty),
                    new OleDbParameter("@ExpertNameEn", expert.ExpertNameEn ?? string.Empty),
                    new OleDbParameter("@Type", expert.Type ?? string.Empty),
                    new OleDbParameter("@Mail_text", expert.Mail_text ?? string.Empty),
                    new OleDbParameter("@Address", expert.Address ?? string.Empty),
                    new OleDbParameter("@Gov_COD", expert.Gov_Code),
                    new OleDbParameter("@Education_DegreeCOD", expert.Education_DegreeCOD),
                    new OleDbParameter("@Specialist_COD", expert.Specialist_COD)
                };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Expert added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing expert in the database.
        /// </summary>
        /// <param name="id">The ID of the expert to update.</param>
        /// <param name="expert">The updated expert information.</param>
        /// <returns>A success or error message.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateExpert(int id, ExpertInfo expert)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = @"UPDATE Expert_Info SET 
                                 ExpertNameAr = @ExpertNameAr, 
                                 ExpertNameEn = @ExpertNameEn, 
                                 Type = @Type, 
                                 Mail_text = @Mail_text, 
                                 Address = @Address, 
                                 Gov_COD = @Gov_COD, 
                                 Education_DegreeCOD = @Education_DegreeCOD,
                                 Specialist_COD = @Specialist_COD
                                 WHERE ID = @ID";

                var parameters = new[]
                {
                    new OleDbParameter("@ExpertNameAr", expert.ExpertNameAr ?? string.Empty),
                    new OleDbParameter("@ExpertNameEn", expert.ExpertNameEn ?? string.Empty),
                    new OleDbParameter("@Type", expert.Type ?? string.Empty),
                    new OleDbParameter("@Mail_text", expert.Mail_text ?? string.Empty),
                    new OleDbParameter("@Address", expert.Address ?? string.Empty),
                    new OleDbParameter("@Gov_COD", expert.Gov_Code),
                    new OleDbParameter("@Education_DegreeCOD", expert.Education_DegreeCOD),
                    new OleDbParameter("@Specialist_COD", expert.Specialist_COD),
                    new OleDbParameter("@ID", id)
                };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Expert updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an expert from the database.
        /// </summary>
        /// <param name="id">The ID of the expert to delete.</param>
        /// <returns>A success or error message.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteExpert(int id)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "DELETE FROM Expert_Info WHERE ID = @ID";
                var parameters = new[] { new OleDbParameter("@ID", id) };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Expert deleted successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
