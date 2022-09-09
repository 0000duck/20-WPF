using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;


/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      CGlobalCmd
功能描述： CGlobalCmd类中定义了saker中使用的SCPI命令集，命令的添加或者修改都在此类中进行
作 者：    sn02736
版 本：    00.01.00.00
完成日期： 
修改历史： 
<作者>               <修改时间>               <版本>                    <修改描述>
*****************************************************************************************************************/

namespace saker_Winform.Global
{
    public class CGlobalCmd
    {
        #region Commands
        public const string STR_CMD_INQUIRE = "*IDN?";// 查询
      
        #region 读取仪器参数
        public const string STR_CMD_GET_SERVERIP = ":ULTRalab:SERVer?";//获取取心跳IP
        public const string STR_CMD_GET_CHANNELS = ":WAVeform:CHANsend?";//获取通道数   
        public const string STR_CMD_GET_AUTOSEND = ":Wav:AutoSend?";
        public const string STR_CMD_GET_MAC = ":LAN:MAC?";//获取MAC地址
        public const string STR_CMD_GET_SCALE = ":CHANnel<n>:SCALe?";//获取垂直档位，后期调用使用replace      
        public const string STR_CMD_GET_OFFSET = ":CHANnel<n>:OFFSet? ";//获取偏移
        public const string STR_CMD_GET_IMPENDENCE = ":CHANnel<n>:IMPedance?";//获取阻抗
        public const string STR_CMD_GET_COUPLING = ":CHANnel<n>:COUPling? ";//获取耦合
        public const string STR_CMD_GET_PROBERATIO= ":CHANnel<n>:PROBe?";//获取探头比
        public const string STR_CMD_GET_TRIGGERSOURCE = ":TRIGger:EDGE:SOURce?"; //获取触发源
        public const string STR_CMD_GET_TRIGGERMODE = ":TRIGger:SWEep?";//获取触发模式
        public const string STR_CMD_GET_TRIGGERLEVEL = ":TRIGger:EDGE:LEVel? ";//获取触发电平
        public const string STR_CMD_GET_HORIZONTALTIMEBASE = ":TIMebase:SCALe?";//获取水平时基
        public const string STR_CMD_GET_HORIZONTAOFFSET = ":TIMebase:OFFSet?";//获取水平偏移
        public const string STR_CMD_GET_MEMDEPTH = ":ACQuire:MDEPth?";//获取存储深度       
        public const string STR_CMD_GET_HOLDOFF = ":TRIGger:HOLDoff?";//获取触发释抑  
        public const string STR_CMD_GET_WAVEPARAS = ":WAVeform:PREamble?";//获取通道参数
        public const string STR_CMD_GET_SAMPLERATE = ":ACQuire:SRATe?";//获取采样率
        public const string STR_CMD_GET_EXTDELAY = ":CALibration:EXTDelay?";//多机的通道延时
        public const string STR_CMD_GET_READMODE = ":WAV:MODE?";//设置内存模式读取
        public const string STR_CMD_GET_READTYPE = ":WAV:FORM?";//设置读取类型，字节
        public const string STR_CMD_GET_TRIGGERSTATUS = ":TRIGger:STATus?";//获取触发状态
        public const string STR_CMD_GET_CHAN_STATE = ":CHANnel<n>:DISPlay?";// 查询通道开关状态
        public const string STR_CMD_GET_CAL_STATUS = ":CALibration:STATus? chan1";//查询校准完成状态
        public const string STR_CMD_GET_DEV_CAL_RESULT = ":CALibration:EXTDelay?";//查询设备校准结果
        public const string STR_CMD_GET_HUFFMAN = ":Wav:Huffman?";//查询huffman使能状态
        public const string STR_CMD_GET_WAVEPARASALL = "Wav:source CHAN1;:WAVeform:PREamble?;Wav:source CHAN2;:WAVeform:PREamble?;Wav:source CHAN3;:WAVeform:PREamble?;Wav:source CHAN4;:WAVeform:PREamble?";


        #endregion
        #region 设置仪器参数      
        public const string STR_CMD_SET_RUN = "RUN";
        public const string STR_CMD_SET_SINGLE = "SINGle";
        public const string STR_CMD_SET_READMODE = ":WAV:MODE RAW";//设置内存模式读取
        public const string STR_CMD_SET_READTYPE = ":WAV:FORM BYTE";//设置读取类型，字节
        public const string STR_CMD_SET_OPENCHANNEL = ":CHANnel<n>:DISPlay ON";
        public const string STR_CMD_SET_CHANNELS = ":WAVeform:CHANsend ";//设置通道
        public const string STR_CMD_SET_AUTOSEND = "Wav:AutoSend ";//发送数据使能打开
        public const string STR_CMD_SET_REPEAT = "Wav:Repeat ";//数据重发命令
        public const string STR_CMD_SET_SERVERIP = ":ULTRalab:SERVer ";//设置心跳IP
        public const string STR_CMD_SET_SCALE = ":CHANnel<n>:SCALe ";//设置垂直档位       
        public const string STR_CMD_SET_OFFSET = ":CHANnel<n>:OFFSet ";//设置偏移
        public const string STR_CMD_SET_IMPENDENCE = ":CHANnel<n>:IMPedance ";//设置阻抗
        public const string STR_CMD_SET_COUPLING = ":CHANnel<n>:COUPling ";//设置耦合
        public const string STR_CMD_SET_PROBERATIO = ":CHANnel<n>:PROBe ";//设置探头比
        public const string STR_CMD_SET_TRIGGERSOURCE = ":TRIGger:EDGE:SOURce "; //设置触发源
        public const string STR_CMD_SET_TRIGGERMODE = ":TRIGger:SWEep ";//设置触发模式
        public const string STR_CMD_SET_TRIGGERLEVEL = ":TRIGger:EDGE:LEVel ";//设置触发电平
        public const string STR_CMD_SET_HORIZONTALTIMEBASE = ":TIMebase:SCALe ";//设置水平时基
        public const string STR_CMD_SET_HORIZONTAOFFSET = ":TIMebase:OFFSet ";//设置水平偏移
        public const string STR_CMD_SET_MEMDEPTH = ":ACQuire:MDEPth ";//设置存储深度
        public const string STR_CMD_SET_HOLDOFF = ":TRIGger:HOLDoff ";//设置触发释抑
        public const string STR_CMD_CAL_TRIG_DELAY = ":CALibration:CHDelay EXTCHAN";//校准外部触发延时
        public const string STR_CMD_CAL_CHAN_DELAY = ":CALibration:CHDelay chall";//校准通道延时
        public const string STR_CMD_SET_TRIG_EDGE_SLOP_POS = ":TRIGger:EDGE:SLOPe POSitive";//上升沿
        public const string STR_CMD_SET_TRIG_EDGE_SLOP_NEG = ":TRIGger:EDGE:SLOPe NEGative";//下降沿
        public const string STR_CMD_SET_TRIG_TFORce = ":TFORce";//触发命令
        public const string STR_CMD_SET_PORT = ":Wav:SendPort ";
        public const string STR_CMD_SET_STOP = ":STOP";
        public const string STR_CMD_SET_HUFFMAN = ":Wav:Huffman ";//打开huffman使能
        public const string STR_CMD_SET_CHANSOURCE = "wav:Source CHAN<n>";
        public const string STR_CMD_SET_TCALIBRATE = "CHANnel<n>:TCALibrate ";
        public const string STR_CMD_SET_KEY_PRESS_CLEAR = ":SYSTem:KEY:PRESs CLEar";
        #endregion
        #endregion
    }
}
