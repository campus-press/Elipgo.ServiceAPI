using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Examen.Elipgo.DAO.Models;
using Examen.Elipgo.DAO.Response;

namespace Examen.Elipgo.DAO.Interfaces
{
    public interface IStoreRepository
    {
        Task<StatusResponse<IEnumerable<StoreDAO>>> Get();
        Task<StatusResponse<StoreDAO>> Get(int id);
        Task<StatusResponse> Post(StoreDAO articleDao);
        Task<StatusResponse> Put(StoreDAO articleDao);
        Task<StatusResponse> Delete(int id);
    }
}
