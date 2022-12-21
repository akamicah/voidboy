using DirectoryService.Core.Entities;

namespace DirectoryService.Core.RepositoryInterfaces;

public interface IPlaceRepository : IBaseRepository<Place>
{
    public Task<Place?> FindByName(string Name);
}