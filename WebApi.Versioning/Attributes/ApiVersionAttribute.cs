using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Versioning.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    sealed class ApiVersionAttribute : Attribute
    {
        readonly string _version;
        
        public ApiVersionAttribute(string version)
        {
            _version = version;
        }

        public string Version => _version;
    }
}