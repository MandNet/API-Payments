using API_Payments.DTO;
using API_Payments.Models;

namespace API_Payments.Services
{
    public interface IRequestInterface
    {
        Task<ResponseDTO<List<RequestModel>>> List(int num = 0);
        Task<ResponseDTO<RequestModel>> Insert(RequestModel request);
        Task<ResponseDTO<RequestModel>> Update(RequestModel request);
        Task<ResponseDTO<RequestModel>> GetById(int requestId);
        Task<ResponseDTO<List<RequestModel>>> ListByProcessor(string Processor);
        Task<ResponseDTO<List<RequestModel>>> ListByStatus(int status);
        Task<ResponseDTO<List<RequestModel>>> MarkToBeProcessed();
        Task<ResponseDTO<List<RequestModel>>> MarkToBeProcessed(RequestModel request);
    }
}

