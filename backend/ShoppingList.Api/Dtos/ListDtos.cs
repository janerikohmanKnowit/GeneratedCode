namespace ShoppingList.Api.Dtos;

public record ShoppingListSummaryDto(Guid Id, string Name, int ItemCount);
public record ShoppingListCreateDto(string Name);
public record ShoppingItemCreateDto(string Text);
public record ShoppingItemUpdateDto(string Text);
