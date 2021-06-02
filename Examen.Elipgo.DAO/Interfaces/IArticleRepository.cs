using Examen.Elipgo.DAO.Models;
using Examen.Elipgo.DAO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Examen.Elipgo.DAO.Interfaces
{
    public interface IArticleRepository
    {
        Task<StatusResponse<IEnumerable<ArticleDAO>>> Get();
        Task<StatusResponse<ArticleDAO>> Get(int id);
        Task<StatusResponse<List<ArticleDAO>>> Get(List<KeyValuePair<string, string>> paramList);
        Task<StatusResponse<IEnumerable<ArticleDAO>>> GetArticlesByStore(int storeId);
        Task<StatusResponse> Post(ArticleDAO articleDao);
        Task<StatusResponse> Put(ArticleDAO articleDao);
        Task<StatusResponse> Delete(int id);
    }
}
