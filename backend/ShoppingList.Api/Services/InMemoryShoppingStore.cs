using Models = ShoppingList.Api.Models;

namespace ShoppingList.Api.Services;

public class InMemoryShoppingStore : IShoppingStore
{
    private readonly List<Models.ShoppingList> _seed;
    private List<Models.ShoppingList> _state;
    private readonly object _lock = new();

    public InMemoryShoppingStore()
    {
        _seed = BuildSeed();
        _state = Clone(_seed);
    }

    public IEnumerable<Models.ShoppingList> GetLists() => _state;

    public Models.ShoppingList? GetList(Guid id) => _state.FirstOrDefault(l => l.Id == id);

    public Models.ShoppingList CreateList(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required", nameof(name));
        var list = new Models.ShoppingList { Name = name.Trim(), Items = new() };
        lock (_lock)
        {
            _state.Add(list);
        }
        return list;
    }

    public bool DeleteList(Guid id)
    {
        lock (_lock)
        {
            var idx = _state.FindIndex(l => l.Id == id);
            if (idx < 0) return false;
            _state.RemoveAt(idx);
            return true;
        }
    }

    public Models.ShoppingItem? AddItem(Guid listId, string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var list = GetList(listId);
        if (list is null) return null;
        var item = new Models.ShoppingItem { Text = text.Trim() };
        lock (_lock)
        {
            list.Items.Add(item);
        }
        return item;
    }

    public bool UpdateItem(Guid listId, Guid itemId, string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return false;
        var list = GetList(listId);
        if (list is null) return false;
        var item = list.Items.FirstOrDefault(i => i.Id == itemId);
        if (item is null) return false;
        lock (_lock)
        {
            item.Text = text.Trim();
        }
        return true;
    }

    public bool DeleteItem(Guid listId, Guid itemId)
    {
        var list = GetList(listId);
        if (list is null) return false;
        lock (_lock)
        {
            var idx = list.Items.FindIndex(i => i.Id == itemId);
            if (idx < 0) return false;
            list.Items.RemoveAt(idx);
            return true;
        }
    }

    public void Reset()
    {
        lock (_lock)
        {
            _state = Clone(_seed);
        }
    }

    private static List<Models.ShoppingList> BuildSeed()
    {
        return new List<Models.ShoppingList>
        {
            new() { Name = "Grocery list Friday", Items = new() { new() { Text = "Milk" }, new() { Text = "Bread" }, new() { Text = "Eggs" } } },
            new() { Name = "Weekend party", Items = new() { new() { Text = "Chips" }, new() { Text = "Salsa" }, new() { Text = "Soda" } } },
            new() { Name = "School start clothes", Items = new() { new() { Text = "Jeans" }, new() { Text = "Sneakers" }, new() { Text = "Jacket" } } }
        };
    }

    private static List<Models.ShoppingList> Clone(IEnumerable<Models.ShoppingList> lists)
    {
        return lists.Select(l => new Models.ShoppingList
        {
            Id = Guid.NewGuid(),
            Name = l.Name,
            Items = l.Items.Select(i => new Models.ShoppingItem { Id = Guid.NewGuid(), Text = i.Text }).ToList()
        }).ToList();
    }
}
