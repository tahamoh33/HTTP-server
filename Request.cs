using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPServer
{
    public enum RequestMethod
    {
        GET,
        POST,
        HEAD
    }

    public enum HTTPVersion
    {
        HTTP10,
        HTTP11,
        HTTP09
    }

    class Request
    {

        RequestMethod method;
        public string relativeURI;
        Dictionary<string, string> headerLines;

        public Dictionary<string, string> HeaderLines
        {
            get { return headerLines; }
        }

        HTTPVersion httpVersion;
        string requestString;
        string[] contentLines;

        public Request(string requestString)
        {
            this.requestString = requestString;
        }
        /// <summary>
        /// Parses the request string and loads the request line, header lines and content, returns false if there is a parsing error
        /// </summary>
        /// <returns>True if parsing succeeds, false otherwise.</returns>
        public bool ParseRequest()
        {
            //throw new NotImplementedException();
            try
            {
                //TODO: parse the receivedRequest using the \r\n delimeter   
                string Parse = "\r\n";
                string[] HttpRequest = requestString.Split(new[] { Parse }, StringSplitOptions.None);
                Parse = "\r\n\r\n";
                string[] Request_Header = requestString.Split(new[] { Parse }, StringSplitOptions.None);
                Parse = "\r\n";
                //string[] RequestHeader = requestString.Split(new[] { Parse }, StringSplitOptions.None);
                // check that there is atleast 3 lines: Request line, Host Header, Blank line (usually 4 lines with the last empty line for empty content)
                if (HttpRequest.Length == 3 || HttpRequest.Length == 4)
                    return true;
                else
                    return false;
                // Parse Request line
                Parse = " ";
                string[] requestLines = HttpRequest[0].Split(new[] { Parse }, StringSplitOptions.None);
                // Validate blank line exists
                for (int i = 2; i < HttpRequest.Length; i++)
                {
                    if (string.IsNullOrEmpty(HttpRequest[i]))
                        break;
                }
                // Load header lines into HeaderLines dictionary
                Parse = ":";
                string[] Header = HttpRequest[1].Split(new[] { Parse }, StringSplitOptions.None);
                for (int x = 0; x < Header.Length; x += 2)
                {
                    headerLines.Add(Header[x], Header[x + 1]);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        private bool ParseRequestLine()
        {
            throw new NotImplementedException();
        }

        private bool ValidateIsURI(string uri)
        {
            return Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);
        }

        private bool LoadHeaderLines()
        {
            throw new NotImplementedException();
        }

        private bool ValidateBlankLine()
        {
            throw new NotImplementedException();
        }

    }
}