# Rank Generator .NET
This is a neat little class library that can be used to generate ranks from collections of property-rich prefixes, titles, and postfixes.
Useful for RPGs!
````c#
var generator = RankGenerator.Initialize("military.json");
generator.NextRank(); // Brigadier General
generator.NextRank(); // General
generator.Merge(fantasyGenerator);
generator.RandomRank(); // Legendary General of Camelot
````

## How does it work?
Prefixes, titles, and postfixes are retrieved from JSON files in the following format:
````javascript
{
    "Prefixes": [{/* Affix object */}, ...],
    "Titles": [{/* Title object */}, ...],
    "Postfixes": [{/* Affix object */}, ...]
}
````
Each title object can have the following properties:
````javascript
{
  "Phrase": "King",
  "Categories": ["nobility", "ruling"],
  "Tier": 1337 //dictates sequential generation order
}
````
Affixes have additional properties:
````javascript
{
  "Phrase": "Honorable", // Prefix
  "Categories": ["nobility"],
  "Tier": 9, // combined with Title's tier to get overall Rank tier
  
  /* Optional properties: */
  "Whitelist": ["King", "Queen", "Prince", "Princess"] // restrict to only these titles
  "Blacklist": ["Jester"], // unnecessary if a whitelist exists
  "RestrictCategories": true, // restrict to titles that have a category in common
  "MinimumTier": 1336, // floor for title tiers
  "MaximumTier": 1338 // cieling for title tiers
}
````
Once you have [a JSON file](https://github.com/bcanseco/rank-generator/blob/master/RankGenerator.Examples/Sample%20Data) set up with this data, you can invoke methods from a `RankGenerator` object. Use the static `Initialize()` method, which accepts a path to the JSON file.
````c#
var generator = RankGenerator.Initialize("government.json")
Console.WriteLine(generator.NextRank()); // "Honorable King"
````
You can even merge other JSON files into your generator at runtime to diversify your pool of available ranks.
````c#
var corporationGenerator = RankGenerator.Initialize("corporation.json");
generator.Merge(corporationGenerator);
Console.WriteLine(generator.RandomRank()); // "Honorable King of Human Resources"
````
[Rank](https://github.com/bcanseco/rank-generator/blob/master/RankGenerator/Classes/Rank.cs) objects are implicitly casted as strings, so they can be used anywhere in your application without losing valuable phrase metadata.

## Getting started
1. Download or `git clone` the repository.
2. Launch [Rank Generator.sln](https://github.com/bcanseco/rank-generator/blob/master/Rank%20Generator.sln).
3. Start the project to check out the examples.

## ToDo
* Add more (*and better*) unit tests and XML documentation comments.
* Add more error checking, particularly with the JSON.NET initialization.
* Get more functionality out of postfixes when generating `NextRank()`s.
* Refactor to avoid multiple enumerations
