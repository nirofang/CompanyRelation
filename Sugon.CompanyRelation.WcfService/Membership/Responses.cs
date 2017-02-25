using System.Runtime.Serialization;
using System.Text;

namespace Sugon.CompanyRelation.WcfService.Membership
{
    [DataContract]
    public class LoginResponse : BaseResponse
    {
        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string TrueName { get; set; }

        [DataMember]
        public string UserPwd { get; set; }

        [DataMember]
        public StringBuilder UserRoles { get; set; }
    }

    [DataContract]
    public class ChangePasswordResponse : BaseResponse
    {
    }
}