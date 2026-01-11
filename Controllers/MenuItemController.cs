using Microsoft.AspNetCore.Mvc;
using Api.Data;



[ApiController]
[Route("api/menu-items")]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _service;



    public MenuItemController(IMenuItemService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var menuItem = _service.GetAll();
        return Ok(menuItem);
    }

    [HttpPost]
    public IActionResult Create(NewItemDto dto)
    {
        return Ok(_service.Create(dto));
    }

  
}
