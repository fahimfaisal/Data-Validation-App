using DataValidation;
using DataValidation.ConfigurationClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTests
{
    [TestClass]
    public class UnitTest
    {


        ///Task ram required is less than the processor ram

        [TestMethod]
        public void TestRAM_1()
        {
            //Arrange 

            int processorRam = 4;
            int taskRam = 2;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, processorRam, 300, 100);
            Task task = new Task(0, 2.5, 1.6, taskRam, 300, 100);
            bool isRamLessOrEqual = true;

            //Act

            bool result = processor.CheckRam(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isRamLessOrEqual, actualResult, "Ram not less");


        }


        ///Task ram required is equal to the processor ram

        [TestMethod]
        public void TestRAM_2()
        {
            //Arrange 

            int processorRam = 2;
            int taskRam = 2;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, processorRam, 300, 100);
            Task task = new Task(0, 2.5, 1.6, taskRam, 300, 100);
            bool isRamLessOrEqual = true;

            //Act

            bool result = processor.CheckRam(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isRamLessOrEqual, actualResult, "Ram not equal");

        }


        ///Task ram required is greater than the processor ram

        [TestMethod]
        public void TestRAM_3()
        {
            //Arrange 

            int processorRam = 2;
            int taskRam = 4;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, processorRam, 300, 100);
            Task task = new Task(0, 2.5, 1.6, taskRam, 300, 100);
            bool isRamLessOrEqual = false;

            //Act

            bool result = processor.CheckRam(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isRamLessOrEqual, actualResult, "Ram not greater");
        }



        ///Task download speed required is less the the processor download speed

        [TestMethod]
        public void TestDOWNLOAD_1()
        {
            //Arrange 

            int processorSpeed = 300;
            int taskSpeed = 200;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, 4, processorSpeed, 100);
            Task task = new Task(0, 2.5, 1.6, 2, taskSpeed, 100);
            bool isSpeedLessOrEqual = true;

            //Act

            bool result = processor.CheckDownloadSpeed(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isSpeedLessOrEqual, actualResult, "Task Download speed not less");
        }




        ///Task download speed required is equal the the processor download speed



        [TestMethod]
        public void TestDOWNLOAD_2()
        {
            //Arrange 

            int processorSpeed = 300;
            int taskSpeed = 300;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, 4, processorSpeed, 100);
            Task task = new Task(0, 2.5, 1.6, 2, taskSpeed, 100);

            bool isSpeedLessOrEqual = true;

            //Act

            bool result = processor.CheckDownloadSpeed(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isSpeedLessOrEqual, actualResult, "Task Download speed not Equal");
        }



        ///Task download speed required is greater the the processor download speed



        [TestMethod]
        public void TestDOWNLOAD_3()
        {
            //Arrange 

            int processorSpeed = 200;
            int taskSpeed = 300;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, 4, processorSpeed, 100);
            Task task = new Task(0, 2.5, 1.6, 2, taskSpeed, 100);

            bool isSpeedLessOrEqual = false;

            //Act

            bool result = processor.CheckDownloadSpeed(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isSpeedLessOrEqual, actualResult, "Task download speed is not greater");
        }



        ///Task upload speed required is less than the processor upload speed

        [TestMethod]
        public void TestUPLOAD_1()
        {
            //Arrange 

            int processorSpeed = 300;
            int taskSpeed = 200;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, 4, 400, processorSpeed);
            Task task = new Task(0, 2.5, 1.6, 2, 300, taskSpeed);
            bool isSpeedLessOrEqual = true;

            //Act

            bool result = processor.CheckUploadSpeed(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isSpeedLessOrEqual, actualResult, "Task upload speed not less");
        }




        ///Task upload speed required is equal to the processor upload speed

        [TestMethod]
        public void TestUPLOAD_2()
        {
            //Arrange 

            int processorSpeed = 300;
            int taskSpeed = 300;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, 4, 400, processorSpeed);
            Task task = new Task(0, 2.5, 1.6, 2, 300, taskSpeed);

            bool isSpeedLessOrEqual = true;

            //Act

            bool result = processor.CheckUploadSpeed(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isSpeedLessOrEqual, actualResult, "Task upload speed not Equal");
        }


        ///Task upload speed required is greater than processor upload speed

        [TestMethod]
        public void TestUPLOAD_3()
        {
            //Arrange 

            int processorSpeed = 200;
            int taskSpeed = 300;
            ProcessorType type = new ProcessorType();
            Processor processor = new Processor(0, type, 1.6, 4, 400, processorSpeed);
            Task task = new Task(0, 2.5, 1.6, 2, 300, taskSpeed);

            bool isSpeedLessOrEqual = false;

            //Act

            bool result = processor.CheckUploadSpeed(task);
            bool actualResult = result;

            //Assert

            Assert.AreEqual(isSpeedLessOrEqual, actualResult, "Task upload speed is not greater");
        }


        ///First taff file has valid format

        [TestMethod]
        public void TestTAFF_1()
        {

            //Arrange 
            var path = @"firstTaff.taff";
            bool isValid = false;
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            bool expectedResult = true;

            //Act
            string line = fileReader.readTaff(path, out Errorlist);

            if (Errorlist.Count == 0)
                isValid = true;

            //Assert
            Assert.AreEqual(expectedResult, isValid, "The format of taff file is invalid");


        }

        ///Second taff file has valid format

        [TestMethod]
        public void TestTAFF_2()
        {
            //Arrange 
            var path = @"secondTaff.taff";
            bool isValid = false;
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            bool expectedResult = true;

            //Act
            string line = fileReader.readTaff(path, out Errorlist);

            if (Errorlist.Count == 0)
                isValid = true;

            //Assert
            Assert.AreEqual(expectedResult, isValid, "The format of taff file is invalid");
        }



        ///Third taff file has invalid format

        [TestMethod]
        public void TestTAFF_3()
        {
            //Arrange 
            var path = @"thirdTaff.taff";
            bool isValid = true;
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            bool expectedResult = false;

            //Act
            string line = fileReader.readTaff(path, out Errorlist);

            if (Errorlist.Count > 0)
                isValid = false;

            //Assert
            Assert.AreEqual(expectedResult, isValid, "The format of taff file is  valid");
        }


        ///First cff file has valid format


        [TestMethod]
        public void TestCFF_1()
        {
            //Arrange 
            var path = @"firstcff.taff";
            bool isValid = false;
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            bool expectedResult = true;

            //Act
            string line = fileReader.Readcff(path, out Errorlist);

            if (Errorlist.Count == 0)
                isValid = true;

            //Assert
            Assert.AreEqual(expectedResult, isValid, "The format of cff file is invalid");

        }


        ///Second cff file has valid format

        public void TestCFF_2()
        {
            //Arrange 
            var path = @"secondcff.taff";
            bool isValid = false;
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            bool expectedResult = true;

            //Act
            string line = fileReader.Readcff(path, out Errorlist);

            if (Errorlist.Count == 0)
                isValid = true;

            //Assert
            Assert.AreEqual(expectedResult, isValid, "The format of cff file is invalid");

        }

        ///Third cff file has invalid format
        public void TestCFF_3()
        {
            //Arrange 
            var path = @"thirdcff.taff";
            bool isValid = true;
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            bool expectedResult = true;

            //Act
            string line = fileReader.readTaff(path, out Errorlist);

            if (Errorlist.Count > 0)
                isValid = false;

            //Assert
            Assert.AreEqual(expectedResult, isValid, "The format of cff file is valid");


        }
        
        [TestMethod]
        public void TestErrorsTaff_1()
        {
            //Arrange 
            var path = @"firstTaff.taff";
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            int expectedResult = 3;

            //Act
            string line = fileReader.Readcff(path, out Errorlist);

            int originalResult = Errorlist.Count;

            //Assert
            Assert.AreEqual(expectedResult, originalResult, "The format of cff file is invalid");
        }
        [TestMethod]
        public void TestErrorsTaff_2()
        {
            //Arrange 
            var path = @"firstTaff.taff";         
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            int expectedResult = 2;

            //Act
            string line = fileReader.Readcff(path, out Errorlist);

            int originalResult = Errorlist.Count;

            //Assert
            Assert.AreEqual(expectedResult, originalResult, "The format of cff file is invalid");
        }
        [TestMethod]
        public void TestErrorsTaff_3()
        {
            //Arrange 
            var path = @"firstTaff.taff";
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            int expectedResult = 5;

            //Act
            string line = fileReader.Readcff(path, out Errorlist);

            int originalResult = Errorlist.Count;

            //Assert
            Assert.AreEqual(expectedResult, originalResult, "The format of cff file is invalid");
        }
        
        
        [TestMethod]
      
        public void TestErrorTaff_1()
        {
            var path = @"firstTaff.taff";
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            int expectedResult = 3;

            //Act
            string line = fileReader.Readcff(path, out Errorlist);

            int originalResult = Errorlist.Count;

            //Assert
            Assert.AreEqual(expectedResult, originalResult, "The format of cff file is invalid");
        }

        [TestMethod]

        public void TestErrorTaff_2()
        {
            var path = @"firstTaff.taff";
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            int expectedResult = 3;

            //Act
            string line = fileReader.Readcff(path, out Errorlist);

            int originalResult = Errorlist.Count;

            //Assert
            Assert.AreEqual(expectedResult, originalResult, "The format of cff file is invalid");
        }

        [TestMethod]

        public void TestErrorTaff_3()
        {
            var path = @"firstTaff.taff";
            List<String> Errorlist = new List<string>();
            FileReader fileReader = new FileReader();
            int expectedResult = 3;

            //Act
            string line = fileReader.Readcff(path, out Errorlist);

            int originalResult = Errorlist.Count;

            //Assert
            Assert.AreEqual(expectedResult, originalResult, "The format of cff file is invalid");

        }

    }
}
