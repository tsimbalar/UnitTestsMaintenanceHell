namespace ProductionCode.Lib.Data
{
    public class Account
    {
        private decimal _balance;
        public Account(decimal initialBalance)
        {
            _balance = initialBalance;
        }

        public int Id { get; set; }

        public decimal Balance { get { return _balance; } }

        public int UserId { get; set; }

        public void Add(decimal amount)
        {
            _balance = _balance + amount;
        }

        public void Remove(decimal amount)
        {
            _balance = _balance - amount;
        }
    }
}