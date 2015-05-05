using BattleNET;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WatchdogService.Enums;

namespace WatchdogService.Classes
{
    static class BE_Parser
    {
        /* Regular expressions */
        static String[] RegularExpressions = { @"Player #(\d*) (.*) \((.*):\d*\) connected",
                                        @"Player #(\d*) (.*) - GUID: (\w*) \(unverified\)",
                                        @"Verified GUID \((\w*)\) of player #(\d*) (.*)", 
                                        @"Player #(\d*) (.*) disconnected",
                                        @"Player #(\d*) (.*) \((.*)\) has been kicked by BattlEye: .*",
                                        @"\((\w*)\) (.*): @(.*)",
                                        @"\((\w*)\) (.*): (.*)",
                                        @"(\d)\s{3}(\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}):\d{4}\s{1,4}-?\d{1,4}\s{3,4}(\w+)\(.{1,2}\)\s{1,2}(.*) (?:\(Lobby\))"};

        public static BE_Message Parse(String pInputString)
        {
            for (int index = 0; index < RegularExpressions.Length; index++)
            {
                Regex regex = new Regex(RegularExpressions[index]);

                if (regex.IsMatch(pInputString))
                {
                    Match match = regex.Match(pInputString);

                    switch (index)
                    {
                        case 0:
                            return new BE_Message(BE_MessageType.Connected, new BE_Player() { Id = int.Parse(match.Groups[1].Value), Name = match.Groups[2].Value, IP = match.Groups[3].Value });

                        case 1:
                            return new BE_Message(BE_MessageType.Unverified, new BE_Player() { Id = int.Parse(match.Groups[1].Value), Name = match.Groups[2].Value, Guid = match.Groups[3].Value });

                        case 2:
                            return new BE_Message(BE_MessageType.Verified, new BE_Player() { Guid = match.Groups[1].Value, Id = int.Parse(match.Groups[2].Value), Name = match.Groups[3].Value });

                        case 3:
                        case 4:
                            return new BE_Message(BE_MessageType.Disconnected, new BE_Player() { Id = int.Parse(match.Groups[1].Value), Name = match.Groups[2].Value });

                        case 5:
                        case 6:
                            // TODO: To be implemented in the future;
                            break;

                        case 7:
                            var PlayerList = new List<BE_Player>();

                            int i = 1;
                            while(i < match.Groups.Count)
                            {
                                PlayerList.Add(new BE_Player()
                                    {
                                        Id = int.Parse(match.Groups[i++].Value),
                                        IP = match.Groups[i++].Value,
                                        Guid = match.Groups[i++].Value,
                                        Name = match.Groups[i++].Value
                                    });
                            }
                            return new BE_Message(BE_MessageType.Players, PlayerList);
                      
                        default:
                            return new BE_Message(BE_MessageType.Unknown, pInputString);
                    }
                }
            }
            return new BE_Message(BE_MessageType.Unknown, pInputString);
        }
    }
}
