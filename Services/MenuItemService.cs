using Microsoft.Extensions.Caching.Memory;
using Api.Data;
using AutoMapper;
using Api.Services;
using Api.Repositories;

public class MenuItemService : IMenuItemService
{
    private readonly AppDbContext _db;
    private readonly ITenantProvider _tenant;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    private const string CACHE_KEY = "MENUITEMS";

    public MenuItemService(
        AppDbContext db,
        ITenantProvider tenant,
        IMemoryCache cache,
        IMapper mapper)
    {
        _db = db;
        _tenant = tenant;
        _cache = cache;
        _mapper = mapper;
 
    }


    public IEnumerable<MenuItem> GetAll()
    {
        var key = $"{CACHE_KEY}_{_tenant.TenantId}";

        // Try to get from cache first
        if (_cache.TryGetValue(key, out List<MenuItem> cachedMenuItem))
        {
            Console.WriteLine($"MenuItem Load From Catch TenantId: {_tenant.TenantId}");
            return cachedMenuItem;
        }

        Console.WriteLine($"Cache miss, fetching MenuItem from DB for TenantId: {_tenant.TenantId}");

        // Fetch from DB
        var menuItemDB = _db.MenuItems
            .Where(p => p.TenantId == _tenant.TenantId)
            .ToList();

        // Store in cache
        _cache.Set(key, menuItemDB, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        return menuItemDB;
    }

    public MenuItem Create(NewItemDto dto)
    {
        var item = _mapper.Map<MenuItem>(dto);

        item.TenantId = _tenant.TenantId;

        _db.MenuItems.Add(item);
        _db.SaveChanges();

        _cache.Remove($"{CACHE_KEY}_{_tenant.TenantId}");

        return item;
    }


}
