﻿using Discord;
using Discord.Commands;

using System;
using System.Linq;
using System.IO;

using System.Text;
using System.Text.RegularExpressions;

using System.Threading;
using System.Threading.Tasks;

using System.Collections;
using System.Collections.Generic;

using Discord.WebSocket;

using Newtonsoft.Json;

namespace DiscordBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private ulong GuildID = 781613878407725077;

        [Command("obfuscate")]
        public async Task Obfuscate()
        {
            ulong UserID = Context.Message.Author.Id;

            SocketGuildUser User = Program.Client.GetGuild(GuildID).GetUser(UserID);

            bool License = false;
            bool ServerBoost = false;

            foreach (SocketRole Role in User.Roles) 
            { 
                if (Role.Name == "Buyer")
                { 
                    License = true; 
                }
                else if (Role.Name == "Server Booster")
                {
                    ServerBoost = true;
                };
            };

            if (File.Exists("freepremium.txt"))
                License = true;

            Program.UserDataTemplate UserData = Program.UserData[UserID];

            if ((!License) && (Program.UserData.ContainsKey(UserID)))
            {
                /*
                
                if (UserData.Obfuscations >= 3)
                {
                    if ((DateTime.Now.Minute - Program.UserData[UserID].ObfuscationTime) >= (60 * 24))
                    {
                        UserData.Obfuscations = 0;
                    }
                    else
                    {
                        var Embed = new EmbedBuilder();

                        Embed.WithAuthor((new EmbedAuthorBuilder { Name = "ALPHA Obfuscation" }));
                        Embed.WithFooter(F => F.Text = $"ALPHA Obfuscator {Program.VERSION}").WithColor(Color.DarkGrey).WithCurrentTimestamp();

                        Embed.WithTitle("Obfuscation Error");

                        Embed.WithDescription("You have already used the allowed 3 daily obfuscations without owning a premium license. Please upgrade to receive unlimited access to the obfuscator, along with many other restricted features: https://www.google.com/upgrade");

                        await ReplyAsync($"{User.Mention}, Thank you for using ALPHA Obfuscation!", (false), Embed.Build());

                        return;
                    };                 
                };

                */
            };

            Program.UserData[UserID] = UserData;

            async void CreateMessage()
            {
                var Embed = new EmbedBuilder();
                IUserMessage IUserMessage = null;
   
                Embed.WithAuthor((new EmbedAuthorBuilder { Name = "ALPHA Obfuscation" }));
                Embed.WithFooter(F => F.Text = $"ALPHA Obfuscator {Program.VERSION}").WithColor(Color.DarkGrey).WithCurrentTimestamp();

                if (License)
                {
                    Embed.WithDescription("Please upload a file or respond with formatted text to complete the process. **If you want this session to be private, please obfuscate in DMs!**");

                    Embed.WithTitle("Obfuscation Settings");

                    Embed.AddField(":regional_indicator_a: Encrypt All Strings", "Encrypts all strings in the file where they are defined. **This setting should NOT be used in large scripts and will cause a large performance loss and increase in obfuscation time.**");
                    Embed.AddField(":regional_indicator_b: Disable Super Operators", "Disables generation of Super Operators. This may fix some errors in scripts, but will also decrease performance.");
                    Embed.AddField(":regional_indicator_c: Maximum Security", "Uses more aggressive forms of security to prevent deobfuscation. This will GREATLY affect performance speed and is NOT reccommended on large scipts.");
                    Embed.AddField(":regional_indicator_d: Enhanced Output", "Generates the ULTIMATE output. Makes file size greatly larger but improves security.");
                    Embed.AddField(":regional_indicator_e: Premium Output", "Uses a differnt script structure than normal. Requires loadstring() or load(). This feature will also make PSU_MAX_SECURITY more secure against opcode hooking.");
                    Embed.AddField("Vanity Bytecode", "Replaces the normal Base36 formatted bytecode with a custom skin. Type the mode you would like to use with the file you upload. **Available Modes: Chinese, Arabic, Korean, Emoji, Greek, Symbols1, Symbols2, Symbols3, Default (Base36)**");

                    IUserMessage = await ReplyAsync($"{User.Mention}, Thank you for using ALPHA Obfuscation!", (false), Embed.Build());

                    if (Program.Processes.ContainsKey(UserID)) { Program.StopObfuscationProcess(UserID); };
                    Program.Processes[UserID] = (new Program.ObfuscationProcess { UserId = UserID, IUserMessage = IUserMessage, License = true });

                    async void AddReactions() { await IUserMessage.AddReactionAsync(Program.A); await IUserMessage.AddReactionAsync(Program.B); await IUserMessage.AddReactionAsync(Program.C); await IUserMessage.AddReactionAsync(Program.D); await IUserMessage.AddReactionAsync(Program.E); };
                    new Thread(AddReactions).Start();
                }
                else if (ServerBoost)
                {
                    Embed.WithDescription("Please upload a file or respond with formatted text to complete the process. **If you want this session to be private, please obfuscate in DMs!**");

                    Embed.WithTitle("Obfuscation Settings");

                    Embed.AddField(":regional_indicator_a: Encrypt All Strings", "Encrypts all strings in the file where they are defined. **This setting should NOT be used in large scripts and will cause a large performance loss and increase in obfuscation time.**");
                    Embed.AddField(":regional_indicator_b: Disable Super Operators", "Disables generation of Super Operators. This may fix some errors in scripts, but will also decrease performance.");

                    IUserMessage = await ReplyAsync($"{User.Mention}, Thank you for using ALPHA Obfuscation!", (false), Embed.Build());

                    if (Program.Processes.ContainsKey(UserID)) { Program.StopObfuscationProcess(UserID); };
                    Program.Processes[UserID] = (new Program.ObfuscationProcess { UserId = UserID, IUserMessage = IUserMessage, License = false, Boost = true });

                    async void AddReactions() { await IUserMessage.AddReactionAsync(Program.A); await IUserMessage.AddReactionAsync(Program.B); };
                    new Thread(AddReactions).Start();
                }
                else
                {
                    Embed.WithDescription("Please upload a file or respond with formatted text to complete the process. *If you want this session to be private, please obfuscate in DMs!*\n\n**You are using a non-licensed version of this product. Please upgrade to remove limitations and unlock all features: https://www.google.com/upgrade**");

                    IUserMessage = await ReplyAsync($"{User.Mention}, Thank you for using ALPHA Obfuscation! You have **∞** obfuscations remaining today!", (false), Embed.Build());

                    if (Program.Processes.ContainsKey(UserID)) { Program.StopObfuscationProcess(UserID); };
                    Program.Processes[UserID] = (new Program.ObfuscationProcess { UserId = UserID, IUserMessage = IUserMessage, License = false });
                };      
            };

            new Thread(CreateMessage).Start();
        }
    };
};
