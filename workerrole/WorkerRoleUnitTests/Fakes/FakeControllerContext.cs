// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This fake controller context can be used to mock HTTP requests when performing 
// unit tests.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added to bring HttpContextBase and HttpRequestBase into scope.
using System.Web;
// Added to bring NameValueCollection into scope.
using System.Collections.Specialized;

namespace WorkerRoleUnitTests.Fakes
{
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
