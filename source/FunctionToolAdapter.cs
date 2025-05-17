using OpenAI.RealtimeConversation;

#pragma warning disable OPENAI002

public class FunctionToolAdapter {
    private IFunctionTool[] tools = new IFunctionTool[] {
    };

    public void Register(ConversationSessionOptions options)
    {
        foreach (IFunctionTool tool in tools)
        {
            ConversationFunctionTool metadata = tool.GetToolMetadata();
            options.Tools.Add(metadata);
        }

        options.Tools.Add(
            // We'll add a simple function tool that enables the model to interpret user input to figure out when it
            // might be a good time to stop the interaction.
            new ConversationFunctionTool()
            {
                Name = "user_wants_to_finish_conversation",
                Description = "Invoked when the user says goodbye, expresses being finished, or otherwise seems to want to stop the interaction.",
                Parameters = BinaryData.FromString("{}")
            });
    }

    public ConversationItem Trigger(string id, string toolName, string paramsJson) {
        foreach (IFunctionTool tool in tools) {
            ConversationFunctionTool metadata = tool.GetToolMetadata();
            if (metadata.Name == toolName) {
                Dictionary<string, object> parameters = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(paramsJson);
                string result = tool.Invoke(parameters);
                Console.WriteLine(result);
                
                return ConversationItem.CreateFunctionCallOutput( callId: id, output: result );
            }
        }
     
        Console.WriteLine($"Tool not found: {toolName}");
        return null;
    }
}