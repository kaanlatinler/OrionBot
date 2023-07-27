using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrionBot.Commands.Slash_Commands
{
    public class ModerationSL : ApplicationCommandModule
    {
        [SlashCommand("ban", "Bir kullanıcıyı serverdan banlar")]
        public async Task Ban(InteractionContext ctx, [Option("user", "Banlamak istediğiniz kullanıcı")] DiscordUser user,
                                                      [Option("reason", "Ban sebebi")] string reason = null)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {

                var member = (DiscordMember)user;
                await ctx.Guild.BanMemberAsync(member, 0, reason);

                var banMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " Banlandı",
                    Description = "Sebep: " + reason,
                    Color = DiscordColor.Red,
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

        [SlashCommand("kick", "Kullanıcıyı serverdan kickler.")]
        public async Task Kick(InteractionContext ctx, [Option("user", "kicklemek istediğiniz kullanıcı")] DiscordUser user)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var member = (DiscordMember)user;
                await member.RemoveAsync();

                var kickMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " serverdan kicklendi.",
                    Description = ctx.User.Username + "tarafından kicklendi",
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(kickMessage));
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

        [SlashCommand("timeout", "bir kullanıcıya zaman aşımı uygular")]
        public async Task Timeout(InteractionContext ctx, [Option("user", "timeout atılacak kullanıcı")] DiscordUser user,
                                                          [Option("süre", "timeout süresi")] long duration)
        {
            await ctx.DeferAsync();

            if (ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                var timeDuration=DateTime.Now + TimeSpan.FromSeconds(duration);
                var member = (DiscordMember)user;
                await member.TimeoutAsync(timeDuration);

                var timeoutMessage = new DiscordEmbedBuilder()
                {
                    Title = member.Username + " zaman aşımına uğradı.",
                    Description = "Kalan süre: " + TimeSpan.FromSeconds(duration).ToString(),
                    Color = DiscordColor.Red
                };

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(timeoutMessage));
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
