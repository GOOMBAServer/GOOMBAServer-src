//
//        GOOMBAServer
//
//  Created by: FreeBSoD on GitHub
//  Aka GOOMBAAAAAAAAAAAAAAAAAAAAA
//      AAAAAAAAAAAAA
//
//
//     Thanks for viewing!
//
//

using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Xml;

namespace GOOMBAServer
{
    public static class ExtensionClass
    {
        // Extension method to append the element
        public static T[] Append<T>(this T[] array, T item)
        {
            List<T> list = new List<T>(array);
            list.Add(item);

            return list.ToArray();
        }
    }
    class Module1
    {
        // Vars
        public static XmlDocument doc = new XmlDocument();

        public static string version = "0.1";
        public static HttpListener listener;
        public static string url = "";
        public static int requestCount = 0;
        public static string pageData =
            "<h1 style=\"font-family: verdana;\">GOOMBAServer Error</h1><h3 style=\"font-family: verdana;\">HTTP 500 Server Error <small>(GoombaErr #2)</small></h3><br /><br /><p style=\"font-family: verdana;\">GOOMBAServer v0.1</p>";
        public static MySqlConnection connection;
        public static string server;
        public static string database;
        public static string uid;
        public static string password;

        //Initialize values
        public static void InitializeSQL(string host, string db, string user, string pass)
        {
            server = host;
            database = db;
            uid = user;
            password = pass;
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("SQL #0 Says: Cannot connect to server.");
                        File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nSQL #0 Says: Cannot connect to server.");
                        pageData = "SQL #0 Says: Cannot connect to server.";
                        break;

                    case 1045:
                        Console.WriteLine("SQL #1045 Says: Invalid user name and/or password.");
                        File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nSQL #1045 Says: Invalid user name and/or password.");
                        pageData = "SQL #1045 Says: Invalid user name and/or password.";
                        break;
                }
                Console.WriteLine(ex.Message);
                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\n" + ex.Message);
                return false;
            }
        }

        //Close connection
        public static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Query
        public static string[] Query(string cmad)
        {
            Console.WriteLine("Query Started.");
            string query = cmad;

            if (query.ToUpper().StartsWith("SELECT"))
            {
                Console.WriteLine("ooOOH, a select query!");
                //Open connection
                if (OpenConnection() == true)
                {
                    Console.WriteLine("Open");
                    //create mysql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    var ret = cmd.ExecuteReader();
                    string[] vs = { };
                    var i = -1;
                    while (ret.Read())
                    {
                        i++;
                        vs.Append(ret[i].ToString());
                    }
                    Console.WriteLine("Done");
                    connection.Close();
                    return vs;
                }
            }
            else
            {
                //Open connection
                if (OpenConnection() == true)
                {
                    Console.WriteLine("Open");
                    //create mysql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Done");
                    string[] strtoret = { };
                    connection.Close();
                    return strtoret;

                }
                string[] strtorets = { };
                return strtorets;
            }
            string[] strtoretss = { };
            return strtoretss;
        }


        static void runGoombaCode(HttpListenerRequest req, string file = "")
        {
            List<string> strVars = new List<string>();
            List<string> strVars2 = new List<string>();
            var isIndex = false;
            if (req.Url.AbsolutePath == "" || req.Url.AbsolutePath == "/")
            {
                isIndex = true;
            }
            var i = 0;
            var goombaMode = false;
            pageData = "";

            StreamReader streamReader = new StreamReader(file);
            var toread = streamReader.ReadToEnd();
            streamReader.Close();
            if (isIndex == true)
            {

                streamReader = new StreamReader(Environment.CurrentDirectory + @"\www\" + "index.goomba");
                toread = streamReader.ReadToEnd();
                streamReader.Close();
            }
            using (var reader = new StringReader(toread))
            {
                for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                {
                    var commandline = line.Split(' ');
                    i += 1;
                    if (goombaMode)
                    {
                        if (line == "->STOPITGOOMBA<-")
                        {
                            if (goombaMode == true)
                            {
                                goombaMode = false;
                            }
                            else
                            {
                                pageData = "Goomba says: Unexpected STOPITGOOMBA at line " + i + ". Code will not continue.";
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Unexpected STOPITGOOMBA at line " + i + ". Code will not continue.");
                                return;
                            }
                        }
                        else if (line == "->GOOMBATIME<-")
                        {
                            if (goombaMode == false)
                            {
                                goombaMode = true;
                            }
                            else
                            {
                                pageData = "Goomba says: Unexpected GOOMBATIME at line " + i + ". Code will not continue.";
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "Goomba says: Unexpected GOOMBATIME at line " + i + ". Code will not continue.");
                                return;
                            }
                        }
                        // First command ever added to GOOMBA!
                        else if (commandline[0] == "SPELLWORD:")
                        {
                            pageData += commandline[1] + " ";
                        }
                        // Comments but think??? what???
                        else if (line.StartsWith("THINK"))
                        {

                        }
                        // Create variable
                        else if (commandline[0] == "CREATEHTMLVAR:")
                        {
                            if (commandline.Length != 3)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting 2 arguments after CREATEHTMLVAR at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting 2 arguments after CREATEHTMLVAR at line " + i + ". Code will not continue.";
                                return;
                            }
                            else
                            {
                                var rng = new Random().Next(65535);
                                if (rng == 69)
                                {
                                    rng = 69420;
                                }

                                string theName = Environment.CurrentDirectory + @"\www\" + "AksTEMPMethod" + rng + ".xml";
                                File.WriteAllText(theName, pageData);
                                try
                                {
                                    doc.Load(theName);
                                    XmlElement elem = doc.GetElementById(commandline[2]);
                                    if (elem != null)
                                    {
                                        strVars.Add(commandline[1]);
                                        strVars2.Add(elem.InnerText);
                                        File.Delete(theName);
                                    }
                                    else
                                    {
                                        File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting valid ID at line " + i + ". Code will not continue.");
                                        pageData = "Goomba says: Expecting valid ID at line " + i + ". Code will not continue.";
                                        return;
                                    }
                                }
                                catch
                                {
                                    File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Unexpected error at line " + i + ". Code will not continue.");
                                    pageData = "Goomba says: Unexpected error at line " + i + ". Code will not continue.";
                                    File.Delete(theName);
                                    return;
                                }
                            }
                        }
                        // Get variable and print it
                        else if (commandline[0] == "SPELLVAR:")
                        {
                            if (strVars.Contains(commandline[1]))
                            {
                                int ok = strVars.FindIndex(commandline[1].StartsWith);
                                pageData += strVars2[ok];
                            }
                            else
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Undefined variable " + commandline[1] + " at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Undefined variable " + commandline[1] + " at line " + i + ". Code will not continue.";
                                return;
                            }
                        }
                        // Write a file.
                        else if (commandline[0] == "WRITEPAPER:")
                        {
                            if (commandline.Length == 1)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting 2 arguments after WRITEPAPER at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting 2 arguments after WRITEPAPER at line " + i + ". Code will not continue.";
                                return;
                            }
                            if (commandline.Length == 2)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting 2 arguments after WRITEPAPER at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting 2 arguments after WRITEPAPER at line " + i + ". Code will not continue.";
                                return;
                            }
                            if (commandline.Length > 1000)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "Goomba says: WRITEPAPER does not accept " + commandline.Length + " arguments at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: WRITEPAPER does not accept " + commandline.Length + " arguments at line " + i + ". Code will not continue.";
                                return;
                            }
                            var txt = "";
                            for (int j = 2, loopTo = commandline.Length - 1; j <= loopTo; j++)
                                txt += commandline[j] + " ";
                            File.WriteAllText((Environment.CurrentDirectory + @"\www\" + commandline[1]).Replace("..", ""), txt.Replace(@"\n", "\n"));
                        }
                        // SPELLSENTENCE is the same as SPELLWORD, but it allows spaces.
                        else if (commandline[0] == "SPELLSENTENCE:")
                        {
                            if (commandline.Length == 1)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting 1 argument after SPELLSENTENCE at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting 1 argument after SPELLSENTENCE at line " + i + ". Code will not continue.";
                                return;
                            }
                            if (commandline.Length > 1000)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: SPELLSENTENCE does not accept " + commandline.Length + " arguments at line " + i + ".Code will not continue.");
                                pageData = "Goomba says: SPELLSENTENCE does not accept " + commandline.Length + " arguments at line " + i + ". Code will not continue.";
                                return;
                            }
                            var txt = "";
                            for (int j = 1, loopTo = commandline.Length - 1; j <= loopTo; j++)
                                txt += commandline[j] + " ";
                            pageData += txt.Replace(@"\n", "<br />");
                        }
                        else if (commandline[0] == "APPENDTOPAPER:")
                        {
                            if (commandline.Length == 1)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting 2 arguments after APPENDTOPAPER at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting 2 arguments after APPENDTOPAPER at line " + i + ". Code will not continue.";
                                return;
                            }
                            if (commandline.Length == 2)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting 2 arguments after APPENDTOPAPER at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting 2 arguments after APPENDTOPAPER at line " + i + ". Code will not continue.";
                                return;
                            }
                            if (commandline.Length > 1000)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: APPENDTOPAPER does not accept " + commandline.Length + " arguments at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: APPENDTOPAPER does not accept " + commandline.Length + " arguments at line " + i + ". Code will not continue.";
                                return;
                            }
                            var txt = "";
                            for (int j = 2, loopTo = commandline.Length - 1; j <= loopTo; j++)
                                txt += commandline[j] + " ";
                            File.AppendAllText((Environment.CurrentDirectory + @"\www\" + commandline[1]).Replace("..", ""), txt.Replace(@"\n", "\n"));
                        }
                        else if (commandline[0] == "READPAPER:")
                        {
                            if (commandline[1] == "USEADVANCEDTEXT:")
                            {
                                StreamReader streamReader2 = new StreamReader(Environment.CurrentDirectory + @"\www\" + commandline[2]);
                                var ok = streamReader2.ReadToEnd();
                                pageData += ok.Replace(@"\n", "<br />");
                                return;
                            }
                            else if (commandline.Length != 2)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting 1 argument after READPAPER, not " + commandline.Length + " at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting 1 argument after READPAPER, not " + commandline.Length + " at line " + i + ". Code will not continue.";
                                return;
                            }
                            StreamReader streamReader1 = new StreamReader(Environment.CurrentDirectory + @"\www\" + commandline[1]);
                            var txt = streamReader1.ReadToEnd();
                            pageData += txt;
                        }
                        else if (commandline[0] == "CONNECT:")
                        {
                            if (commandline.Length != 5)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting 4 arguments after CONNECT, not " + commandline.Length + " at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting 4 arguments after CONNECT, not " + commandline.Length + " at line " + i + ". Code will not continue.";
                            }
                            InitializeSQL(commandline[1], commandline[2], commandline[3], commandline[4]);
                        }
                        else if (commandline[0] == "REMOVE:")
                        {
                            if (commandline.Length != 2)
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: REMOVE accepts 1 argument at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: REMOVE accepts 1 argument at line " + i + ". Code will not continue.";
                            }
                            pageData = pageData.Replace(commandline[1], "");
                        }
                        // Query a MySQL query.
                        else if (commandline[0] == "QUERY:")
                        {
                            if (line.Substring(7, 1) == "\"")
                            {
                                if (line.LastIndexOf('"') > 7)
                                {
                                    Query(line.Substring(line.IndexOf('"') + 1, line.LastIndexOf('"') - 1 - line.IndexOf('"')));
                                }
                                else
                                {
                                    File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting \" after QUERY, at line " + i + ". Code will not continue.");
                                    pageData = "Goomba says: Expecting \" after QUERY, at line " + i + ". Code will not continue.";
                                }
                            }
                            else
                            {
                                File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nGoomba says: Expecting \" right after QUERY, at line " + i + ". Code will not continue.");
                                pageData = "Goomba says: Expecting \" right after QUERY, at line " + i + ". Code will not continue.";
                            }
                        }
                        else
                        {
                            File.AppendAllText(Environment.CurrentDirectory + @"\www\goomba_errors", "\nUndefined GOOMBA_THING " + commandline[0] + " at line " + i + ". Code will not continue.");
                            pageData = "Undefined GOOMBA_THING " + commandline[0] + " at line " + i + ". Code will not continue.";
                            return;
                        }
                    }
                    else
                    {
                        if (line == "->GOOMBATIME<-")
                        {
                            // Is GOOMBA running?
                            if (goombaMode == false)
                            {
                                // No!
                                goombaMode = true;
                            }
                            else
                            {
                                // yep...
                                pageData = "Goomba says: Unexpected GOOMBATIME at line " + i + ". Code will not continue.";

                                return;
                            }
                        }
                        else if (line == "->STOPITGOOMBA<-")
                        {
                            if (goombaMode == true)
                            {
                                goombaMode = false;
                            }
                            else
                            {
                                pageData = "Goomba says: Unexpected STOPITGOOMBA at line " + i + ". Code will not continue.";

                                return;
                            }
                        }
                        else
                        {
                            pageData += "\n" + line;
                            goombaMode = false;
                        }
                    }

                }
                goombaMode = false;
            }
        }
        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // While a user hasn't visited the `shutdown` url, keep on handling requests
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;

                // Print out some info about the request
                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                Console.WriteLine();


                // Find all files to check
                List<string> allFiles = GetFilesFromPath(Environment.CurrentDirectory + @"\www");
                foreach (string file in allFiles)
                {
                    string thething = "/" + file.Replace(Environment.CurrentDirectory + @"\www\", "").Replace("\\", "/");
                    if (req.Url.AbsolutePath == thething || req.Url.AbsolutePath == "" || req.Url.AbsolutePath == "/")
                    {
                        if (req.Url.AbsolutePath.EndsWith(".goomba"))
                        {
                            runGoombaCode(req, file);
                        }
                        else
                        {
                            var isIndex = false;
                            if (req.Url.AbsolutePath == "" || req.Url.AbsolutePath == "/")
                            {
                                isIndex = true;
                            }
                            if (isIndex)
                            {
                                runGoombaCode(req, file);
                            }
                            else
                            {
                                StreamReader sr = new StreamReader(file);
                                pageData = sr.ReadToEnd();
                            }
                        }
                        goto foundfile;
                    }
                    else
                    {
                        pageData = "<h1 style=\"font-family: verdana;\">GOOMBAServer Error</h1><h3 style=\"font-family: verdana;\">HTTP 404 Not Found <small>(GoombaErr #1)</small></h3><br /><br /><p style=\"font-family: verdana;\">GOOMBAServer v0.1</p>";
                    }
                }
            // File is found
            foundfile:
                // Write the response info
                byte[] data = Encoding.UTF8.GetBytes(pageData);
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                // Write out to the response stream (asynchronously), then close it
                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        static List<string> GetFilesFromPath(string path)
        {
            List<string> files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(path));

                foreach (string dir in Directory.GetDirectories(path))
                {
                    files.AddRange(GetFilesFromPath(dir));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return files;
        }

        public static void Main(string[] args)
        {
            url = File.ReadAllText(Environment.CurrentDirectory + "\\GOOMBA.cfg");
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);
            Console.WriteLine("Make sure you have index.goomba in your /www folder.");
            // Handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().GetResult();
            // Close the listener
            listener.Close();
            Console.ReadKey();
        }
    }
}