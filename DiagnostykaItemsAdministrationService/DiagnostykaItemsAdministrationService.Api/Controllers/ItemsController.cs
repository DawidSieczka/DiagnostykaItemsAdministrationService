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
}
