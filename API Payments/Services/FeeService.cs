using API_Payments.Data;
using API_Payments.DTO;
using API_Payments.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Payments.Services
{
    public class FeeService : IFeeInterface
    {
        private readonly AppDbContext _context;
        public FeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseDTO<FeeModel>> Generate()
        {
            ResponseDTO<FeeModel> resp = new ResponseDTO<FeeModel>();
            FeeModel fee = new FeeModel();
            try
            {
                Random randNum = new Random();

                decimal num = randNum.Next(200)/(decimal)100;

                resp = GetLast().Result;
                if (resp.Success)
                {
                    fee = resp.Data;
                    fee.Id = 0;
                    fee.Date = DateTime.Now;
                    fee.Value *= num;
                }
                else
                {
                    fee = new FeeModel();
                    fee.Id = 0;
                    fee.Date = DateTime.Now;
                    fee.Value = num;
                }

                if (fee.Value < (decimal)0.10) 
                {
                    fee.Value += 1;
                }

                resp = Insert(fee).Result;
                if (resp.Success)
                {
                     resp.Message = "Fee generated successfully";
                     resp.Success = true;
                }
                else
                {
                    resp.Message = "Error generating fee";
                    resp.Success = false;
                }
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<FeeModel>> GetById(int feeId)
        {
            ResponseDTO<FeeModel> resp = new ResponseDTO<FeeModel>();
            try
            {
                var fee = await _context.TFees.FirstOrDefaultAsync(feebd => feebd.Id == feeId);
                if (fee == null)
                {
                    resp.Data = null;
                    resp.Message = "No fee found";
                    return resp;
                }

                resp.Data = fee;
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

        public async Task<ResponseDTO<FeeModel>> GetLast()
        {
            ResponseDTO<FeeModel> resp = new ResponseDTO<FeeModel>();
            try
            {
                ResponseDTO<List<FeeModel>> lresp = new ResponseDTO<List<FeeModel>>();
                lresp = List(1).Result;
                if (lresp.Success)
                {
                    resp.Data = lresp.Data[0];
                    resp.Success = true;
                }
                else
                {
                    resp.Data = null;
                    resp.Message = "Fee not found";
                    resp.Success = false;
                }
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<FeeModel>> Insert(FeeModel fee)
        {
            ResponseDTO<FeeModel> resp = new ResponseDTO<FeeModel>();
            try
            {
                _context.ChangeTracker.Clear();
                await _context.AddAsync(fee);
                var ret = await _context.SaveChangesAsync();

                resp.Data = fee;
                resp.Message = "Fee successfully inserted";
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

        public async Task<ResponseDTO<List<FeeModel>>> List(int num = 0)
        {
            ResponseDTO<List<FeeModel>> resp = new ResponseDTO<List<FeeModel>>();
            try
            {
                List<FeeModel> fees = new List<FeeModel>();
                if (num > 0)
                {
                    fees = (List<FeeModel>)_context.TFees.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .Take(num)
                                                                     .ToList();
                }
                else
                {
                    fees = (List<FeeModel>)_context.TFees.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .ToList();
                }

                resp.Success = true;
                resp.Data = fees;
                if (num > 0)
                    resp.Message = "List of the last " + num.ToString() + " fees retrieved successfully (" + fees.Count.ToString() + ")";
                else
                    resp.Message = "List of all fees retrieved successfully (" + fees.Count.ToString() + ")";
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
