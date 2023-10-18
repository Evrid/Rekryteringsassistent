using Newtonsoft.Json;

namespace Rekryteringsassistent.Models;

public class AnalysisInputModel
{
    //[JsonProperty(PropertyName = "Candidate")]
    public Candidate? Candidate { get; set; }

    //[JsonProperty(PropertyName = "Criteria")]
    public Criteria? Criteria { get; set; }

    //    public IFormFile? CV_File { get; set; }
}