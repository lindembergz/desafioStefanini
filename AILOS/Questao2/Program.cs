using System;
using System.Text.Json;


/*
OBS: Apesar do time Paris Saint - Germain ter realizado 109 goals em 2013, o resultado da contagem de goals
na requisição para football_matches de fato não reflete com o resultado esperado 109, 
sendo o 62 goals de acordo com a API.

https://jsonmock.hackerrank.com/api/football_matches?team1=Paris%20Saint-Germain&year=2013

Página 1:
3 + 1 + 2 + 2 + 3 + 1 + 2 + 1 + 2 + 4 = 21 gols
Página 2:
4 + 3 + 4 + 5 + 2 + 5 + 2 + 3 + 2 + 2 = 32 gols
Página 3:
3 + 1 + 1 + 4 = 9 gols
Total de gols em 2013 = 21 + 32 + 9 = 62 gols

 */
class Program
{
    static async Task Main(string[] args)
    {
        var client = new HttpClient();

        Console.Write("Entre com o nome do 1º time: ");
        string? time1 = Console.ReadLine();
        Console.Write("Entre com o ano: ");
        int.TryParse(Console.ReadLine(), out int anoTime1 );

        Console.Write("Entre com o nome do 2º time: ");
        string? time2 = Console.ReadLine();

        Console.Write("Entre com o ano: ");
        int.TryParse(Console.ReadLine(), out int anoTime2);
   
        var result1 = await GetTeamGoalsAsync(client,  time1, anoTime1);
        Console.WriteLine(result1);

        var result2 = await GetTeamGoalsAsync(client,  time2, anoTime2);
        Console.WriteLine(result2);
    }

    static async Task<string> GetTeamGoalsAsync(HttpClient client, string teamName, int year)
    {
        if (!string.IsNullOrEmpty(teamName))
        {
            int totalGoals = 0;
            int currentPage = 1;

            var baseUrl = "https://jsonmock.hackerrank.com/api/football_matches";
            var queryParams = new List<string>();

            queryParams.Add($"team1={teamName}");
            if (year > 0)
            {
              queryParams.Add($"year={year}");
            }

            var url = $"{baseUrl}?{string.Join("&", queryParams)}";

            while (true)//requisitar todas as paginas
            {
                var response = await client.GetAsync($"{url}&page={currentPage}");

                response.EnsureSuccessStatusCode();//lança uma exception se nao retornar 200

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var rootObject = await JsonSerializer.DeserializeAsync<RootObject>(await response.Content.ReadAsStreamAsync(), options);

                foreach (var match in rootObject.Data)
                {
                    if (match.Team1 == teamName)
                        totalGoals += int.Parse(match.Team1goals);
                    if (match.Team2 == teamName)
                        totalGoals += int.Parse(match.Team2goals);
                }

                if (currentPage >= rootObject.Total_pages)// sai do while depois de ler a ultima pagina
                    break;

                currentPage++;//incrementa para a proxima pagina
            }

            return $"Team {teamName} scored {totalGoals} goals in {year}";
        }
        return "";
    }
}

class RootObject
{
    public int Page { get; set; }
    public int Per_page { get; set; }
    public int Total { get; set; }
    public int Total_pages { get; set; }
    public List<Match> Data { get; set; }
}

class Match
{
    public string Competition { get; set; }
    public int Year { get; set; }
    public string Round { get; set; }
    public string Team1 { get; set; }
    public string Team2 { get; set; }
    public string Team1goals { get; set; }
    public string Team2goals { get; set; }
}