
# Real-time Life Cycle Assessment – Bombyx project

### About:
<img align="left" src="https://i.imgur.com/aJduNdT.png">
The Bombyx tool is developed as a plugin for Grasshopper based on Rhinoceros and includes an SQL material and component database. Users are able to choose different materials and building systems and quickly modify the building’s geometry while continuously receiving the calculated environmental impact in real-time. Visualization of the results, e.g. colour code indicating how the design performs in relation to a benchmark or optimization potential. 


### Official download:
[food4Rhino](https://www.food4rhino.com/app/bombyx)


### Requirements and dependencies:
1. [Rhino3d](https://www.rhino3d.com/)
2. [Grasshopper](https://www.grasshopper3d.com/) (included in Rhino 6)
3. .NET Framework 4.6 (https://dotnet.microsoft.com/download/dotnet-framework)
4. Internet connection
5. A database is needed - we use Azure at the moment - thus the SQL scripts are provided to create a datatable and to insert materials into it:   
   [SQL scripts](../master/Bombyx.Data/SQLscripts)
   
   Example of Config.cs for Azure connection string:
   
   ```
   public static class Config
    {
        public static string connectAzure = "Server=[your_server];" +
            "Initial Catalog=[your_database];Persist Security Info=False;User ID=[your_username];" +
            "Password=[your_password];MultipleActiveResultSets=False;Encrypt=True;" +
            "TrustServerCertificate=False;Connection Timeout=30;";
    }
   ```


### Usage:
<p align="center">
   <img src="https://i.imgur.com/A6hUShl.png">
</p>

### Bombyx WIP 0.3 version contains:
* Component level
<img src="https://i.imgur.com/nqFFgX6.png"> The purpose of this method is to use predefined components form the database (Bauteilkatalog) to speed up the process of designing elements.


* Material level
<img src="https://i.imgur.com/7qWgSIj.png">

* Services
<img src="https://i.imgur.com/XZ7PrYQ.png">



### In development:
- Component impact
- Element impact
- Window / Door impact
- Operational impact


### Future goals:
+ Visualization
+ Own item selector implementation
