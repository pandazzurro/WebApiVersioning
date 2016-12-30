using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;

namespace WebApi.Versioning
{
    public class VersionControllerSelector : DefaultHttpControllerSelector
    {
        public VersionControllerSelector(HttpConfiguration config) : base(config)
        {
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            try
            {
                var routeData = request.GetRouteData();
                var area = routeData.Values["area"].ToString();     //recupero l'area per verificare se deve intervenire il selector sulla versione o meno
                
                var controllers = GetControllerMapping();

                string controllerName = null;       //Stringa con il controller selezionato

                //recupero la versione dall'uri/queryString e uso il custom selector che agisce sul nome del controller
                if (area == "uri")
                {
                    string apiVersion = GetVersionFromUri(routeData);
                    //Concateno al nome del controller la versione: all'URL api/v1/test attiverò il controller TestV1Controller
                    controllerName = string.Concat("name",routeData.Values["controller"].ToString(), "V", apiVersion.Substring(1));                   
                }

                if (area == "query")
                {
                    string apiVersion = GetVersionFromQueryString(request);
                    //Concateno al nome del controller la versione: all'URL api/v1/test attiverò il controller TestV1Controller
                    controllerName = string.Concat("name",routeData.Values["controller"].ToString(), "V", apiVersion);
                }

                //recupero la versione dall'uri e seleziono il controller a partire dall'attribute ApiVersionAttribute
                if (area == "custom")
                {
                    string apiVersion = GetVersionFromUri(routeData);

                }

                HttpControllerDescriptor controllerDescriptor;

                if (controllers.TryGetValue(controllerName, out controllerDescriptor))
                {
                    return controllerDescriptor;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var values = request.GetQueryNameValuePairs();

            if(!values.Any(x => x.Key == "api-version"))
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Versione API non specificata nella queryString. Chiave 'api-version' mancante");
                throw new HttpResponseException(response);
            }

            string apiVersion = values.First(x => x.Key == "api-version").Value;

            if (!Regex.IsMatch(apiVersion, "^\\d+"))
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent($"Versione API deve essere un numero intero. Valore trovato: '{apiVersion}'");
                throw new HttpResponseException(response);
            }
            return apiVersion;
        }

        private string GetVersionFromUri(IHttpRouteData routeData)
        {
            var apiVersion = routeData.Values["version"].ToString();
            if (string.IsNullOrEmpty(apiVersion))
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("Versione API non specificata nell'URL");
                throw new HttpResponseException(response);
            }
            if (!Regex.IsMatch(apiVersion, "^v\\d+"))
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent($"Versione API deve essere nel formato 'vXXX' dove XXX è un numero intero. Valore : '{apiVersion}'");
                throw new HttpResponseException(response);
            }

            return apiVersion;
        }

    }

}