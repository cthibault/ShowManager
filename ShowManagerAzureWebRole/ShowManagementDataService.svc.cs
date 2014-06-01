//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace ShowManagerAzureWebRole
{
    public class ShowManagementDataService : DataService<ShowManagementDBEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("ApplicationInstances", EntitySetRights.AllRead | EntitySetRights.AllWrite);
            config.SetEntitySetAccessRule("ParserTypes", EntitySetRights.AllRead | EntitySetRights.AllWrite);
            config.SetEntitySetAccessRule("Parsers", EntitySetRights.AllRead | EntitySetRights.AllWrite);
            config.SetEntitySetAccessRule("Shows", EntitySetRights.AllRead | EntitySetRights.AllWrite);

            config.UseVerboseErrors = true;
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }
    }
}
