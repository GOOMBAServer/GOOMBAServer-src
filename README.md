# GOOMBAServer
GOOMBAServer - A Server-Side programming language for websites!

GOOMBAServer is based on PHP, NodeJS, and ASP.NET (Easier explanation: server side languages for websites).
Using GOOMBAServer is free for everyone, But you should have some computer skills.
If you are using an unchanged GOOMBA.cfg file, just run GOOMBAServer.exe and go to http://localhost:8000/

It has been tested on:
- Windows 10 64 bit                  [SUCCESS]
- Ubuntu 20.04 VPS server            [SUCCESS]
- Lubuntu 20.04.3                    [SUCCESS]

# Sorry for no updates, Development will now be continued. (2/4/2022)

# Compiling GOOMBAServer
Compiling GOOMBAServer is easy!

You need the MySQL.Data NuGet package installed first.
Go to Tools > NuGet Package Manager > Manage NuGet packages for solution.
Then close Visual Studio, and open `Compile.vbs`

If you get an error when compiling, you can create an issue.

# Sample Codes
Database Sample:
>THINK Connect to the DB (Cannot contain spaces, currently)  
CONNECT: host db user pass  
THINK Insert something.  
QUERY: "INSERT INTO Mytable VALUES ('Test 12312');"  
THINK Select query.  
QUERY: "SELECT * FROM Mytable;"  
THINK Create an array of the query.  
ARRAYADD: MyCoolArray  
THINK Show the value from MyCoolArray at index 0 on the screen  
SPELLARRAY: MyCoolArray 0 


Print text Sample  

>THINK Print something!  
SPELLSENTENCE: Hey, earth!  

# Can I use this source?
Yes, you are allowed to use this source if you follow the license.

# Q&A
Q: Is there an official website?
A: Yes, https://www.goombaserver.org/

Q: Are there tutorials for GOOMBAServer?
A: On the website, yes. and in the readme, aka what you are reading right now. 

Q: How can I support the development?
A: Since I don't force people to donate, you can support it by finding bugs, and contributing!
