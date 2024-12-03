"Please refer to the document *Readme.pdf* available on our website, [www.cybelin.com](http://www.cybelin.com). For any issues or to discuss potential collaborations, feel free to contact me at **alvaro@cybelin.com**."

**Introduction.**

The Cybelin integrated solution includes a minimalistic ASP.NET Core middleware application designed to capture and log details of HTTP requests and responses and to block the HTTP request received from malicious IPs. 

The middleware captures information such as the HTTP method, headers, query string, request body, HTTP version, and client IP, as well as the response headers, status code, response size, and response time. All this information is stored in a SQL Server database using Entity Framework Core.

The integrated solution includes also monitoring software that can be configured to blacklist IPs that the middleware will block. The monitor software can monitor different servers and includes functionality to replicate the blacklisted IPs in all the servers.

When the monitor software is used to supervise a server, it can trigger 12 types of alerts:

1. **Outbound Traffic per Client IP in     Bytes per Minute**
        Monitors the volume of data being sent out by each client IP every minute.     A sudden increase may indicate data exfiltration or potential data leaks.
2. **Number of Requests per Client IP     per Minute**
        Tracks the number of requests made by each client IP per minute. A high     request rate could signify suspicious activity, such as attempts to     overwhelm the server or unauthorized scraping.
3. **Number of 4XX Responses per Client     IP per Minute**
        Alerts when a client IP receives multiple 4XX (client-side error)     responses within a minute. This pattern might indicate automated tools     trying to access restricted resources or a fuzzing attack.
4. **Number of Requests Without Response     per Client IP per Minute**
        Detects when requests from a specific client IP do not receive a response     within a minute. This could indicate network issues or a client trying to     overload the system with requests.
5. **Outbound Traffic per Endpoint in     Bytes per Minute**
        Measures the data volume being sent out per endpoint every minute. High     traffic to certain endpoints may signal data extraction attempts or     unintended data exposure.
6. **Number of Requests per Endpoint per     Minute**
        Monitors the number of requests to each endpoint per minute. A sudden     spike could indicate an attempted attack on a particular API function.
7. **Number of 4XX Responses per     Endpoint per Minute**
        Tracks the number of 4XX responses for each endpoint every minute. Many     errors on an endpoint might indicate automated tools trying to access     restricted resources or a fuzzing attack.
8. **Number of Requests Without Response     per Endpoint per Minute**
        Detects endpoints that frequently fail to respond to requests. This might     indicate performance issues or that an endpoint is being targeted in an     attack.
9. **Outbound Traffic per Server in     Bytes per Minute**
        Measures data volume being sent out by the server every minute. Sudden     spikes can reveal unauthorized data access or other anomalies.
10. **Number of Requests per Server per     Minute**
         Tracks the total request count per server each minute, identifying     unusually high traffic that could point to a DDoS attack or heavy load.
11. **Number of 4XX Responses per Server     per Minute**
         Alerts on multiple 4XX errors server-wide per minute, helping to identify     issues affecting multiple clients or endpoints.
12. **Number of Requests Without Response     per Server per Minute**
         Detects when the server fails to respond to multiple requests within a     minute, indicating potential overload, system faults, or targeted     disruption attempts.

 

The Cybelin middleware is easy to integrate with your APIs. Currently, it supports APIs developed with ASP.NET Core, and we are developing middleware for APIs written in Python, Java, JavaScript, and Go. For more information on new middleware please visit our website [www.cybelin.com](http://www.cybelin.com).

 

**NuGet Packages**

Several NuGet packages are used in the middleware to provide essential functionality, such as database management, API documentation, and design-time services.

Microsoft.EntityFrameworkCore.SqlServer: This package enables the application to work with SQL Server databases. It allows the application to store and retrieve data from a SQL Server database using Entity Framework Core.

Microsoft.EntityFrameworkCore.Tools: This package provides tools for working with Entity Framework Core migrations, allowing the developer to update the database schema as the application evolves.

 Microsoft.EntityFrameworkCore.Design: This package provides design-time services required for scaffolding and managing migrations in Entity Framework Core.

 Swashbuckle.AspNetCore:Swashbuckle integrates Swagger with ASP.NET Core, enabling automatic generation of API documentation. It provides an interactive UI (Swagger UI) for exploring and testing API endpoints.

 

**Installation.**

The Cybelin Monitor software works in conjunction with the logging and malicious IP blocking middleware.

To run the software, you need to download the source code for both projects:

- **Data     Loss Prevention Middleware Project:** This contains the source code for logging     and malicious IP blocking middleware. It’s an ASP.NET Core application     that includes middleware and some test endpoints you can run with Swagger     to check how the software operates.
- **Cybelin     Monitor Project:** This is the monitoring software used to detect alerts     through the middleware.

To get started, you must create two databases used by the software:

1. **Create     the Cybelin database:** This is the main database used by the monitoring     software. To set it up, create a new database in SQL Server named Cybelin     and then run the SQL script “Create Cybelin Database.sql.”
2. **Create     the CybelinServer database:** This database is used by the example API     and middleware to monitor logs and block blacklisted IPs. In SQL Server,     create a database named CybelinServer and then run the SQL script “Create     CybelinServer Database.sql.”. 

Alternatively you can create the tables in the CybelinServer database by opening the middleware solution in Visual Studio 2022, changing the connection string in the file appsettings.json and executing the command “Update-database” in the console of Visual Studio. But if you this you must add manually a record in the Configurations table with the following values: Key = “MaliciousIpCheckIntervalInSeconds” and Value=60.

Next, open the Cybelin Monitor project in Visual Studio 2022 and update the connection string in the monitoring software within the DataContext class in the OnConfiguring event. Ensure the connection string points to the Cybelin database correctly.

To verify that the Cybelin database is set up correctly and the connection string is valid, go to the “Manage Monitor” menu and select “Manage Monitor Configurations.” If the database and connection string are correct, the “Manage Monitor Configurations” window should display several records allowing you to configure an email account to send alert notifications.

Run the monitoring software, then go to the “Manage Servers” menu and select “Manage Servers.” Add a server with the name “CybelinServer” and the correct connection string for accessing the CybelinServer database. **IMPORTANT:** The connection string should be entered without starting and ending quotation marks.

To verify that the CybelinServer database is set up correctly and the connection string is valid, go to the “Manage Servers” menu and select “Manage Servers Configurations.” A window named Server Configurations will open with a combobox. Select the server “CybelinServer” from the combobox, and you should see a configuration record in the grid named “MaliciousIpCheckIntervalInSeconds” with a value of 55. If this record appears correctly, it indicates that the CybelinServer database and connection string are set up successfully.

Once both databases are correctly created and the Cybelin Monitor software is running correctly, open the Data Loss Prevention Middleware project in Visual Studio 2022, and update the connection string in the appsettings.json file with the correct connection details for the CybelinServer database.

 
