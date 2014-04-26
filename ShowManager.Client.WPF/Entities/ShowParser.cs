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
    public partial class ShowParser : TrackableObject, IAuditableEntity
    {
        partial void OnAppInstanceKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.AppInstanceKey);
        }
        partial void OnParserKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.ParserKey);
        }
        partial void OnShowKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.ShowKey);
        }
        partial void OnShowParserKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.ShowParserKey);
        }
    }
}
