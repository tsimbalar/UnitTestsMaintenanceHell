namespace ProductionCode.Lib.Communications
{
    public interface IEmailSender
    {
        void Send(string emailAddress, string message);
    }
}