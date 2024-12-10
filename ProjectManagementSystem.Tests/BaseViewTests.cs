using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Views;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectManagementSystem.Tests
{
    [TestClass]
    public class BaseViewTests
    {
        private BaseView _baseView;

        [TestInitialize]
        public void Setup()
        {
            _baseView = new BaseView();
        }

        [TestMethod]
        public void Test_GettextCenter_CorrectlyCentersText()
        {
            // Arrange
            string text = "Hello";
            int maxWidth = 20;

            // Act
            string result = _baseView.GettextCenter(text, maxWidth);

            // Assert
            string expected = "       Hello        "; // 7 spaces on the left, 7 spaces on the right
            Assert.AreEqual(expected, result, "Text was not centered correctly.");
        }

        [TestMethod]
        public void Test_CheckExit_InputIsZero_ReturnsTrue()
        {
            // Arrange
            string input = "0";

            // Act
            bool result = _baseView.CheckExit(input);

            // Assert
            Assert.IsTrue(result, "CheckExit should return true for input '0'.");
        }

        [TestMethod]
        public void Test_CheckExit_InputIsNonZero_ReturnsFalse()
        {
            // Arrange
            string input = "1";

            // Act
            bool result = _baseView.CheckExit(input);

            // Assert
            Assert.IsFalse(result, "CheckExit should return false for any input other than '0'.");
        }

        [TestMethod]
        public void Test_GetInput_ReturnsCorrectInput()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);

            var reader = new StringReader("test input");
            Console.SetIn(reader);

            // Act
            string input = _baseView.GetInput("Enter something:");

            // Assert
            Assert.AreEqual("test input", input, "GetInput did not return the expected input.");
        }

        [TestMethod]
        public void Test_DisplayMessage_OutputsMessage()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);
            string message = "This is a message.";

            // Act
            _baseView.DisplayMessage(message);

            // Assert
            Assert.AreEqual(message + Environment.NewLine, writer.ToString(), "DisplayMessage did not output the correct message.");
        }

        [TestMethod]
        public void Test_DisplayError_OutputsErrorMessageInRed()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);
            string errorMessage = "This is an error.";

            // Act
            _baseView.DisplayError(errorMessage);

            // Assert
            string expectedOutput = "[ERROR] This is an error." + Environment.NewLine;
            Assert.IsTrue(writer.ToString().Contains(expectedOutput), "DisplayError did not output the correct error message.");
        }

        [TestMethod]
        public void Test_DisplaySuccess_OutputsSuccessMessageInGreen()
        {
            // Arrange
            var writer = new StringWriter();
            Console.SetOut(writer);
            string successMessage = "This is a success.";

            // Act
            _baseView.DisplaySuccess(successMessage);

            // Assert
            string expectedOutput = "[SUCCESS] This is a success." + Environment.NewLine;
            Assert.IsTrue(writer.ToString().Contains(expectedOutput), "DisplaySuccess did not output the correct success message.");
        }


    }
}
