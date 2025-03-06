using API_Payments.Data;
using API_Payments.DTO;
using API_Payments.Enum;
using API_Payments.Models;
using API_Payments.Utilities;
using Microsoft.EntityFrameworkCore;

namespace API_Payments.Services
{
    public class RequestService : IRequestInterface
    {
        private readonly AppDbContext _context;
        public RequestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDTO<RequestModel>> GetById(int requestId)
        {
            ResponseDTO<RequestModel> resp = new ResponseDTO<RequestModel>();
            try
            {
                var request = await _context.TRequests.FirstOrDefaultAsync(requestbd => requestbd.Id == requestId);
                if (request == null)
                {
                    resp.Data = null;
                    resp.Message = "No request found";
                    return resp;
                }

                request.Card = EncryptionUtility.DecryptNew(request.Card);
                resp.Data = request;
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<RequestModel>> Insert(RequestModel request)
        {
            ResponseDTO<RequestModel> resp = new ResponseDTO<RequestModel>();
            try
            {
                request.Card = Utilities.Utilities.OnlyNumbers(request.Card);
                request.Card = EncryptionUtility.EncryptNew(request.Card);

                _context.ChangeTracker.Clear();
                await _context.AddAsync(request);
                var ret = await _context.SaveChangesAsync();

                request.Card = EncryptionUtility.DecryptNew(request.Card);

                resp.Data = request;
                resp.Message = "Request successfully inserted";
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<List<RequestModel>>> List(int num = 0)
        {
            ResponseDTO<List<RequestModel>> resp = new ResponseDTO<List<RequestModel>>();
            try
            {
                List<RequestModel> requests = new List<RequestModel>();
                if (num > 0)
                {
                    requests = (List<RequestModel>)_context.TRequests.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .Take(num)
                                                                     .ToList();
                }
                else
                {
                    requests = (List<RequestModel>)_context.TRequests.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .ToList();
                }

                for (int i = 0; i < requests.Count; i++)
                {
                    requests[i].Card = EncryptionUtility.DecryptNew(requests[i].Card);
                }

                resp.Success = true;
                resp.Data = requests;
                if (num > 0)
                    resp.Message = "List of the last " + num.ToString() + " requests retrieved successfully (" + requests.Count.ToString() + ")";
                else
                    resp.Message = "List of all requests retrieved successfully (" + requests.Count.ToString() + ")";
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }


        public async Task<ResponseDTO<List<RequestModel>>> ListByProcessor(string Processor)
        {
            ResponseDTO<List<RequestModel>> resp = new ResponseDTO<List<RequestModel>>();
            try
            {
                List<RequestModel> requests = new List<RequestModel>();
                requests = (List<RequestModel>)_context.TRequests.ToListAsync().Result
                                                                    .Where(requestbd => requestbd.ProcessorCode == Processor)
                                                                    .ToList();

                for (int i = 0; i < requests.Count; i++)
                {
                    requests[i].Card = EncryptionUtility.DecryptNew(requests[i].Card);
                }

                resp.Success = true;
                resp.Data = requests;
                resp.Message = "List of requsts with processor code " + Processor + "(" + requests.Count.ToString() + ")";
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<List<RequestModel>>> ListByStatus(int status)
        {
            ResponseDTO<List<RequestModel>> resp = new ResponseDTO<List<RequestModel>>();
            try
            {
                List<RequestModel> requests = new List<RequestModel>();
                requests = (List<RequestModel>)_context.TRequests.ToListAsync().Result
                                                                    .Where(requestbd => requestbd.Status == status)
                                                                    .ToList();

                for (int i = 0; i < requests.Count; i++)
                {
                    requests[i].Card = EncryptionUtility.DecryptNew(requests[i].Card);
                }

                resp.Success = true;
                resp.Data = requests;
                resp.Message = "List of requsts with processor status " + RequestStatusEnumDescription.GetDescription(status) + "(" + requests.Count.ToString() + ")";
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<List<RequestModel>>> MarkToBeProcessed()
        {
            ResponseDTO<List<RequestModel>> resp = new ResponseDTO<List<RequestModel>>();
            resp.Data = new List<RequestModel>();
            try
            {
                string processor = Guid.NewGuid().ToString();
                List<RequestModel> requests = new List<RequestModel>();
                requests = ListByStatus((int)RequestStatusEnum.Saved).Result.Data;
                foreach (RequestModel request in requests)
                {
                    request.Status = (int)RequestStatusEnum.BeingProcessed;
                    request.ProcessorCode = processor;
                    _context.Update(request);
                    resp.Data.Add(request);
                }
                await _context.SaveChangesAsync();
                resp.Success = true;
                resp.Message = "List of requests marked to be processed (" + requests.Count.ToString() + ")";
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }
        public async Task<ResponseDTO<List<RequestModel>>> MarkToBeProcessed(RequestModel request)
        {
            ResponseDTO<List<RequestModel>> resp = new ResponseDTO<List<RequestModel>>();
            resp.Data = new List<RequestModel>();
            try
            {
                string processor = Guid.NewGuid().ToString();
                request.Status = (int)RequestStatusEnum.BeingProcessed;
                request.ProcessorCode = processor;
                _context.Update(request);
                resp.Data.Add(request);
                await _context.SaveChangesAsync();
                resp.Success = true;
                resp.Message = "List of requests marked to be processed (1)";
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<RequestModel>> Update(RequestModel request)
        {
            ResponseDTO<RequestModel> resp = new ResponseDTO<RequestModel>();
            try
            {
                request.Card = Utilities.Utilities.OnlyNumbers(request.Card);
                request.Card = EncryptionUtility.EncryptNew(request.Card);

                _context.ChangeTracker.Clear();
                _context.Update(request);
                await _context.SaveChangesAsync();

                request.Card = EncryptionUtility.DecryptNew(request.Card);

                resp.Data = request;
                resp.Message = "Request successfully updated";
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }
    }
}
