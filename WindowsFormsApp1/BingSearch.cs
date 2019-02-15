using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using Microsoft.Azure.CognitiveServices.Search.WebSearch.Models;

namespace WindowsFormsApp1
{
    class BingSearch
    {
        public static string WebResults(WebSearchClient client, string question)
        {
            try
            {
                var webData = client.Web.SearchAsync(query: question).Result;
                //Notícias
                if (webData?.News?.Value?.Count > 0)
                {
                    var firstNewsResult = webData.News.Value.FirstOrDefault();

                    if (firstNewsResult != null)
                    {
                        return firstNewsResult.Name;
                    }
                    else
                    {
                        return string.Format("'{0}' sem nenhum título encontrada", question);
                    }
                }
                //Cálculos
                else if (webData?.Computation?.Value != null)
                {
                    return webData.Computation.Value;
                }
                //Outras Buscas
                else if (webData?.WebPages?.Value?.Count > 0)
                {
                    var firstWebPageResult = webData.WebPages.Value.FirstOrDefault();

                    if (firstWebPageResult != null)
                    {
                        return firstWebPageResult.Name;
                    }
                    else
                    {
                        return string.Format("'{0}' sem nenhum título encontrada", question);
                    }
                }
                else
                {
                    return string.Format("'{0}' nenhuma informação encontrada", question);
                }
            }
            catch (Exception ex)
            {
                return string.Format("Falha ao localizar '{0}'", question);
            }
        }

        public static void WebSearchWithResponseFilter(WebSearchClient client)
        {
            try
            {
                IList<string> responseFilterstrings = new List<string>() { "news" };
                var webData = client.Web.SearchAsync(query: "Microsoft", responseFilter: responseFilterstrings).Result;
                Console.WriteLine("\r\nSearching for \" Microsoft \" with response filter \"news\"");

                if (webData?.News?.Value?.Count > 0)
                {
                    var firstNewsResult = webData.News.Value.FirstOrDefault();

                    if (firstNewsResult != null)
                    {
                        Console.WriteLine("News Results #{0}", webData.News.Value.Count);
                        Console.WriteLine("First news result name: {0} ", firstNewsResult.Name);
                        Console.WriteLine("First news result URL: {0} ", firstNewsResult.Url);
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find first News results!");
                    }
                }
                else
                {
                    Console.WriteLine("Didn't see any News data..");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Encountered exception. " + ex.Message);
            }
        }

        public static void WebSearchWithAnswerCountPromoteAndSafeSearch(WebSearchClient client)
        {
            try
            {
                IList<string> promoteAnswertypeStrings = new List<string>() { "videos" };
                var webData = client.Web.SearchAsync(query: "Music Videos", answerCount: 2, promote: promoteAnswertypeStrings, safeSearch: SafeSearch.Strict).Result;
                Console.WriteLine("\r\nSearching for \"Music Videos\"");

                if (webData?.Videos?.Value?.Count > 0)
                {
                    var firstVideosResult = webData.Videos.Value.FirstOrDefault();

                    if (firstVideosResult != null)
                    {
                        Console.WriteLine("Video Results # {0}", webData.Videos.Value.Count);
                        Console.WriteLine("First Video result name: {0} ", firstVideosResult.Name);
                        Console.WriteLine("First Video result URL: {0} ", firstVideosResult.ContentUrl);
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find videos results!");
                    }
                }
                else
                {
                    Console.WriteLine("Didn't see any data..");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encountered exception. " + ex.Message);
            }
        }

        public static void WebResultsWithCountAndOffset(WebSearchClient client)
        {
            try
            {
                var webData = client.Web.SearchAsync(query: "Best restaurants in Seattle", offset: 10, count: 20).Result;
                Console.WriteLine("\r\nSearching for \" Best restaurants in Seattle \"");

                if (webData?.WebPages?.Value?.Count > 0)
                {
                    var firstWebPagesResult = webData.WebPages.Value.FirstOrDefault();

                    if (firstWebPagesResult != null)
                    {
                        Console.WriteLine("Web Results #{0}", webData.WebPages.Value.Count);
                        Console.WriteLine("First web page name: {0} ", firstWebPagesResult.Name);
                        Console.WriteLine("First web page URL: {0} ", firstWebPagesResult.Url);
                    }
                    else
                    {
                        Console.WriteLine("Couldn't find first web result!");
                    }
                }
                else
                {
                    Console.WriteLine("Didn't see any Web data..");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Encountered exception. " + ex.Message);
            }
        }
    }
}
