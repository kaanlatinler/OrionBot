using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using OrionBot.Engine.External_Classes;
using System;
using System.Threading.Tasks;
namespace OrionBot.Commands.Slash_Commands
{
    public class FunSL : ApplicationCommandModule
    {
        [SlashCommand("anket2", "Bir Anket Başlat")]
        public async Task pollCommand(InteractionContext ctx, [Option("soru", "Sorunun Konusu")] string Question,
                                                              [Option("zamanlimiti", "Anketin Suresi")] long TimeLimit,
                                                              [Option("option1", "Option 1")] string Option1,
                                                              [Option("option2", "Option 1")] string Option2,
                                                              [Option("option3", "Option 1")] string Option3,
                                                              [Option("option4", "Option 1")] string Option4)
        {

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                                            .WithContent("Komut Kullanıldı"));

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

            foreach (var emoji in result)
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

            int totalVotes = count1 + count2 + count3 + count4;

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

        [SlashCommand("kartcek2", "Botla karşılıklı oynadığınız bir kart oyunu.")]
        public async Task cardCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                                            .WithContent("Kartlar Çekildi!!"));

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

        [SlashCommand("yardım2", "Özellikler Komutlar")]
        public async Task HelpCommand(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                                            .WithContent("Yardım Ediliyor!"));

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

