namespace DiagnostykaItemsAdministrationService.Domain.Entities;

public class Color
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<Item> Items { get; set; } = new List<Item>();
}