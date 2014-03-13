using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.Unity;
using ShowManager.Client.WPF.Events;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.ViewModels
{
    class EditShowViewModel : BaseViewModel, IEditShowViewModel
    {
        public EditShowViewModel(IUnityContainer unityContainer, IEventPublisher eventPublisher)
            : base(unityContainer, eventPublisher)
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
            this.CloseCommand = new RelayCommand(() => { }, () => false);
            this.SaveCommand = new RelayCommand(this.OnSave, () => this.Show != null);
            this.RefreshCommand = new RelayCommand(this.OnRefresh, () => this.Show != null && this.Show.ShowKey > 0);
            this.CancelCommand = new RelayCommand(this.OnCancel);
            this.CloneCommand = new RelayCommand(this.OnClone, () => this.Show != null && this.Show.ShowKey > 0);
            this.DeleteCommand = new RelayCommand(this.OnDelete, () => this.Show != null && this.Show.ShowKey > 0);            
        }
        #endregion

        #region Open
        public bool TryOpen(string header, Show show)
        {
            // TODO: Check for unsaved changes and prompt the user
            bool canOpen = this.HandleUnsavedChanges();

            if (canOpen)
            {
                this.LoadData(header, show);

                this.IsOpen = true;
            }

            return canOpen;
        }
        #endregion

        #region Close
        public RelayCommand CloseCommand { get; private set; }

        public bool TryClose()
        {
            // TODO: Check for unsaved changes and prompt the user
            bool canClose = this.HandleUnsavedChanges();

            if (canClose)
            {
                this.IsOpen = false;
            }

            return canClose;
        }
        #endregion


        #region LoadData
        private void LoadData(string header, Show show)
        {
            this.FlushContext();

            this.Header = header;

            if (show != null && show.ShowKey > 0)
            {
                show = this.Refresh(show.ShowKey);
            }
            else if (show == null)
            {
                show = new Show();
            }

            var entityDescriptor = this.Context.GetEntityDescriptor(show);
            if (entityDescriptor == null)
            {
                // Add the new entity to the Context so that its changes can be tracked
                this.Context.AddToShows(show);
            }

            this.Show = show;
        }
        #endregion

        #region Refresh
        public RelayCommand RefreshCommand { get; private set; }
        public Action<Show> Refreshed { get; set; }

        private void OnRefresh()
        {
            if (this.Show != null)
            {
                var show = this.Refresh(this.Show.ShowKey);

                // What should happen if Refresh returns null?
                if (show != null)
                {
                    this.Show = show;
                }
            }
        }

        private Show Refresh(int showKey)
        {
            Show show = null;

            try
            {
                var showsQuery = this.Context.Shows
                    //.Expand(s => s.ShowParsers)
                    .Where(s => s.ShowKey == showKey);

                show = showsQuery.SingleOrDefault();

                if (this.Refreshed != null)
                {
                    this.Refreshed(show);
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return show;
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

        #region HandleUnsavedChanges
        private bool HandleUnsavedChanges()
        {
            return true;
        }
        #endregion

        #region FlushContext
        private void FlushContext()
        {
            this._context = null;
        }
        #endregion


        #region Show
        public Show Show
        {
            get { return this._show; }
            set { this.Set(() => this.Show, ref this._show, value); }
        }
        private Show _show;
        #endregion

        #region Header
        public string Header
        {
            get { return this._header; }
            set { this.Set(() => this.Header, ref this._header, value); }
        }
        private string _header;
        #endregion

        #region IsOpen
        public bool IsOpen
        {
            get { return this._isOpen; }
            set { this.Set(() => this.IsOpen, ref this._isOpen, value); }
        }
        private bool _isOpen;
        #endregion

        #region Context
        private ShowManagement.ShowManagementDBEntities Context
        {
            get
            {
                if (this._context == null)
                {
                    this._context = ContextFactory.Create(MergeOption.OverwriteChanges);
                }
                return this._context;
            }
        }
        private ShowManagement.ShowManagementDBEntities _context;
        #endregion
    }
}
