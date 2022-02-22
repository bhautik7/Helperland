using Helperland.Models;

namespace Helperland.Repository
{
    public interface IServiceRequestAddressRepository
    {
        ServiceRequestAddress Add(ServiceRequestAddress serviceRequestAddress);
    }
}
