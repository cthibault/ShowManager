using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShowManager.Client.WPF.Entities;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.UnitTest
{
    [TestClass]
    public class ChangeTrackerUnitTests
    {
        [TestMethod]
        public void AddObject_SingleRecord()
        {
            var changeTracker = new ChangeTracker();

            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };
            changeTracker.Add(testObject, false);

            Assert.IsTrue(changeTracker.Objects.Count() == 1);
            Assert.IsTrue(changeTracker.ObjectChangeTrackers.Count == 1);
        }

        [TestMethod]
        public void AddObjectTwice_SingleRecord()
        {
            var changeTracker = new ChangeTracker();

            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            changeTracker.Add(testObject, false);
            changeTracker.Add(testObject, false);

            Assert.IsTrue(changeTracker.Objects.Count() == 1);
            Assert.IsTrue(changeTracker.ObjectChangeTrackers.Count == 1);
        }

        [TestMethod]
        public void AddTwoObjects_TwoRecord()
        {
            var changeTracker = new ChangeTracker();

            var testObject1 = new TestObject { X = 0, Y = 0, Name = "One" };
            var testObject2 = new TestObject { X = 1, Y = 1, Name = "Two" };

            changeTracker.Add(testObject1, false);
            changeTracker.Add(testObject2, false);

            Assert.IsTrue(changeTracker.Objects.Count() == 2);
            Assert.IsTrue(changeTracker.ObjectChangeTrackers.Count == 2);
        }

        [TestMethod]
        public void AddObject_RemoveObject_NoRecords()
        {
            var changeTracker = new ChangeTracker();

            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            changeTracker.Add(testObject, false);
            changeTracker.Remove(testObject);

            Assert.IsTrue(changeTracker.Objects.Count() == 0);
            Assert.IsTrue(changeTracker.ObjectChangeTrackers.Count == 0);
        }


        [TestMethod]
        public void SingleObject_TrackOff_HasChangesReturnsFalse()
        {
            var changeTracker = new ChangeTracker();

            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };
            changeTracker.Add(testObject, false);

            testObject.Name = "Test Changes";

            Assert.IsFalse(changeTracker.HasChanges());
        }

        [TestMethod]
        public void SingleObject_TrackOn_HasChangesReturnsTrue()
        {
            var changeTracker = new ChangeTracker();

            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };
            changeTracker.Add(testObject, true);

            testObject.Name = "Test Changes";

            Assert.IsTrue(changeTracker.HasChanges());
        }

        [TestMethod]
        public void MultipleObject_MixedTrackChanges_HasChangesReturnsTrue()
        {
            var changeTracker = new ChangeTracker();

            var testObject1 = new TestObject { X = 0, Y = 0, Name = "One" };
            var testObject2 = new TestObject { X = 0, Y = 0, Name = "Two" };

            changeTracker.Add(testObject1, true);
            changeTracker.Add(testObject2, false);

            testObject1.Name = "One Changes";
            testObject2.Name = "Two Changes";

            var oct1 = changeTracker.GetObjectChangeTracker(testObject1);
            var oct2 = changeTracker.GetObjectChangeTracker(testObject2);

            Assert.IsTrue(changeTracker.HasChanges());

            Assert.IsTrue(changeTracker.HasChanges(testObject1));
            Assert.IsFalse(changeTracker.HasChanges(testObject2));
            
            Assert.IsTrue(oct1.HasChanges);
            Assert.IsFalse(oct2.HasChanges);
        }
    }
}
