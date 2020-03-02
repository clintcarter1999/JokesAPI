
# Jokes ASP.NET REST API Project

I created this project as a learning exercise for ASP.Net MVC Core 3+.

## Coding Challenge
Develop a RESTful API that allows users to anonymously create, modify, read, and delete jokes.  This is a fun coding challenge I accepted.  I much prefer learning by creating something real. 

### Requirements and Considerations
I would define these as the stakeholder's customer interests.  Each had design decisions impacting the release of phase 1 of this project. There are trade-offs that I cannot make without further information from the stakeholders.  I would push back to get more information.

 - **Build the web application in C#, .NET Core**
 - ***Consider Scalability*** 
	 - *How would your application:*
		 - handle 2 million Jokes?  
		 - a slow connection speed?
		 - a spike in requests?
 - ***Design and create RESTful endpoints (CRUD)***
	 - **Keep in mind things such as:**
		 - Route urls
		 - Parameters needed
		 - Model Validation
		 - Testability
		 - Logging
		 - Error Handling
	 - **Add an endpoint to retrieve a random joke**
 - **Store Jokes in a Memory or SQLite database**
	 - Pre-population of Joke Data
 - **Provide Swagger documentation**

## Bonus
 - Add the ability to filter/search jokes - (*done*)
 - Authorization (on modify data routes) - (*done*)
 - Docker-ize your application - (*Todo*)

## Design Decisions and Knowlege Gaps

 - **how to handle 2 million Jokes?**  
	 - **Pushback**:  How do we measure success? what is "acceptable?"
				 - Size of Database (potential cost of storing)?
				 - Type of Database (SQLite vs __________?)
				 - Responsiveness? (what is acceptable to the client?)
	
 - **how to handle a slow connection speed?**  
	 - **Pushback**: What approach is acceptable to the client?
				- Keep retrying the same command N number of times?
				- Fail after the first time and let the user know there is an issue with the network?
				- Thoughts? Ideas?  
				
**Knowledge Gap**: I did not directly address slow connections or request spikes...at least not purposefully.  More research is needed to understand solutions and how to setup the testing.

**Useful Articles:**

 - [ASP.Net Core Performance Best Practices](https://docs.microsoft.com/en-us/aspnet/core/performance/performance-best-practices?view=aspnetcore-3.1)
 - [Reducing Latency by Pre-building Singletons in Asp.Net Core](https://andrewlock.net/reducing-latency-by-pre-building-singletons-in-asp-net-core/)
 - [Maximizing .Net Core API Performance](https://medium.com/asos-techblog/maximising-net-core-api-performance-11ad883436c)
 - [ASP.Net Core Load/Stress Testing](https://docs.microsoft.com/en-us/aspnet/core/test/load-tests?view=aspnetcore-3.1)
 - [Top 7 .Net Application Performance Problems](https://www.eginnovations.com/blog/top-7-net-application-performance-problems/)
## Scalability Considerations
I am thinking about scalability both vertically (same server) as well as horizontally (services distributed across multiple servers).   Studying this helped me decide to use the async/await design pattern for my RESTful API.

This is an amazing video explaining how .Net provides scalability via async.  I really like .Net core's approach here...
 - [Best Practices for Buliding Async APIs with ASP.Net Core](https://www.youtube.com/watch?v=_T3kvAxAPpQ)

### *Vertical Scaling*
ASP.Net Core's asynchronous capability lends well to scaling vertically on the same server.  The asyc/await design allows the application to make effective use of the handles/threads available.  The .Net Core Async design allows applications to make us of increasing resources (adding hard-drive space, memory, cpu, cache).

### *Horizontal Scaling*
The stateless architecture of RESTful API lend well to scaling horizontally across multiple servers.  Client information is not stored on the server.  It just receives a single requests and provides a single response.  The issue here is the database.  SQLite is not distributed.

### *Distributed Cache*
I came across an interesting url with regard to distributed cache in .Net Core applications.  My quick glimpse of the information shows some powerful ability to add scalability.  Here is the article:
 -  [https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1)

**Knowledge Gap:** *I did not have time to research and implement distributed caching.  However, I consider it important in the design.*

## Logging Consideration
I decided to use Serilog and log it to a structured logging service (Seq by Datalust).  
Serilog updates Seq in batches to minimize logging performance bottlenecks.

Useful articles: 	 
 - https://stackify.com/nlog-vs-log4net-vs-serilog/
 -  https://blog.elmah.io/serilog-vs-nlog/
 -  [https://docs.datalust.co/docs/using-serilog](https://docs.datalust.co/docs/using-serilog)
 - [https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/](https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/)

**NOTE**: 
*I have years of experience supporting enterprise software. This has taught me to be a defensive logger.  I like to log as much potentially helpful information without degrading performance where possible.  This allows our company to more quickly provide solutions for customers.*

## Localization Consideration
I used a lot of hard-coded strings in this example.  It is not localizable at this point.

## Testing Considerations

 - I used Postman and Swagger for manual testing of the API. 
 - I used xUnit / Moq to test the repository / api (See the JokesAPI.Tests Project)

Goal: To study the Microsoft.AspNetCore.Mvc.Testing package.

Quick Exerpt of the article referenced below:
*"...The release of ASP.NET Core 2.1 introduced a handy new package in  [Microsoft.AspNetCore.Mvc.Testing](https://blogs.msdn.microsoft.com/webdev/2018/03/05/asp-net-core-2-1-0-preview1-functional-testing-of-mvc-applications). Its primary goal is streamlining end-to-end MVC and Web API testing by hosting the full web stack (database included) in memory while providing a client to test "from the outside in" by issuing http requests to our application.
Having this test host available means, we can write tests that look and execute quickly like unit tests but exercise almost all layers of our code without the need for any network or database - rad! 😎 ..."*

Useful article:
 - [ Painless Integration Testing with ASP.NET Core Web API](https://fullstackmark.com/post/20/painless-integration-testing-with-aspnet-core-web-api)

## Authentication / Authorization
I used Java Web Token based authentication. I did not focus on authorization (roles).  In fact, there's not even a logout.  Not proud of it but at least I have something to demo.

**Knowledge Gap:** *I need to research this area more deeply to understand roles/permissions based authorization in .Net Core*

**WARNING**
NOTE: This is NOT a secure implementation.  Currently the HttpGet for all users returns the user's password (Noooooooooo Clint. Say it ain't so!).  

Yep, I did that to make it easy for you to simply query the current users and use their password.    You are welcome to create your own user and password. 

TODO: Add automapper to map entity model to presentation model (so we don't pass around things like passwords) :-)


## Database Table Considerations
I have kept this simple at this time with just a model and controller for Jokes with "id" and "joke" columns.  However, with more time I would add Author, Category, Tags, Rating, MinAgeAppropriate, and DateAdded.  We might even keep track of how many times a joke is served.  I would likely have a Category Model/Controller as well as a Tags Model/Controller.  

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

Here are things you may need to install and how to install them:

 - **Visual Studio 2019 Community Edition**
   This is a free installation - [https://visualstudio.microsoft.com/downloads/](https://visualstudio.microsoft.com/downloads/)
- Get the latest code from my GitHub Repository and open that in Visual Studio 2019 Community Edition.  

  **Note**: I used Visual Studio's GitHub interface and GitBash CLI. It worked fairly well.  
  
- Next, you need to install several packages (for Entity Framework, Sqlite, Serilog, and Seq).

	**I used the Visual Studio NuGet Package Manager to install:**
	Simple right click on the JokesAPI Project and select Manage Nugets Packages.  Then browse for and install the following packages:
	
	 Microsoft.AspNetCore.Authentication.JwtBearer(3.1.1)
	 Microsoft.EntityFrameworkCore.Sqlite (3.1.0)
	 Microsoft.EntityFrameworkCore.Tools (3.1.0)
	 Microsoft.IdentityModel.Tokens (5.6.0)
	 Serilog.AspNetCore (3.2.0)
	 Serilog.Formmating.Compact (1.1.0)
	 Serilog.Sinks.Seq (4.0.0) 
	 Swashbuckle.AspNetCore (5.0.0)
	 Swashbuckle.AspNetCore.Swagger (5.0.0)
	 Swashbuckle.AspNetCore.SwaggerGen (5.0.0)
	 Swashbuckle.AspNetCore.SwaggerUI (5.0.0)
	 System.IdentityModel.Tokens.Jwt (5.6.0)

**LOGGING SETUP**
 The packages for Serilog is listed above.
 
## SEQ Structured Logging Setup
Next, you'll need to install the Seq Structured Logging Server

Download and install the *Seq Structured Logging Server** (5.1)  [https://datalust.co/seq/](https://datalust.co/seq/)
Here is a good article on setting this up: [# Setting up Serilog in ASP.NET Core 3 ](https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/)

The Seq log server on your machine might use a different port than this project.  You will need to figure out the port and make the necessary change inside Program.cs inside Main.

As you can see, my Seq log server is using port 5341.  
![SeqLogCodeSetup](https://user-images.githubusercontent.com/5245897/72556913-47762980-3865-11ea-9f08-52dc73aa6a6f.png)

Note: The serilog instantiation happes at the top of Main prior to Configuration which allows us to start logging immediately.  The Microsoft Logging feature is ony available after the Configuration methods run.

**Launching the Seq Structured Log View**
There's probably a way better way to make this happen.  For now, I am simply clicking the url link inside my Program.cs Main.
![HowToLaunchSeqLogViewer](https://user-images.githubusercontent.com/5245897/72557301-1518fc00-3866-11ea-93ea-6660138e5b3a.png)

**SQLite Database**
  Next, you'll need to install Sqlite 

 - **Sqlite for Windows**.  Here is a tutorial on that: [https://www.sqlitetutorial.net/download-install-sqlite/](https://www.sqlitetutorial.net/download-install-sqlite/)
 *Note: You need to set a System Environment Variable for Sqlite3 to the folder where you place the Sqlite files.*
```
```
## Running the tests

**AUTOMATED TESTS**
There is a test project inside the solution called JokesAPI.Tests
You will need to add the following Nuget packages to that project:
 - Moq, by Daniel Cazzulino, kzu, Version 4.13.1
 - xunit by James Newkirk, Brad Wilson v2.4.0, v.2.4.1
 - xunit.runner.visualstudio by  James Newkirk, Brad Wilson v2.4.0, v.2.4.1

**MANUAL TESTING**
I am using Postman to test the API against a real repository/db source.

There are a ton of tutorials on how to use Postman.  I assume the reviewers of this project are familiar.  

Note: You can also test from the Swagger page that comes up by default as the landing page.

You can download a copy of Postman here: [Postman Download](https://www.getpostman.com/downloads/)

**Postman Collection**
Here is a link to my Postman Collection.  [Postman Collection of API Tests](!%5BPOSTUsingTokenVariable%5D%28https://user-images.githubusercontent.com/5245897/72574277-11996b00-388e-11ea-84cc-630badf977ee.png%29)  

To Use That Link: *Open Postman, then click on that link to install it into your Postman application.  Note that you may have to change the Port...more on that below...*

NOTE: This collection provides login, user management, and bad data validation.

**POSTMan Environment Variables**
I HIGHLY suggest you learn about using Environment Variables inside Postman if you are not familiar.  Environment variables allow you to run the login, get the web token, save it to a environment variable {{token}}, and then use that {{token}} variable in the authentication of other API calls without having to copy/paste the token string (which changes with each login session).

Here is a good article on how to make that happen:  [# Using Postman Environment Variables & Auth Tokens](https://medium.com/@codebyjeff/using-postman-environment-variables-auth-tokens-ea9c4fe9d3d7)

This is my login Postman environment setup:
![MyPostManSystemEnvSetup](https://user-images.githubusercontent.com/5245897/72614978-3676f800-38f9-11ea-8f05-71e7d7a074e3.png)

**Saving away the web token on login**
![PostmanSavingWebToken](https://user-images.githubusercontent.com/5245897/72615461-5d81f980-38fa-11ea-89bb-a1fb2a5ddc35.png)


## Deployment

Add additional notes about how to deploy this on a live system.  If you are seeing this then I have not developed a deployment plan.  In a real-life project, I would not move forward with development without understanding this key aspect. 

## Built With
Visual Studio 2019 Community Edition, Entity Framework, LINQ, Serilog, Seq, Sqlite

## Contributing
I am not accepting contributors at this time.

## Versioning

I am using [SemVer](http://semver.org/)  for versioning. 
For the versions available, see the  [tags on this repository](https://github.com/your/project/tags).

## Authors

-   **Clint Carter**  -  _Initial work_  -  [My LinkedIn Page](http://www.linkedIn.com/in/clintcarter1999)


## License
This project is licensed under the MIT License - see the  [LICENSE.md](https://gist.github.com/PurpleBooth/LICENSE.md)  file for details

## Acknowledgments

 -   Hat tip to BorrowWorks for providing this very fun coding assignment.  It is way easier to learn when you have an actual project.
 - Laurie Sutherlin ([https://www.linkedin.com/in/laurie-sutherlin-95814132/](https://www.linkedin.com/in/laurie-sutherlin-95814132/)) (*Technology Business Development Manager at System Soft Technologies*).  Laurie makes a personal investment in learning as much about the resource before matching them with opportunities.  I am thankful for the opportunity she provided here.

 -   Thanks to all the people who have taken time to produce excellent articles, blocks, tutorials, and videos for 
	 - ASP.Net Core 3.0 REST API with a Sqlite DB, 
	 - Serilog vs default Microsoft Logging, 
	 - Structured Logging, 
	 - Using Middleware, 
	 - Async Best Design Practices, 
	 - Robert Glazer's book [Elevate by Robert Glazer](https://www.amazon.com/Elevate-Beyond-Limits-Success-Yourself/dp/1492691488/ref=sr_1_1?crid=3TFVPOMG2U6MV&keywords=elevate%20by%20robert%20glazer&qid=1579027201&sprefix=Elevate%20by%20Rober,aps,154&sr=8-1) for heping me win the morning (get more done!)
-   StackEdit.io for providing a cool online GitHub readme markup editor.

## Thanks
I am thankful the chance to dig into learning ASP.Net Core RESTful API.  I recently finished a class on React.js where we used async/promises in a similar manner.  I definitely preferred ASP.Net Core's strongly typed compiled environment over transpiled JavaScript/Node.js .  However, JavaScript is getting pretty powerful.  Definitely some cool stuff there! I really like the props destructuring!

I added several great 'Dad' jokes to my 'act' during this project!!

**My 3 favorites:**

How do you catch a UNIQUE rabbit?  *Unique up on him!*
How do you catch a TAME rabbit? **Tame way, unique up on him!*
What do you call a nose without a body? *Nobody Nose!* 

![Picture of Clint Carter ](https://media-exp2.licdn.com/dms/image/C4E03AQGN2o3h3XtNAg/profile-displayphoto-shrink_200_200/0?e=1584576000&v=beta&t=fOdGtATS_XFihlBXQ6BU8WYYT5Gmo31O_jx2zeNrxi8)
Clint Carter is a Senior .Net Full-Stack Developer.  Over 20 years of experience of exceeding expectations and delivering quality software in multiple industries (Oil & Gas, Telcom, Medical, Engineering Design, Software Developer Tools (Visual Basic IDE Power Tools)).  I love solving hard problems, continous learning, and making a difference/helping people.  
