# codestormers

## 👥 Team Members
- Faysal890 (Team Leader)
- RedwanAhmedTapu
- Rafi-Sama

## 🧑‍🏫 Mentor
- atiq-bs23

## 🌌 CosmoVerse - Codestormers Team  

Welcome to **CosmoVerse**, an innovative project crafted during the Learnathon initiative! Our platform leverages advanced technologies to provide an engaging and immersive learning experience, allowing users to explore and understand the cosmos.

## 🌟 Project Description
CosmoVerse is an innovative project developed as part of the Learnathon initiative. It combines cutting-edge technologies to create an engaging learning platform where users can explore and understand galaxies. The application leverages 3D models to visualize planetary systems, user authentication for personalized experiences, and a robust backend for seamless performance.

## 🛠️ Technologies Used
- **ASP.NET Core** for backend development
- **Entity Framework** for database management
- **React.js** for frontend (if applicable)
- **Three.js** for 3D rendering
- **JWT** for user authentication

## 📖 API Documentation
The API documentation is available on [SwaggerHub](https://app.swaggerhub.com/apis-docs/cosmoverse/cosmo-verse_api/v1#/).

## 🚀 Getting Started 
### 1. Clone the repository
    git clone https://github.com/Learnathon-By-Geeky-Solutions/codestormers.git
### 2. Install dependencies
  - ### For API:
    ```bash
    cd codestormersbackend/CosmoVerse/CosmoVerse
    ```
    Install dependencies:
    ```bash
    dotnet restore
    ```
    Setup .env file like this [Example🔗](.env.example)
    Or, you can set the database connection string in appsettings.json

    Database setup:
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```
    Run application:
    ```bash
    dotnet run
    ```


## Development Guidelines
1. Create feature branches
2. Make small, focused commits
3. Write descriptive commit messages
4. Create pull requests for review

## 📂 Resources
- [Project Documentation](docs/)
- [Development Setup](docs/setup.md)
- [Contributing Guidelines](CONTRIBUTING.md)
