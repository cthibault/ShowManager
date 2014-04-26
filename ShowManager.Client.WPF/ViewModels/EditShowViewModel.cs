using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Entities;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.Models;
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
            this.CancelCommand = new RelayCommand(this.OnCancel);
            this.SaveCommand = new RelayCommand(this.OnSave, () => this.HasUnsavedChanges);
            this.RefreshCommand = new RelayCommand(this.OnRefresh, () => this.Show != null && this.Show.ShowKey > 0);
            this.DeleteCommand = new RelayCommand(this.OnDelete, () => this.Show != null && this.Show.ShowKey > 0);
        }
        #endregion


        #region Actions
        public Action<Show> Save { get; set; }
        public Action<Show> Refresh { get; set; }
        public Action<Show> Cancel { get; set; }
        public Action<Show> Delete { get; set; }
        #endregion



        #region Save
        public RelayCommand SaveCommand { get; private set; }
        private void OnSave()
        {
            if (this.Save != null)
            {
                this.Save(this.Show);
            }
        }
        #endregion

        #region Refresh
        public RelayCommand RefreshCommand { get; private set; }
        private void OnRefresh()
        {
            if (this.Refresh != null)
            {
                this.Refresh(this.Show);
            }
        }
        #endregion

        #region Cancel
        public RelayCommand CancelCommand { get; private set; }
        private async void OnCancel()
        {
            if (this.Cancel != null)
            {
                this.Cancel(this.Show);
            }
        }
        #endregion

        #region Delete
        public RelayCommand DeleteCommand { get; private set; }
        private async void OnDelete()
        {
            if (this.Delete != null)
            {
                this.Delete(this.Show);
            }
        }
        #endregion


        #region Populate
        public void Populate(string header, Show show)
        {
            this.Header = header;

            this.Show = show;

            this.ChangeTracker.Clear();

            this.ChangeTracker.Add(this.Show, true);

            foreach (var showParser in this.Show.ShowParsers)
            {
                this.ChangeTracker.Add(showParser, true);
            }

            this.IsOpen = true;
        } 
        #endregion

        #region Clear
        public Task<bool> TryClearAsyc()
        {
            var tcs = new TaskCompletionSource<bool>();

            if (this.HasUnsavedChanges)
            {
                Action accept = () => 
                    {
                        this.Clear();
                        tcs.TrySetResult(true);
                    };

                Action reject = () => 
                    {
                        this.PromptModel = null;
                        tcs.TrySetResult(false);
                    };

                this.PromptModel = new PromptModel("Discard Changes?", accept, reject);
            }
            else
            {
                this.Show = null;
                tcs.TrySetResult(true);
            }

            return tcs.Task;
        } 
        
        private void Clear()
        {
            this.ChangeTracker.Dispose();
            this.Show = null;
            this.PromptModel = null;
        }
        #endregion

        #region Close
        //TODO: Temp fix until the VisualTemplate is updated so that the close button is not visible
        //  -Always disabled
        public RelayCommand CloseCommand 
        {
            get 
            {
                if (this._closeCommand == null)
                {
                    this._closeCommand = new RelayCommand(() => { /* Do Nothing */ }, () => false);
                }
                return this._closeCommand;
            }
        }
        private RelayCommand _closeCommand;

        public async Task<bool> TryClearAndCloseAsync()
        {
            bool clear = await this.TryClearAsyc();
            if (clear)
            {
                this.IsOpen = false;
            }

            return clear;
        }

        public async Task CloseAndDiscardChangesAsync()
        {
            this.Clear();
            this.IsOpen = false;
        }
        #endregion

        
        
        #region HasUnsavedChanges
        public bool HasUnsavedChanges
        {
            get
            {
                return this.IsOpen
                    && this.ChangeTracker.HasChanges();
            }
        } 
        #endregion


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

        #region PromptModel
        public IPromptModel PromptModel
        {
            get { return this._promptModel; }
            set { this.Set(() => this.PromptModel, ref this._promptModel, value); }
        }
        private IPromptModel _promptModel;
        #endregion

        #region ObjectChangeTracker
        public ChangeTracker ChangeTracker
        {
            get
            {
                if (this._changeTracker == null)
                {
                    this._changeTracker = new ChangeTracker();
                }
                return this._changeTracker;
            }
        }
        private ChangeTracker _changeTracker;
        #endregion
    }
}
