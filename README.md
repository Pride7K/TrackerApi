# TrackerApi

Still Working on

RestFul API with .NET Core



This project is just to test the knowledge with the .NET Core´s API and entity framework core concepts.

Which techs are being used in the project?


- C#
- ASP NET Core
- Entity Framework Core
- Sql Lite
- XUnit
- JWT Authentication

### How is it structured ?


[MVCS](https://quantiphi.com/an-introduction-to-mvcs-architecture/) Structure, but without the View since this project is not about frontend concepts.

The business layers can be find inside the services folder.

### Database

Configurations about database it´s in the the AppDbContext.cs on the method OnConfiguring.

![image](https://user-images.githubusercontent.com/52872315/195220917-1c5da99a-c1ba-4162-bc66-42542405ef79.png)


We can see that it´s configured to be cache shared. It´s because we in this way we can share the same cache in memory for every thread, reducing the numbers of connections in database.

We also can see the DbSet that are used to inform which classes are tables in our database.

### Models

Each model represents a table in database, which each propertie represents a column.

![image](https://user-images.githubusercontent.com/52872315/195221328-249f2dd5-8dc1-45b5-a9f4-3c2b27fd6ed5.png)

### Controllers

![image](https://user-images.githubusercontent.com/52872315/195221421-f166200d-0f90-426e-a72d-f525bf517f10.png)

Here we have our controller named as "UserController" that in inherits from "ControllerBase". That´s because doing this we can use some return types such as BadRequest() or Ok() that help us to make a restful Api.

We can also see some Dependency injection.

![image](https://user-images.githubusercontent.com/52872315/195221862-7132358f-f29e-4c5f-a6f8-5c98a69dc80b.png)

That´s possible because it´s mapped in the Startup class.

![image](https://user-images.githubusercontent.com/52872315/195221990-d494c201-5437-4749-bdf8-60879fca448b.png)

It´s was choosed to use AddScoped because since the same instance keep´s from each web request, makes sense to use in a controller.

Backing to our controller we can see that we are calling our business layer by the variable called "_services" that it´s being populated by DI.

![image](https://user-images.githubusercontent.com/52872315/195222403-c0158fca-96ec-4ba5-ad4f-c37eba0365f6.png)

One cool thing that we can see in the image above is the routing.

As we can see we are doing routing in a different way, as an example, "skip:int". We could have done in the normal way such as [FromRoute] but doing in this way it´s forces the consumer to pass a value with the type that we are expecting. If the consumer passes a value such as string instead a int he is going to get a 404 error.

