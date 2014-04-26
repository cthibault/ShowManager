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
    public class ObjectChangeTrackerUnitTests
    {
        [TestMethod]
        public void TrackOff_NoChange_HasChangesReturnFalse()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, false);

            Assert.IsFalse(objectChangeTracker.HasChanges);
        }

        [TestMethod]
        public void TrackOn_NoChange_HasChangesReturnFalse()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, true);

            Assert.IsFalse(objectChangeTracker.HasChanges);
        }

        [TestMethod]
        public void TrackOff_OneChange_HasChangesReturnFalse()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, false);

            testObject.Name = "Test With Changes";

            Assert.IsFalse(objectChangeTracker.HasChanges);
        }

        [TestMethod]
        public void TrackOn_OneChange_HasChangesReturnTrue()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, true);

            testObject.Name = "Test With Changes";

            Assert.IsTrue(objectChangeTracker.HasChanges);
        }

        [TestMethod]
        public void TrackOn_OneChange_CorrectChangeRecord()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, true);

            string propertyName = "Name";
            string oldValue = "Test";
            string newValue1 = "Test with Changes";

            testObject.Name = newValue1;

            Assert.IsTrue(objectChangeTracker.Changes.Count() == 1);

            var change = objectChangeTracker.Changes.First();

            Assert.AreEqual(propertyName, change.ValueName);
            Assert.AreEqual(oldValue, change.OriginalValue);
            Assert.AreEqual(newValue1, change.CurrentValue);

            string newValue2 = "Curtis";

            testObject.Name = newValue2;

            Assert.IsTrue(objectChangeTracker.Changes.Count() == 1);

            change = objectChangeTracker.Changes.First();

            Assert.AreEqual(propertyName, change.ValueName);
            Assert.AreEqual(oldValue, change.OriginalValue);
            Assert.AreEqual(newValue2, change.CurrentValue);
        }

        [TestMethod]
        public void TrackOn_OneChange_Revert_HasChangesReturnFalse()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, true);

            testObject.Name = "Test With Changes";

            testObject.Name = "Test";

            Assert.IsFalse(objectChangeTracker.HasChanges);
        }

        [TestMethod]
        public void TrackOn_MultipleChanges_HasChangesReturnTrue()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, true);

            testObject.Name = "Test With Changes";

            testObject.X = 1;

            Assert.IsTrue(objectChangeTracker.HasChanges);
        }

        [TestMethod]
        public void TurnTrackOff_ClearsChanges()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, true);

            testObject.Name = "Test With Changes";

            testObject.X = 1;

            Assert.IsTrue(objectChangeTracker.HasChanges);

            objectChangeTracker.TrackChanges = false;

            Assert.IsFalse(objectChangeTracker.HasChanges);
        }

        [TestMethod]
        public void DisposeObjectChangeTracker_ObjectNull_EntityRemains()
        {
            var testObject = new TestObject { X = 0, Y = 0, Name = "Test" };

            var objectChangeTracker = new ObjectChangeTracker(testObject, true);

            objectChangeTracker.Dispose();
            
            Assert.IsNull(objectChangeTracker.Object);
            Assert.IsNotNull(testObject);
        }
    }
}
