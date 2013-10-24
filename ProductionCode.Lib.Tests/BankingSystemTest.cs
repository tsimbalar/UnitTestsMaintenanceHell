using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using ProductionCode.Lib.Data;

namespace ProductionCode.Lib.Tests
{
    [TestClass]
    public class BankingSystemTest
    {
        // TODO : test that one cannot pass negative amount to transfer !

        [TestMethod]
        [ExpectedException(typeof(AccountNotfFoundException))]
        public void Transfer_with_unknown_fromAccountId_must_throw_AccountNofFoundException()
        {
            // Arrange
            var unknownAccountId = 666;
            var noSourceAccountRepo = new Mock<IAccountRepository>();
            noSourceAccountRepo.Setup(r => r.GetById(unknownAccountId))
                             .Returns<Account>(null);

            var sut = MakeSut(noSourceAccountRepo.Object);


            // Act
            sut.Transfer(unknownAccountId, 999, 100m);

            // Assert		
            // expected exception of type AccountNofFoundException
        }

        [TestMethod]
        [ExpectedException(typeof(AccountNotfFoundException))]
        public void Transfer_with_unknown_toAccountId_must_throw_AccountNotfFoundException()
        {
            // Arrange
            var existingAccountId = 42;
            var unknownAccountId = 666;
            var noTargetAccountRepo = new Mock<IAccountRepository>();
            noTargetAccountRepo.Setup(r => r.GetById(existingAccountId))
                             .Returns(AnyAccount(existingAccountId));
            noTargetAccountRepo.Setup(r => r.GetById(unknownAccountId))
                             .Returns<Account>(null);

            var sut = MakeSut(noTargetAccountRepo.Object);


            // Act
            sut.Transfer(existingAccountId, unknownAccountId, 200m);

            // Assert		
            // expected exception of type AccountNofFoundException
        }


        // TODO:  fix the test Transfer_must_transfer_amount_to_destination
        [TestMethod]
        public void Transfer_must_transfer_amount_to_destination()
        {
            // Arrange	
            var source = AnyAccount(44);
            var destination = AnyAccount(45);
            var amount = 28.34m;
            var expectedBalance = destination.Balance + amount;

            var repo = new Mock<IAccountRepository>();
            repo.Setup(r => r.GetById(source.Id)).Returns(source);
            repo.Setup(r => r.GetById(destination.Id)).Returns(destination);


            var sut = MakeSut(repo.Object);

            // Act
            sut.Transfer(source.Id, destination.Id, amount);

            // Assert		
            Assert.AreEqual(expectedBalance, destination.Balance, "Should have added amount to destination's balance");
        }


        [TestMethod]
        public void Transfer_must_take_amount_from_source()
        {
            // Arrange	
            var source = AnyAccount(42);
            var destination = AnyAccount(43);

            var amount = 17.34m;
            var expectedBalance = source.Balance - amount;

            var repo = new Mock<IAccountRepository>();
            repo.Setup(r => r.GetById(source.Id)).Returns(source);
            repo.Setup(r => r.GetById(destination.Id)).Returns(destination);

            var sut = MakeSut(repo.Object);

            // Act
            sut.Transfer(source.Id, destination.Id, amount);

            // Assert		
            Assert.AreEqual(expectedBalance, source.Balance, "Should have taken amount from source's balance");
        }


        [TestMethod]
        public void Transfer_must_notify_recipient()
        {
            // Arrange		
            var source = AnyAccount(44);
            var destination = AnyAccount(45);
            var repo = new Mock<IAccountRepository>();
            repo.Setup(r => r.GetById(source.Id)).Returns(source);
            repo.Setup(r => r.GetById(destination.Id)).Returns(destination);


            var notifier = new Mock<IUserNotificationService>();
            notifier.Setup(n => n.Notify(It.IsAny<int>(), It.IsAny<string>()));

            var sut = MakeSut(repo.Object, notifier.Object);

            // Act
            sut.Transfer(source.Id, destination.Id, 45.50m);

            // Assert		
            notifier.Verify(n=> n.Notify(destination.UserId, "Money was transferred to your account"));
        }

        #region Test Helper Methods

        private static BankingSystem MakeSut(
                    IAccountRepository repo = null,
                    IUserNotificationService userNotificationService = null)
        {

            repo = repo ?? new Mock<IAccountRepository>(MockBehavior.Strict).Object;
            userNotificationService = userNotificationService ?? new Mock<IUserNotificationService>().Object;

            return new BankingSystem(repo, userNotificationService);
        }

        private static Account AnyAccount(int accountId)
        {
            var fixture = new Fixture();
            var account = fixture.Create<Account>();
            account.Id = accountId;
            return account;
        }

        #endregion

    }
}
