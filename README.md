
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


### Usage with Honeybee_Item Selector:
<p align="center">
   <img src="https://i.imgur.com/kOlomya.png">
</p>

### Bombyx WIP 0.3 version contains:
<img src="https://i.imgur.com/Qwha58A.png">

#### Component level
The purpose of Component level method is to use predefined components form the database (Bauteilkatalog) to speed up the process of designing elements.


* <img align="left" src="https://i.imgur.com/F293wrp.png"> Component groups returns a list of groups of components from database
* <img align="left" src="https://i.imgur.com/WoeUszU.png"> Components takes a component group as input parameter and returns a list of components from database
* <img align="left" src="https://i.imgur.com/C7gY4K8.png"> Default component requires a component and reference study period as inputs and returns a list LCA factors for the corresponding component
* <img align="left" src="https://i.imgur.com/AUMqIlM.png"> Window component returns a corresponding window component from database
* <img align="left" src="https://i.imgur.com/V2BobEe.png"> Generated component is currently under development
<hr>

#### Energy
KBOB Energy


* <img align="left" src="https://i.imgur.com/pUaY2H5.png"> KBOB Energy list returns a list of energy carriers from database
* <img align="left" src="https://i.imgur.com/dUzZUKK.png"> KBOB Energy requires Energy demand and Energy carrier as inputs and returns building energy properties
<hr>

#### Impacts
The main goal of the plug-in is presented with impacts. In case of Materials level approach, the impacts are connected in order: ... > material > layer > component > element > building (layer and component can be replaced with window component from impacts tab) and in case of Component level, the order is: ... > default component > element > building (default component can be replaced with window component from component level tab)

* <img align="left" src="https://i.imgur.com/ZBn5cud.png"> Layer impact takes material properties (list) and thickness as input parameters
* <img align="left" src="https://i.imgur.com/tjWF6Zl.png"> Component impact takes layer properties (list), reference study period and reference service life as input parameters and returns a list of LCA factors
* <img align="left" src="https://i.imgur.com/tvMHrQU.png"> Window impact requires multiple inputs from the user: frame properties, filling properties, frame percentage, reference study period, reference service life and U value; and returns a list of window properties (window component is connected to its own Element impact)
* <img align="left" src="https://i.imgur.com/UScf1bV.png"> Element impact takes one or more Component and/or Window impacts, area in square meters and a functionality (ext wall, int wall, window, floor, roof, ceiling) as inputs and returns a list of LCA factors, U-value and UA-value
* <img align="left" src="https://i.imgur.com/littJit.png"> Building impact takes one or more Element impacts, reference study period and net floor area and returns LCA impact results (Global warming potential, Primary energy, UBP)
<hr>

#### Materials
Materials level method allows user to select any material from the database to design different layer and component impacts (may result in unrealistic design of layers and components)


* <img align="left" src="https://i.imgur.com/23wYHjz.png"> KBOB Material groups returns a predefined list of material groups to be connected to item selector
* <img align="left" src="https://i.imgur.com/DsVmjsW.png"> KBOB Materials returns a list of materials from the selected KBOB material group
* <img align="left" src="https://i.imgur.com/dRbvd8Y.png"> KBOB Material returns properties of the selected material
<hr>

#### Services
KBOB Services 


* <img align="left" src="https://i.imgur.com/tG3BK1b.png"> KBOB Services returns a list of predefined energy groups
* <img align="left" src="https://i.imgur.com/sOQekAn.png"> KBOB Services takes an energy group as input parameter and returns a list of services 
* <img align="left" src="https://i.imgur.com/QDmu5fd.png"> KBOB Service returns all properties corresponding to the selected service
<hr>

### In development:
- Generated components


### Future goals:
+ Visualization
+ Own item selector implementation
