using Models = global::ShoppingList.Api.Models;

namespace ShoppingList.Api.Services;

public interface IShoppingStore
{
    IEnumerable<Models.ShoppingList> GetLists();
    Models.ShoppingList? GetList(Guid id);
    Models.ShoppingList CreateList(string name);
    bool DeleteList(Guid id);

    Models.ShoppingItem? AddItem(Guid listId, string text);
    bool UpdateItem(Guid listId, Guid itemId, string text);
    bool DeleteItem(Guid listId, Guid itemId);

    void Reset();
}
