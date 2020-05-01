using Moq;
using NUnit.Framework;
using TestNinja.Mocking;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperServiceTests
    {
        private HousekeeperService _service;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private Housekeeper _houseKeeper;
        private DateTime _statementDate = new DateTime(2017, 1, 1);
        private string _statementFileName;
        [SetUp]
        public void SetUp()
        {
            _houseKeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper>
            {
              _houseKeeper
            }.AsQueryable()); ;

            _statementFileName = "filename";
            _statementGenerator = new Mock<IStatementGenerator>();
            _statementGenerator.Setup(sg =>
              sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate)).Returns(() => _statementFileName);
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();

            _service = new HousekeeperService(
                unitOfWork.Object,
                _statementGenerator.Object,
                _emailSender.Object,
                _messageBox.Object);
        }

        [Test]
        public void SendStatementsEmail_WhenCalled_GenerateStatements()
        {
            _service.SendStatementEmails(_statementDate);
            _statementGenerator.Verify(sg =>
            sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, (_statementDate)));
            VerifyEmailSend();
        }

        public void SendStatementsEmails_HouseKeepersEmailIsNull_ShoulNotGenerateStatements()
        {
            _houseKeeper.Email = null;
            _service.SendStatementEmails(_statementDate);
            _statementGenerator.Verify(sg =>
            sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, (_statementDate)),
            Times.Never);
        }

        public void SendStatementsEmails_HouseKeepersEmailIsWhiteSpace_ShoulNotGenerateStatements()
        {
            _houseKeeper.Email = " ";
            _service.SendStatementEmails(_statementDate);
            _statementGenerator.Verify(sg =>
            sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, (_statementDate)),
            Times.Never);
        }

        public void SendStatementsEmails_HouseKeepersEmailIsEmpty_ShoulNotGenerateStatements()
        {
            _houseKeeper.Email = "";
            _service.SendStatementEmails(_statementDate);
            _statementGenerator.Verify(sg =>
            sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, (_statementDate)),
            Times.Never);
        }

        public void SendStatementsEmails_WhenCalled_EmailTheStatement()
        {
            _statementGenerator.Setup(sg =>
           sg.SaveStatement(_houseKeeper.Oid, _houseKeeper.FullName, _statementDate)).Returns(_statementFileName);

        }

        public void SendStatementsEmails_StatementFileNameIsNull_ShouldNotEmailTheStatement()
        {
            _statementFileName = null;

            _service.SendStatementEmails(_statementDate);
            VerifyEmailNotSend();
        }

        public void SendStatementsEmails_StatementFileNameIsEmptyString_ShouldNotEmailTheStatement()
        {
            _statementFileName = "";

            _service.SendStatementEmails(_statementDate);
            VerifyEmailNotSend();
        }
        public void SendStatementsEmails_StatementFileNameIsWhiteSpace_ShouldNotEmailTheStatement()
        {
            _statementFileName = " ";

            _service.SendStatementEmails(_statementDate);
            VerifyEmailNotSend();
        }

        public void SendStatementsEmails_EmailSendingFails_DisplayAMessageBox()
        {
            _emailSender.Setup(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()
                )).Throws<Exception>();

            _service.SendStatementEmails(_statementDate);

            _messageBox.Verify(mb => mb.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK));

        }


        private void VerifyEmailNotSend()
        {
            _emailSender.Verify(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()),
                Times.Never);
        }

        private void VerifyEmailSend()
        {
            _service.SendStatementEmails(_statementDate);
            _emailSender.Verify(es => es.EmailFile(_houseKeeper.Email, _houseKeeper.StatementEmailBody, _statementFileName,
                It.IsAny<string>()));
        }
    }
}
