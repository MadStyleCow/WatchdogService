using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogService.Enums
{
    public enum BE_MessageType
    {
        Unknown,
        Invalid,
        Connected,
        Unverified,
        Verified,
        Disconnected,
        Kick,
        Ban,
        Briefing,
        Whitelist,
        Players,
        Chat,
        Advice
    }
}
