using Dblleaf.UWP.Huaban.Helpers;
using Dblleaf.UWP.Huaban.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Dblleaf.UWP.Huaban.Services
{
    public abstract class ServiceBase
    {
        protected HbHttpHelper HttpHelper { get; set; }
        public ServiceBase(HbHttpHelper httpHelper)
        {
            HttpHelper = httpHelper;
        }
    }
}
