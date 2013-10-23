using System;
using ProductionCode.Lib.Communications;
using ProductionCode.Lib.Data;

namespace ProductionCode.Lib
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _mailSender;
        private readonly ISmsSender _smsSender;

        public UserNotificationService(IUserRepository userRepository, IEmailSender mailSender)//, ISmsSender smsSender)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            if (mailSender == null) throw new ArgumentNullException("mailSender");
            _userRepository = userRepository;
            _mailSender = mailSender;
            //_smsSender = smsSender;
        }

        public void Notify(int userId, string message)
        {
            var user = _userRepository.GetById(userId);
            if(user == null) throw new UserNotFoundException();
            _mailSender.Send(user.EmailAddress, message);
            //_smsSender.Send(new Sms(user.PhoneNumber, message));

        }
    }
}