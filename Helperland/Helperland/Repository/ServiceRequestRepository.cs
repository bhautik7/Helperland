using Helperland.Models;

namespace Helperland.Repository
{
    public class ServiceRequestRepository:IServiceRequestRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public ServiceRequestRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

        public ServiceRequest Add(ServiceRequest serviceRequest)
        {
            _helperlandContext.ServiceRequest.Add(serviceRequest);
            _helperlandContext.SaveChanges();
            return serviceRequest;
        }
    }
}
