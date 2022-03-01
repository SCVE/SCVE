// See https://aka.ms/new-console-template for more information


using System.Text.Json;
using System.Text.Json.Serialization;
using Tester.ProjectStructure;

var videoProject = JsonSerializer.Deserialize<VideoProject>(
@"
{
    ""sequences"": [
        {
            ""guid"": ""a665087f-a4c0-4acc-9dc3-e3f0a6287d33"",
            ""name"": ""asset.name"",
            ""location"": ""/""
        }
    ],
    ""images"": [        
    ] 
}
", new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true
});

int a = 5;