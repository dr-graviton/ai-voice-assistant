## Implement weather tool

### Anatomy of a tool
1. Code - the core logic of the weather tool
2. Metadata - description of the tool and params
3. Tool Regisration
4. Tool Invocation

### Step 1: Implement tool
Download this [class](WeatherTool.cs) and include it in the project

### Step 2: Register the tool
Open FunctionToolAdapter.cs and include the weather tool in the *tools* variable.

### Step 3: Launch
Launch the app and ask about the weather

### Step 4: Hook up a live service
private static string get_weather_for_location(string location)
{
    return Task.Run(async () =>
    { 
        string url = $"";
        // post request with json {"location": "ashburn va"}
        var json = JsonSerializer.Serialize(new { location = location });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response: {responseBody}");
        return responseBody;
    }).Result;
}

## Next
[Integrate web data](Idara.md) 
