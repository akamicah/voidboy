using DirectoryService.Core.Entities;

namespace DirectoryService.Core.Services.Interfaces;

public interface ISessionProvider
{
    public Task<Session?> GetRequesterSession();
}