using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using static System.Console;

var channel = args[0];
var username = args[1];
var year = int.Parse(args[2]);
var month = int.Parse(args[3]);

var logs = new List<string>();
var emoteOccurences = new Dictionary<string, int>();
var emoteFilePath = $@"{Environment.CurrentDirectory}\emotes.txt";

using var client = CreateHttpClientInstance();

for (int i = year; i <= DateTime.Now.Year; i++)
{
    for (int j = month; j <= DateTime.Now.Month; j++)
    {
        Write($"Fetching logs of '{username}' in channel '{channel}' at {CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(j)}-{i} ... ");
        
        var response = await client.GetAsync($"https://logs.ivr.fi/channel/{channel}/user/{username}/{i}/{j}");

        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.TooManyRequests or HttpStatusCode.BadRequest or HttpStatusCode.InternalServerError)
        {
            WriteLine("No logs found.");
            continue;
        }

        var content = response.Content.ReadAsStringAsync().Result.Split('\n');

        logs.AddRange(content.SelectMany(e => e.Split(' ', ',', '?', '.', '!', ';', ':')));

        WriteLine($"{content.Length} messages found!");
    }
}

logs.RemoveAll(s => string.IsNullOrWhiteSpace(s));

var emotes = File.ReadAllText(emoteFilePath).Split(' ').ToList();

WriteLine("Counting all emote occurences ...");

foreach (var emote in emotes)
    emoteOccurences[emote] = logs.Count(e => e == emote);

WriteLine("Emote Occurences: ");

foreach(var kvp in emoteOccurences.Where(e => e.Value != 0).OrderByDescending(e => e.Value))
    WriteLine($" > {kvp.Key}: {kvp.Value}");

static HttpClient CreateHttpClientInstance()
{
    var client = new HttpClient();

    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

    return client;
}