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
    public partial class Parser : TrackableObject, IAuditableEntity
    {
        partial void OnAppInstanceKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.AppInstanceKey);
        }
        partial void OnParserKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.ParserKey);
        }
        partial void OnParserTypeKeyChanging(int value)
        {
            this.OnPropertyChanging(() => this.ParserTypeKey);
        }
        partial void OnExcludedCharactersChanging(string value)
        {
            this.OnPropertyChanging(() => this.ExcludedCharacters);
        }
        partial void OnPatternChanging(string value)
        {
            this.OnPropertyChanging(() => this.Pattern);
        }


    }
}
