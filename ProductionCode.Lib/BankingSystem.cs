using System;
using ProductionCode.Lib.Data;

namespace ProductionCode.Lib
{
    public class BankingSystem
    {
        private readonly IAccountRepository _accountRepository;

        public BankingSystem(IAccountRepository accountRepository, IUserNotificationService userNotificationService)
        {
            if (accountRepository == null) throw new ArgumentNullException("accountRepository");
            _accountRepository = accountRepository;
        }

        public decimal Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            var source = _accountRepository.GetById(fromAccountId);
            if(source == null) throw new AccountNotfFoundException();

            var destination = _accountRepository.GetById(toAccountId);
            if(destination == null) throw  new AccountNotfFoundException();

            source.Remove(amount);
            destination.Add(amount);

            return destination.Balance;
        }
    }
}