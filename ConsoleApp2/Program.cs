var channel = args[0];
var username = args[1];

using var client = new HttpClient();

client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/http"));
