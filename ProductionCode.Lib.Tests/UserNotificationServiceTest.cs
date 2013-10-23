using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using ProductionCode.Lib.Communications;
using ProductionCode.Lib.Data;

namespace ProductionCode.Lib.Tests
{
    [TestClass]
    public class UserNotificationServiceTest
    {

        [TestMethod]
        [ExpectedException(typeof (UserNotFoundException))]
        public void NotifyUser_with_unknown_userId_must_throw_UserNotFoundException()
        {
            // Arrange		
            var unknownUserId = 23;
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.GetById(unknownUserId)).Returns<User>(null);

            var sut = new UserNotificationService(userRepository.Object, new Mock<IEmailSender>().Object);

            // Act
            sut.Notify(unknownUserId, "whatever");

            // Assert		
            // expected exception of type UserNotFoundException
        }


        [TestMethod]
        public void NotifyUser_must_send_an_email()
        {
            // Arrange	
            var existingUser = CreateUser();
	        var userId = existingUser.Id;
            var message = "something to notify";

            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.GetById(userId)).Returns(existingUser);

            var mailSender = new Mock<IEmailSender>();
            mailSender.Setup(s => s.Send(It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();

            var sut = new UserNotificationService(userRepository.Object, mailSender.Object);

            // Act
            sut.Notify(userId, message );

            // Assert		
            mailSender.Verify(s=> s.Send(existingUser.EmailAddress, message),
                                    Times.Once);
        }



        #region Test Helper Methods

        private static User CreateUser()
        {
            return new User
            {
                EmailAddress = "tibo.desodt@gmail.com",
                Id = 13
            };
        }

        #endregion
    }
}