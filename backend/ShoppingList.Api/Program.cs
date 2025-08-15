var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for Angular dev and SSR
const string CorsPolicy = "AllowAngular";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true) // permissive for demo
            .AllowCredentials()
    );
});

// In-memory store
builder.Services.AddSingleton<ShoppingList.Api.Services.IShoppingStore, ShoppingList.Api.Services.InMemoryShoppingStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);

// API endpoints
app.MapGet("/api/lists", (ShoppingList.Api.Services.IShoppingStore store) =>
{
    var lists = store.GetLists().Select(l => new ShoppingList.Api.Dtos.ShoppingListSummaryDto(l.Id, l.Name, l.Items.Count));
    return Results.Ok(lists);
}).WithName("GetLists").WithOpenApi();

app.MapGet("/api/lists/{id:guid}", (Guid id, ShoppingList.Api.Services.IShoppingStore store) =>
{
    var list = store.GetList(id);
    return list is null ? Results.NotFound() : Results.Ok(list);
}).WithName("GetList").WithOpenApi();

app.MapPost("/api/lists", async (HttpContext ctx, ShoppingList.Api.Services.IShoppingStore store) =>
{
    var dto = await ctx.Request.ReadFromJsonAsync<ShoppingList.Api.Dtos.ShoppingListCreateDto>();
    if (dto is null || string.IsNullOrWhiteSpace(dto.Name)) return Results.BadRequest(new { error = "Name required" });
    var created = store.CreateList(dto.Name);
    return Results.Created($"/api/lists/{created.Id}", created);
}).WithName("CreateList").WithOpenApi();

app.MapDelete("/api/lists/{id:guid}", (Guid id, ShoppingList.Api.Services.IShoppingStore store) =>
{
    return store.DeleteList(id) ? Results.NoContent() : Results.NotFound();
}).WithName("DeleteList").WithOpenApi();

app.MapPost("/api/lists/{id:guid}/items", async (Guid id, HttpContext ctx, ShoppingList.Api.Services.IShoppingStore store) =>
{
    var dto = await ctx.Request.ReadFromJsonAsync<ShoppingList.Api.Dtos.ShoppingItemCreateDto>();
    if (dto is null || string.IsNullOrWhiteSpace(dto.Text)) return Results.BadRequest(new { error = "Text required" });
    var item = store.AddItem(id, dto.Text);
    return item is null ? Results.NotFound() : Results.Created($"/api/lists/{id}/items/{item.Id}", item);
}).WithName("AddItem").WithOpenApi();

app.MapPut("/api/lists/{id:guid}/items/{itemId:guid}", async (Guid id, Guid itemId, HttpContext ctx, ShoppingList.Api.Services.IShoppingStore store) =>
{
    var dto = await ctx.Request.ReadFromJsonAsync<ShoppingList.Api.Dtos.ShoppingItemUpdateDto>();
    if (dto is null || string.IsNullOrWhiteSpace(dto.Text)) return Results.BadRequest(new { error = "Text required" });
    return store.UpdateItem(id, itemId, dto.Text) ? Results.NoContent() : Results.NotFound();
}).WithName("UpdateItem").WithOpenApi();

app.MapDelete("/api/lists/{id:guid}/items/{itemId:guid}", (Guid id, Guid itemId, ShoppingList.Api.Services.IShoppingStore store) =>
{
    return store.DeleteItem(id, itemId) ? Results.NoContent() : Results.NotFound();
}).WithName("DeleteItem").WithOpenApi();

app.MapPost("/api/reset", (ShoppingList.Api.Services.IShoppingStore store) =>
{
    store.Reset();
    return Results.NoContent();
}).WithName("Reset").WithOpenApi();
app.Run();
