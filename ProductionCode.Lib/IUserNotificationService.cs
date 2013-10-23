namespace ProductionCode.Lib
{
    public interface IUserNotificationService
    {
        void Notify(int userId, string message);
    }
}