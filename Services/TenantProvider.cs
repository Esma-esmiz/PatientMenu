namespace Api.Services;

using Microsoft.AspNetCore.Http;

public class TenantProvider : ITenantProvider
{
     private readonly IHttpContextAccessor _accessor;

    public TenantProvider(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    public Guid TenantId
    {
        get
        {
           // Get tenant id from HttpContext
            if (_accessor.HttpContext?.Items["TenantId"] is Guid id)
            {
                Console.WriteLine($"TenantProvider: TenantId = {id}");
                return id;
            }

            Console.WriteLine("TenantProvider: TenantId not found, returning Guid.Empty");
            return Guid.Empty;
        }
    }
    
}
