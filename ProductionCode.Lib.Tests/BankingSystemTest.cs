using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;

namespace ProductionCode.Lib.Tests
{
    [TestClass]
    public class BankingSystemTest
    {
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
                             .Returns(MakeAccount(existingAccountId));
            noTargetAccountRepo.Setup(r => r.GetById(unknownAccountId))
                             .Returns<Account>(null);

            var sut = MakeSut(noTargetAccountRepo.Object);


            // Act
            sut.Transfer(existingAccountId, unknownAccountId, 200m);

            // Assert		
            // expected exception of type AccountNofFoundException
        }


        [TestMethod]
        public void Transfer_must_transfer_amount_to_destination()
        {
            // Arrange	
            var source = MakeAccount();
            var destination = MakeAccount();

            var repo = new Mock<IAccountRepository>();
            repo.Setup(r => r.GetById(source.Id)).Returns(source);
            repo.Setup(r => r.GetById(destination.Id)).Returns(destination);

            var amount = 28.34m;
            var expectedBalance = destination.Balance + amount;

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
            var source = MakeAccount();
            var destination = MakeAccount();

            var repo = new Mock<IAccountRepository>();
            repo.Setup(r => r.GetById(source.Id)).Returns(source);
            repo.Setup(r => r.GetById(destination.Id)).Returns(destination);

            var amount = 17.34m;
            var expectedBalance = source.Balance - amount;

            var sut = MakeSut(repo.Object);

            // Act
            sut.Transfer(source.Id, destination.Id, amount);

            // Assert		
            Assert.AreEqual(expectedBalance, source.Balance, "Should have taken amount from source's balance");
        }



        #region Test Helper Methods

        private static BankingSystem MakeSut(
                    IAccountRepository repo = null,
                    IUserService userService = null
            )
        {
            repo = repo ?? new Mock<IAccountRepository>(MockBehavior.Strict).Object;
            userService = userService ??  new Mock<IUserService>(MockBehavior.Strict).Object;

            return new BankingSystem(repo, userService);
        }

        private static Account MakeAccount(int? accountId = null)
        {
            var fixture = new Fixture();
            var account = fixture.Create<Account>();
            if (accountId.HasValue)
            {
                account.Id = accountId.Value;
            }

            return account;
        }

        #endregion

    }
}
