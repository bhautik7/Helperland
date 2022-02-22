using Helperland.Models;

namespace Helperland.Repository
{
    public class ServiceRequestAddressRepository:IServiceRequestAddressRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public ServiceRequestAddressRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

        public ServiceRequestAddress Add(ServiceRequestAddress serviceRequestAddress)
        {
            _helperlandContext.ServiceRequestAddress.Add(serviceRequestAddress);
            _helperlandContext.SaveChanges();
            return serviceRequestAddress;
        }
    }
}
