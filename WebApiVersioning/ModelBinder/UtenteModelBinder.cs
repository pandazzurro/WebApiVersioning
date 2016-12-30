using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.ModelBinding;
using System.Web.Http.Validation;
using System.Web.Http.ValueProviders;
using System.Xml.Linq;
using System.Xml.Serialization;
using WebApi.Binder.Models;

namespace WebApi.Binder.ModelBinder
{
    public class UtenteModelBinder : IModelBinder
    {
        private readonly ModelMetadataProvider _modelMetadataProvider;
        public UtenteModelBinder()
        {
            _modelMetadataProvider = GlobalConfiguration.Configuration.Services.GetModelMetadataProvider();            
        }

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(UtenteViewModel))
            {
                Log.Debug("Modello non corretto");
                return false;
            }
            ValueProviderResult val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            UtenteViewModel result = null;
            if (actionContext.Request.Content.Headers.ContentType.MediaType == "application/json")
            {
                try
                {
                    var stringa = actionContext.Request.Content.ReadAsStringAsync().Result;
                    Log.Verbose(stringa);
                    var key = stringa;
                    result = JsonConvert.DeserializeObject<UtenteViewModel>(key, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Include,
                        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                    });

                    // altre manipolazioni
                }
                catch(Exception ex)
                {
                    Log.Error("Errore deserializazione", ex);
                }
            }
            else
            {
                var stringa = actionContext.Request.Content.ReadAsStringAsync().Result;
                Log.Verbose(stringa);

                var key = actionContext.Request.Content.ReadAsStreamAsync().Result;
                result = (UtenteViewModel)new XmlSerializer(typeof(UtenteViewModel)).Deserialize(key);

                // altre manipolazioni
            }

            bindingContext.Model = result;
            var defaultModelBinder = new DefaultBodyModelValidator().Validate(result, typeof(UtenteViewModel), _modelMetadataProvider, actionContext, string.Empty);
            return defaultModelBinder;
        }
    }
}