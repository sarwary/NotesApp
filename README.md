# NotesApp
This project includes CRUD operations for not taking app using .NET Web API

This solution includes three project to improve scalability and seperatoin of concerns:

1. Notes.Common.DataContext.Sqlite
2. Notes.Common.EntityModels
3. Notes.WebApi

Additionaly, SQLite database is used to store and manage data. the SQL database file is called Note.db.
Please keep it in the same folder where the projects are, so that the code can find it's location

How to run the project:
1. Download the projects into one folder including Notes.db file.
2. Build each project using the command below in command line app such us cmd(for window) or Terminal(for MacOs)
    >dotnet build
3. Navigate to Note.WebApi Project and run below command
   >dotnet run (ensure that project is running)
4. Open Any web browser and navigate to: https://localhost:5001/swagger/index.html


Note: Please ensure to have .NET SDK 7.0 or latest installed for the project to run.
