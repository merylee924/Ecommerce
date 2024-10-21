Shopping Cart Web Application
Overview
This project is a web application developed using .NET, designed to provide a seamless shopping experience. The application follows the Model-View-ViewModel (MVVM) architectural pattern, ensuring a clean separation of concerns and maintainable code.

Features
User-Friendly Interface: An intuitive and responsive design that enhances user experience.
MVVM Architecture: Utilizes the MVVM pattern to separate the user interface logic from business logic, making it easier to manage and scale the application.
Cookie-Based Cart Storage: The application stores cart content in cookies, allowing users to maintain their shopping cart state across sessions without requiring a backend database for this feature.
Dynamic Product Listing: Displays a list of available products that users can easily add to their cart.
Real-Time Cart Updates: Users can view and modify their cart in real time, enhancing the overall shopping experience.
Technologies Used
.NET: The application is built on the .NET framework, providing a robust backend.
MVVM Pattern: This architectural pattern is used to separate the user interface from the underlying business logic.
Cookies: Used to store cart information on the client-side for a persistent shopping experience.
Installation
Clone the repository:
git clone <repository_url>
Navigate to the project directory:
cd ShoppingCartWebApp
Restore dependencies:dotnet restore
Run the application:dotnet run
Usage
To view the list of products, navigate to: https://localhost:7216/Products
To view the contents of your shopping cart, navigate to: https://localhost:7216/ShoppingCarts
