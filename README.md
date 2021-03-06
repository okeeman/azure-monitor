# azure-monitor
Automatically exported from code.google.com/p/azure-monitor

Higher Diploma in Computing project
Windows Azure Cloud Service monitor
Third iteration

This is a health monitor for a Windows Azure Cloud Service. It provides the user with a browser-based GUI which they can use to see the health status of their cloud service. It also provides round-the-clock monitoring that sends SMS texts to the user's phone if their cloud service encounters an issue. All of the components of the monitor are hosted on Windows Azure, i.e. web role, worker role and database.

The monitor has a 3-tier architecture:
(i) Presentation layer
(ii) Application layer
(iii) Database layer

There are three major components of the monitor:
(i) Web role
(ii) Worker role
(iii) SQL Azure database

Web role

    The web role functions by polling the database for the latest results for cloud services belonging to the logged in user. The results are displayed in the browser. A green heart displays if all of the cloud service's instances are healthy and a red heart displays if any of the instances are unhealthy. Ajax and jQuery are used to provide an auto-refresh feature. The results are updated every 30 seconds.
    CRUD functionality has been enabled in the web role. When the user enters or edits their Windows Azure information a validation check is performed by doing a test request to the Windows Azure Service Management API.
    Dependency Injection has been configured so that the database may be isolated for unit tests.
    HTTPS has been configured for logged in users. The health check and CRUD functionality are available only to logged-in users.
    The CRUD includes a validation check which ensures that the details entered by the user are valid before adding them to the database. 

Worker role

    The worker role sends requests to Azure's Service Management API every 30 seconds. It examines the results obtained and if any of the cloud service's instances are unhealthy it sends an SMS text to the user's phone.
    The worker role gets the user's subscription, cloud service details and management certificate details from the database. It uses this information to form the request to Azure's Service Management API. The SMS texts are sent using an API provided by Clickatell.
    The code was refactored into smaller chunks such that it would be easier to perform unit tests. The Windows Azure Service Management API returns an XML file. The logic for parsing this file was refactored into a discrete method which could be tested using fakes.
    Dependency Injection has been configured so that the database may be isolated for unit tests.
    The worker role writes the health status results to a database in Windows Azure. This allows for a historical record to be kept. The results could be queried to examine the health status performance over time. 

User acceptance testing (UAT)

    UAT testing was performed by two .NET developers. Both were able to set up an Azure Doctor account and have it monitor their cloud services.
    The SMS feature was successfully tested.
    Both testers recommended that a tutorial be included in the site to instruct users as to how to add a management certificate to Windows Azure and where to obtain the subscription and cloud service details required. A tutorial was developed and included as a result of this feedback.
    It was also suggested that the Overview and Check Health links should only be displayed when the user is logged in. This was also addressed.
    Testing uncovered some failure scenarios which I had not anticipated, e.g., if a user tries to make a health request before setting up their details in Azure Doctor an exception would be thrown. A check was put into the application to prevent users from making a request before setting up their Azure Doctor account correctly. 

Web role dynamic results display

    The web role display can handle a variable number of cloud services and a variable number of instances per cloud service. 

Unit tests and code coverage.

    Unit tests were carried out in the Web role using Dependency Injection to isolate the real database.
    In the Worker role refactoring of the code allowed fake results to be used to test the logic which parses the results returned from the Windows Azure Service Management API.
    The code coverage percentages are good for code I wrote. I did not test code supplied in the MVC template by Microsoft. 

Load testing

    A load test was carried out using the functionality built into Visual Studio 2012 Ultimate. 

Issues overcome for the third iteration

    The greatest challenge overcome was gaining an understanding of Dependency Injection (DI). The books "The Art of Unit Testing" and "Dependency Injection in .NET" were of great benefit as were the instructional videos on Pluralsight. Constructor injection was used. The default constructor will connect to the real database whilst the second constructor is provided with a fake database for use during unit testing.
    The worker role had been writing the results for only one cloud service. It was determined that the reason for this was that it was reusing the same entity and that it needed a new one created each time through the foreach loop.
    Setting up CRUD functionality and validation of input proved to be more complicated than I had anticipated. On a couple of pages I am displaying data from more than one database table. A view can be based on only one model so I created ViewModels composed of data from more than one table.
    Status Active/Inactive, database redesign. While creating the CRUD functionality the delete part posed a dilemma. I decided that it would be better to hold onto the historical data and allow it to be queried using ad hoc SQL scripts. This was influenced by what I saw in my workplace in a commercial application database written by a major software vendor. It also makes use of an Active/Inactive field and ad hoc SQL scripts are commonly developed to query the data.
    IE10 update: In the last week of the project Microsoft issued an update for Internet Explorer which had a disastrous effect on my home page. It worked fine in every other major browser and in Compatibility Mode in IE10. I ended up having to remove a custom script I had written which created a scrolling text banner on the page. I also completely rewrote the way that the affected divs displayed in order to get around the issue. 
