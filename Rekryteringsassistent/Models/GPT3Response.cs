namespace Rekryteringsassistent.Models;

public class GPT3Response
{
    public string? id { get; set; }
    public string? object1 { get; set; }
    public string? created { get; set; }
    public string? model { get; set; }
    public Choice[]? choices { get; set; }
}

public class Choice
{
    public int index { get; set; }
    public Message? message { get; set; }
    public string? finish_reason { get; set; }
}

public class Message
{
    public string? role { get; set; }
    public string? content { get; set; }
}