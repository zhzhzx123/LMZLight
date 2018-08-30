using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class BattleClient : UDPClientBase
    {
        public BattleClient()
        {
            StartServer(NetworkConstent.UDPBattleClientPort);
        }
    }

}
