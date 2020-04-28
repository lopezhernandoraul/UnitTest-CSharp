using NUnit.Framework;
using System;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class DemeritPointsCalculatorTests
    {
        [Test]
        [TestCase(-1)]
        [TestCase(301)]
        public void CalculateDemeritPoints_SpeedIsOutOfRange_ThrowArgumentOutOfRangeException(int speed)
        {
            var calculator = new DemeritPointsCalculator();
            //We need to use the lambda expression because we are testing results that throw exceptions
            Assert.That(() => calculator.CalculateDemeritPoints(speed), Throws
                .Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(64, 0)]
        [TestCase(65, 0)]
        [TestCase(66, 0)]
        [TestCase(70, 1)]
        [TestCase(75, 2)]

        //In the "Test Cases" the first parameter refers to "Speed" 
        //and the second parameter refers to "Expected Result"
        public void CalculateDemeritPoints_WhenCalled_ReturnDemeritpoints(int speed,int expectedResult)
        {
            var calculator = new DemeritPointsCalculator();
            var result = calculator.CalculateDemeritPoints(speed);
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
