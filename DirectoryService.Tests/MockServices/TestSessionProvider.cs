using DirectoryService.Core.Entities;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Shared;

namespace DirectoryService.Tests.MockServices;

public class TestSessionProvider : ISessionProvider
{
    public async Task<Session?> GetRequesterSession()
    {
        // Relies on testSeed sql
        return new Session()
        {
            UserId = new Guid("6465b186-5fc9-46d2-842f-da8542ba9939"),
            AsAdmin = true,
            Role = UserRole.Admin,
            Scope = TokenScope.Owner,
            Token = new Guid("b4b7349b-40f8-40a2-a829-926f5d5f3124")
        };
    }
}