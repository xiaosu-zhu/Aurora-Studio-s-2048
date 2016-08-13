using System;
using System.Diagnostics;
using Aurora.Studio._2048.Core.Game;
using Com.Aurora.Shared.Extensions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void MoveLeftTest()
        {
            var p = Calculator.New();
            Calculator.MoveLeft(ref p);
            p = new int[][] { new int[] { 2, 2, 2, 2 }, new int[] { 2, 4, 0, 0 }, new int[] { 2, 0, 2, 0 }, new int[] { 4, 4, 0, 2 } };
            var k = new int[][] { new int[] { 4, 4, 0, 0 }, new int[] { 2, 4, 0, 0 }, new int[] { 4, 0, 0, 0 }, new int[] { 8, 2, 0, 0 } };
            Calculator.MoveLeft(ref p);
            Assert.AreEqual(k, p);
        }
    }
}