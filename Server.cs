using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace HTTPServer
{
    class Server
    {
        Socket serverSocket;
        public Server(int portNumber, string redirectionMatrixPath)
        {
          
            //TODO: call this.LoadRedirectionRules passing redirectionMatrixPath to it
            //this.LoadRedirectionRules = redirectionMatrixPath;
            this.LoadRedirectionRules( redirectionMatrixPath); 
            //TODO: initialize this.serverSocket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            serverSocket.Bind(hostEndPoint);
            

        }

        public void StartServer()
        {
            Console.WriteLine("....");
            // TODO: Listen to connections, with large backlog.
            serverSocket.Listen(100);
           
            // TODO: Accept connections in while loop and start a thread for each connection on function "Handle Connection"
            while (true)
            {
                //TODO: accept connections and start thread for each accepted connection.
                Socket clientSocket = this.serverSocket.Accept();
                Console.WriteLine("New client accepted: {0}", clientSocket.RemoteEndPoint);
                Thread newthread = new Thread(new ParameterizedThreadStart(HandleConnection));
                //Start the thread
                newthread.Start(clientSocket);
            }
        }

        public void HandleConnection(object obj)
        {
            // TODO: Create client socket 
            Socket clientSock = (Socket)obj;
            // set client socket ReceiveTimeout = 0 to indicate an infinite time-out period
            clientSock.ReceiveTimeout=0;
            // TODO: receive requests in while true until remote client closes the socket.
            while (true)
            {

                byte[] data;
                int receivedLength;
                try
                {
                    // TODO: Receive request
                    data = new byte[1024 * 1024];
                    receivedLength = clientSock.Receive(data);
                    // TODO: break the while loop if receivedLen==0
                    if (receivedLength == 0)
                    {
                        Console.WriteLine("Client: {0} ended the connection", clientSock.RemoteEndPoint);
                        break;
                    }
                    // TODO: Create a Request object using received request string
                    Request request = new Request(Encoding.ASCII.GetString(data, 0, receivedLength));
                    // TODO: Call HandleRequest Method that returns the response

                    Response respone = HandleRequest(request);
                    // TODO: Send Response back to client
                    clientSock.Send(data, 0, receivedLength, SocketFlags.None);

                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }

            // TODO: close client socket
            clientSock.Close();
        }

        Response HandleRequest(Request request)
        {
            Response response;
            
            //throw new NotImplementedException();
            try
            {

                string physical_path = request.HeaderLines["Host"];
                /*char[] com_remover = { '.', 'c', 'o', 'm'};
                char[] www_remover = { 'w', 'w', 'w', '.' };
                string Newpath = physical_path.TrimEnd(com_remover);
                Newpath = Newpath.TrimStart(www_remover);
                */
                //contactus
                //TODO: check for bad request 
                if(! request.ParseRequest())
                {
                    response = new Response(StatusCode.BadRequest, "text/html", Configuration.BadRequestDefaultPageName , null);
                    return response;  
                }
                //TODO: map the relativeURI in request to get the physical path of the resource.
               
                //TODO: check for redirect
                else if(Configuration.RedirectionRules.ContainsKey(physical_path))
                {
                 response = new Response(StatusCode.Redirect, "text/html",Configuration.RedirectionDefaultPageName, "www."+Configuration.RedirectionRules[physical_path]+".com" );
                    return response;    
                }
                else
                {
                    string PhysicalPath = Configuration.RootPath + "/" + Configuration.RedirectionRules[physical_path];
                    if (File.Exists(PhysicalPath))
                    {
                        return new Response(StatusCode.Redirect, "text/html", Configuration.MainDefaultPageName, Configuration.RedirectionRules[physical_path]);
                    }
                    else
                    {
                        
                        return new Response(StatusCode.NotFound, "text/html", Configuration.NotFoundDefaultPageName, null);
                    }
                }

                /*response = new Response(StatusCode.OK, "text/html",Configuration.MainDefaultPageName, null  );
                return response;   
            }
            else {
                response = new Response(StatusCode.NotFound, "text/html",Configuration.NotFoundDefaultPageName,null );
                return response;   
            }*/
                //TODO: check file exists

                //TODO: read the physical file

                // Create OK response
            }
            catch (Exception ex)
            {
                
                // TODO: log exception using Logger class
                Logger.LogException(ex);
                // TODO: in case of exception, return Internal Server Error. 
               string path = Configuration.RootPath + '\\' + Configuration.InternalErrorDefaultPageName;
                return new Response(StatusCode.InternalServerError , "text/html" ,Configuration.InternalErrorDefaultPageName, path );
            }
        }

        private string GetRedirectionPagePathIFExist(string relativePath)
        {
            // using Configuration.RedirectionRules return the redirected page path if exists else returns empty
            
            return string.Empty;
        }

        private string LoadDefaultPage(string defaultPageName)
        {
            string filePath = Path.Combine(Configuration.RootPath, defaultPageName);
            // TODO: check if filepath not exist log exception using Logger class and return empty string
            
            // else read file and return its content
            return string.Empty;
        }

        private void LoadRedirectionRules(string filePath)
        {
            try
            {
                // TODO: using the filepath paramter read the redirection rules from file 
                // then fill Configuration.RedirectionRules dictionary 
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                StreamReader streamReader = new StreamReader(fileStream);
                while (streamReader.Peek()!=-1)
                {
                    string line = streamReader.ReadLine();
                    string[] data = line.Split(',');
                    if (data[0] == "") break;
                    Configuration.RedirectionRules.Add(data[0],data[1]);
                }
                fileStream.Close();


            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                Environment.Exit(1);
            }
        }
    }
}