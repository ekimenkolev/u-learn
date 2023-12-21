// Вставьте сюда финальное содержимое файла LevenshteinCalculator.cs
using System;
using System.Configuration;
using System.Collections.Generic;
using DocumentTokens = System.Collections.Generic.List<string>;

namespace Antiplagiarism
{
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {
            var result = new List<ComparisonResult>();
            for (int i = 0; i < documents.Count; i++)
                for (int j = i + 1; j < documents.Count; j++)
                    result.Add(CompareDocuments(documents[i], documents[j]));
            return result;
        }
        
		ComparisonResult CompareDocuments(DocumentTokens first, DocumentTokens second)
        {
            double different;
            var opt = new double[first.Count + 1, second.Count + 1];

            for (int i = 0; i <= first.Count; i++) 
			{ 
				opt[i, 0] = i; 
			}
            for (int j = 0; j <= second.Count; j++) 
			{ 
				opt[0, j] = j; 
			}

            for (int i = 1; i <= first.Count; i++)
                for (int j = 1; j <= second.Count; j++)
                {
                    if(first[i - 1] == second[j - 1])
                    	different = 0;
                else
                    different = TokenDistanceCalculator.
					GetTokenDistance(first[i - 1], second[j - 1]);
                
                    opt[i, j] = Math.Min(Math.Min(opt[i - 1, j] + 1,
                    opt[i, j - 1] + 1),opt[i - 1, j - 1] + 
                    different);
                }
            return new ComparisonResult(first, second, opt[first.Count, second.Count]);
        }
    }
}

