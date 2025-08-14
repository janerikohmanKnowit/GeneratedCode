namespace ShoppingList.Api.Models;

public class ShoppingItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Text { get; set; } = string.Empty;
}
