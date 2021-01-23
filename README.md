# Online Chat Application

This is online chat application with Angular, ASP.NET Core, SignalR and SqlServer following the principles of Clean Architecture. It has the following functionalities </br>

- As a new user you can resgister with your email address, first name and last name </br>
- Registered user can login with his email address. </br>
- LoggedIn user has dashboard and from where he can see list users and he can chat with any one from the list. Chat always happen between two users. </br>
- User can see chat history.</br>
- Application has the sign out functionality.</br></br>

## Technologies

- ASP.NET Core 3.1
- Entity Framework Core 3.1
- Angular 10
- Signal R
- Sql Server
- Angular Material

## Getting Started

1. Install the latest [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)
2. Install the latest [Node.js LTS](https://nodejs.org/en/)
3. Install the latest [git](https://git-scm.com/downloads)
4. Run `git clone https://github.com/Limon-7/SignalRChat.git` to clone this repository
5. Navigate to `ClientChatApp` and run `npm install ` to install npm packages for the front end (Angular)
6. run `npm start` to launch the front end (Angular)

## Database Configuration

The Application uses data-store in SQL Server.

Update the **connection** connection string within **server\ChatAPI** , so that application can point to a valid SQL Server instance.

```json
  "ConnectionStrings": {
	"connection": "Server=(localdb)\\MSSQLLocalDB;Database=ChatAppDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
```

When you run **update-database** command, the migrations will be applied and the database will be automatically created.

## Overview

### Domain (Chat.Data)

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application (Chat.Core)

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project.

### RestFull API (ChatAPI)

This layer is a restfull api based on .Net Core 3.1.

### Front-End (Anuglar 9)

Front-end is a single page application based on angular 10. This only communicates with restfull api layer to store or retrieve data.

## Run solution project

- API: Now double click and run the `BSChat.sln` file with visual studio . Run the project with IISExpress by clicking on the IISExpress button on the toolbar of visual studio.
- To run Client Side: Go to ClientChatApp and open it in terminal.
  **ng serve**

## Images

#### mobile

![Chat App](/images/desktopview.png)
![Chat Box](/images/mobileview2.png)
![Chat Box2](/images/mobileview1.png)

### Desktop

![Chat App](/images/desktopview.png)
![Chat Logout](/images/logout.png)
![Chat Login](/images/signup1.png)
![Chat Signup](/images/signup2.png)
![Swagger](/images/swagger.png)
