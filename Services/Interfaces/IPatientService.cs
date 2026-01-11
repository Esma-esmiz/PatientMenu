public interface IPatientService
{
    IEnumerable<Patient> GetAll();
    Patient Create(NewPatientDto patient);
    Task<IEnumerable<MenuItem>> GetAllowedMenuAsync(Guid patientId);
}
