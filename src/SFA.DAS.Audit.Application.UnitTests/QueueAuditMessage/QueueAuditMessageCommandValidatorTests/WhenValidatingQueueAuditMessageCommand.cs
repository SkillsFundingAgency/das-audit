using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.Audit.Application.QueueAuditMessage;
using SFA.DAS.Audit.Application.Validation;
using SFA.DAS.Audit.Test.Shared.ObjectMothers;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Application.UnitTests.QueueAuditMessage.QueueAuditMessageCommandValidatorTests
{
    public class WhenValidatingQueueAuditMessageCommand
    {
        private QueueAuditMessageCommandValidator _validator;

        private QueueAuditMessageCommand _command;

        [SetUp]
        public void Arrange()
        {
            _validator = new QueueAuditMessageCommandValidator();

            _command = new QueueAuditMessageCommand
            {
                Message = AuditMessageMother.Create()
            };
        }

        [Test]
        public async Task ThenItShouldReturnAValidResultIfNoIssues()
        {
            // Act
            var actual = await _validator.ValidateAsync(_command);

            // Assert
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.IsValid);
        }


        [Test]
        public async Task ThenItShouldReturnFalseWhenValidationFailsAndTheErrorDictionaryIsPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand {Message = new AuditMessage() });

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.IsNotNull(actual.Errors.Single(c=>c.Property.Equals("AffectedEntity") && c.Description.Equals("No value supplied for AffectedEntity")));
            Assert.IsNotNull(actual.Errors.Single(c=>c.Property.Equals("Description") && c.Description.Equals("No value supplied for Description")));
            Assert.IsNotNull(actual.Errors.Single(c=>c.Property.Equals("Source") && c.Description.Equals("No value supplied for Source")));
            Assert.IsNotNull(actual.Errors.Single(c=>c.Property.Equals("ChangedBy") && c.Description.Equals("No value supplied for ChangedBy")));
            Assert.IsNotNull(actual.Errors.Single(c=>c.Property.Equals("ChangeAt") && c.Description.Equals("No value supplied for ChangeAt")));
        }


        [Test]
        public async Task ThenTheResultIsNotValidWhenTheActorPropertyIsNotCorrectlyPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage {ChangedBy = new Actor {} } });

            //Assert
            Assert.IsNotNull(actual.Errors.Single(c => c.Property.Equals("ChangedBy") && c.Description.Equals("No value supplied for ChangedBy")));

        }



        [Test]
        public async Task ThenTheChangeByErrorIsNotAddedWhenThereIsAnIdPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { ChangedBy = new Actor { Id = "123456"} } });

            //Assert
            Assert.IsNull(actual.Errors.SingleOrDefault(c => c.Property.Equals("ChangedBy") && c.Description.Equals("No value supplied for ChangedBy")));

        }


        [Test]
        public async Task ThenTheChangeByErrorIsNotAddedWhenThereIsAnEmailPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { ChangedBy = new Actor { EmailAddress = "123456" } } });

            //Assert
            Assert.IsNull(actual.Errors.SingleOrDefault(c => c.Property.Equals("ChangedBy") && c.Description.Equals("No value supplied for ChangedBy")));

        }

        [Test]
        public async Task ThenTheEntityErrorIsAddedWhenTheEntityTypeIsNotPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { AffectedEntity = new Entity { Id = "123456" } } });

            //Assert
            Assert.IsNotNull(actual.Errors.Single(c => c.Property.Equals("AffectedEntity") && c.Description.Equals("No value supplied for AffectedEntity")));
        }

        [Test]
        public async Task ThenTheEntityErrorIsAddedWhenTheEntityIdIsNotPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { AffectedEntity = new Entity { Type = "123456" } } });

            //Assert
            Assert.IsNotNull(actual.Errors.Single(c => c.Property.Equals("AffectedEntity") && c.Description.Equals("No value supplied for AffectedEntity")));
        }

        [Test]
        public async Task ThenTheAffectedEntityErrorIsNotAddedWhenThereIsAnIdAndTypePopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { AffectedEntity = new Entity { Type = "123456",Id="test" } } });

            //Assert
            Assert.IsNull(actual.Errors.SingleOrDefault(c => c.Property.Equals("AffectedEntity") && c.Description.Equals("No value supplied for AffectedEntity")));

        }


        [Test]
        public async Task ThenTheSourceErrorIsAddedWhenTheComponentPropertyIsNotPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { Source = new Source { Component= "123456" } } });

            //Assert
            Assert.IsNotNull(actual.Errors.Single(c => c.Property.Equals("Source") && c.Description.Equals("No value supplied for Source")));
        }

        [Test]
        public async Task ThenTheSourceErrorIsAddedWhenTheSystemPropertyIsNotPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { Source = new Source { System = "123456" } } });

            //Assert
            Assert.IsNotNull(actual.Errors.Single(c => c.Property.Equals("Source") && c.Description.Equals("No value supplied for Source")));
        }

        [Test]
        public async Task ThenTheSourceErrorIsAddedWhenTheVersionPropertyIsNotPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { Source = new Source { Version = "123456" } } });

            //Assert
            Assert.IsNotNull(actual.Errors.Single(c => c.Property.Equals("Source") && c.Description.Equals("No value supplied for Source")));
        }

        [Test]
        public async Task ThenTheSourceErrorIsNotAddedWhenTheObjectIsPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand { Message = new AuditMessage { Source = new Source { Version = "123456",System = "ret",Component = "werwer"} } });

            //Assert
            Assert.IsNull(actual.Errors.SingleOrDefault(c => c.Property.Equals("Source") && c.Description.Equals("No value supplied for Source")));

        }

        [Test]
        public async Task ThenItShouldReturnFalseWhenTheQueueMessageIsNullAndTheValidationFailsAndTheErrorDictionaryIsPopulated()
        {
            //Act
            var actual = await _validator.ValidateAsync(new QueueAuditMessageCommand());

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsFalse(actual.IsValid);
            Assert.AreEqual(1, actual.Errors.Length);
        }
    }
}
