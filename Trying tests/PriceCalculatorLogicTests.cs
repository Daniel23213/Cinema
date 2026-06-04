//using Cinema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PriceCalculatorLogicTests
{
    [TestMethod]

    [DataRow("Normal", 44, 12)]
    [DataRow("NORMAL", 44, 12)]
    [DataRow("NorMal", 43, 12)]
    [DataRow("luxe", 42, 14)]
    [DataRow("luxe", 42, 14)]
    [DataRow("luxe plus", 42, 16)]
    public void Get_Price_No_Discount(string seatType, int age, int result)
    {
        decimal testPrice = PriceCalculatorLogic.GetPrice(seatType, age);

        Assert.AreEqual(result, testPrice);
    }

    [TestMethod]

    [DataRow("Normal", 10, 9)]
    [DataRow("Normal", 17, 9)]
    [DataRow("Normal", 1, 9)]
    [DataRow("Normal", 80, 10.2)]
    [DataRow("Normal", 65, 10.2)]
    [DataRow("Normal", 99, 10.2)]
    public void Get_Price_Age_Discount(string seatType, int age, double result)
    {
        decimal testPrice = PriceCalculatorLogic.GetPrice(seatType, age);

        Assert.AreEqual((decimal)result, testPrice);
    }

    [TestMethod]

    [DataRow("Normal", 25, true, 9.6)]
    [DataRow("luxe", 30, true, 11.2)]

    [DataRow("Normal", 65, true, 9.6)]
    [DataRow("Normal", 75, true, 9.6)]

    [DataRow("Normal", 17, true, 9)]
    [DataRow("Normal", 15, true, 9)]
    public void Get_Price_Student_And_Age_Discount(string seatType, int age, bool isStudent, double result)
    {
        decimal testPrice = PriceCalculatorLogic.GetPrice(seatType, age, isStudent);

        Assert.AreEqual((decimal)result, testPrice);
    }
}