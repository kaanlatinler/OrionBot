using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Google.Apis.Services;
using OpenAI_API;
using System.Linq;
using System.Threading.Tasks;

namespace OrionBot.Commands.Prefix
{
    public class UserRequestedCommands : BaseCommandModule
    {
        [Command("gpt")]
        public async Task ChatGPT(CommandContext ctx, params string[] message)
        {

            var api = new OpenAIAPI("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");

            var chat = api.Chat.CreateConversation();
            chat.AppendSystemMessage("soru yaz");

            chat.AppendUserInput(string.Join(" ", message));

            string response = await chat.GetResponseFromChatbotAsync();

            var responseMsg = new DiscordEmbedBuilder()
            {
                Title = string.Join(" ", message),
                Description = response,
                Color = DiscordColor.Green
            };
            await ctx.Channel.SendMessageAsync(embed: responseMsg);
        }
    }
}
