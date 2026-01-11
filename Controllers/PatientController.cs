using Microsoft.AspNetCore.Mvc;
using Api.Data;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;


        public PatientController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var patients = _service.GetAll();
            return Ok(patients);
        }

        [HttpPost]
        public IActionResult Create(NewPatientDto dto)
        {
            return Ok(_service.Create(dto));
        }

        [HttpGet("{patientId}/allowed-menu")]
        public async Task<IActionResult> GetAllowedMenu(Guid patientId)
        {
            var menu = await _service.GetAllowedMenuAsync(patientId);
            return Ok(menu);
        }

    }
}
