using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAPITAL.Engine.Utilities;
using CAPITAL.ORM.Exceptions;
using CAPITAL.ORM.Objects;

namespace CAPITAL.Engine.Test.Tests
{
    [TestClass]
    public class UtilityTest
    {
        [TestMethod]
        public void GetFrequenciesTest()
        {
            AccountUtil freqUtil = new AccountUtil();
            var frequencies = freqUtil.GetFrequencies();

            if (frequencies == null || frequencies.Count() < 1)
                Assert.Fail("No Frequencies returned.");
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void CreateInvalidFeedback()
        {
            FeedbackUtil feedbackUtil = new FeedbackUtil();
            Feedback feedback = new Feedback();

            feedbackUtil.SendFeedback(feedback);
        }

        [TestMethod]
        public void CreateValidFeedback()
        {
            FeedbackUtil feedbackUtil = new FeedbackUtil();
            Feedback feedback = new Feedback();
            feedback.UserId = 1;
            feedback.Message = "FEEDBACK TEST MESSAGE";
            feedback.ReportType = "REPORT TYPE";
            feedbackUtil.SendFeedback(feedback);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelException))]
        public void CreateInvalidRegistration()
        {
            MessengerUtil messengerUtil = new MessengerUtil();
            Registration registration = new Registration();

            messengerUtil.Register(registration);
        }

        [TestMethod]
        public void CreateValidRegistration()
        {
            MessengerUtil messengerUtil = new MessengerUtil();
            Registration registration = new Registration();
            registration.UserId = 1;
            registration.URI = "FAKE URI";

            messengerUtil.Register(registration);
        }
    }
}
