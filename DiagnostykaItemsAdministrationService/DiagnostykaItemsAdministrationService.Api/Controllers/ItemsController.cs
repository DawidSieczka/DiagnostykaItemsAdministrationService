using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.DeleteItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.UpdateItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;
using Microsoft.AspNetCore.Mvc;

namespace DiagnostykaItemsAdministrationService.Api.Controllers;

/// <summary>
/// Controller for items.
/// </summary>
public class ItemsController : ApiBase
{
    /// <summary>
    /// Gets a specific item by id.
    /// </summary>
    /// <param name="id">Id of type int.</param>
    /// <returns>item model.</returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        var itemDto = await Sender.Send(new GetItemByIdQuery() { Id = id });
        return Ok(itemDto);
    }

    /// <summary>
    /// Creates item.
    /// </summary>
    /// <param name="createItemCommand">Item model.</param>
    /// <returns>Id of type int.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateItemAsync(CreateItemCommand createItemCommand)
    {
        var id = await Sender.Send(createItemCommand);
        return Ok(id);
    }

    /// <summary>
    /// Updates item.
    /// </summary>
    /// <param name="updateItemCommand">Item model.</param>
    /// <returns>no Content.</returns>
    [HttpPut]
    public async Task<ActionResult> UpdateItemAsync(UpdateItemCommand updateItemCommand)
    {
        await Sender.Send(updateItemCommand);
        return NoContent();
    }

    /// <summary>
    /// Deletes item.
    /// </summary>
    /// <param name="id">Item Id of type int.</param>
    /// <returns>no content.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItemByIdAsync(int id)
    {
        await Sender.Send(new DeleteItemCommand() { Id = id });
        return NoContent();
    }
}