namespace ProductionCode.Lib.Communications
{
    public interface ISmsSender
    {
        void Send(Sms sms );
    }

    public class Sms
    {
        private readonly string _phoneNumber;
        private readonly string _message;
        public Sms(string phoneNumber, string message)
        {
            _phoneNumber = phoneNumber;
            _message = message;
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
        }

        public string Message
        {
            get { return _message; }
        }
    }
}
