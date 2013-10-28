namespace ProductionCode.Lib.Data
{
    public interface IAccountRepository
    {
        Account GetById(int accountId);
    }
}
