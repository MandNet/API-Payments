using API_Payments.Data;
using API_Payments.DTO;
using API_Payments.Models;
using API_Payments.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API_Payments.Services
{
    public class CardService : ICardInterface
    {
        private readonly AppDbContext _context;
        private readonly ILogInterface _logServicet;
        public CardService(AppDbContext context, ILogInterface logServicet)
        {
            _context = context;
            _logServicet = logServicet;
        }

        public async Task<ResponseDTO<CardModel>> Debt(string cardNumber, decimal value)
        {
            ResponseDTO < CardModel > resp = new ResponseDTO<CardModel>();

            try
            {
                var card = GetByNumber(cardNumber).Result;
                if (!card.Success)
                {
                    resp.Data = null;
                    resp.Message = "Card not found";
                    resp.Success = false;
                    return resp;
                }

                resp = Debt(card.Data.Id, value).Result;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }
        public async Task<ResponseDTO<CardModel>> Debt(int cardId, decimal value)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                var card = GetById(cardId).Result;
                if (!card.Success)
                {
                    resp.Data = null;
                    resp.Message = "Card not found";
                    resp.Success = false;
                    return resp;
                }
                if ((card.Data.Balance+card.Data.Limit) < value)
                {
                    resp.Data = null;
                    resp.Message = "Insufficient balance";
                    resp.Success = false;
                    return resp;
                }
                if (card.Data.Balance >= value)
                {
                    card.Data.Balance -= value;
                }
                else
                {
                    card.Data.Limit -= (value - card.Data.Balance);
                    card.Data.Balance = 0;
                }
                await Update(card.Data);
                resp = card;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<CardModel>> Delete(CardModel card)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                _context.ChangeTracker.Clear();
                _context.Remove(card);
                await _context.SaveChangesAsync();
                resp.Data = card;
                resp.Message = "Card successfully deleted";
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

        public async Task<ResponseDTO<CardBalanceDTO>> GetBalance(string number)
        {
            ResponseDTO<CardBalanceDTO> resp = new ResponseDTO<CardBalanceDTO>();
            try
            {

                var card = GetByNumber(number);
                if (!card.Result.Success)
                {
                    resp.Data = null;
                    resp.Message = "Card not found";
                    resp.Success = false;
                    return resp;
                }
                resp.Data = new CardBalanceDTO();
                resp.Data.Balance = card.Result.Data.Balance;
                resp.Data.Limit = card.Result.Data.Limit;
                resp.Data.Total = card.Result.Data.Balance + card.Result.Data.Limit;
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

        public async Task<ResponseDTO<CardModel>> GetById(int cardId)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                var card = await _context.TCards.FirstOrDefaultAsync(transacaoBanco => transacaoBanco.Id == cardId);
                if (card == null)
                {
                    resp.Data = null;
                    resp.Message = "No card found";
                    resp.Success = false;
                    return resp;
                }

                try
                {
                    if (!string.IsNullOrEmpty(card.Number))
                    {
                        card.Number = EncryptionUtility.DecryptNew(card.Number);
                    }
                }
                catch (Exception)
                {
                }
                resp.Data = card;
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

        public async Task<ResponseDTO<CardModel>> GetByNumber(string number)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                number = Utilities.Utilities.OnlyNumbers(number);
                number = EncryptionUtility.EncryptNew(number);
                var card = await _context.TCards.FirstOrDefaultAsync(scard => scard.Number == number);
                if (card == null)
                {
                    resp.Data = null;
                    resp.Message = "No card found";
                    resp.Success = false;
                    return resp;
                }

                try
                {
                    if (!string.IsNullOrEmpty(card.Number))
                    {
                        card.Number = EncryptionUtility.DecryptNew(card.Number);
                    }
                }
                catch (Exception)
                {
                }

                resp.Data = card;
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

        public async Task<ResponseDTO<CardModel>> Insert(CardModel card)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                resp = ValidateCard(card);
                if (!resp.Success)
                {
                    return resp;
                }

                card = resp.Data!;
                Random randNum = new Random();

                card.Balance = randNum.Next(100);
                card.Limit = randNum.Next(100);

                _context.ChangeTracker.Clear();
                await _context.AddAsync(card);
                var ret = await _context.SaveChangesAsync();

                card.Number = EncryptionUtility.DecryptNew(card.Number);

                resp.Data = card;
                resp.Message = "Card successfully inserted";
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

        public async Task<ResponseDTO<List<CardModel>>> List(int num = 0)
        {
            ResponseDTO<List<CardModel>> resp = new ResponseDTO<List<CardModel>>();
            try
            {
                List<CardModel> cards = new List<CardModel>();
                if (num > 0)
                {
                    cards = (List<CardModel>)_context.TCards.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .Take(num)
                                                                     .ToList();
                }
                else
                {
                    cards = (List<CardModel>)_context.TCards.ToListAsync().Result
                                                                     .OrderByDescending(c => c.Id)
                                                                     .ToList();
                }

                for (int i=0; i < cards.Count; i++)
                {
                    cards[i].Number = EncryptionUtility.DecryptNew(cards[i].Number);
                }

                resp.Success = true;
                resp.Data = cards;
                if (num > 0)
                    resp.Message = "List of the last " + num.ToString() + " cards retrieved successfully (" + cards.Count.ToString() + ")";
                else
                    resp.Message = "List of all cards retrieved successfully (" + cards.Count.ToString() + ")";
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<CardModel>> Update(CardModel card)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            CardModel cardObj = new CardModel();
            string ncard;
            try
            {
                resp = ValidateCard(card);
                if (!resp.Success)
                {
                    return resp;
                }
                cardObj = resp.Data;
                ncard = cardObj.Number;

                var old_card = GetById(card.Id).Result;
                if (!old_card.Success)
                {
                    resp.Data = null;
                    resp.Message = "Card not found";
                    resp.Success = false;
                    return resp;
                }

                _context.ChangeTracker.Clear();
                cardObj.Number = ncard;
                _context.Update(cardObj);
                await _context.SaveChangesAsync();

                cardObj.Number = EncryptionUtility.DecryptNew(cardObj.Number);

                resp.Data = cardObj;
                resp.Message = "Card successfully updated";
                resp.Success = true;

                LogModel log = new LogModel();
                log.After = Utilities.JsonUtility.Serialize(card);
                log.Before = Utilities.JsonUtility.Serialize(old_card.Data);
                log.Date = DateTime.Now;
                await _logServicet.Insert(log);
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error: " + ex.Message;
                resp.Success = false;
            }
            return resp;
        }

        public async Task<ResponseDTO<CardModel>> UpdateBalance(string number, decimal value)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                var card = await GetByNumber(number);
                if (!card.Success)
                {
                    resp.Data = null;
                    resp.Message = "Card not found";
                    resp.Success = false;
                    return resp;
                }
                card.Data.Balance = value;
                await Update(card.Data);
                resp.Data = card.Data;
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

        public async Task<ResponseDTO<CardModel>> UpdateLimit(string number, decimal value)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                var card = await GetByNumber(number);
                if (!card.Success)
                {
                    resp.Data = null;
                    resp.Message = "Card not found";
                    resp.Success = false;
                    return resp;
                }
                card.Data.Limit = value;
                await Update(card.Data);
                resp.Data = card.Data;
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

        public async Task<ResponseDTO<CardModel>> UpdateStatus(string number, int status)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                var card = await GetByNumber(number);
                if (!card.Success)
                {
                    resp.Data = null;
                    resp.Message = "Card not found";
                    resp.Success = false;
                    return resp;
                }
                card.Data.Status = status;
                await Update(card.Data);
                resp.Data = card.Data;
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

        private ResponseDTO<CardModel> ValidateCard(CardModel card)
        {
            ResponseDTO<CardModel> resp = new ResponseDTO<CardModel>();
            try
            {
                card.Number = Utilities.Utilities.OnlyNumbers(card.Number);

                if (card.Number.Length != 15)
                {
                    resp.Data = null;
                    resp.Message = "Card number must have 15 digits";
                    resp.Success = false;
                    return resp;
                }

                card.Number = EncryptionUtility.EncryptNew(card.Number);
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Message = "Error validating card: " + ex.Message;
                resp.Success = false;
                return resp;
            }

            resp.Data = card;
            resp.Success = true;
            return resp;
        }
    }
}
