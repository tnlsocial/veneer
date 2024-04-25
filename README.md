# veneer!
A C# ASP.NET MVC application that can generate captions on your supplied screenshots. 

## Development

Set the right folders and font in ```appsettings.Development.json```,
```json
{
  ...,
  "ImageDirs": {
    "tvshow": "d:\\temp\\tvshow\\img",
    "movie": "d:\\temp\\movie\\img"
  },
  "Font": "d:\\temp\\fonts\\OpenSansEmoji.ttf"
}
```

Don't forget to supply images and a font as well!

Load the solution in Visual Studio and run. 
