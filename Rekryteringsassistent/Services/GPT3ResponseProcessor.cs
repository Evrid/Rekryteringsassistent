using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using Rekryteringsassistent.Models;
using System.Buffers.Text;
using System.Security.Policy;
using System.Text.RegularExpressions;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rekryteringsassistent.Services;

public class GPT3ResponseProcessor
{
    public AIAnalysisResult ProcessGPT3Response(string gpt3Response)
    {
        // Initialize variables to hold the extracted data
        string strengths = "";
        string weaknesses = "";
        string stringrating = "";
        int rating = 0;

        //Good response:
        //Based on the provided CV, the candidate, named asd asd, seems to have some relevant qualifications for the position. The candidate states that they are good at Java, which is a required skill for this position.Additionally, they mention having one year of experience, meeting the minimum experience requirement.\n\nStrengths:\n - Proficiency in Java: The candidate's expertise in Java is certainly a strength as it aligns with the required skill set for the position. This proficiency indicates that the candidate is likely to be able to handle Java-related tasks efficiently.\n\nWeaknesses:\n- Lack of detail in experience: The candidate's CV does not provide any additional information about their experience apart from the one year mentioned.It would have been beneficial to have more details regarding the type of projects they have worked on or the specific tasks they have undertaken during that period.\n\nCandidate rating: 7 / 10\nBased on the given information, the candidate seems to possess the required skill of Java and has the minimum required experience.However, the lack of detail in their experience is a shortcoming that might affect their overall suitability for the position.

        // Split the text into sentences
        var sentences = gpt3Response.Split('.').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

        // Loop through each sentence to identify strengths and weaknesses
        foreach (var sentence in sentences)
        {
            if (sentence.ToLower().Contains("strengths") || sentence.ToLower().Contains("strength") || sentence.ToLower().Contains("strong"))
            {
                strengths += sentence + ". ";
                // rating++;  // Increase rating for each strength
            }
            else if (sentence.ToLower().Contains("weaknesses") || sentence.ToLower().Contains("weakness"))
            {
                weaknesses += sentence + ". ";
                //   rating--;  // Decrease rating for each weakness
            }
            //else if (sentence.ToLower().Contains("rating"))
            //{
            //    // Use Regex to extract the first integer value in the sentence
            //    Match match = Regex.Match(sentence, @"\d+");
            //    if (match.Success)
            //    {
            //        int extractedRating = int.Parse(match.Value);
            //        rating = extractedRating;  // Set rating to the extracted value
            //    }
            //}



        }
        foreach (var sentence in sentences)
        {
            if (sentence.ToLower().Contains("rating"))
            {
                stringrating += sentence + ". ";
            }
        }

        // Create and return the AIAnalysisResult object
        return new AIAnalysisResult
        {
            Rating = rating,
            Strengths = strengths,
            Weaknesses = weaknesses,
            StringRating= stringrating
        };
    }

}