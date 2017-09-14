using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Umbraco.Core;
using Umbraco.RestApi.Models;

namespace Umbraco.RestApi.Controllers
{
    public class QueryStructureModelBinder : IModelBinder
    {
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// </summary>
        /// <returns>
        /// true if model binding is successful; otherwise, false.
        /// </returns>
        /// <param name="actionContext">The action context.</param><param name="bindingContext">The binding context.</param>
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var queryStructure = new QueryStructure();

            var query = actionContext.Request.GetQueryNameValuePairs().ToArray();

            int pageSize;
            if (query.Any(x => x.Key.InvariantEquals("size")) && int.TryParse(query.Single(x => x.Key.InvariantEquals("size")).Value, out pageSize))
            {
                queryStructure.PageSize = pageSize;
            }

            int pageIndex;
            if (query.Any(x => x.Key.InvariantEquals("page")) && int.TryParse(query.Single(x => x.Key.InvariantEquals("page")).Value, out pageIndex))
            {
                queryStructure.Page = pageIndex;
            }

            if (query.Any(x => x.Key.InvariantEquals("query")))
            {
                queryStructure.Query = query.Single(x => x.Key.InvariantEquals("query")).Value;
            }

            bindingContext.Model = queryStructure;
            return true;
        }
    }
}