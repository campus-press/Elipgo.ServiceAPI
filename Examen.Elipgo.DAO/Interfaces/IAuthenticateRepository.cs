using Examen.Elipgo.DAO.Models;
using Examen.Elipgo.DAO.Response;
using System.Threading.Tasks;

namespace Examen.Elipgo.DAO.Interfaces
{
    public interface IAuthenticateRepository
    {
        Task<StatusResponse<object>> Login(LoginDAO model);
    }
}
