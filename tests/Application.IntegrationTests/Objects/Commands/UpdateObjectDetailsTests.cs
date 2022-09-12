using FluentAssertions;
using NUnit.Framework;
using OMS.Application.Common.Exceptions;
using OMS.Application.Objects.Commands.CreateObject;
using OMS.Application.Objects.Commands.UpdateObject;
using OMS.Domain.Entities;

namespace OMS.Application.IntegrationTests.TodoItems.Commands;

using static Testing;

public class UpdateObjectDetailsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidObjectId()
    {
        var objectType = new ObjectType { Name = "Type1" };
        await AddAsync(objectType);

        var command = new UpdateObjectCommand { 
            Id = Guid.NewGuid(), 
            Name = "Name",
            Description = "Description",
            ObjectTypeId = objectType.Id
        };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldUpdateObject()
    {
        var userId = await RunAsDefaultUserAsync();

        var objectTypeId = await SendAsync(new CreateObjectTypeCommand
        {
            Name = "Type1",
        });
        var objectTypeId2 = await SendAsync(new CreateObjectTypeCommand
        {
            Name = "Type2",
        });

        var objectId = await SendAsync(new CreateObjectCommand
        {
            Name = "Object1",
            Description = "Description",
            ObjectTypeId = objectTypeId
        });

        var command = new UpdateObjectCommand
        {
            Id = objectId,
            Name = "Object2",
            Description = "Description2",
            ObjectTypeId = objectTypeId2
        };

        await SendAsync(command);

        var item = await FindAsync<Domain.Entities.Object>(objectId);

        item.Should().NotBeNull();
        item!.Name.Should().Be(command.Name);
        item.Description.Should().Be(command.Description);
        item.ObjectTypeId.Should().Be(command.ObjectTypeId);
        item.CreatedBy.Should().Be(userId);
        item.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
