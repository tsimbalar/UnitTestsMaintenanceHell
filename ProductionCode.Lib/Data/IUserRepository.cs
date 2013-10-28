namespace ProductionCode.Lib.Data
{
    public interface IUserRepository
    {
        User GetById(int userId);
    }
}