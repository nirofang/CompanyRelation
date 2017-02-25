using System.Runtime.Serialization;

namespace Sugon.CompanyRelation.WcfService.Common
{
    [DataContract]
    public class GenerateIDResponse : BaseResponse
    {
        public GenerateIDResponse()
        {
            this.ID = "";
        }

        [DataMember]
        public string ID { get; set; }
    }
}