using ClanServer;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace WebServerCore
{
    class Program
    {        
        static void Main(string[] args)
        {
            var client = new TcpClient();
            
            Listener(client);

            try {
                Task.Run(() => DataHandler.SendData(client));
            } catch (Exception e) {
                Console.WriteLine("Error: " + e);
            }





        }
        public static void Listener(TcpClient client)
        {
            var listener = new TcpListener(new IPEndPoint(IPAddress.IPv6Any, 80));
            listener.Server.DualMode = true;

            listener.Start();

            Console.WriteLine("Waiting for a connection...");
            client = listener.AcceptTcpClient();
        }
        static void RequestHandler()
        {
            StreamReader sr = new StreamReader(client.GetStream());
            StreamWriter sw = new StreamWriter(client.GetStream());
            string request = sr.ReadLine();
            Console.WriteLine(request);
            string[] tokens = request.Split(' ');
            string methode = tokens[0];
            string page = tokens[1];

            switch (methode) {
                case "GET":
                    if (page == "/") {
                        page = "/Clan.html";
                    } else if (page == "/phpmyadmin") {
                        page = "../phpmyadmin";
                    }
                    StreamReader file = new StreamReader("../../web/basedigits/" + page);
                    sw.WriteLine("HTTP/1.0 200 OK\n");

                    string data = file.ReadLine();
                    while (data != null) {
                        sw.WriteLine(data);
                        sw.Flush();
                        data = file.ReadLine();
                    }
                    break;

                //case "POST":
                //    if (page == "/") {
                //        page = "/Clan.html";
                //    }
                //    StreamReader php = new StreamReader("../../web/basedigits/" + page);
                //    sw.WriteLine("HTTP/1.0 200 OK\n");

                //    string data1 = php.ReadLine();
                //    while (data1 != null) {
                //        sw.WriteLine(data1);
                //        sw.Flush();
                //        data1 = php.ReadLine();
                //    }
                //    break;

                    //DateTime localDate = DateTime.Now;
                    //string time = localDate.ToString();
                    //time = time.Replace('.', '-');
                    //time = time.Replace(' ', '_');
                    //time = time.Replace(':', '-');
                    //StreamWriter clientStream = new StreamWriter(@"../../Logs/Bewerbung " + time + ".txt");

                    //string test = "";
                    //while (test != sr.ReadLine()) {
                    //    clientStream.WriteLine(test);
                    //    clientStream.Flush();
                    //    test = sr.ReadLine();
                    //}

                    //for (int i = 0; i < 60; i++) {
                    //    if (test != "") {
                    //        clientStream.WriteLine(test);
                    //        clientStream.Flush();
                    //        test = sr.ReadLine();
                    //    }
                    //}
                    //foreach (var item in sr.ReadToEnd()) {
                    //    clientStream.Write(item);
                    //    clientStream.Flush();
                    //}
                    //sr.Close();
                    //sw.WriteLine("HTTP/1.0 200 OK\n");
                    //while (!sr.EndOfStream) {
                    //    string formData = sr.ReadLine();
                    //    clientStream.WriteLine(formData);
                    //}
                    ////string formData = sr.ReadToEnd();
                    ////clientStream.WriteLine(formData);
                    //clientStream.Close();

                    //break;
            }
            client.Close();
        }
    }
}
