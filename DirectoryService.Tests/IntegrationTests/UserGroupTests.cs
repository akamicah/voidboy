using DirectoryService.Core.Entities;
using DirectoryService.Core.Services;
using DirectoryService.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Tests.IntegrationTests;

public class UserGroupTests : TestBase
{
    [OneTimeSetUp]
    public void Setup()
    {
        TestSetup();
        
    }

    [Test]
    public async Task UserGroupMembershipsTest()
    {
        var userService = _factory!.Services.GetRequiredService<UserService>();
        var userGroupService = _factory!.Services.GetRequiredService<UserGroupService>();
        var user1 = await userService.FindUser("testadmin");
        var user2 = await userService.FindUser("user");
        var user3 = await userService.FindUser("user2");
        
        Assert.Multiple(() =>
        {
            Assert.That(user1, Is.Not.Null);
            Assert.That(user2, Is.Not.Null);
            Assert.That(user3, Is.Not.Null);
        });

        // Create a new group
        var newGroup = await userGroupService.Create(user1!.Id, new UserGroup()
        {
            Name = "Test Group",
            Description = "This is my test group",
            Rating = MaturityRating.Everyone,
            Internal = false
        });
        
        Assert.That(newGroup, Is.Not.Null);
        
        // Add Members
        await userGroupService.AddGroupMembers(newGroup!.Id, new []{ user2!.Id, user3!.Id });
        
        // Retrieve Members
        var members = await userGroupService.GetGroupMembers(newGroup!.Id, PaginatedRequest.All());
        
        // Should contain owner and two new members
        Assert.That(members.Data!.Count, Is.EqualTo(3));
        
        // Delete Members
        await userGroupService.RemoveGroupMembers(newGroup.Id, members.Data.Select(u => u.Id));
        
        // Retrieve Members
        members = await userGroupService.GetGroupMembers(newGroup!.Id, PaginatedRequest.All());
        
        // Should contain only owner as owner can't be removed
        Assert.That(members.Data!.Count, Is.EqualTo(1));

    }
}