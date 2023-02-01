using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.DeleteItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.UpdateItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerResponse(StatusCodes.Status200OK, "Returns item model.", typeof(ItemDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item has not been found", typeof(BaseExceptionModel))]
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
    [SwaggerResponse(StatusCodes.Status201Created, "Returns new created item's id with route.", typeof(int))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Color has not been found", typeof(BaseExceptionModel))]
    [HttpPost]
    public async Task<IActionResult> CreateItemAsync(CreateItemCommand createItemCommand)
    {
        var itemId = await Sender.Send(createItemCommand);
        return Created(GetCreatedRoute(nameof(ItemsController), itemId), itemId);
    }

    /// <summary>
    /// Updates item.
    /// </summary>
    /// <param name="updateItemCommand">Item model.</param>
    /// <returns>no Content.</returns>
    [SwaggerResponse(StatusCodes.Status204NoContent, "Item updated. Returns No Content.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item or Color has not been found", typeof(BaseExceptionModel))]
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
    [SwaggerResponse(StatusCodes.Status204NoContent, "Item deleted. Returns No Content.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Item has not been found", typeof(BaseExceptionModel))]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItemByIdAsync(int id)
    {
        await Sender.Send(new DeleteItemCommand() { Id = id });
        return NoContent();
    }
}