using Sugon.CompanyRelation.WcfService.Common;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Sugon.CompanyRelation.WcfService.StockRights
{
    [ServiceContract]
    public interface IStockRightsService : IService
    {
        [OperationContract]
        Task<GenerateIDResponse> GenerateIDAsync(GenerateIDRequest request);

        [OperationContract]
        Task<GetCompanyListResponse> GetCompanyListAsync(GetCompanyListRequest request);
    }
}
