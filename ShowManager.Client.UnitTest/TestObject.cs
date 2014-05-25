using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.Entities;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.UnitTest
{
    class TestObject : TrackableObject
    {
        public int X
        {
            get { return this._x; }
            set
            {
                if (this._x != value)
                {
                    this.OnPropertyChanging(() => this.X);
                    this._x = value;
                    this.OnPropertyChanged(() => this.X);
                }
            }
        }
        private int _x;

        public int Y
        {
            get { return this._y; }
            set
            {
                if (this._y != value)
                {
                    this.OnPropertyChanging(() => this.Y);
                    this._y = value;
                    this.OnPropertyChanged(() => this.Y);
                }
            }
        }
        private int _y;

        public string Name
        {
            get { return this._name; }
            set
            {
                if (this._name != value)
                {
                    this.OnPropertyChanging(() => this.Name);
                    this._name = value;
                    this.OnPropertyChanged(() => this.Name);
                }
            }
        }
        private string _name;
    }
}
