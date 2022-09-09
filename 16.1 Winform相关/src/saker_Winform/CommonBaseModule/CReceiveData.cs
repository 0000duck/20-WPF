using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_SocketServer;

namespace saker_Winform.CommonBaseModule
{
    public class CReceiveData
    {
        public AsyncUserToken Token { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public DateTime StartTime { get; set; }
        public string DeviceGUID { get; set; }
        public Module.Module_Channel Channel { get; set; }
        public string WaveTableName { get; set; }       
        public CReceiveData(AsyncUserToken token, int offset ,int length,string deviceGUID,Module.Module_Channel channel,string waveTableName,DateTime startTime)
        {
            this.Token = token;
            this.Offset = offset;
            this.Length = length;
            this.DeviceGUID = deviceGUID;
            this.Channel = channel;
            this.WaveTableName = waveTableName;
            this.StartTime = startTime;
        }
        
    }
   
}
