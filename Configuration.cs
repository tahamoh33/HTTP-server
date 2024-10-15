using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    static class Configuration
    {
        public static string ServerHTTPVersion = "HTTP/1.1";
        public static string ServerType = "FCISServer";
        public static Dictionary<string, string> File =  System.IO.File.ReadAllLines(@"C:\Users\Yasmeen Abdelaziz\Downloads\Template[2021-2022]v4 (1)\redirectionRules (1).txt")
                                       .Select(x => x.Split(','))
                                       .ToDictionary(x => x[0], x => x[1]);
        public static Dictionary<string, string> RedirectionRules ;
        static char[] html_remover = {'h', 't', 'm', 'l',',' };
        static char[] coma_remover = {','};
        static string New_Key;
        static string New_Value;
        public static void ReFormate_File() { 
        foreach (KeyValuePair<string, string> kvp in File)
        {
                New_Key = kvp.Key.TrimEnd(coma_remover);
                New_Value = kvp.Value.TrimStart(html_remover);
                RedirectionRules.Add(New_Key, New_Value);
        }
        }
        public static string RootPath = "C:\\inetpub\\wwwroot\\fcis1";
        public static string MainDefaultPageName = "main.html";
        public static string RedirectionDefaultPageName = "Redirect.html";
        public static string BadRequestDefaultPageName = "BadRequest.html";
        public static string NotFoundDefaultPageName = "NotFound.html";
        public static string InternalErrorDefaultPageName = "InternalError.html";

    }
}
