namespace ProductionCode.Lib.Data
{
    public class User
    {
        private readonly int _id;
        private readonly string _emailAddress;
        //private readonly string _phoneNumber;

        public User(int id, string emailAddress)//, string phoneNumber)
        {
            _id = id;
            _emailAddress = emailAddress;
            //_phoneNumber = phoneNumber;
        }

        public int Id
        {
            get { return _id; }
        }

        public string EmailAddress
        {
            get { return _emailAddress; }
        }

        //public string PhoneNumber
        //{
        //    get { return _phoneNumber; }
        //}
    }
}