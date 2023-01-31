namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.Models;

public class ItemDto
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public ColorDto Color { get; set; }
}