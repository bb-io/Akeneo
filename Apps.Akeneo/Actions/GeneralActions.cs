using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Request.Product;
using Apps.Akeneo.Models.Response;
using Apps.Akeneo.Models.Response.Product;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Akeneo.Actions
{
    [ActionList]
    public class GeneralActions : AkeneoInvocable
    {
        public GeneralActions(InvocationContext invocationContext) : base(invocationContext){ }

        [Action("Get all locales", Description = "Returns a list of language codes that are enabled on the Akeneo instance")]
        public async Task<LocalesResponse> GetAllLocales()
        {
            var request = new RestRequest($"locales");
            var result = await Client.Paginate<LocaleEntity>(request);
            return new LocalesResponse
            {
                Locales = result.Where(x => x.Enabled).Select(x => x.Code)
            };
        }
    }
}
