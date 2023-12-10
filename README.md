# ActivityProject Back End
The ActivityProject Backend serves as the core server side component of a full stack app designed to manage and track user activities. This RESTful API provides the necessary endpoints for user authentication, activity creation, real-time communication, and other interactions.

Core Components:
Application Layer: Handles incoming requests, executing business logic, and returning responses. It's organised into various services, each responsible for different aspects of the application, such as user accounts management, activity scheduling, and messaging.

Domain Layer: Defines the business logic and rules of the application. It includes models representing the essential entities and interfaces for services that operate on these entities.

Infrastructure Layer: Implements persistence logic and interacts with the database. It contains the implementations for repositories, data access patterns, and other service integrations.

NewActivityProject: The primary web project that runs the application. It hosts the API controllers that serve as the entry points for all backend operations.

Additional Details:
Security: Incorporates authentication and authorisation strategies to ensure secure access to the application's features.

Real-time Communication: Utilises SignalR hubs for enabling real time interactions.

Image Handling: Offers services for image upload and management.

Persistence: Employs Entity Framework for ORM, facilitating robust and agile data handling.

The backend is designed with a focus on clean architecture, ensuring separation of concerns and making the system easy to maintain and extend. It's currently deployed and running live on Railway along with the front end.

## Built With

- [.Net7] The framework used for the backend
- [Ef Core] ORM used for data access
- [SignalR] Real-time web functionality
- [PostGres] Database system
- [Swagger]API documentation and testing tools
- [X Unit/fluent validation/moq] unit and integration testing
- [Cloudinary] Image upload

## Clone
git clone https://github.com/AAouiche/NewActivityProject.git

## Deployment
front end - https://reactactivities-production.up.railway.app/
back end - https://newactivityproject-production-47a4.up.railway.app/ (this link doesnt show anything but can be used to test endpoints)
  
