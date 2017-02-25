using System.Runtime.Serialization;

namespace Sugon.CompanyRelation.WcfService
{
    [DataContract]
    public abstract class BaseRequest
    {
        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Roles { get; set; }

        [DataMember]
        public string DocCrt { get; set; }

        [DataMember]
        public string DocCrtTm { get; set; }
    }
}