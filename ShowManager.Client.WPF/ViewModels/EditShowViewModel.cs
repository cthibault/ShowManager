﻿using System;
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
        public EditShowViewModel(IUnityContainer unityContainer, ShowManagementDBEntities context, List<ParserType> parserTypes)
            : base(unityContainer)
        {
            this.Context = context;
            this.ParserTypes = parserTypes.OrderBy(pt => pt.SortSeq).ToList();

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
            // App Bar Commands
            this.CancelCommand = new RelayCommand(this.OnCancel);
            this.SaveCommand = new RelayCommand(this.OnSave, () => this.HasUnsavedChanges);
            this.RefreshCommand = new RelayCommand(this.OnRefresh, () => this.Show != null && this.Show.ShowKey > 0);
            this.DeleteCommand = new RelayCommand(this.OnDelete, () => this.Show != null && this.Show.ShowKey > 0);

            // Show Detail Commands
            this.BrowseDirectoryCommand = new RelayCommand(this.OnBrowseDirectory);

            // Parser Grid Commands
            this.AddParserCommand = new RelayCommand(this.OnAddParser, () => this.Show != null);
            this.CloneParserCommand = new RelayCommand<Parser>(this.OnCloneParser);
            this.DeleteParserCommand = new RelayCommand<Parser>(this.OnDeleteParser);
        }
        #endregion


        #region Actions
        public Action<Show> Save { get; set; }
        public Action<Show> Refresh { get; set; }
        public Action<Show> Cancel { get; set; }
        public Action<Show> Delete { get; set; }
        #endregion

        #region Show
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
            var confirmDelete = await this.ConfirmDelete();

            if (this.Delete != null && confirmDelete)
            {
                this.Delete(this.Show);
            }
        }
        private Task<bool> ConfirmDelete()
        {
            var tcs = new TaskCompletionSource<bool>();

            Action accept = () =>
            {
                tcs.TrySetResult(true);
            };

            Action reject = () =>
            {
                this.PromptModel = null;
                tcs.TrySetResult(false);
            };

            this.PromptModel = new PromptModel("Delete?", accept, reject);

            return tcs.Task;
        }
        #endregion 

        #region BrowseDirectory
        public RelayCommand BrowseDirectoryCommand { get; private set; }
        private void OnBrowseDirectory()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = "Select Show's Directory",
                RootFolder = Environment.SpecialFolder.MyComputer,
                SelectedPath = this.Show.Directory
            };

            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.Show.Directory = dialog.SelectedPath;
            }
        }
        #endregion
        #endregion

        #region Parsers
        #region Add
        public RelayCommand AddParserCommand { get; private set; }
        private void OnAddParser()
        {
            var parser = new Parser { AppInstanceKey = this.Show.AppInstanceKey };

            parser.Track(this.ChangeTracker, true);

            this.Show.Parsers.Add(parser);
        }
        #endregion

        #region Delete
        public RelayCommand<Parser> CloneParserCommand { get; private set; }
        private void OnCloneParser(Parser parser)
        {
            if (parser != null)
            {
                var newParser = new Parser();

                newParser.Track(this.ChangeTracker, true);

                newParser.AppInstanceKey = parser.AppInstanceKey;
                newParser.ParserTypeKey = parser.ParserTypeKey;
                newParser.Pattern = parser.Pattern;
                newParser.ExcludedCharacters = parser.ExcludedCharacters;
                newParser.ShowKey = parser.ShowKey;

                this.Show.Parsers.Add(newParser);
            }
        }
        #endregion

        #region Delete
        public RelayCommand<Parser> DeleteParserCommand { get; private set; }
        private void OnDeleteParser(Parser parser)
        {
            if (parser != null)
            {
                this.Show.Parsers.Remove(parser);

                // HACK
                var parserChangeTracker = this.ChangeTracker.GetObjectChangeTracker(parser);

                if (parserChangeTracker != null)
                {
                    parserChangeTracker.ManualOverrideHasChanges = true;
                }
            }
        }
        #endregion
        #endregion


        #region Populate
        public void Populate(string header, Show show)
        {
            this.Header = header;

            this.Show = show;

            this.ChangeTracker.Clear();

            this.Show.Track(this.ChangeTracker, true);

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

        #region ParserTypes
        public List<ParserType> ParserTypes
        {
            get
            {
                if (this._parserTypes == null)
                {
                    this._parserTypes = new List<ParserType>();
                }
                return this._parserTypes;
            }
            private set { this._parserTypes = value; }
        }
        private List<ParserType> _parserTypes;
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

        #region Context
        private ShowManagementDBEntities Context { get; set; }
        #endregion
    }
}
