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

namespace OrionBot.Commands
{
    internal class FunCommands : BaseCommandModule
    {
        [Command("sa")]
        public async Task hiCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Cami mi lan burası As");
        }

        [Command("anket")]

        public async Task PollCommand(CommandContext ctx, int TimeLimit, string Option1, string Option2, string Option3, string Option4, params string[] Question)
        {
            var interactvity = ctx.Client.GetInteractivity();
            TimeSpan timer = TimeSpan.FromSeconds(TimeLimit);
            DiscordEmoji[] optionEmojis = {DiscordEmoji.FromName(ctx.Client, ":one:", false),
                                           DiscordEmoji.FromName(ctx.Client, ":two:", false),
                                           DiscordEmoji.FromName(ctx.Client, ":three:", false),
                                           DiscordEmoji.FromName(ctx.Client, ":four:", false),};

            string optionsString = optionEmojis[0] + " | " + Option1 + "\n" +
                                   optionEmojis[1] + " | " + Option2 + "\n" +
                                   optionEmojis[2] + " | " + Option3 + "\n" +
                                   optionEmojis[3] + " | " + Option4;

            var pollMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.Azure)
                .WithTitle(string.Join(" ", Question))
                .WithDescription(optionsString)
                );

            var putReactOn = await ctx.Channel.SendMessageAsync(pollMessage);

            foreach (var emoji in optionEmojis)
            {
                await putReactOn.CreateReactionAsync(emoji);
            }

            var result = await interactvity.CollectReactionsAsync(putReactOn, timer);

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            foreach(var emoji in result)
            {
                if (emoji.Emoji == optionEmojis[0])
                {
                    count1++;
                }
                if (emoji.Emoji == optionEmojis[1])
                {
                    count2++;
                }
                if (emoji.Emoji == optionEmojis[2])
                {
                    count3++;
                }
                if (emoji.Emoji == optionEmojis[3])
                {
                    count4++;
                }
            }

            int totalVotes = count1+ count2 + count3 + count4;

            string resultsString = optionEmojis[0] + " | " + count1 + " Oylar \n" +
                                   optionEmojis[1] + " | " + count2 + " Oylar \n" +
                                   optionEmojis[2] + " | " + count3 + " Oylar \n" +
                                   optionEmojis[3] + " | " + count4 + " Oylar \n\n" +
                                   "Toplam Oy Sayısı = " + totalVotes;

            var resultsMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                
                .WithColor(DiscordColor.Green)
                .WithTitle("Sonuçlar")
                .WithDescription(resultsString)
                );

            await ctx.Channel.SendMessageAsync(resultsMessage);
        }
        [Command("yardım")]
        
        public async Task HelpButton(CommandContext ctx)
        {
            DiscordLinkButtonComponent button1 = new DiscordLinkButtonComponent("https://orionnbot.orionn.com.tr/index.html", "Yardım");


            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()

                .WithColor(DiscordColor.HotPink)
                .WithTitle("Yardıma mı ihtiyacınız var?")
                .WithDescription("Web sitemiz üzerinden istediğiniz tüm yardımı alabilirsiniz eğer yanıtlar yetersiz gelirse site üzerinden bana ulaşabilirsiniz.")
                )
                .AddComponents(button1);
                
            await ctx.Channel.SendMessageAsync(message);
        }
    }
}
