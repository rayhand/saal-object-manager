using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMS.Application.Common.Models;
using OMS.Application.Objects.Commands.CreateObject;
using OMS.Application.Objects.Queries.GetObjectsWithPagination;
using OMS.Application.ObjectTypes.Queries.GetObjectsTypesWithPagination;

namespace OMS.WebUI.Controllers;

[Authorize]
public class ObjectsTypesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ObjectTypeDto>>> GetObjectTypesWithPaginationQuery([FromQuery] GetObjectTypesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateObjectTypeCommand command)
    {
        return await Mediator.Send(command);
    }
}
