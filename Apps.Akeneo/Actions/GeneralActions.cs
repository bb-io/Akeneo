using Apps.Akeneo.Invocables;
using Apps.Akeneo.Models.Entities;
using Apps.Akeneo.Models.Response;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

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
