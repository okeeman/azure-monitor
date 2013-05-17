// ----------------------------------------------------------------------------------
// Owen Sweeney 2013
// ----------------------------------------------------------------------------------
// This class is used to make the SMS push notifications using the details provided 
// by the user. The API used is Clickatell.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added to bring HttpWebRequest into scope.
using System.Net;

namespace WorkerRole1.Classes
{
    public class PushNotifications
    {
        public void PushSMS(string userPhoneNumber, string message, string username, string password)
        {
            Uri getDetailsRequest = new Uri("http://api.clickatell.com/http/sendmsg?user=" + username + "&password=" + password + "&api_id=3411315&to=" + userPhoneNumber + "&text=" + message);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getDetailsRequest);
            request.Method = "POST";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        }
    }
}
