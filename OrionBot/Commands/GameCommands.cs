using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using OrionBot.External_Classes;

namespace OrionBot.Commands
{
    public class GameCommands : BaseCommandModule
    {
        [Command("kartçek")]
        public async Task SimpleCardGame(CommandContext ctx)
        {
            var userCard = new CardBuilder();
            var botCard = new CardBuilder();

            var UserCard = new CardBuilder();

            var userCardMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Azure)
                    .WithTitle("Senin Kartın")
                    .WithDescription("Bir " + UserCard.SelectedCard + " Çektin")
                );

            await ctx.Channel.SendMessageAsync(userCardMessage);

            var BotCard = new CardBuilder();

            var botCardMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                    .WithColor(DiscordColor.Azure)
                    .WithTitle("Benim Kartım")
                    .WithDescription("Bir " + BotCard.SelectedCard + " Çektim")
                );

            await ctx.Channel.SendMessageAsync(botCardMessage);

            if (UserCard.SelectedNumber > BotCard.SelectedNumber)
            {
                var winnerMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithColor(DiscordColor.Green)
                        .WithTitle("Kazandınız Clap Clap Clap!!! ")
                    );

                await ctx.Channel.SendMessageAsync(winnerMessage);
            }
            else if (UserCard.SelectedNumber < BotCard.SelectedNumber)
            {
                var loserMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithColor(DiscordColor.Red)
                        .WithTitle("Ahahahaah Ezik Kaybettin sjsjjsjssjsjs")
                    );

                await ctx.Channel.SendMessageAsync(loserMessage);
            }
            else
            {
                var equalMessage = new DiscordMessageBuilder()
                    .AddEmbed(new DiscordEmbedBuilder()
                        .WithColor(DiscordColor.Purple)
                        .WithTitle("Şansına Sokim :D")
                    );

                await ctx.Channel.SendMessageAsync(equalMessage);
            }
        }

    }
}
