using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowManager.Client.WPF.ShowManagement;

namespace ShowManager.Client.WPF.Providers
{
    abstract class BaseProvider
    {
        protected BaseProvider(ShowManagementDBEntities context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this._context = context;
        }


        #region SaveContext
        public virtual Task<bool> SaveContext()
        {
            var tcs = new TaskCompletionSource<bool>();

            try
            {
                this.UpdateAuditableProperties();

                this.Context.BeginSaveChanges(asyncResult => this.OnSaveContextComplete(asyncResult, tcs), null);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }
        protected virtual void OnSaveContextComplete(IAsyncResult result, TaskCompletionSource<bool> tcs)
        {
            try
            {
                this.Context.EndSaveChanges(result);

                tcs.TrySetResult(true);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }
        #endregion

        #region UpdateAuditableProperties
        public virtual void UpdateAuditableProperties()
        {
            DateTime now = DateTime.Now;

            foreach (var entityDescriptor in this.Context.Entities.Where(e => e.State == EntityStates.Added || e.State == EntityStates.Modified))
            {
                var auditableEntity = entityDescriptor.Entity as IAuditableEntity;

                if (auditableEntity != null)
                {
                    if (entityDescriptor.State == EntityStates.Modified)
                    {
                        auditableEntity.ModifiedBy = "wpfClient";
                        auditableEntity.ModifiedDtm = now;
                    }
                    else if (entityDescriptor.State == EntityStates.Added)
                    {
                        auditableEntity.CreatedBy = "wpfClient";
                        auditableEntity.CreatedDtm = now;
                        auditableEntity.ModifiedBy = "wpfClient";
                        auditableEntity.ModifiedDtm = now;
                    }
                }
            }
        }
        #endregion


        #region Context
        public ShowManagementDBEntities Context
        {
            get { return this._context; }
        }
        private readonly ShowManagementDBEntities _context;
        #endregion
    }
}
