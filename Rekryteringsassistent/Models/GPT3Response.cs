namespace Rekryteringsassistent.Models
{
    public class GPT3Response
    {
        public string id { get; set; }
        public string object1 { get; set; }
    public string created { get; set; }
    public string model { get; set; }
    public Choices[]? choices { get; set; }
}
}

