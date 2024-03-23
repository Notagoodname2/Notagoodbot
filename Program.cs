using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using System;
using System.Linq;
using System.Threading.Tasks;
using TestBot.commands;

namespace TestBot
{
    internal class Program
    {
        private static DiscordClient client { get; set; }
        private static CommandsNextExtension commands {  get; set; }
        static async Task Main(string[] args)
        {
            var JsonRead = new JsonRead();
            await JsonRead.ReadJson();
            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = JsonRead.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };
            client = new DiscordClient(discordConfig);
            client.Ready += Client_Ready;
            client.ComponentInteractionCreated += Client_ComponentInteractionCreated;
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { JsonRead.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,

            };
            commands = client.UseCommandsNext(commandsConfig);     
            commands.RegisterCommands<TestCommands>();
            Console.WriteLine("Bot Launched Sucessfully");
            await client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task Client_ComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            switch (args.Interaction.Data.CustomId)
            {
                case "verifybutton":
                    // Get the guild member who interacted with the button
                    var member = await args.Guild.GetMemberAsync(args.User.Id);

                    // Get the verified role
                    var verifiedRole = args.Guild.Roles.FirstOrDefault(kv => kv.Value.Name == "Verified").Value;

                    if (member != null && verifiedRole != null)
                    {
                        // Add the verified role to the member who clicked the button
                        await member.GrantRoleAsync(verifiedRole);

                        // Respond to the interaction with a message indicating success
                        var response = new DiscordInteractionResponseBuilder().WithContent("You have been verified!").AsEphemeral(true);
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                    }
                    else
                    {
                        // Respond to the interaction with a message indicating failure to find the role or member
                        var response = new DiscordInteractionResponseBuilder().WithContent("Unable To Verify").AsEphemeral(true);
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
                    }
                    break;
            }

        }

        private static async Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {

        }

    }
}
