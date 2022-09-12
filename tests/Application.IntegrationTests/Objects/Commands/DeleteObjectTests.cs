using FluentAssertions;
using NUnit.Framework;
using OMS.Application.Common.Exceptions;
using OMS.Application.Objects.Commands.CreateObject;
using OMS.Application.Objects.Commands.DeleteObject;

namespace OMS.Application.IntegrationTests.TodoItems.Commands;

using static Testing;

public class DeleteObjectTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidObjectId()
    {
        var command = new DeleteObjectCommand(Guid.NewGuid());

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteObject()
    {
        var objectTypeId = await SendAsync(new CreateObjectTypeCommand
        {
            Name = "Type1",
        });

        var command = new CreateObjectCommand
        {
            Name = "Object1",
            Description = "Description",
            ObjectTypeId = objectTypeId
        };
        var objectId = await SendAsync(command);

        await SendAsync(new DeleteObjectCommand(objectId));

        var item = await FindAsync<Domain.Entities.Object>(objectId);

        item.Should().BeNull();
    }
}
