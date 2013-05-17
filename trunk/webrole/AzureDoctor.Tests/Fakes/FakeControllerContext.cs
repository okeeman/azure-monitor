// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// A fake controller context is created to allow the mocking of the Ajax request in
// the unit tests.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added to bring HttpContextBase and HttpRequestBase into scope.
using System.Web;
// Added to bring ControllerContext into scope.
using System.Web.Mvc;
// Added to bring NameValueCollection into scope.
using System.Collections.Specialized;

namespace AzureDoctor.Tests
{
    class FakeControllerContext : ControllerContext
    {
        HttpContextBase _context = new FakeHttpContext();

        public override HttpContextBase HttpContext
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }

        class FakeHttpContext : HttpContextBase
        {
            HttpRequestBase _request = new FakeHttpRequest();

            public override HttpRequestBase Request
            {
                get
                {
                    return _request;
                }
            }
        }

        class FakeHttpRequest : HttpRequestBase
        {
            public override string this[string key]
            {
                get
                {
                    return null;
                }
            }

            public override NameValueCollection Headers
            {
                get
                {
                    return new NameValueCollection();
                }
            }
        }
    }
}
