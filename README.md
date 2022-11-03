# Twitch-Emote-Counter
A console application written in C# that counts all emotes typed by a user in a specified twitch channel.

### How to Use
- enter all emotes to be scanned in `emotes.txt` separated with a **SPACE**
- Using CLI, type `dotnet run -- [channel-name] [username] [year] [month]`

| args | description |
| :---: | :---: |
|*channel-name* | twitch channel name |
| *username* | twitch username |
| *year* | initial year |
| *month* | initial month |

**NOTE**: program may not fetch all logs if too many requests were made (`Response Error Code 429`)
