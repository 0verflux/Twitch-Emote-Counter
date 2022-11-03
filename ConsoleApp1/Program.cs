using System.Net;
using System.Net.Http.Headers;

// declare variables
var channel = args[0];
var username = args[1];
var year = int.Parse(args[2]);
var month = int.Parse(args[3]);

var logs = new List<string>();
var emoteOccurences = new Dictionary<string, int>();
var emoteFilePath = $@"{Environment.CurrentDirectory}\emotes.txt";

using var client = CreateHttpClientInstance();
var monthlyLogCount = DateTime.Now.Year - year + (DateTime.Now.Month - month);
var ctr = 0;

for (int i = year; i <= DateTime.Now.Year; i++)
{
    for (int j = month; j <= DateTime.Now.Month; j++)
    {
        Console.SetCursorPosition(0, 0);
        Console.Write(new string(' ', 512));
        Console.SetCursorPosition(0, 0);

        Console.Write($"Fetching logs of '{username}' in channel '{channel}' at {i}/{j} ({(float)ctr++ / monthlyLogCount:P})");
        
        // get logs at specified URL
        var response = await client.GetAsync($"https://logs.ivr.fi/channel/{channel}/user/{username}/{i}/{j}");

        // get next monthly log if response fails
        if (response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.TooManyRequests or HttpStatusCode.BadRequest or HttpStatusCode.InternalServerError)
            continue;

        // split all messages to words in current log
        logs.AddRange(response.Content.ReadAsStringAsync().Result.Split('\n', ' ', ',', '?', '.', '!', ';', ':'));
    }
}

// remove all empty or whitespace strings
logs.RemoveAll(s => string.IsNullOrWhiteSpace(s));

// read emotes text file
var emotes = File.ReadAllText(emoteFilePath).Split(' ').ToList();

Console.SetCursorPosition(0, 2);
Console.WriteLine("Counting all emote occurences ...");

// count all emote occurences in user logs
foreach (var emote in emotes)
    emoteOccurences[emote] = logs.Count(e => e == emote);

Console.SetCursorPosition(0, 4);
foreach(var kvp in emoteOccurences.Where(e => e.Value != 0).OrderByDescending(e => e.Value))
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");

static HttpClient CreateHttpClientInstance()
{
    var client = new HttpClient();

    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

    return client;
}