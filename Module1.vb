Imports System.Net
Imports System.Globalization

Module Module1

    Sub Main()
        Dim prefixes(0) As String
        prefixes(0) = "http://*:8080/HttpListener/"
        ProcessRequests(prefixes)
    End Sub

    Private Sub ProcessRequests(ByVal prefixes() As String)
        If Not System.Net.HttpListener.IsSupported Then
            Console.WriteLine(
                "Windows XP SP2, Server 2003, or higher is required to " &
                "use GOOMBAServer")
            Exit Sub
        End If

        If prefixes Is Nothing OrElse prefixes.Length = 0 Then
            Throw New ArgumentException("prefixes")
        End If

        ' Create a listener and add the prefixes.
        Dim listener As System.Net.HttpListener =
            New System.Net.HttpListener()
        For Each s As String In prefixes
            listener.Prefixes.Add(s)
        Next

        Try
            ' Start the listener to begin listening for requests.
            listener.Start()
            Console.WriteLine("Listening...")

            ' Set the number of requests this application will handle.
            Dim numRequestsToBeHandled As Integer = 10

            For i As Integer = 0 To numRequestsToBeHandled
                Dim response As HttpListenerResponse = Nothing
                Try
                    ' Note: GetContext blocks while waiting for a request.
                    Dim context As HttpListenerContext = listener.GetContext()

                    ' Create the response.
                    response = context.Response
                    Dim responseString As String =
                        "<HTML><BODY>The time is currently " &
                        DateTime.Now.ToString(
                        DateTimeFormatInfo.CurrentInfo) &
                        "</BODY></HTML>"
                    Dim buffer() As Byte =
                        System.Text.Encoding.UTF8.GetBytes(responseString)
                    response.ContentLength64 = buffer.Length
                    Dim output As System.IO.Stream = response.OutputStream
                    output.Write(buffer, 0, buffer.Length)

                Catch ex As HttpListenerException
                    Console.WriteLine(ex.Message)
                Finally
                    If response IsNot Nothing Then
                        response.Close()
                    End If
                End Try
            Next
        Catch ex As HttpListenerException
            Console.WriteLine(ex.Message)
        Finally
            ' Stop listening for requests.
            Console.WriteLine("Done Listening...")
            Console.ReadKey()
        End Try
    End Sub
End Module