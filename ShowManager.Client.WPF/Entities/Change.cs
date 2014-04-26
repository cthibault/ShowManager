﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowManager.Client.WPF.Entities
{
    public struct Change
    {
        public Change(string valueName, object originalValue, object currentValue)
        {
            this._valueName = valueName;
            this._originalValue = originalValue;
            this._currentValue = currentValue;
        }

        public string ValueName { get { return this._valueName; } }
        private readonly string _valueName;

        public object OriginalValue { get { return this._originalValue; } }
        private readonly object _originalValue;

        public object CurrentValue { get { return this._currentValue; } }
        private readonly object _currentValue;
    }
}
