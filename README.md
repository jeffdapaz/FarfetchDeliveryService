# FarfetchDeliveryService

This project is an exercice that demonstrates use of a Neo4j database to search a less effort (shortest) route between two points.

This project consist of two RESTfull APIs built using .NET Core 2.2 and one API as gateway using OCELOT.

- The FarfetchDeliveryServiceGatewayApi is the gateway API.
- The FarfetchDeliveryServiceBestRouteApi is the API where is possible search for the best routes between two points.
- The FarfetchDeliveryServiceApi is the API where is possible add, update or delete points and routes.

This project also show how to implements authetication and authorization on APIs.

Instructions to run the project:

1 - This project uses a SQL Server database to persist users account. To create this database, run the SQL Server script named "CreateDeliveryServiceDataBase.sql" located in "Files" folder to create the "FarfetchDeliveryService" database;

2 - In the file "\FarfetchDeliveryServiceExercice\FarfetchDeliveryServiceApi\appsettings.json", set the "connectionStrings.FarfetchDeliveryService" property with the "FarfetchDeliveryService" database connection string;

3 - This project uses the Neo4j, a nosql graph database to persist Points and Routes. Is possible to download Neo4j in this link:  https://neo4j.com/download/;

4 - Create a new database in Neo4j. Is recommended to use 3.5.14 database version due compatibility with graph algorithms;

5 - Install graph algorithms in the created database. See how: https://neo4j.com/docs/graph-algorithms/current/introduction/; 

6 - Run the script located in the file "Neo4J_Script.txt" in "Files" folder to create a sample data that reflects the Farfetch Delivery Service Exercice example;

7 - In the files:

"\FarfetchDeliveryServiceExercice\FarfetchDeliveryServiceBestRouteApi\appsettings.json" "\FarfetchDeliveryServiceExercice\FarfetchDeliveryServiceApi\appsettings.json"

Set the "Neo4j" properties with the Neo4j database connection data. Is necessary that the url follow the pattern example:  "http://localhost:7474/db/data";

8 - In the file "\FarfetchDeliveryServiceExercice\FarfetchDeliveryServiceGatewayApi\ocelot.json", make sure the APIs URLs are set correctly.

9 - In "Files" folder there is the "FarfetchDeliveryServiceExercice.json" Postman collection. This collection has examples to call the APIs, so only need to import this collection to Postman.

10 - In the Postman collection make sure the "URL" variable has the correctly "FarfetchDeliveryServiceGatewayApi" URL.

11 - All the APIs (except the gateway API) has the swagger configured. So is possible to make calls to the APIs by swagger. The swagger has coments to describe all actions.

12 - For run the project, make sure the "FarfetchDeliveryServiceExercice" solution is set to start all the APIs, or publish the APIs.   

13 - For call the "FarfetchDeliveryServiceBestRouteApi" is necessary to send in the requests header the key "Token" and the value "M5e5rbPFubPW7NU6pjuNAAspxx0ji075" for authetication. Its already configured in the Postman collection.

14 - To make calls to "FarfetchDeliveryServiceApi", first is necessary do a user login. Call the "Account/Login" action and send a user object. The database already has two users persisted:

Login: User
Password: 123456 

Login: Admin
Password: abcdef 

15 - After login, the action will return a token. Use this token as bearer authetication to be possible make calls to "FarfetchDeliveryServiceApi". Is possible to set this token in all requests examples in postman collection by "Token" variable. Only user "Admin" can create, update or delete points and routes.
