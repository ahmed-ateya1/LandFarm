using LandFarm;
using LandFarm.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace LandFarm.Controllers
{
    /// <summary>
    /// Controller for managing project information.
    /// </summary>
    [ApiController]
    [Route("api/projects")]
    public class ProjectInfoController : ControllerBase
    {
        private readonly string _connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\.NET Project\LandFarm\MainDB-App.accdb";

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <returns>A list of all projects.</returns>
        [HttpGet("all")]
        public IActionResult GetAllProjects()
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "SELECT * FROM Project_Info";
                var dataTable = dbContext.ExecuteQuery(query);

                var result = new List<ProjectInfo>();
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new ProjectInfo
                    {
                        ID = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0,
                        ProjectNameAr = row["ProjectNameAr"]?.ToString() ?? string.Empty,
                        ProjectNameEn = row["ProjectNameEn"]?.ToString() ?? string.Empty,
                        Pro_Type = row["Pro_Type"] != DBNull.Value ? Convert.ToInt32(row["Pro_Type"]) : 0,
                        Pro_Place = row["Pro_Place"] != DBNull.Value ? Convert.ToInt32(row["Pro_Place"]) : 0,
                        St_Date = row["St_Date"] != DBNull.Value ? Convert.ToDateTime(row["St_Date"]) : DateTime.MinValue,
                        Customer_ID = row["Customer_ID"] != DBNull.Value ? Convert.ToInt32(row["Customer_ID"]) : 0,
                        St_Budget = row["St_Budget"]?.ToString() ?? string.Empty,
                        Gov_COD = row["Gov_COD"] != DBNull.Value ? Convert.ToInt32(row["Gov_COD"]) : 0,
                        Pro_Address = row["Pro_Address"]?.ToString() ?? string.Empty,
                        Pro_LocationDetail = row["Pro_LocationDetail"]?.ToString() ?? string.Empty,
                        Pro_Mail = row["Pro_Mail"]?.ToString() ?? string.Empty
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
        /// Adds a new project.
        /// </summary>
        /// <param name="project">The project information to add.</param>
        /// <returns>A success message if the operation is successful.</returns>
        [HttpPost("add")]
        public IActionResult AddProject(ProjectInfo project)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = @"INSERT INTO Project_Info 
                                 (ProjectNameAr, ProjectNameEn, Pro_Type, Pro_Place, St_Date, Customer_ID, St_Budget, Gov_COD, Pro_Address, Pro_LocationDetail, Pro_Mail) 
                                 VALUES (@ProjectNameAr, @ProjectNameEn, @Pro_Type, @Pro_Place, @St_Date, @Customer_ID, @St_Budget, @Gov_COD, @Pro_Address, @Pro_LocationDetail, @Pro_Mail)";

                var parameters = new[]
                {
                    new OleDbParameter("@ProjectNameAr", project.ProjectNameAr ?? string.Empty),
                    new OleDbParameter("@ProjectNameEn", project.ProjectNameEn ?? string.Empty),
                    new OleDbParameter("@Pro_Type", project.Pro_Type),
                    new OleDbParameter("@Pro_Place", project.Pro_Place),
                    new OleDbParameter("@St_Date", project.St_Date != default ? (object)project.St_Date : DBNull.Value) { OleDbType = OleDbType.Date },
                    new OleDbParameter("@Customer_ID", project.Customer_ID != 0 ? (object)project.Customer_ID : DBNull.Value),
                    new OleDbParameter("@St_Budget", project.St_Budget ?? string.Empty),
                    new OleDbParameter("@Gov_COD", project.Gov_COD),
                    new OleDbParameter("@Pro_Address", project.Pro_Address ?? string.Empty),
                    new OleDbParameter("@Pro_LocationDetail", project.Pro_LocationDetail ?? string.Empty),
                    new OleDbParameter("@Pro_Mail", project.Pro_Mail ?? string.Empty)
                };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Project added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="project">The updated project information.</param>
        /// <returns>A success message if the operation is successful.</returns>
        [HttpPut("update/{id}")]
        public IActionResult UpdateProject(int id, ProjectInfo project)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);

                if (project == null)
                    return BadRequest("Project object cannot be null.");

                string query = @"UPDATE Project_Info SET 
                                 ProjectNameAr = @ProjectNameAr, 
                                 ProjectNameEn = @ProjectNameEn, 
                                 Pro_Type = @Pro_Type, 
                                 Pro_Place = @Pro_Place, 
                                 St_Date = @St_Date, 
                                 Customer_ID = @Customer_ID, 
                                 St_Budget = @St_Budget, 
                                 Gov_COD = @Gov_COD, 
                                 Pro_Address = @Pro_Address, 
                                 Pro_LocationDetail = @Pro_LocationDetail, 
                                 Pro_Mail = @Pro_Mail
                                 WHERE ID = @ID";

                var parameters = new[]
                {
                    new OleDbParameter("@ProjectNameAr", project.ProjectNameAr ?? string.Empty),
                    new OleDbParameter("@ProjectNameEn", project.ProjectNameEn ?? string.Empty),
                    new OleDbParameter("@Pro_Type", project.Pro_Type),
                    new OleDbParameter("@Pro_Place", project.Pro_Place),
                    new OleDbParameter("@St_Date", project.St_Date != default ? (object)project.St_Date : DBNull.Value),
                    new OleDbParameter("@Customer_ID", project.Customer_ID != 0 ? (object)project.Customer_ID : DBNull.Value),
                    new OleDbParameter("@St_Budget", project.St_Budget ?? string.Empty),
                    new OleDbParameter("@Gov_COD", project.Gov_COD),
                    new OleDbParameter("@Pro_Address", project.Pro_Address ?? string.Empty),
                    new OleDbParameter("@Pro_LocationDetail", project.Pro_LocationDetail ?? string.Empty),
                    new OleDbParameter("@Pro_Mail", project.Pro_Mail ?? string.Empty),
                    new OleDbParameter("@ID", id)
                };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Project updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a project.
        /// </summary>
        /// <param name="id">The ID of the project to delete.</param>
        /// <returns>A success message if the operation is successful.</returns>
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteProject(int id)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "DELETE FROM Project_Info WHERE ID = @ID";
                var parameters = new[] { new OleDbParameter("@ID", id) };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Project deleted successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
