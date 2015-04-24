using BattleNET;
using System;
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
                                        @"\((\w*)\) (.*): (.*)"};

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
                      
                        default:
                            return new BE_Message(BE_MessageType.Unknown, pInputString);
                    }
                }
            }
            return new BE_Message(BE_MessageType.Unknown, pInputString);
        }
    }
}
