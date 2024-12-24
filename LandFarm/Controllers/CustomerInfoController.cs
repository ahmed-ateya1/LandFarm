using LandFarm;
using LandFarm.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.OleDb;

namespace LandFarm.Controllers
{
    /// <summary>
    /// Controller for managing customer information.
    /// </summary>
    [ApiController]
    [Route("api/customers")]
    public class CustomerInfoController : ControllerBase
    {
        private readonly string _connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\.NET Project\LandFarm\MainDB-App.accdb";

        /// <summary>
        /// Retrieves all customers from the database.
        /// </summary>
        /// <returns>A list of customers.</returns>
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "SELECT * FROM Customer_Info";
                var dataTable = dbContext.ExecuteQuery(query);

                var result = new List<CustomerInfo>();
                foreach (DataRow row in dataTable.Rows)
                {
                    result.Add(new CustomerInfo
                    {
                        ID = row["ID"] != DBNull.Value ? Convert.ToInt32(row["ID"]) : 0,
                        NameAr = row["NameAr"]?.ToString() ?? string.Empty,
                        NameEn = row["NameEn"]?.ToString() ?? string.Empty,
                        Type = row["Type"] != DBNull.Value ? Convert.ToInt32(row["Type"]) : 0,
                        Mail_text = row["Mail_text"]?.ToString() ?? string.Empty,
                        Address = row["Address"]?.ToString() ?? string.Empty,
                        Gov_COD = row["Gov_COD"] != DBNull.Value ? Convert.ToInt32(row["Gov_COD"]) : (int?)null,
                        ST_Date = row["ST_Date"] != DBNull.Value ? Convert.ToDateTime(row["ST_Date"]) : DateTime.MinValue
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
        /// Adds a new customer to the database.
        /// </summary>
        /// <param name="customer">The customer information to add.</param>
        /// <returns>A success or error message.</returns>
        [HttpPost]
        public IActionResult AddCustomer(CustomerInfo customer)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);

                string query = @"INSERT INTO Customer_Info 
                         (NameAr, NameEn, Type, Mail_text, Address, Gov_COD, ST_Date) 
                         VALUES (@NameAr, @NameEn, @Type, @Mail_text, @Address, @Gov_COD, @ST_Date)";

                var parameters = new[]
                {
                    new OleDbParameter("@NameAr", customer.NameAr ?? string.Empty),
                    new OleDbParameter("@NameEn", customer.NameEn ?? string.Empty),
                    new OleDbParameter("@Type", customer.Type),
                    new OleDbParameter("@Mail_text", customer.Mail_text ?? string.Empty),
                    new OleDbParameter("@Address", customer.Address ?? string.Empty),
                    new OleDbParameter("@Gov_COD", customer.Gov_COD.HasValue ? (object)customer.Gov_COD.Value : DBNull.Value),
                    new OleDbParameter("@ST_Date", customer.ST_Date != default ? (object)customer.ST_Date : DBNull.Value)
                };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Customer added successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing customer in the database.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customer">The updated customer information.</param>
        /// <returns>A success or error message.</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, CustomerInfo customer)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);

                string query = @"UPDATE Customer_Info SET 
                         NameAr = @NameAr, 
                         NameEn = @NameEn, 
                         Type = @Type, 
                         Mail_text = @Mail_text, 
                         Address = @Address, 
                         Gov_COD = @Gov_COD, 
                         ST_Date = @ST_Date
                         WHERE ID = @ID";

                var parameters = new[]
                {
                    new OleDbParameter("@NameAr", customer.NameAr ?? string.Empty),
                    new OleDbParameter("@NameEn", customer.NameEn ?? string.Empty),
                    new OleDbParameter("@Type", customer.Type),
                    new OleDbParameter("@Mail_text", customer.Mail_text ?? string.Empty),
                    new OleDbParameter("@Address", customer.Address ?? string.Empty),
                    new OleDbParameter("@Gov_COD", customer.Gov_COD.HasValue ? (object)customer.Gov_COD.Value : DBNull.Value),
                    new OleDbParameter("@ST_Date", customer.ST_Date != default ? (object)customer.ST_Date : DBNull.Value),
                    new OleDbParameter("@ID", id)
                };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Customer updated successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>A success or error message.</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            try
            {
                var dbContext = new AccessDbContext(_connectionString);
                string query = "DELETE FROM Customer_Info WHERE ID = @ID";
                var parameters = new[] { new OleDbParameter("@ID", id) };

                dbContext.ExecuteNonQuery(query, parameters);
                return Ok("Customer deleted successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
