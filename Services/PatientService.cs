using Microsoft.Extensions.Caching.Memory;
using Api.Data;
using AutoMapper;
using Api.Services;
using Api.Repositories;

public class PatientService : IPatientService
{
    private readonly AppDbContext _db;
    private readonly ITenantProvider _tenant;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;
    private readonly IMenuIteamRepository _itemRepo;

    private const string CACHE_KEY = "PATIENTS";

    public PatientService(
        AppDbContext db,
        ITenantProvider tenant,
        IMemoryCache cache,
        IMapper mapper,
        IMenuIteamRepository repo)
    {
        _db = db;
        _tenant = tenant;
        _cache = cache;
        _mapper = mapper;
        _itemRepo = repo;
    }

    public async Task<IEnumerable<MenuItem>> GetAllowedMenuAsync(Guid patientId)
    {
        return await _itemRepo.GetAllowedMenuAsync(patientId);
    }

    public IEnumerable<Patient> GetAll()
    {
        var key = $"{CACHE_KEY}_{_tenant.TenantId}";

        // Try to get from cache first
        if (_cache.TryGetValue(key, out List<Patient> cachedPatients))
        {
            Console.WriteLine($"Patinet Cache hit for TenantId: {_tenant.TenantId}");
            return cachedPatients;
        }

        Console.WriteLine($"Cache miss, fetching from DB for TenantId: {_tenant.TenantId}");

        // Fetch from DB
        var patientsFromDb = _db.Patients
            .Where(p => p.TenantId == _tenant.TenantId)
            .ToList();

        // Store in cache
        _cache.Set(key, patientsFromDb, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        return patientsFromDb;
    }

    public Patient Create(NewPatientDto dto)
    {
        var patient = _mapper.Map<Patient>(dto);

        patient.TenantId = _tenant.TenantId;

        _db.Patients.Add(patient);
        _db.SaveChanges();

        _cache.Remove($"{CACHE_KEY}_{_tenant.TenantId}");

        return patient;
    }
}
