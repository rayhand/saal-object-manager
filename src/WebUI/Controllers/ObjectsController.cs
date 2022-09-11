using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OMS.Application.Common.Models;
using OMS.Application.Objects.Commands.CreateObject;
using OMS.Application.Objects.Commands.DeleteObject;
using OMS.Application.Objects.Commands.UpdateObject;
using OMS.Application.Objects.Queries.GetObjectDetails;
using OMS.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace OMS.WebUI.Controllers;

[Authorize]
public class ObjectsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<ObjectBriefDto>>> GetObjectsWithPagination([FromQuery] GetObjectsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ObjectDto>> GetObjectDetails(Guid id)
    {
        return await Mediator.Send(new GetObjectDetailsQuery { Id = id });
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateObjectCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateObjectCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteObjectCommand(id));
        return NoContent();
    }
}
