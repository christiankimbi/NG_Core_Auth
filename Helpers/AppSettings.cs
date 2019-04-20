using System;
namespace NG_Core_Auth.Helpers
{
    public class AppSettings
    {

        //Properties for the JWT Signature
        public string Site { get; set; }
        public string Audience { get; set; }
        public string ExpireTime { get; set; }
        public string Secret { get; set; }
    }
}
