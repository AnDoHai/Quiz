using Tms.DataAccess;
using Tms.DataAccess.Common;
using Tms.DataAccess.Repositories;
using Tms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tms.Services
{
    public interface IAccountTokenService : IEntityService<AccountToken>
    {
        bool CreateToken(int accountId, out string token, AccountTokenType tokenType, bool isAdminAccountSide = false);
        bool IsValidToken(string token, int userId, AccountTokenType tokenType, bool isAdminAccountSide = false);
        bool IsValidAccessToken(string accessToken, AccountTokenType tokenType, out int customerId);
    }
    public class AccountTokenService : EntityService<AccountToken>, IAccountTokenService
    {
        private readonly IAccountTokenRepository _accountTokenRepository;
        public AccountTokenService(IUnitOfWork unitOfWork, IAccountTokenRepository accountTokenRepository)
        : base(unitOfWork, accountTokenRepository)
        {
            _accountTokenRepository = accountTokenRepository;
        }

        public bool CreateToken(int accountId, out string token, AccountTokenType tokenType, bool isAdminAccountSide = false)
        {
            token = string.Empty;
            try
            {
                var accountToken = new AccountToken();
                accountToken.AccountId = accountId;
                accountToken.IsAdminAccountSide = isAdminAccountSide;
                accountToken.TokenType = (int)tokenType;
                token = Guid.NewGuid().ToString().Replace("-", "").ToLower();
                accountToken.TokenKey = token;
                accountToken.ExpiredDate = DateTime.Now.AddDays(1);
                _accountTokenRepository.Insert(accountToken);
                UnitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred when create token,ex:", ex);
                return false;
            }


        }

        public bool IsValidToken(string token, int userId, AccountTokenType tokenType, bool isAdminAccountSide = false)
        {
            var lastToken = _accountTokenRepository.GetAll()
            .Where(x => x.AccountId == userId && x.IsAdminAccountSide == isAdminAccountSide && x.TokenType == (int)tokenType)
            .OrderByDescending(x => x.ExpiredDate).FirstOrDefault();
            if (lastToken != null && lastToken.TokenKey == token && lastToken.ExpiredDate > DateTime.Now)
            {
                return true;
            }
            return false;
        }

        public bool IsValidAccessToken(string accessToken, AccountTokenType tokenType, out int customerId)
        {
            customerId = 0;
            var accountToken = _accountTokenRepository.Find(x => x.TokenKey == accessToken && x.TokenType == (int)tokenType);
            if (accountToken == null || accountToken.ExpiredDate <= DateTime.Now)
                return false;

            customerId = accountToken.AccountId;

            _accountTokenRepository.Delete(accountToken);
            UnitOfWork.SaveChanges();
            return true;
        }
    }
}
