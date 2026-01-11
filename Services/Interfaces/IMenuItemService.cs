public interface IMenuItemService
{
    IEnumerable<MenuItem> GetAll();
    MenuItem Create(NewItemDto item);
  
}
