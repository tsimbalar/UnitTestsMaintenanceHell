using System;

namespace ProductionCode.Lib
{
    public class BankingSystem
    {
        private readonly IAccountRepository _accountRepository;

        public BankingSystem(IAccountRepository accountRepository, IUserService userService)
        {
            if (accountRepository == null) throw new ArgumentNullException("accountRepository");
            _accountRepository = accountRepository;
        }

        public void Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            var source = _accountRepository.GetById(fromAccountId);
            if(source == null) throw new AccountNotfFoundException();
            var destination = _accountRepository.GetById(toAccountId);
            if(destination == null) throw  new AccountNotfFoundException();

            source.Balance = source.Balance - amount;
            destination.Balance = destination.Balance + amount;
            
        }
    }
}