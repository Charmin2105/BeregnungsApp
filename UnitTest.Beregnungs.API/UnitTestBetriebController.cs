using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using REST.Api.Controllers;
using REST.Api.Entities;
using REST.Api.Helpers;
using REST.Api.Services;

namespace UnitTest.Beregnungs.API
{
    [TestClass]
    public class UnitTestBetriebController
    {
        [TestMethod]
        public void TestMethod1()
        {
            var testBetriebe = GetTestBetriebe();
            var controler = new BetriebController(testBetriebe,null,null,null,null);

            var result = controler.GetBetriebe() as List<Betrieb>;

        }

    }
}
