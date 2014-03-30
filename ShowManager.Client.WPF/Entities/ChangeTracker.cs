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
        public ChangeTracker(ITrackableEntity entity, bool trackChanges = false)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            this.Entity = entity;
            this.TrackChanges = trackChanges;
        }

        #region Entity
        public ITrackableEntity Entity { get; private set; } 
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

                        if (this.Entity != null)
                        {
                            this.Entity.PropertyChanging += this.OnPropertyChanging;
                            this.Entity.PropertyChanged += this.OnPropertyChanged;
                        }
                    }
                    else
                    {
                        if (this.Entity != null)
                        {
                            this.Entity.PropertyChanging -= this.OnPropertyChanging;
                            this.Entity.PropertyChanged -= this.OnPropertyChanged;
                        }

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
            get { return this.ChangeDictionary.Any(); }
        } 
        #endregion

        #region ChangeDictionary
        public Dictionary<string, Tuple<object, object>> ChangeDictionary
        {
            get
            {
                if (this._changeDictionary == null)
                {
                    this._changeDictionary = new Dictionary<string, Tuple<object, object>>();
                }
                return this._changeDictionary;
            }
        }
        private Dictionary<string, Tuple<object, object>> _changeDictionary; 
        #endregion

        #region OnPropertyChanging
        public void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (!this.ChangeDictionary.ContainsKey(e.PropertyName))
            {
                object value = this.Entity.GetType().GetProperty(e.PropertyName).GetValue(this.Entity, null);

                this.ChangeDictionary.Add(e.PropertyName, new Tuple<object, object>(value, null));
            }
        } 
        #endregion

        #region OnPropertyChanged
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Tuple<object, object> historical = null;

            this.ChangeDictionary.TryGetValue(e.PropertyName, out historical);

            if (historical == null)
            {
                historical = new Tuple<object, object>(null, null);
            }

            object value = this.Entity.GetType().GetProperty(e.PropertyName).GetValue(this.Entity, null);

            bool bothNull = historical.Item1 == null && value == null;

            if (bothNull || (historical.Item1 != null && historical.Item1.Equals(value)))
            {
                if (this.ChangeDictionary.ContainsKey(e.PropertyName))
                {
                    this.ChangeDictionary.Remove(e.PropertyName);
                }
            }
            else
            {
                var newHistorical = new Tuple<object, object>(historical.Item1, value);

                if (this.ChangeDictionary.ContainsKey(e.PropertyName))
                {
                    this.ChangeDictionary[e.PropertyName] = newHistorical;
                }
                else
                {
                    this.ChangeDictionary.Add(e.PropertyName, newHistorical);
                }
            }
        } 
        #endregion

        #region Dispose
        public void Dispose()
        {
            this.TrackChanges = false;
            this.ChangeDictionary.Clear();
            this.Entity = null;
        } 
        #endregion
    }
}
