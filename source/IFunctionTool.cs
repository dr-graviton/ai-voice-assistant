using OpenAI.RealtimeConversation;
#pragma warning disable OPENAI002

public interface IFunctionTool{
    public ConversationFunctionTool GetToolMetadata();
    public string Invoke(Dictionary<string, object> parameters);
}