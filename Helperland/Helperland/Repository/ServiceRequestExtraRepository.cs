using Helperland.Models;

namespace Helperland.Repository
{
    public class ServiceRequestExtraRepository:IServiceRequestExtraRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public ServiceRequestExtraRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

        public ServiceRequestExtra Add(ServiceRequestExtra serviceRequestExtra)
        {
            _helperlandContext.ServiceRequestExtra.Add(serviceRequestExtra);
            _helperlandContext.SaveChanges();
            return serviceRequestExtra;
        }
    }
}
