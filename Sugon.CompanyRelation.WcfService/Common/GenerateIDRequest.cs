using System.Runtime.Serialization;

namespace Sugon.CompanyRelation.WcfService.Common
{
    [DataContract]
    public class GenerateIDRequest : BaseRequest
    {
        public GenerateIDRequest()
        {
            DocCode = string.Empty;
        }

        [DataMember]
        public string DocCode { get; set; }
    }
}