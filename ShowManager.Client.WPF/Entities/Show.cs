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
    public partial class Show : TrackableObject, IAuditableEntity
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

        protected override void TrackAdditionalEntities(ChangeTracker changeTracker, bool track)
        {
            if (changeTracker != null)
            {
                foreach (var showParser in this.ShowParsers)
                {
                    showParser.Track(changeTracker, track);
                }
            }
        }
    }
}
