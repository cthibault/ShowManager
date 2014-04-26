using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.Entities
{
    public class ChangeTracker : IDisposable
    {
        #region ObjectChangeTrackers
        public List<ObjectChangeTracker> ObjectChangeTrackers
        {
            get
            {
                if (this._objectChangeTrackers == null)
                {
                    this._objectChangeTrackers = new List<ObjectChangeTracker>();
                }
                return this._objectChangeTrackers;
            }
            protected set { this._objectChangeTrackers = value; }
        }
        private List<ObjectChangeTracker> _objectChangeTrackers; 
        #endregion

        #region Objects
        public IEnumerable<ITrackableObject> Objects
        {
            get
            {
                return this.ObjectChangeTrackers.Select(oct => oct.Object);
            }
        } 
        #endregion

        #region SetTrackChanges
        /// <summary>
        /// Sets Track Changes for all Trackable Objects
        /// </summary>
        /// <param name="trackChanges"></param>
        public void SetTrackChanges(bool trackChanges)
        {
            this.ObjectChangeTrackers.ForEach(oct => oct.TrackChanges = trackChanges);
        } 

        /// <summary>
        /// Sets Track Changes for the provided Trackable Objects
        /// </summary>
        /// <param name="trackableObjects"></param>
        /// <param name="trackChanges"></param>
        public void SetTrackChanges(IEnumerable<ITrackableObject> trackableObjects, bool trackChanges)
        {
            if (trackableObjects != null)
            {
                foreach (var trackableObject in trackableObjects)
                {
                    var objectChangeTracker = this.GetObjectChangeTracker(trackableObject);
                    if (objectChangeTracker != null)
                    {
                        objectChangeTracker.TrackChanges = trackChanges;
                    }
                }
            }
        }
        #endregion

        #region Add
        public ObjectChangeTracker Add(ITrackableObject trackableObject, bool trackChanges = false)
        {
            ObjectChangeTracker objectChangeTracker = this.GetObjectChangeTracker(trackableObject); ;

            if (objectChangeTracker == null)
            {
                objectChangeTracker = new ObjectChangeTracker(trackableObject, trackChanges);

                this.ObjectChangeTrackers.Add(objectChangeTracker);
            }
            else
            {
                objectChangeTracker.TrackChanges = trackChanges;
            }

            return objectChangeTracker;
        }
        public IEnumerable<ObjectChangeTracker> Add(IEnumerable<ITrackableObject> trackableObjects, bool trackChanges = false)
        {
            List<ObjectChangeTracker> objectChangeTrackers = new List<ObjectChangeTracker>();

            if (trackableObjects != null)
            {
                foreach (var trackableObject in trackableObjects)
                {
                    var objectChangeTracker = this.Add(trackableObject, trackChanges);
                    if (objectChangeTracker != null)
                    {
                        objectChangeTrackers.Add(objectChangeTracker);
                    }
                }
            }

            return objectChangeTrackers;
        } 
        #endregion

        #region Remove
        public bool Remove(ITrackableObject trackableObject)
        {
            bool removed = false;

            var objectChangeTracker = this.GetObjectChangeTracker(trackableObject);
            if (objectChangeTracker != null)
            {
                removed = this.ObjectChangeTrackers.Remove(objectChangeTracker);
            }

            return removed;
        }
        public void Remove(IEnumerable<ITrackableObject> trackableObjects)
        {
            if (trackableObjects != null)
            {
                foreach (var trackableObject in trackableObjects)
                {
                    this.Remove(trackableObject);
                }
            }
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            this.ObjectChangeTrackers.Clear();
        }
        #endregion

        #region HasChanges
        public bool HasChanges()
        {
            bool hasChanges = this.ObjectChangeTrackers.Any(oct => oct.HasChanges);

            return hasChanges;
        }
        public bool HasChanges(ITrackableObject trackableObject)
        {
            bool hasChanges = false;

            var objectChangeTracker = this.GetObjectChangeTracker(trackableObject);
            if (objectChangeTracker != null)
            {
                hasChanges = objectChangeTracker.HasChanges;
            }

            return hasChanges;
        } 
        #endregion

        #region GetObjectChangeTracker
        public ObjectChangeTracker GetObjectChangeTracker(ITrackableObject trackableObject)
        {
            ObjectChangeTracker objectChangeTracker = null;

            if (trackableObject != null)
            {
                objectChangeTracker = this.ObjectChangeTrackers.SingleOrDefault(oct => object.ReferenceEquals(oct.Object, trackableObject));
            }

            return objectChangeTracker;
        } 
        #endregion

        #region Dispose
        public void Dispose()
        {
            this.Clear();
        }
        #endregion
    }
}
