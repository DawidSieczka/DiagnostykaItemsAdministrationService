namespace DiagnostykaItemsAdministrationService.Domain.Entities;

public class Item
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public virtual int? ColorId { get; set; }
    public virtual Color? Color { get; set; }
}