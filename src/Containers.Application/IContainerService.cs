using Containers.Models;

namespace Containers.Application;

public interface IContainerService
{
    public IEnumerable<Container> GetAllContainers();
}