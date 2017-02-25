using System.Runtime.Serialization;

namespace Sugon.CompanyRelation.WcfService.Membership
{
    [DataContract]
    public class LoginRequest : BaseRequest
    {
    }

    [DataContract]
    public class ChangePasswordRequest : BaseRequest
    {
        [DataMember]
        public string NewPwd { get; set; }
    }
}