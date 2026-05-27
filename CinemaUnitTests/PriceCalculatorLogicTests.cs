using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PriceCalculatorLogicTests
{
    [TestMethod]

    [DataRow("Normal", 44, 12, false)]
    [DataRow("NORMAL", 44, 12)]
    [DataRow("NorMal", 43, 12)]
    [DataRow("luxe", 42, 14, false)]
    [DataRow("luxe", 42, 14)]
    [DataRow("luxePlus", 42, 16, false)]
    public void Get_Price_No_Discount(string seatType, int age, decimal result, bool isStudent = false)
    {
        
    }
}
