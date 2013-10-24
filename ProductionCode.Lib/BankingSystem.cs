using System;
using ProductionCode.Lib.Data;

namespace ProductionCode.Lib
{
    public class BankingSystem
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserNotificationService _userNotificationService;

        public BankingSystem(IAccountRepository accountRepository, IUserNotificationService userNotificationService)
        {
            if (accountRepository == null) throw new ArgumentNullException("accountRepository");
            _accountRepository = accountRepository;
            _userNotificationService = userNotificationService;
        }

        public decimal Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            var source = _accountRepository.GetById(fromAccountId);
            if(source == null) throw new AccountNotfFoundException();

            var destination = _accountRepository.GetById(toAccountId);
            if(destination == null) throw  new AccountNotfFoundException();

            source.Remove(amount);
            //destination.Add(amount);

            _userNotificationService.Notify(destination.UserId, "Money was transferred to your account");

            return destination.Balance;
        }
    }
}