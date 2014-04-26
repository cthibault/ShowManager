using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.Entities
{
    public class ObjectChangeTracker : IDisposable
    {
        public ObjectChangeTracker(ITrackableObject trackableObject, bool trackChanges = false)
        {
            if (trackableObject == null)
            {
                throw new ArgumentNullException("trackableObject");
            }

            this.Object = trackableObject;
            this.TrackChanges = trackChanges;
        }

        #region Object
        public ITrackableObject Object { get; private set; }
        #endregion

        #region TrackChanges
        public bool TrackChanges
        {
            get { return this._trackChanges; }
            set
            {
                if (this._trackChanges != value)
                {
                    if (value)
                    {
                        this._trackChanges = value;

                        if (this.Object != null)
                        {
                            this.Object.PropertyChanging += this.OnPropertyChanging;
                            this.Object.PropertyChanged += this.OnPropertyChanged;
                        }
                    }
                    else
                    {
                        if (this.Object != null)
                        {
                            this.Object.PropertyChanging -= this.OnPropertyChanging;
                            this.Object.PropertyChanged -= this.OnPropertyChanged;
                        }

                        this.ChangesInternal.Clear();

                        this._trackChanges = value;
                    }
                }
            }
        }
        private bool _trackChanges;
        #endregion

        #region HasChanges
        public bool HasChanges
        {
            get { return this.ChangesInternal.Any(); }
        }
        #endregion

        #region Changes
        public IEnumerable<Change> Changes
        {
            get { return this.ChangesInternal.Values; }
        }
        #endregion

        #region ChangesInternal
        private Dictionary<string, Change> ChangesInternal
        {
            get
            {
                if (this._changeDictionary == null)
                {
                    this._changeDictionary = new Dictionary<string, Change>();
                }
                return this._changeDictionary;
            }
        }
        private Dictionary<string, Change> _changeDictionary;
        #endregion

        #region OnPropertyChanging
        public void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (!this.ChangesInternal.ContainsKey(e.PropertyName))
            {
                object value = this.Object.GetType().GetProperty(e.PropertyName).GetValue(this.Object, null);

                this.ChangesInternal.Add(e.PropertyName, new Change(e.PropertyName, value, null));
            }
        }
        #endregion

        #region OnPropertyChanged
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Change change;

            if (!this.ChangesInternal.TryGetValue(e.PropertyName, out change))
            {
                change = new Change(e.PropertyName, null, null);
            }

            object value = this.Object.GetType().GetProperty(e.PropertyName).GetValue(this.Object, null);

            bool bothNull = change.OriginalValue == null && value == null;

            if (bothNull || (change.OriginalValue != null && change.OriginalValue.Equals(value)))
            {
                if (this.ChangesInternal.ContainsKey(e.PropertyName))
                {
                    this.ChangesInternal.Remove(e.PropertyName);
                }
            }
            else
            {
                var newChange = new Change(e.PropertyName, change.OriginalValue, value);

                if (this.ChangesInternal.ContainsKey(e.PropertyName))
                {
                    this.ChangesInternal[e.PropertyName] = newChange;
                }
                else
                {
                    this.ChangesInternal.Add(e.PropertyName, newChange);
                }
            }
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            this.TrackChanges = false;
            this.ChangesInternal.Clear();
            this.Object = null;
        }
        #endregion
    }
}
