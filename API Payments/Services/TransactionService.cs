using API_Payments.Data;
using API_Payments.DTO;
using API_Payments.Models;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace API_Payments.Services
{
    public class TransactionService : ITransactionInterface
    {
        private readonly AppDbContext _context;
        private readonly ICardInterface _cardService;
        public TransactionService(AppDbContext context, 
                                  ICardInterface cardSerice)
        {
            _context = context;
            _cardService = cardSerice;
        }

        public async Task<ResponseDTO<TransactionModel>> GetById(int transactionId)
        {
            ResponseDTO<TransactionModel> resp = new ResponseDTO<TransactionModel>();
            try
            {
                var transaction = await _context.TTransactions.FirstOrDefaultAsync(transactionbd => transactionbd.Id == transactionId);
                if (transaction == null)
                {
                    resp.Data = null;
                    resp.Message = "No request found";
                    return resp;
                }

                resp.Data = transaction;
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

        public async Task<ResponseDTO<TransactionModel>> GetLast(string cardNumber)
        {
            ResponseDTO<TransactionModel> resp = new ResponseDTO<TransactionModel>();
            try
            {
                ResponseDTO<List<TransactionModel>> lresp = new ResponseDTO<List<TransactionModel>>();
                lresp = ListByCard(cardNumber).Result;
                if (lresp.Success)
                {
                    resp.Data = lresp.Data[0];
                    resp.Success = true;
                }
                else
                {
                    resp.Data = null;
                    resp.Message = "Transaction not found";
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

        public async Task<ResponseDTO<TransactionModel>> Insert(TransactionModel transaction)
        {
            ResponseDTO<TransactionModel> resp = new ResponseDTO<TransactionModel>();
            try
            {
                _context.ChangeTracker.Clear();
                await _context.AddAsync(transaction);
                var ret = await _context.SaveChangesAsync();

                resp.Data = transaction;
                resp.Message = "Transaction successfully inserted";
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

        public async Task<ResponseDTO<List<TransactionModel>>> List(int num = 0)
        {
            ResponseDTO<List<TransactionModel>> resp = new ResponseDTO<List<TransactionModel>>();
            try
            {
                List<TransactionModel> transactions = new List<TransactionModel>();
                if (num > 0)
                {
                    transactions = (List<TransactionModel>)_context.TTransactions.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .Take(num)
                                                                     .ToList();
                }
                else
                {
                    transactions = (List<TransactionModel>)_context.TTransactions.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .ToList();
                }

                resp.Success = true;
                resp.Data = transactions;
                if (num > 0)
                    resp.Message = "List of the last " + num.ToString() + " transactions retrieved successfully (" + transactions.Count.ToString() + ")";
                else
                    resp.Message = "List of all transactions retrieved successfully (" + transactions.Count.ToString() + ")";
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;

        }

        public async Task<ResponseDTO<List<TransactionModel>>> ListByCard(string cardNumber)
        {
            ResponseDTO<List<TransactionModel>> resp = new ResponseDTO<List<TransactionModel>>();
            try
            {
                int cardId = 0;
                var card = _cardService.GetByNumber(cardNumber).Result;
                if (card.Data != null) 
                {
                    cardId = card.Data.Id;
                }

                List<TransactionModel> transactions = new List<TransactionModel>();
                transactions = (List<TransactionModel>)_context.TTransactions.ToListAsync().Result
                                                                    .Where(transactionbd => transactionbd.Card_Id == cardId)
                                                                    .ToList();

                resp.Success = true;
                resp.Data = transactions;
                resp.Message = "List of transactions of card " + cardId.ToString() + "(" + transactions.Count.ToString() + ")";
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
