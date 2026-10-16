using _6trpo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using _6trpo.BD;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
namespace _6trpo.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : Controller
    {
  private readonly WebApiDatabaseContext _context;
        public ProductsController(WebApiDatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Employee>> HttpGetAllEmloye()
        {
            return await _context.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound("Сотрудник не найден");
            }

            return Ok(employee);
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            employee.Id = 0;
            _context.Employees.Add(employee);

            await _context.SaveChangesAsync();

            return Ok(employee);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEployee(
    int id, Employee updatedEmployee)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            employee.FullName = updatedEmployee.FullName;
            employee.Position = updatedEmployee.Position;
            employee.Salary = updatedEmployee.Salary;
            employee.HireDate = updatedEmployee.HireDate;
            employee.IsActive = updatedEmployee.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchEmployee(
    int id, [FromBody] JsonElement data)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            if (data.ValueKind != JsonValueKind.Object)
                return BadRequest("Ожидается JSON-объект");

            if (data.TryGetProperty("fullName", out var name))
                employee.FullName = name.GetString()!;

            if (data.TryGetProperty("position", out var position))
                employee.Position = position.GetString()!;

            if (data.TryGetProperty("salary", out var salary))
                employee.Salary = salary.GetDecimal();

            if (data.TryGetProperty("hireDate", out var hireDate))
                employee.HireDate = DateOnly.Parse(hireDate.GetString()!);

            if (data.TryGetProperty("isActive", out var isActive))
                employee.IsActive = isActive.GetBoolean();

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("filter")]
        public async Task<List<Employee>> FilterEmployees(
    [FromQuery] string? position,
    [FromQuery] decimal? minSalary)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(position))
            {
                query = query.Where(e => e.Position == position);
            }

            if (minSalary.HasValue)
            {
                query = query.Where(e => e.Salary >= minSalary.Value);
            }

            return await query.ToListAsync();
        }
    }
}
