using OpenAI.RealtimeConversation;
#pragma warning disable OPENAI002

public class WeatherTool : IFunctionTool
{
    public ConversationFunctionTool GetToolMetadata()
    {
        return new ConversationFunctionTool()
        {
            Name = "get_weather_for_location",
            Description = "gets the weather for a location",
            Parameters = BinaryData.FromString("""
            {
              "type": "object",
              "properties": {
                "location": {
                  "type": "string",
                  "description": "The city and state, e.g. San Francisco, CA"
                },
                "unit": {
                  "type": "string",
                  "enum": ["c","f"]
                },
                "date": {
                  "type": "string",
                  "description": "The date in the format YYYY-MM-DD"
                }
              },
              "required": ["location","unit"]
            }
            """)
        };
    }

    public string Invoke(Dictionary<string, object> parameters)
    {
        string location = parameters["location"].ToString();
        string unit = parameters["unit"].ToString();

        return get_weather_for_location(location, unit);
    }

    public static string get_weather_for_location(string location)
    {
        Console.WriteLine($" <<< get_weather_for_location: location={location}");
        return $"The weather in {location} is 72°F and sunny.";
    }
    
}