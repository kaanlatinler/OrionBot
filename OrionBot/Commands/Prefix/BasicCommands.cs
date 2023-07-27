using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using OpenAI_API;
using OrionBot.Engine.LevelSystem;

namespace OrionBot.Commands.Prefix
{
    internal class BasicCommands : BaseCommandModule
    {
        [Command("sa")]
        public async Task hiCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Cami mi lan burası As");
        }

        [Command("profil")]
        public async Task ProfileCommand(CommandContext ctx)
        {
            string username = ctx.User.Username;
            ulong guildID = ctx.Guild.Id;
            string avatarURL = ctx.User.AvatarUrl;

            var userDetails = new DUser()
            {
                UserName = ctx.User.Username,
                guildID = ctx.Guild.Id,
                avatarURL = ctx.User.AvatarUrl,
                Level = 1,
                XP = 0
            };

            var levelEngine = new LevelEngine();
            var doesExist = levelEngine.CheckedUserExists(username, guildID);

            if (doesExist == false)
            {
                var isStored = levelEngine.StoreUserDetails(userDetails);
                if (isStored == true)
                {
                    var succcessMessage = new DiscordEmbedBuilder()
                    {
                        Title = "Profil Kayıt Etme Başarılı",
                        Description = "Profilinizi tekrar görmek için lütfen !profil komutunu kullanın",
                        Color = DiscordColor.Green
                    };

                    await ctx.Channel.SendMessageAsync(embed: succcessMessage);
                    var pulledUser = levelEngine.GetUser(username, guildID);

                    var profile = new DiscordMessageBuilder()
                        .AddEmbed(new DiscordEmbedBuilder()
                        .WithColor(DiscordColor.MidnightBlue)
                        .WithTitle("Profilin Sahibi: " + pulledUser.UserName)
                        .WithThumbnail(pulledUser.avatarURL)
                        .AddField("Level", pulledUser.Level.ToString(), true)
                        .AddField("XP", pulledUser.XP.ToString(), true)
                        );

                    await ctx.Channel.SendMessageAsync(profile);
                }
                else
                {
                    var failedMessage = new DiscordEmbedBuilder()
                    {
                        Title = "Profil Kayıt Edilirken bir hata oldu",
                        Color = DiscordColor.Red
                    };

                    await ctx.Channel.SendMessageAsync(embed: failedMessage);
                }
            }

            else
            {
                var pulledUser = levelEngine.GetUser(username, guildID);

                var profile = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.MidnightBlue)
                    .WithTitle("Profilin Sahibi: " + pulledUser.UserName)
                    .WithThumbnail(pulledUser.avatarURL)
                    .AddField("Level", pulledUser.Level.ToString(), true)
                    .AddField("XP", pulledUser.XP.ToString(), true)
                    );

                await ctx.Channel.SendMessageAsync(profile);
            }
        }
    }
}