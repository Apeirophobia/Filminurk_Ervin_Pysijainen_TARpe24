using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Data
{
    public class EnvironmentExample // rename class to just "Environment"
    {
        public static string smtpaddress = ""; // server address for sending emails via smtp
        public static string gmailapppassword = ""; // password for user, that has access to this server (google app password)
        public static string gmailusername = ""; // username of email sending user (gmail account)
        public static string accuweatherkey = ""; // api key to use accuweather services
    }
}
