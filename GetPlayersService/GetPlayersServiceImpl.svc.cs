using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace GetPlayersService
{
    public class GetPlayersServiceImpl : IGetPlayersService
    {
        public string GetPlayersXML()
        {
            return "<xml>Test</xml>";
        }

        public string GetPlayersJSON()
        {
            return "testjson";
        }
    }
}
