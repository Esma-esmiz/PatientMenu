using Microsoft.AspNetCore.Mvc;
using Api.Data;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/tenants")]
    public class TenantController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TenantController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_db.Tenants.ToList());

        [HttpPost]
        public IActionResult Create(TenantDto dto)
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Tenants.Add(tenant);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetAll), new { id = tenant.Id }, tenant);
        }
    }
}
