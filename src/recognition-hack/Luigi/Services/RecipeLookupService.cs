using System;
using System.Collections.Generic;
using System.Linq;
using Luigi.Models;
using RestSharp;

namespace Luigi.Services
{
    public class RecipeLookupService
    {
        public static RecipeResponse Query(List<string> ingredients)
        {
            var csvQuery = String.Join(",", ingredients.Select(x => x.ToString()).ToArray());

            var restClient = new RestClient("http://www.recipepuppy.com");

            var request = new RestRequest("api", Method.GET);

            request.AddHeader("Accepts", "application/json;version=1");
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("i", csvQuery);

            IRestResponse<RecipeResponse> response = restClient.Execute<RecipeResponse>(request);
            return response.Data;
        }

    }
}
