using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sugon.CompanyRelation.WcfService.Membership
{
    [ServiceContract]
    public interface IMembershipService : IService
    {
        [OperationContract]
        Task<LoginResponse> LoginAsync(LoginRequest request);

        [OperationContract]
        Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
