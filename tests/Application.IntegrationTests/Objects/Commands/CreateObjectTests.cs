using FluentAssertions;
using NUnit.Framework;
using OMS.Application.Common.Exceptions;
using OMS.Application.Objects.Commands.CreateObject;

namespace OMS.Application.IntegrationTests.TodoItems.Commands;

using static Testing;

public class CreateObjectTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateObjectCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateObject()
    {
        var userId = await RunAsDefaultUserAsync();
        
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
