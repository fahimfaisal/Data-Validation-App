using DataValidation.ConfigurationClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestRAM_1()
        {
            //Arrange 
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0,type ,1.6,2,300,100);
            Task task = new Task(0, 2.5, 1.6, 4 , 300, 100);

            //Act

            bool result = processor.CheckRam(task);

            //Assert

            Assert.AreEqual( true, result, "Task has greater ram than processor");
           

        }
        
        [TestMethod]
        public void TestRAM_2()
        {
        }
        [TestMethod]
        public void TestRAM_3()
        {
        }

        [TestMethod]
        public void TestDOWNLOAD_1()
        {
        }

        [TestMethod]
        public void TestDOWNLOAD_2()
        {
        }
        [TestMethod]
        public void TestDOWNLOAD_3()
        {
        }

        [TestMethod]
        public void TestUPLOAD_1()
        {
        }

        [TestMethod]
        public void TestUPLOAD_2()
        {
        }
        [TestMethod]
        public void TestUPLOAD_3()
        {
        }

        [TestMethod]
        public void TestTAFF_1()
        {
        }
        [TestMethod]
        public void TestTAFF_2()
        {
        }
       
        [TestMethod]
        public void TestCFF_3()
        {
        }

        public void TestErrorsCFF()
        {
        }
        [TestMethod]
      
        
        public void TestErrorTaff()
        {
        }

    }
}
