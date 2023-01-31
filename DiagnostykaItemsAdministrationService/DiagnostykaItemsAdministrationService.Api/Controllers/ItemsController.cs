using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DiagnostykaItemsAdministrationService.Api.Controllers;

public class ItemsController : ApiBase
{

    /// <summary>
    /// Check
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    public async Task GetItemByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateItemAsync(CreateItemCommand createItemCommand)
    {
        var id = await Sender.Send(createItemCommand);
        return Ok(id);
    }
}
