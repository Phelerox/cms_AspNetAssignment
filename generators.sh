dotnet aspnet-codegenerator controller -name CountriesController -m Country -udl -dc ApplicationDbContext -outDir Controllers --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CitiesController -m City -udl -dc ApplicationDbContext -outDir Controllers --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PersonsController -m Person -udl -dc ApplicationDbContext -outDir Controllers --referenceScriptLibraries -f
