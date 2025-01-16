using HtmlAgilityPack;
using OpenAI.RealtimeConversation;
#pragma warning disable OPENAI002

public class IdaraWebTool : IFunctionTool { 
    public ConversationFunctionTool GetToolMetadata(){
        return new ConversationFunctionTool()
        {
            Name = "get_idara_info",
            Description = "Access information about Idara Jaferia - an islamic center in Maryland.",
            Parameters = BinaryData.FromString("""
            {
              "type": "object",
              "properties": {
                "infoType": {
                  "type": "string",
                  "enum": ["General","About","ResidentAlim"],
                  "description": "The type of information to retrieve. General: Use this for prayer times."
                }
              },
              "required": ["infoType"]
            }
            """)
        };
    }

    public string Invoke(Dictionary<string, object> parameters){
        string infoType = parameters["infoType"].ToString();
        InfoType type = (InfoType)Enum.Parse(typeof(InfoType), infoType);

        switch(type){
            case InfoType.General:
                return crawl_website("https://jaferia.org/");
            case InfoType.About:
                return crawl_website("https://jaferia.org/idara-e-jaferia/");
            case InfoType.ResidentAlim:
                return crawl_website("https://jaferia.org/resident-alim/");
            default:
                return "Invalid infoType. Please provide a valid infoType: General, About, or ResidentAlim.";
        }
    }

     public static string crawl_website(string url)
    {
        //hit the web url and get the html content
        HtmlWeb web = new HtmlWeb();
        HtmlDocument htmlDoc = web.Load(url);
        
        var nodes = htmlDoc.DocumentNode.SelectNodes("//script | //style");
        if (nodes != null)
        {
            foreach (var nd in nodes)
            {
                nd.Remove();
            }
        }

        List<string> txtNodes = new List<string>();
        HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
        TraverseDom(bodyNode, txtNodes);

        return String.Join("\n", txtNodes);
        
    }

    private static void TraverseDom(HtmlNode node, List<string> textNodes){
        if (node == null) return;

        // If the node is a text node, add it to the list
        if (node.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(node.InnerText))
        {
            textNodes.Add(node.InnerText);
        }

        // Recursively traverse child nodes
        foreach (var child in node.ChildNodes)
        {
            TraverseDom(child, textNodes);
        }
    }    
}

public enum InfoType{
    General,
    About,
    ResidentAlim
}