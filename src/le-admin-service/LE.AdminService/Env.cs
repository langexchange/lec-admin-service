using System;

namespace LE.AdminService
{
    public static class Env
    {
        public readonly static string DB_CONNECTION_STRING = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        public readonly static string SECRET_KEY = Environment.GetEnvironmentVariable("SECRET_KEY") ?? string.Empty;

        //
        public const string SEND_REQUEST = "send-request";
    }
}
