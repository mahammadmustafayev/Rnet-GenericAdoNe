

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace GenericAdo.Services;

public static class ConfigurationService
{
    public static void Configure()
    {
        using (var file =File.OpenText(Path.Combine(Environment.CurrentDirectory,"Properties", "launchSettings.json")))
        {
            var reader = new JsonTextReader(file);
            var jObject = JObject.Load(reader);

            var variables = jObject.GetValue("profiles")
                  .SelectMany(profiles=>profiles.Children())
                  .SelectMany(profile=>profile.Children<JProperty>())
                  .Where(prop=>prop.Name == "environmentVariables")
                  .SelectMany (prop=>prop.Children<JProperty>())
                  .ToList();
            foreach (var variable in variables) 
            {
               Environment.SetEnvironmentVariable(variable.Name,variable.Value.ToString());
               Console.WriteLine($"{variable.Name} -> {variable.Value}");
            }
        }
    }
}
