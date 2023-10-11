﻿using Rekryteringsassistent.Models;
using System.Text.RegularExpressions;

namespace Rekryteringsassistent.Services
{
    public class GPT3ResponseProcessor
    {
        public AIAnalysisResult ProcessGPT3Response(string gpt3Response)
        {
            // Initialize variables to hold the extracted data
            string strengths = "";
            string weaknesses = "";
            int rating = 0;

            // Split the text into sentences
            var sentences = gpt3Response.Split('.').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

            // Loop through each sentence to identify strengths and weaknesses
            foreach (var sentence in sentences)
            {
                if (sentence.ToLower().Contains("exceptional") || sentence.ToLower().Contains("good") || sentence.ToLower().Contains("strong"))
                {
                    strengths += sentence + ". ";
                   // rating++;  // Increase rating for each strength
                }
                else if (sentence.ToLower().Contains("lack") || sentence.ToLower().Contains("weak"))
                {
                    weaknesses += sentence + ". ";
                 //   rating--;  // Decrease rating for each weakness
                }
                else if (sentence.ToLower().Contains("rating"))
                {
                    // Use Regex to extract the first integer value in the sentence
                    Match match = Regex.Match(sentence, @"\d+");
                    if (match.Success)
                    {
                        int extractedRating = int.Parse(match.Value);
                        rating = extractedRating;  // Set rating to the extracted value
                    }
                }
            }

            // Create and return the AIAnalysisResult object
            return new AIAnalysisResult
            {
                Rating = rating,
                Strengths = strengths,
                Weaknesses = weaknesses
            };
        }

    }
}
