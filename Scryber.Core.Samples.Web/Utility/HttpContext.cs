using System;
using Microsoft.AspNetCore.Http;

namespace Scryber.Online.Utility
{
    public static class HttpContextAccess
    {

        private static IHttpContextAccessor _access;

        


        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                if (null == _access)
                {
                    _access = ServiceProvider.GetService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
                    if (null == _access)
                        throw new InvalidOperationException("The service provider does not have the HttpContextAccessor set");
                }
                return _access.HttpContext;
            }
        }
    }
}
