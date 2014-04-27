using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.Entities;
using ShowManager.Client.WPF.Helpers;

namespace ShowManager.Client.WPF.ShowManagement
{
    public abstract class TrackableObject : ITrackableObject
    {
        #region OnPropertyChanging
        public event PropertyChangingEventHandler PropertyChanging;

        public void OnPropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            this.OnPropertyChanging(PropertyHelper.ExtractPropertyName(propertyExpression));
        }
        public void OnPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }
        #endregion

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            this.OnPropertyChanged(PropertyHelper.ExtractPropertyName(propertyExpression));
        }
        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        } 
        #endregion

        public abstract void Track(ChangeTracker changeTracker, bool track);
    }
}
