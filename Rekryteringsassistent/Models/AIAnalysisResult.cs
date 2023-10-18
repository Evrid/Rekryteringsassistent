namespace Rekryteringsassistent.Models;

public class AIAnalysisResult
{
    public int Id { get; set; }
    public double? Rating { get; set; }

    public string? StringRating { get; set; }
    public string Strengths { get; set; }
    public string Weaknesses { get; set; }
    public int CandidateId { get; set; }
    public Candidate Candidate { get; set; }
}