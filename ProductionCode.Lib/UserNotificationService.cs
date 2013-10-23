using System;
using ProductionCode.Lib.Communications;
using ProductionCode.Lib.Data;

namespace ProductionCode.Lib
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _mailSender;

        public UserNotificationService(IUserRepository userRepository, IEmailSender mailSender)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            if (mailSender == null) throw new ArgumentNullException("mailSender");
            _userRepository = userRepository;
            _mailSender = mailSender;
        }

        public void Notify(int userId, string message)
        {
            var user = _userRepository.GetById(userId);
            if(user == null) throw new UserNotFoundException();
            _mailSender.Send(user.EmailAddress, message);

        }
    }
}