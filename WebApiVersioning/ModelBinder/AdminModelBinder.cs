using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;

namespace WebApi.Binder.ModelBinder
{
    public class AdminModelBinder : IModelBinder
    {
        public AdminModelBinder()
        {
            var a = GlobalConfiguration.Configuration.Services.GetModelMetadataProvider();
        }
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            throw new NotImplementedException();
        }
    }
}