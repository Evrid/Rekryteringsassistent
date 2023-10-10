using Microsoft.CodeAnalysis.Diagnostics;

namespace Rekryteringsassistent.Models
{
    public class Candidate
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public byte[]? CV_PDF { get; set; }
        public byte[]? CV_Word { get; set; }

        public string? CV_Text { get; set; }
        public AIAnalysisResult? AnalysisResult { get; set; }
    }

}
