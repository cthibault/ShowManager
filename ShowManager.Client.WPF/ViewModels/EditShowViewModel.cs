using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.ViewModels
{
    class EditShowViewModel : BaseViewModel, IEditShowViewModel
    {
        public EditShowViewModel(IUnityContainer unityContainer)
            : base(unityContainer)
        {
            this.Initialize();
        }

        #region Initialization
        private void Initialize()
        {
            this.InitializeEvents();
            this.InitializeCommands();
        } 
        private void InitializeEvents()
        {
            
        }
        private void InitializeCommands()
        {
            //this.CloseCommand = new RelayCommand(() => { }, () => false);
            this.SaveCommand = new RelayCommand(this.OnSave, () => this.Show != null);
            this.RefreshCommand = new RelayCommand(this.OnRefresh, () => this.Show != null && this.Show.ShowKey > 0);
            this.CancelCommand = new RelayCommand(this.OnCancel);
            //this.CloneCommand = new RelayCommand(this.OnClone, () => this.Show != null && this.Show.ShowKey > 0);
            //this.DeleteCommand = new RelayCommand(this.OnDelete, () => this.Show != null && this.Show.ShowKey > 0);            
        }
        #endregion

        #region Refresh
        public RelayCommand RefreshCommand { get; private set; }
        public Action<Show> Refreshed { get; set; }

        private void OnRefresh()
        {
            
        }
        #endregion

        #region Save
        public RelayCommand SaveCommand { get; private set; }
        public Action<Show> Saved { get; set; }

        private void OnSave()
        {
            // TODO: Implement

            if (this.Saved != null)
            {
                this.Saved(this.Show);
            }
        }
        #endregion

        #region Cancel
        public RelayCommand CancelCommand { get; private set; }
        private void OnCancel()
        {
            this.IsOpen = false;
        }
        #endregion

        #region Clone
        public RelayCommand CloneCommand { get; private set; }
        private void OnClone()
        {
            // TODO: Implement
        }
        #endregion

        #region Delete
        public RelayCommand DeleteCommand { get; private set; }
        public Action<Show> Deleted { get; set; }

        private void OnDelete()
        {
            // TODO: Implement

            if (this.Deleted != null)
            {
                this.Deleted(this.Show);
            }
        }
        #endregion


        public void Populate(string header, Show show)
        {
            this.Header = header;

            this.Show = show;

            this.IsOpen = true;
        }

        public void Reset()
        {
            bool reset = true;

            if (this.HasUnsavedChanges)
            {
                // Prompt the user about the changes they are about to lose
                reset = true;
            }

            if (reset)
            {
                this.Show = null;
            }
        }

        public bool HasUnsavedChanges
        {
            get
            {
                return true;
            }
        }


        #region Show
        public Show Show
        {
            get { return this._show; }
            private set { this.Set(() => this.Show, ref this._show, value); }
        }
        private Show _show;
        #endregion

        #region Header
        public string Header
        {
            get { return this._header; }
            private set { this.Set(() => this.Header, ref this._header, value); }
        }
        private string _header;
        #endregion

        #region IsOpen
        public bool IsOpen
        {
            get { return this._isOpen; }
            private set { this.Set(() => this.IsOpen, ref this._isOpen, value); }
        }
        private bool _isOpen;
        #endregion
    }
}
