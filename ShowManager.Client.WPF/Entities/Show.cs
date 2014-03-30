using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.Helpers;

namespace ShowManager.Client.WPF.ShowManagement
{
    public partial class Show : IAuditableEntity, ITrackableEntity
    {
        partial void OnAppInstanceKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.AppInstanceKey);
        }
        partial void OnDirectoryChanging(string value)
        {
            this.OnPropertyChanging(() => this.Directory);
        }
        partial void OnImdbIdChanging(string value)
        {
            this.OnPropertyChanging(() => this.ImdbId);
        }
        partial void OnShowKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.ShowKey);
        }
        partial void OnTitleChanging(string value)
        {
            this.OnPropertyChanging(() => this.Title);
        }
        partial void OnTvdbIdChanging(long value)
        {
            this.OnPropertyChanging(() => this.TvdbId);
        }

        #region OnPropertyChanging
        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            this.OnPropertyChanging(PropertyHelper.ExtractPropertyName(propertyExpression));
        }
        private void OnPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        } 
        #endregion
    }
}
