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
    public async Task UserGroupCreationAndUpdateTest()
    {
        var userService = _factory!.Services.GetRequiredService<UserService>();
        var userGroupService = _factory!.Services.GetRequiredService<UserGroupService>();
        var user = await userService.FindUser("testadmin");
        
        Assert.That(user, Is.Not.Null);
        
        var group = await userGroupService.Create(user!.Id, new UserGroup()
        {
            Name = "Test Group",
            Description = "This is my test group",
            Rating = MaturityRating.Everyone,
            Internal = false
        });
        
        
        Assert.That(group, Is.Not.Null);

        // Ensure we can retrieve the group
        group = await userGroupService.FindById(group.Id);
        
        Assert.That(group, Is.Not.Null);
        
        Assert.Multiple(() =>
        {
            Assert.That(group.Name, Is.EqualTo("Test Group"));
            Assert.That(group.Description, Is.EqualTo("This is my test group"));
            Assert.That(group.Rating, Is.EqualTo(MaturityRating.Everyone));
        });

        // Update group with new values
        group.Rating = MaturityRating.Adult;
        group.Name = "Adult Group";
        group.Description = "My Adult Group";

        group = await userGroupService.Update(group);
        
        Assert.That(group, Is.Not.Null);
        
        Assert.Multiple(() =>
        {
            Assert.That(group.Name, Is.EqualTo("Adult Group"));
            Assert.That(group.Description, Is.EqualTo("My Adult Group"));
            Assert.That(group.Rating, Is.EqualTo(MaturityRating.Adult));
        });

        
        // Delete group and verify deletion
        await userGroupService.Delete(group);
        
        group = await userGroupService.FindById(group.Id);
        
        Assert.That(group, Is.Null);

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
        
        // Should do nothing as user already a member
        await userGroupService.AddGroupMember(newGroup!.Id, user2!.Id);
        
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

        // Delete group
        await userGroupService.Delete(newGroup);

    }
}