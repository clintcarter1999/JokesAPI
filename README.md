# Jokes ASP.NET REST API Project

A RESTful API that allows users to anonymously create & read jokes.

## Coding Challenge
Develop a RESTful API that allows users to anonymously create & read jokes.

### Requirements and Considerations

 - Build the web application in C#, .NET Core
 - Considere Scalability 
	 - How would your application:
		 - handle 2 million Jokes?
		 - a slow connection speed?
		 - a spike in requests?
 - Design and create RESTful endpoints (CRUD)
	 - Keep in mind things such as:
		 - Route urls
		 - Parameters needed
		 - Model Validation
		 - Testability / Logging
		 - Error Handling
	 - Add an endpoint to retrieve a random joke
 - Store Jokes in a Memory or SQLite database
	 - Pre-population of Joke Data
 - Provide Swagger documentation
 - Submit the deliverable as a Github repository, including documentation on how to run your application.
 #### Bonus
 
 - Docker-ize your application
 - Add the ability to filter/search jokes
 - Authorization

## Learning Curve Management
There were several knowledge gaps starting this project as well as a learning curve.  I have 1-1.5 weeks to provide a working deliverable.  The project should also represent of my use of good design principles, OOP methodologies, best practices, and and long term vision (scalability, readability, maintainability).  

Challenge accepted!

    **NOTE**: I am going to dig into these topics in detail the sections below.  
    Skip to the Getting Started Section below if you are more concered with "how" 
    rather than "why".

 - **Learning Curve**
I have 15+years of C# experience including Microsoft Web Services. However, I had just a few days of .Net Core experience.  I have 1 to 1.5 weeks to provide a deliverable.  Therefore, I need to move the ball every day with a very limited resource (time).  Here is a glympse of how I approached this learning curve.  
  
	I created a list of "must learn" topics and prioritized them as:
	 
	 **Urgent + Important** = Do this before anything else

	 **Important + Not Urgent** = Important but can wait for Urgent tasks

	 **Not Important** = Anything classified like this gives me permission to say "No" on focus on U+I
 
  - **TOPICS TO LEARN**
	  - How to create a RESTful API in .Net Core.   
	  - Model Validation
	  - Using the .Net Package Manager to create Migrations to alter the Database and its tables.
	  - Logging in .Net Core taking into consideration scalability, performance, usability/structured
	  - Using SQLite in .Net Core
	  - Error handling in .Net Core for RESTful services
	  - Scalability Best Practices with .Net Core - Vertical, Horizontal
		  - Vertical - Writing code that takes advantages of server resources (Async)
		  - Horizontal - Ability to scale the API across multiple servers (Stateless)
		  - Potential Bottlenecks
			  - SQLite is not a distributed DB
			  - ????
			  NOTE: Microsoft's distributed caching looks promising
	  - Best Practices for .Net Core RESTful API
	  - Best Practices for Async based RESTful API 
	  - Best Practices for testing API Controllers, Model
	  - Best Practices for automated testing of .Net Core applications
	  - Learning about Middleware
	  - Learning GitHub - I have used many source control providers.  GitHub is new for me.

## Design Decisions and Knowlege Gaps
I brain stormed all the knowledge gaps I had at the start of the project.  Then I prioritized and set out closing them before making a final decisions on how to support scalability, logging, validation, testing, and maintenance.  There are always "unknown-unknowns" especially when rushing a design like this project.  I am adding them to the list as I come to them.

## Scalability Considerations
We have to think about scalability both vertically (same server) as well as horizontally (services distributed across multiple servers).   Studying this helped me decide to use the async/await design pattern for my RESTful API.

### *Vertical Scaling*
ASP.Net Core's asynchronous capability lends well to scaling vertically on the same server.  The asyc/await design allows the application to make effective use of the handles/threads available.  The .Net Core Async design allows applications to make us of increasing resources (adding hard-drive space, memory, cpu, cache).

### *Horizontal Scaling*
The stateless architecture of RESTful API lend well to scaling horizontally across multiple servers.  Client information is not stored on the server.  It just receives a single requests and provides a single response.  The issue here is the database.  SQLite is not distributed.

### *Distributed Cache*
I came across an interesting url with regard to distributed cache in .Net Core applications.  My quick glimpse of the information shows some powerful ability to add scalability.  Here is the article I want to read first:
 [https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1)
If you are reading this then I did not have time to tackle this topic yet.  However, I consider it important in the design.

## Logging Consideration
I decided to use Serilog because it uses structured logging.  It also supports "enrichers" which provides the ability to add information (such as requestor info) or improve the understandability (perhaps of stack traces).  For now, I am logging to a structured viewer called Seq by Datalust.
Serilog updates Seq in batches to minimize logging performance bottlenecks.  

Useful articles: 
	https://stackify.com/nlog-vs-log4net-vs-serilog/
	https://blog.elmah.io/serilog-vs-nlog/
	[https://docs.datalust.co/docs/using-serilog](https://docs.datalust.co/docs/using-serilog)
	[https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/](https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/)

## Testing Considerations
Manual testing using Postman and Swagger are useful for quick testing of API.  However, it doesn't fit in the continuous integration pipeline.  My goal is to understand the new Microsoft.AspNetCore.Mvc.Testing package.  

Quick Exerpt of the article referenced below:
*"...The release of ASP.NET Core 2.1 introduced a handy new package in  [Microsoft.AspNetCore.Mvc.Testing](https://blogs.msdn.microsoft.com/webdev/2018/03/05/asp-net-core-2-1-0-preview1-functional-testing-of-mvc-applications). Its primary goal is streamlining end-to-end MVC and Web API testing by hosting the full web stack (database included) in memory while providing a client to test "from the outside in" by issuing http requests to our application.
Having this test host available means, we can write tests that look and execute quickly like unit tests but exercise almost all layers of our code without the need for any network or database - rad! 😎 ..."*

Useful article:
[ Painless Integration Testing with ASP.NET Core Web API](https://fullstackmark.com/post/20/painless-integration-testing-with-aspnet-core-web-api)

//TODO: Implement automated tsting

//TODO: Provide a link to my Postman collection

## Authorization
Not sure what I will chose. Saving this for last:
	
***Auth0*** - I have some experience with Auth0 in JavaScript 
***Microsoft Identity*** - I am interested in learning about this so I can  make a better design decision

Useful article:
[https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio)

## Deployment
I admit that I have not prioritized learning the deployment best practices and considerations.  This is frustrating for me because it absolutely has to happen before considering the implementation.  It is a knowledge gap I would close in real life before making final decisions on design and implementation.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

What things you need to install the software and how to install them:

 - **Visual Studio 2019 Community Edition**
   This is a free installation - [https://visualstudio.microsoft.com/downloads/](https://visualstudio.microsoft.com/downloads/)
- Get the latest code from my Repo Here and open that in Visual Studio 2019 Community Edition
- Next, you need to install several packages (for Entity Framework, Sqlite, Serilog, and Seq).

	I used the Visual Studio NuGet Package Manager to install:
 - **Entity Framework** - 
 Microsoft.EntityFrameworkCore.InMemory (3.1.0) 
 Microsoft.EntityFrameworkCore.Sqlite (3.1.0)
 Microsoft.EntityFrameworkCore.Tools (3.1.0)
 
 - **Serilog** - This page has a great getting started with Serilog in Visual Studio
 [https://github.com/serilog/serilog/wiki/Getting-Started](https://github.com/serilog/serilog/wiki/Getting-Started)

	 Note: I used the Visual Studio NuGet Package Manager to install:
	 Serilog.AspNetCore (3.2.0
	 Serilog.Formmating.Compact (1.1.0)
	 Serilog.Sinks.Seq (4.0.0) 
 
 Next, you'll need to install the Seq Structured Logging Server

 - **Seq Structured Logging Server** (5.1)  [https://datalust.co/seq/](https://datalust.co/seq/)
 
  Next, you'll need to install Sqlite 

  - **Sqlite for Windows**.  Here is a tutorial on that: [https://www.sqlitetutorial.net/download-install-sqlite/](https://www.sqlitetutorial.net/download-install-sqlite/)
 *Note: You need to set a System Environment Variable for Sqlite3 to the folder where you place the Sqlite files.*
```
```
## Running the tests

Explain how to run the automated tests for this system
//TODO: Provide a link to my Postman Collection:

### Break down into end to end tests

Explain what these tests test and why
```
Give an example

```

### And coding style tests

Explain what these tests test and why

```
Give an example

```

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

 -   Hat tip to BorrowWorks for providing this very fun coding assignment.
 - Laurie Sutherlin ([https://www.linkedin.com/in/laurie-sutherlin-95814132/](https://www.linkedin.com/in/laurie-sutherlin-95814132/)) (*Technology Business Development Manager at System Soft Technologies*).  Laurie makes a personal investment before matching talent with opportunities by taking the time to truly understand what you bring to the table as far as hard AND soft skills.  

 -   Thanks to all the people who have taken time to produce excellent articles, blocks, tutorials, and videos for 
	 - ASP.Net Core 3.0 REST API with a Sqlite DB, 
	 - Serilog vs default Microsoft Logging, 
	 - Structured Logging, 
	 - Using Middleware, 
	 - Async Best Design Practices, 
	 - Robert Glazer's book [Elevate by Robert Glazer](https://www.amazon.com/Elevate-Beyond-Limits-Success-Yourself/dp/1492691488/ref=sr_1_1?crid=3TFVPOMG2U6MV&keywords=elevate%20by%20robert%20glazer&qid=1579027201&sprefix=Elevate%20by%20Rober,aps,154&sr=8-1) for heping me win the morning (get more done!)
-   StackEdit.io for providing a cool online GitHub readme markup editor.


![Picture of Clint Carter ](https://media-exp2.licdn.com/dms/image/C4E03AQGN2o3h3XtNAg/profile-displayphoto-shrink_200_200/0?e=1584576000&v=beta&t=fOdGtATS_XFihlBXQ6BU8WYYT5Gmo31O_jx2zeNrxi8)
Clint Carter is a Senior .Net Full-Stack Developer.  He has over 20 years of experience of exceeding expectations and delivering quality software in multiple industries (Oil & Gas, Telcom, Medical, Engineering Design, Software Developer Tools (Visual Basic IDE Power Tools)).  He likes to have fun solving hard problems, continous learning, and making a difference/helping people.  