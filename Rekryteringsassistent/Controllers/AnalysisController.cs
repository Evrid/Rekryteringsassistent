using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Rekryteringsassistent.Models;
using Rekryteringsassistent.Services;
using System.Diagnostics;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly AIAnalysisService _aiAnalysisService;
    private readonly ILogger<AnalysisController> _logger;

    public AnalysisController(AIAnalysisService aiAnalysisService, ILogger<AnalysisController> logger)
    {
        _aiAnalysisService = aiAnalysisService;
        _logger = logger;
    }

    

    [HttpGet]
    public ActionResult<Candidate> Get()
    {
        // Initialize Candidate and Criteria if you want
        Candidate candidate = new Candidate();
        Criteria criteria = new Criteria();
   //     AnalysisInputModel inputModel = new AnalysisInputModel();
        return Ok(new { Candidate = candidate, Criteria = criteria });
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromForm] AnalysisInputModel inputModel, [FromForm] IFormFile CV_File)

    {

        // Enable request body to be read multiple times
        Request.EnableBuffering();
        Debug.WriteLine("bbbbbbb");
        // Read the request body and log it
        string body;
        using (var reader = new StreamReader(
            Request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024, // Choose an appropriate size
            leaveOpen: true))
        {
            body = await reader.ReadToEndAsync();
            // Log the request body
            _logger.LogInformation("Request body: {body}", body);
            Debug.WriteLine(body);

            // Reset the request body stream position so the next middleware can read it
            Request.Body.Position = 0;
        }

        // Log inputModel
        _logger.LogInformation("Received inputModel: {InputModel}", inputModel);
        Debug.WriteLine("bbbbbbb");

        Debug.WriteLine(inputModel);
     
        Debug.WriteLine(inputModel?.Candidate);
        Debug.WriteLine(inputModel?.Criteria);
        Debug.WriteLine(JsonConvert.SerializeObject(inputModel?.Candidate));
        Debug.WriteLine(JsonConvert.SerializeObject(inputModel?.Criteria));

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        if (CV_File != null)
        {
            // Log CV_File details
            _logger.LogInformation("Received file with name: {FileName} and size: {Size}", CV_File.FileName, CV_File.Length);

            string fileExtension = Path.GetExtension(CV_File.FileName).ToLower();
            using var memoryStream = new MemoryStream();
            await CV_File.CopyToAsync(memoryStream);

            if (fileExtension == ".pdf")
            {
                inputModel.Candidate.CV_PDF = memoryStream.ToArray();
                inputModel.Candidate.ConvertPdfToText();
            }
            else if (fileExtension == ".doc" || fileExtension == ".docx")
            {
                inputModel.Candidate.CV_Word = memoryStream.ToArray();
                inputModel.Candidate.ConvertWordToText();
            }
            else
            {
                _logger.LogWarning("Invalid file format: {FileExtension}", fileExtension);
                return BadRequest("Invalid file format");
            }
        }
        else
        {
            _logger.LogWarning("CV_File is null");
        }

        Debug.WriteLine(inputModel.Candidate.CV_Text);

        var analysisResult = await _aiAnalysisService.AnalyzeCandidate(inputModel.Candidate, inputModel.Criteria);

        return Ok(analysisResult);
    }


    //[HttpPost]
    //public async Task<ActionResult> Post([FromForm] Candidate candidate, [FromForm] Criteria criteria, [FromForm] IFormFile CV_File)
    //{
    //    // Log candidate and criteria
    //    _logger.LogInformation("Received candidate: {Candidate}", candidate);
    //    _logger.LogInformation("Received criteria: {Criteria}", criteria);

    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    if (CV_File != null)
    //    {
    //        // Log CV_File details
    //        _logger.LogInformation("Received file with name: {FileName} and size: {Size}", CV_File.FileName, CV_File.Length);

    //        string fileExtension = Path.GetExtension(CV_File.FileName).ToLower();
    //        using var memoryStream = new MemoryStream();
    //        await CV_File.CopyToAsync(memoryStream);

    //        if (fileExtension == ".pdf")
    //        {
    //            candidate.CV_PDF = memoryStream.ToArray();
    //            candidate.ConvertPdfToText();
    //        }
    //        else if (fileExtension == ".doc" || fileExtension == ".docx")
    //        {
    //            candidate.CV_Word = memoryStream.ToArray();
    //            candidate.ConvertWordToText();
    //        }
    //        else
    //        {
    //            _logger.LogWarning("Invalid file format: {FileExtension}", fileExtension);
    //            return BadRequest("Invalid file format");
    //        }
    //    }
    //    else
    //    {
    //        _logger.LogWarning("CV_File is null");
    //    }

    //    Console.WriteLine(candidate.CV_Text);

    //    var analysisResult = await _aiAnalysisService.AnalyzeCandidate(candidate, criteria);

    //    return Ok(analysisResult);
    //}

}
