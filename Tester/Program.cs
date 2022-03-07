// See https://aka.ms/new-console-template for more information


using System.Text.Json;
using SCVE.Editor.Editing.ProjectStructure;
var jsonContent = File.ReadAllText("testdata/tester.json");

var videoProject = JsonSerializer.Deserialize<VideoProject>(jsonContent, new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = true
});

int a = 5;