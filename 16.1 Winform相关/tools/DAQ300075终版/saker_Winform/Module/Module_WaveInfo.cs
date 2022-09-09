using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using saker_Winform.Global;

namespace saker_Winform.Module
{
    public class  Module_WaveInfo
    {
        //项目
        public string ProjectName { get; set; }
        //开始测试时间
       public string StartTime { get; set; }
        //采集时间
       public string RecordTime { get; set; }
        //测量类型
       public string MeasureType { get; set; }
        //通道ID
       public int ChannelID { get; set; }  
        //设备名称
       public string DeviceName { get; set; }      
        //通道标记
       public string Tag { get; set; }
        //通道波形数据
       public byte[] Data = new byte[1000008];
        //序列号
       public string SN { get; set; }
        //IP地址
       public string IP { get; set; }
        //通道间延时
       public int ChannelDelayTime { get; set; }
        //设备延时
       public int DeviceDelayTime { get; set; }
        //编号
       public int No { get; set; }
        //所在波表
       public string WaveTabelName { get; set; }
        //三个 x 三个 Y
       public string XIncrement { get; set; } // X 方向上相邻两点的时间差
       public string XOrigin { get; set; }//X方向上的起始时间
       public string XReference { get; set; } //X方向上数据的参考时间基准
       public string YIncrement { get; set; }  //Y方向上波形的步进值
       public string YOrigin { get; set; } //Y方向上相对于垂直参考位置的垂直偏移
       public string YReference { get; set; } //Y方向的垂直参考位置
       public int ChannelMode { get; set; } // 1，2，4通道模式
       public string SampRate { get; set; } // 采样率
       public string TrigTimeStamp { get; set; } // 触发时间戳
       public string MemDepth { get; set; }//存储深度
       public string Offset { get; set; }//偏移
    }
}
