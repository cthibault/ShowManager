﻿using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using ReactiveUI;
using ShowManager.Client.WPF.Events;
using ShowManager.Client.WPF.Infrastructure;
using ShowManager.Client.WPF.ShowManagement;
using ShowManager.Client.WPF.ViewModels;
using ShowManager.Client.WPF.Views;
using ShowManager.Client.WPF.Extensions;

namespace ShowManager.Client.WPF.ViewModels
{
    class ShowsViewModel : BaseViewModel, ITabViewModel
    {
        public ShowsViewModel(IUnityContainer unityContainer, IEventPublisher eventPublisher)
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
            this.EventPublisher.GetEvent<ClosingEvent>().Subscribe(e => { });
        }
        private void InitializeCommands()
        {
            this.RefreshAllCommand = ReactiveCommand.Create(Observable.Return(true), RxApp.MainThreadScheduler);
            this.RefreshAllCommand.Subscribe(x => this.OnRefreshAll());

            var showsCollectionIsAvailable = this.WhenAny(vm => vm.Shows, x => x.Value != null);

            this.AddCommand = ReactiveCommand.Create(showsCollectionIsAvailable, RxApp.MainThreadScheduler);
            this.AddCommand.Subscribe(x => this.OnAdd());

            this.EditCommand = ReactiveCommand.Create(showsCollectionIsAvailable, RxApp.MainThreadScheduler);
            this.EditCommand.Subscribe(show => this.OnEdit(show as Show));
        }
        #endregion

        #region RefreshAll
        public ReactiveCommand<object> RefreshAllCommand { get; private set; }        
        private void OnRefreshAll()
        {
            try
            {
                this.Shows = new DataServiceCollection<Show>(this.Context.Shows);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Refresh
        public ReactiveCommand<object> RefreshCommand { get; private set; }
        private void OnRefresh(Show show)
        {
            if (show != null)
            {
                // Detach current show from context and replace with new instance.

                var currentShow = this.Shows.SingleOrDefault(s => s.ShowKey == show.ShowKey);
                if (currentShow != null)
                {
                    var entityDescriptor = this.Context.GetEntityDescriptor(currentShow);
                    if (entityDescriptor != null)
                    {
                        this.Context.Detach(currentShow);
                    }
                    
                    this.Shows.Remove(currentShow);
                }

                this.Context.AttachTo(() => this.Context.Shows, show);
                this.Shows.Add(show);
            }
        }
        #endregion

        #region Add
        public ReactiveCommand<object> AddCommand { get; private set; }
        private void OnAdd()
        {
            try
            {
                var show = new Show();
                this.LaunchEditView("add", show);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Edit
        public ReactiveCommand<object> EditCommand { get; private set; }
        private void OnEdit(Show show)
        {
            if (show != null)
            {
                this.LaunchEditView("edit", show);
            }
        }
        #endregion

        #region Clone
        public ReactiveCommand<object> CloneCommand { get; private set; }
        private void OnClone(Show show)
        {
        }
        #endregion

        #region Delete
        public ReactiveCommand<object> DeleteCommand { get; private set; }
        private void OnDelete(Show show)
        {
        }
        #endregion

        #region LaunchEditView
        private void LaunchEditView(string header, Show show)
        {
            var opened = this.EditShowViewModel.TryOpen(header, show);
        }
        #endregion

        #region Shows
        public DataServiceCollection<Show> Shows
        {
            get { return this._shows; }
            set { this.RaiseAndSetIfChanged(ref this._shows, value); }
        }
        private DataServiceCollection<Show> _shows;
        #endregion

        #region Selected Show
        public Show SelectedShow
        {
            get { return this._selectedShow; }
            set { this.RaiseAndSetIfChanged(ref this._selectedShow, value); }
        }
        private Show _selectedShow;
        #endregion

        #region EditShowViewModel
        public IEditShowViewModel EditShowViewModel
        {
            get
            {
                if (this._editShowViewModel == null)
                {
                    this._editShowViewModel = this.UnityContainer.Resolve<IEditShowViewModel>();

                    this._editShowViewModel.Refreshed = show => this.OnRefresh(show);
                    this._editShowViewModel.Saved = show => this.OnRefresh(show);
                    this._editShowViewModel.Deleted = show => this.OnDelete(show);
                }

                return this._editShowViewModel;
            }
        }
        private IEditShowViewModel _editShowViewModel;
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

        #region ITabViewModel
        public bool IsActive { get; set; }
        public string Title { get { return ShowsViewModel.TitleText; } }
        public IView View
        {
            get
            {
                if (this._view == null)
                {
                    this._view = this.UnityContainer.Resolve<IView>(this.Title);
                    this._view.DataContext = this;
                }
                return this._view;
            }
        }
        private IView _view;
        #endregion

        #region Constants
        public static readonly string TitleText = "shows";
        #endregion
    }
}
