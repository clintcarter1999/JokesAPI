
# Jokes ASP.NET REST API Project

A RESTful API that allows users to anonymously create & read jokes.

## Coding Challenge
Develop a RESTful API that allows users to anonymously create & read jokes.

### Requirements and Considerations
I would define these as the stakeholder's customer interests.  Each had design decisions impacting the release of phase 1 of this project. There are trade-offs that I cannot make without further information.  I would push back to get more information.

 - **Build the web application in C#, .NET Core**
 - ***Considere Scalability*** 
	 - *How would your application:*
		 - handle 2 million Jokes?  
			 - **Pushback**: 
					 - How do we measure what is "acceptable?"
						 - Size of Database (potential cost of storing)
						 - Responsiveness of API against such a large store? What is the acceptable range of time for each given API?  Ex: Retrieve joke by id: [0, 1] second?
						 - 
		 - a slow connection speed?
			 - **Pushback**: 
					 - What approach do we want to take here?
						 - Keep retrying the same command N number of times?
						 - Fail after the first time?
						 - Thoughts? Ideas?  
		 - a spike in requests?
 - ***Design and create RESTful endpoints (CRUD)***
	 - **Keep in mind things such as:**
		 - Route urls
		 - Parameters needed
		 - Model Validation
		 - Testability
		 - Logging
			 -  **NOTE**: 
					 - *I have years of experience supporting enterprise software. This has taught me to be a defensive developer by logging as much information as possible/allowed.  This affects performance slightly.  However, it allows our company to provide search logs and provide solutions for customers faster.  Lots of logging.*
		 - Error Handling
	 - **Add an endpoint to retrieve a random joke**
		 - ***Note**: My current implementation needs refactoring.  It does not handle holes (deleted jokes). I need to build a contiguous list and randomly select from that.  I have a retry/fail pattern that will make X number of attempts before giving up.*
 - **Store Jokes in a Memory or SQLite database**
	 - Pre-population of Joke Data
 - **Provide Swagger documentation**
 
 **

## Bonus
 - Docker-ize your application - (*Todo*)
 - Add the ability to filter/search jokes - (*done*)
 - Authorization (on modify data routes) - (*done*)


## Learning Curve & Challenges
There were several knowledge gaps starting this project as well as a learning curve.  I have 1-1.5 weeks to provide a working deliverable.  The project should also represent of my use of good design principles, OOP methodologies, best practices, and and long term vision (scalability, readability, maintainability).  

Challenge accepted!

## **Learning Curve**
I have 15+years of C# experience. However, I had just a few days of .Net Core experience.  I have 1 to 1.5 weeks to provide a deliverable.  Therefore, I need to move the ball every day with a very limited resource (time).  We are working on a release which is requiring extensive time into the evenings. So I scheduled 5am-7:30am to work on this project.  Win the morning = Win the Day :)  Plus it was pretty fun so Win-Win.

Here is a glympse of how I approached this learning curve.  
I brain stored all knowledge gaps and then prioritized them according to the following priority:
  
| **Urgent + Important**  		|  Do this before anything else |
| **Important + Not Urgent**	| Important but can wait for Urgent tasks |
| **Not Important**				  	| Let's tentatively schedule this to discuss in two weeks if your problem has not been resolved :) |

## Design Decisions and Knowlege Gaps
I brain stormed all the knowledge gaps I had at the start of the project.  Then I prioritized and set out closing them before making a final decisions to make sure my decisions satisfy the stakeholder's customer interests.  There are always "unknown-unknowns" especially when rushing a design like this project.  I am adding them to the list as I come to them.

I typically take a more collaborative approach.  This includes communicating with all stakeholders to define measurable customer interests, discover the knowledge gaps, and setting measurable goals, and working towards the vision and expected product/service.  Stakeholders : customers, product owners, project managers, software leads, other developers, quality assurance/testing, release/build management, etc.

## Scalability Considerations
We have to think about scalability both vertically (same server) as well as horizontally (services distributed across multiple servers).   Studying this helped me decide to use the async/await design pattern for my RESTful API.

This is an amazing video explaining how .Net provides scalability via async. 
[Best Practices for Buliding Async APIs with ASP.Net Core](https://www.youtube.com/watch?v=_T3kvAxAPpQ)

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

## Authentication / Authorization
I used Java Web Token based authentication. I did not focus on authorization (roles).

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
 
 Next, you'll need to install the Seq Structured Logging Server

 - **Seq Structured Logging Server** (5.1)  [https://datalust.co/seq/](https://datalust.co/seq/)
 
**SQLite Database**

  Next, you'll need to install Sqlite 

  - **Sqlite for Windows**.  Here is a tutorial on that: [https://www.sqlitetutorial.net/download-install-sqlite/](https://www.sqlitetutorial.net/download-install-sqlite/)
 *Note: You need to set a System Environment Variable for Sqlite3 to the folder where you place the Sqlite files.*
```
```
## Running the tests

Explain how to run the automated tests for this system
//TODO: Provide a link to my Postman Collection:

### Break down into end to end tests


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
 - Laurie Sutherlin ([https://www.linkedin.com/in/laurie-sutherlin-95814132/](https://www.linkedin.com/in/laurie-sutherlin-95814132/)) (*Technology Business Development Manager at System Soft Technologies*).  Laurie makes a personal investment in learning as much about the resource before matching them with opportunities.  I feel that she is working "with" both sides.  

 -   Thanks to all the people who have taken time to produce excellent articles, blocks, tutorials, and videos for 
	 - ASP.Net Core 3.0 REST API with a Sqlite DB, 
	 - Serilog vs default Microsoft Logging, 
	 - Structured Logging, 
	 - Using Middleware, 
	 - Async Best Design Practices, 
	 - Robert Glazer's book [Elevate by Robert Glazer](https://www.amazon.com/Elevate-Beyond-Limits-Success-Yourself/dp/1492691488/ref=sr_1_1?crid=3TFVPOMG2U6MV&keywords=elevate%20by%20robert%20glazer&qid=1579027201&sprefix=Elevate%20by%20Rober,aps,154&sr=8-1) for heping me win the morning (get more done!)
-   StackEdit.io for providing a cool online GitHub readme markup editor.

## Thanks
I am thankful the chance to dig into learning ASP.Net Core RESTful API.  I recently finished a core on React.js where we used async/promises in a similar manner.  I definitely preferred ASP.Net Core's strongly typed compiled environment over JavaScript/Node.js.  However, I also really enjoyed learning JavaScript.  JavaScript ES6+ is getting pretty powerful!

I added several great 'Dad' jokes during this project!!

My 3 favorites:

"How do you catch a UNIQUE rabbit?  Unique up on him!"
"How do you catch a TAME rabbit? Tame way, unique up on him!"
'What do you call a nose without a body? Nobody Nose!'


![Picture of Clint Carter ](https://media-exp2.licdn.com/dms/image/C4E03AQGN2o3h3XtNAg/profile-displayphoto-shrink_200_200/0?e=1584576000&v=beta&t=fOdGtATS_XFihlBXQ6BU8WYYT5Gmo31O_jx2zeNrxi8)
Clint Carter is a Senior .Net Full-Stack Developer.  Over 20 years of experience of exceeding expectations and delivering quality software in multiple industries (Oil & Gas, Telcom, Medical, Engineering Design, Software Developer Tools (Visual Basic IDE Power Tools)).  I love solving hard problems, continous learning, and making a difference/helping people.  