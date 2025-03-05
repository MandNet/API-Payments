using API_Payments.Data;
using API_Payments.DTO;
using API_Payments.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Payments.Services
{
    public class LogService : ILogInterface
    {
        private readonly AppDbContext _context;
        public LogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDTO<LogModel>> GetById(int logId)
        {
            ResponseDTO<LogModel> resp = new ResponseDTO<LogModel>();
            try
            {
                var log = await _context.TLogs.FirstOrDefaultAsync(logbd => logbd.Id == logId);
                if (log == null)
                {
                    resp.Data = null;
                    resp.Message = "No log found";
                    return resp;
                }

                resp.Data = log;
                resp.Status = true;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Status = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<LogModel>> Insert(LogModel log)
        {
            ResponseDTO<LogModel> resp = new ResponseDTO<LogModel>();
            try
            {
                _context.ChangeTracker.Clear();
                await _context.AddAsync(log);
                var ret = await _context.SaveChangesAsync();

                log.Id = ret;

                resp.Data = log;
                resp.Message = "Log successfully inserted";
                resp.Status = true;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Status = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<List<LogModel>>> List(int num = 0)
        {
            ResponseDTO<List<LogModel>> resp = new ResponseDTO<List<LogModel>>();
            try
            {
                List<LogModel> logs = new List<LogModel>();
                if (num > 0)
                {
                    logs = (List<LogModel>)_context.TLogs.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .Take(num)
                                                                     .ToList();
                }
                else
                {
                    logs = (List<LogModel>)_context.TLogs.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .ToList();
                }

                resp.Status = true;
                resp.Data = logs;
                if (num > 0)
                    resp.Message = "List of the last " + num.ToString() + " logs retrieved successfully (" + logs.Count.ToString() + ")";
                else
                    resp.Message = "List of all logs retrieved successfully (" + logs.Count.ToString() + ")";
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Status = false;
            }
            return resp;
        }
    }
}
