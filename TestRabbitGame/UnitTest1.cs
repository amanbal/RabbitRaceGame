using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitRaceGame.Logic;
using System;

namespace TestRabbitGame
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFactory()
        {
            Punter punter = Factory.GetAPunter(0);
            Assert.AreEqual(punter.Cash,50);
        }
    }
}
