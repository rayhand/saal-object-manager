using OMS.Application.Common.Mappings;
using OMS.Domain.Entities;

namespace OMS.Application.Common.Models;

// Note: This is currently just used to demonstrate applying multiple IMapFrom attributes.
public class LookupDto : IMapFrom<TodoList>, IMapFrom<TodoItem>
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
}
