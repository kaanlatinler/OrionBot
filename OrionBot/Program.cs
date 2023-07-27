using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using OrionBot.Commands.Prefix;
using OrionBot.Commands.Slash_Commands;
using OrionBot.Engine.LevelSystem;
using OrionBot.YouTube_API;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ExecutionEngineException = OrionBot.YouTube_API.ExecutionEngineException;

namespace OrionBot
{
    public sealed class Program
    {
        public static DiscordClient Client { get; private set; }

        public static InteractivityExtension Interactivity { get; private set; }

        public static CommandsNextExtension Commands { get; private set; }

        private static YouTubeVideo _video = new YouTubeVideo();
        private static YouTubeVideo temp = new YouTubeVideo();

        private static ExecutionEngineException _YouTubeEngine = new ExecutionEngineException();

        static async Task Main(string[] args)
        {
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            var configJson = JsonConvert.DeserializeObject<ConfigJSON>(json);

            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            Client = new DiscordClient(config);
            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false

            };
            commandsConfig.StringPrefixes = new[] { "!" };


            Commands = Client.UseCommandsNext(commandsConfig);
            var slashCommandsConfig = Client.UseSlashCommands();

            Commands.RegisterCommands<BasicCommands>();
            Commands.RegisterCommands<UserRequestedCommands>();

            slashCommandsConfig.RegisterCommands<FunSL>();
            slashCommandsConfig.RegisterCommands<ModerationSL>();

            await Client.ConnectAsync();

            ulong channelIdToNotify = 1097958435443703900;
            await StartVideoUploadNotifier(_YouTubeEngine.channelId, _YouTubeEngine.apiKey, Client, channelIdToNotify);
            await Task.Delay(-1);
        }
        private static Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
        private static async Task MessageSendHandler(DiscordClient sender, MessageCreateEventArgs e)
        {
            var levelEngine = new LevelEngine();
            var addedXP = levelEngine.AddXP(e.Author.Username, e.Guild.Id);

            if (levelEngine.levelledUp == true)
            {
                var levelledUpEmbed = new DiscordEmbedBuilder()
                {
                    Title = e.Author.Username + " Level Atladı!!!",
                    Description = "Level: " + levelEngine.GetUser(e.Author.Username, e.Guild.Id).Level.ToString(),
                    Color = DiscordColor.Chartreuse
                };

                await e.Channel.SendMessageAsync(e.Author.Mention, embed: levelledUpEmbed);
            }
        }

        private static async Task StartVideoUploadNotifier(string channelId, string apiKey, DiscordClient client, ulong channelIdNotify)
        {
            var timer = new Timer(30000);//30 saniye
            timer.Elapsed += async (sender, e) =>
            {
                _video = _YouTubeEngine.GetLatestVideo();
                var lastCheckedAt = DateTime.Now;


                if (_video != null)
                {
                    if (temp.videoTitle == _video.videoTitle)
                    {
                        Console.WriteLine("Aynı Video Tespit Edildi");
                    }
                    else if (_video.PublishedAt < lastCheckedAt)
                    {
                        var message = $"YENİ VİDEO: | ***{_video.videoTitle}*** \n" +
                                     $"Yayınlanma Zamanı: {_video.PublishedAt} \n" +
                                     "URL: " + _video.videoUrl;

                        await client.GetChannelAsync(channelIdNotify).Result.SendMessageAsync(message);
                        temp = _video;
                    }
                }
            };

            timer.Start();
        }
    }
}
