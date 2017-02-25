using System.Runtime.Serialization;

namespace Sugon.CompanyRelation.WcfService
{
    [DataContract]
    public abstract class BaseResponse
    {
        public BaseResponse()
        {
            Error = string.Empty;
            Success = false;
        }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Error { get; set; }
    }
}