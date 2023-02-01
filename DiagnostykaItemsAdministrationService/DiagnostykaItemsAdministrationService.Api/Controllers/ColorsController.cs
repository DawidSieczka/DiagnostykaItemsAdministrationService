using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.CreateColor;
using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.DeleteColor;
using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.UpdateColor;
using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.GetAllColors;
using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.GetColorById;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DiagnostykaItemsAdministrationService.Api.Controllers;

/// <summary>
/// Endpoint for colors
/// </summary>
public class ColorsController : ApiBase
{
    /// <summary>
    /// Gets all Colors.
    /// </summary>
    /// <returns>List of colors.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "Returns list of colors.", typeof(IEnumerable<ColorDto>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Color has not been found", typeof(BaseExceptionModel))]
    [HttpGet]
    public async Task<IActionResult> GetAllColorsAsync()
    {
        var allColors = await Sender.Send(new GetAllColorsQuery());

        return Ok(allColors);
    }

    /// <summary>
    /// Get a specific color by id.
    /// </summary>
    /// <param name="id">Color's id type of int.</param>
    /// <returns>Color model.</returns>
    [SwaggerResponse(StatusCodes.Status200OK, "Returns color.", typeof(ColorDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Color has not been found", typeof(BaseExceptionModel))]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetColorByIdAsync(int id)
    {
        var color = await Sender.Send(new GetColorByIdQuery() { Id = id });
        return Ok(color);
    }


    /// <summary>
    /// Creates color.
    /// </summary>
    /// <param name="createColorCommand">Color's model.</param>
    /// <returns>color id of type int.</returns>
    [SwaggerResponse(StatusCodes.Status201Created, "Returns created color id with route.", typeof(int))]
    [HttpPost]
    public async Task<IActionResult> CreateColorAsync(CreateColorCommand createColorCommand)
    {
        var colorId = await Sender.Send(createColorCommand);
        return Created(GetCreatedRoute(nameof(ColorsController),colorId), colorId);
    }

    /// <summary>
    /// Deletes a specific color.
    /// </summary>
    /// <param name="id">Color Id of type int.</param>
    /// <returns>No content.</returns>
    [SwaggerResponse(StatusCodes.Status204NoContent, "Color deleted. Returns No Content.", typeof(IEnumerable<ColorDto>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Color has not been found", typeof(BaseExceptionModel))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteColorAsync(int id)
    {
        await Sender.Send(new DeleteColorCommand() { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Updates a specific color.
    /// </summary>
    /// <param name="updateColorCommand">Color's model.</param>
    /// <returns>No content.</returns>
    [SwaggerResponse(StatusCodes.Status204NoContent, "Color updated. Returns No Content.", typeof(IEnumerable<ColorDto>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Color has not been found", typeof(BaseExceptionModel))]
    [HttpPut]
    public async Task<IActionResult> UpdateColorAsync(UpdateColorCommand updateColorCommand)
    {
        await Sender.Send(updateColorCommand);
        return NoContent();
    }
}