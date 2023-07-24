using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionBot.External_Classes.Slash_Commands
{
    public class ModerationSL:ApplicationCommandModule
    {
        [SlashCommand("ban", "Bir kullanıcıyı serverdan banlar")]
        public async Task Ban(InteractionContext ctx, [Option("user", "Banlamak istediğiniz kullanıcı")] DiscordUser user,
                                                      [Option("reason", "Ban sebebi")] string reason=null)
        {
            await ctx.DeferAsync();

            if(ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {

                var member = (DiscordMember)user;
                await ctx.Guild.BanMemberAsync(member, 0, reason);

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " Banlandı",
                    Description = "Sebep: " + reason,
                    Color=DiscordColor.Red,
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(banMessage));
            }
            else
            {
                var nonAdminMessage = new DiscordEmbedBuilder()
                {
                    Title = "Erişim Engellendi",
                    Description = "Bu komutu kullanmak için admin olmanız gerekmekte.",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nonAdminMessage));
            }
        }
    }
}
