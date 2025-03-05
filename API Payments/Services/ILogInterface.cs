using API_Payments.DTO;
using API_Payments.Models;

namespace API_Payments.Services
{
    public interface ILogInterface
    {
        Task<ResponseDTO<List<LogModel>>> List(int num = 0);
        Task<ResponseDTO<LogModel>> Insert(LogModel log);
        Task<ResponseDTO<LogModel>> GetById(int logId);
    }
}


