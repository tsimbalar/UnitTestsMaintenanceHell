using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProductionCode.Lib.Communications;
using ProductionCode.Lib.Data;

namespace ProductionCode.Lib.Tests
{
    [TestClass]
    public class UserNotificationServiceTest
    {

        [TestMethod]
        [ExpectedException(typeof(UserNotFoundException))]
        public void NotifyUser_with_unknown_userId_must_throw_UserNotFoundException()
        {
            // Arrange		
            var unknownUserId = 23;
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup(r => r.GetById(unknownUserId)).Returns<User>(null);

            var sut = new UserNotificationService(userRepository.Object, new Mock<IEmailSender>().Object);
            // var sut = MakeSut(userRepository.Object)

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
            mailSender.Setup(s => s.Send(It.IsAny<string>(), It.IsAny<string>()));

            var sut = new UserNotificationService(userRepository.Object, mailSender.Object);
            //var sut = MakeSut(userRepository.Object, mailSender.Object);

            // Act
            sut.Notify(userId, message);

            // Assert		
            mailSender.Verify(s => s.Send(existingUser.EmailAddress, message),
                                    Times.Once);
        }

        //[TestMethod]
        //public void NotifyUser_must_send_an_sms()
        //{
        //    // Arrange	
        //    var existingUser = CreateUser();
        //    var userId = existingUser.Id;
        //    var message = "something to notify";

        //    var expectedSms = new Sms(existingUser.PhoneNumber, message);

        //    var userRepository = new Mock<IUserRepository>();
        //    userRepository.Setup(r => r.GetById(userId)).Returns(existingUser);

        //    Sms actualSms = null;
        //    var smsSender = new Mock<ISmsSender>();
        //    smsSender.Setup(s => s.Send(It.IsAny<Sms>())).Callback<Sms>(argument=> actualSms = argument);

        //    var sut = MakeSut(userRepository.Object, smsSender: smsSender.Object);

        //    // Act
        //    sut.Notify(userId, message);

        //    // Assert	
        //    Assert.IsNotNull(actualSms, "Should have sent an sms");
        //    actualSms.AsSource().OfLikeness<Sms>()
        //        .ShouldEqual(expectedSms);
            
        //}


        #region Test Helper Methods


        // MakeSut to add a dependency to ISmsSender 


        //private static UserNotificationService MakeSut(
        //    IUserRepository userRepository = null,
        //    IEmailSender emailSender = null,
        //    ISmsSender smsSender = null
        //    )
        //{
        //    userRepository = userRepository ?? new Mock<IUserRepository>().Object;
        //    emailSender = emailSender ?? new Mock<IEmailSender>().Object;
        //    smsSender = smsSender ?? new Mock<ISmsSender>().Object;

        //    return new UserNotificationService(userRepository, emailSender, smsSender);
        //}

        private static User CreateUser()
        {
            //var fixture = new Fixture();
            //return fixture.Create<User>();
            return new User(13, "tibo.desodt@gmail.com");
        }

        #endregion
    }
}