using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
/*****************************************************************************************************************
                                        普源精电科技有限公司版权所有(2020-)
******************************************************************************************************************
类名:      CSincInter
功能描述： CSincInter类实现sinc插值
作 者：    顾泽滔
版 本：    00.01.00.00
修改历史： 
<作者> <修改时间> <版本> <修改描述>
*****************************************************************************************************************/
namespace ClassLibrary_WfmDataProcess
{
    public class CSincInter
    {
        #region Fields
        /*常量定义*/
        private const int sincFirOrder = 8;
        private const string INTER_X1 = "interp_X1.csv";
        private const string INTER_X2 = "interp_X2.csv";
        private const string INTER_X4 = "interp_X4.csv";
        private const string INTER_X5 = "interp_X5.csv";
        private const string INTER_X8 = "interp_X8.csv";
        private const string INTER_X10 = "interp_X10.csv";
        private const string INTER_X16 = "interp_X16.csv";
        private const string INTER_X20 = "interp_X20.csv";
        private const string INTER_X25 = "interp_X25.csv";
        private const string INTER_X32 = "interp_X32.csv";
        private const string INTER_X40 = "interp_X40.csv";
        private const string INTER_X50 = "interp_X50.csv";
        private const string INTER_X80 = "interp_X80.csv";
        private const string INTER_X100 = "interp_X100.csv";
        private const string INTER_X160 = "interp_X160.csv";
        private const string INTER_X200 = "interp_X200.csv";
        private const string INTER_X252 = "interp_X252.csv";
        /*管理插值系数文件的列表*/
        private List<string> strListFileName = new List<string>();
        public List<CSincFirData> cSincDataList = new List<CSincFirData>();

        /*内插倍数枚举*/
        public  enum  euInterMutipule
        {
            X1 = 1,
            X2,
            X4,
            X5,
            X8,
            X10,
            X16,
            X20,
            X25,
            X32,
            X40,
            X50,
            X80,
            X100,
            X160,
            X200,
            X252
        }
        /*保存滤波器系数的类*/
        public class CSincFirData
        {
            public euInterMutipule euMultiple;//插值倍数
            public int intMultiple;//整型插值倍数
            //public int interCoefMax;
            public int[] interCoefficient;//插值系数
        }
        #endregion

        #region Construction
        /// <summary>
        /// 构造函数
        /// </summary>
        public CSincInter()
        {
            /*插值系数文件添加到管理列表*/
            strListFileName.Add(INTER_X1);
            strListFileName.Add(INTER_X2);
            strListFileName.Add(INTER_X4);
            strListFileName.Add(INTER_X5);
            strListFileName.Add(INTER_X8);
            strListFileName.Add(INTER_X10);
            strListFileName.Add(INTER_X16);
            strListFileName.Add(INTER_X20);
            strListFileName.Add(INTER_X25);
            strListFileName.Add(INTER_X32);
            strListFileName.Add(INTER_X40);
            strListFileName.Add(INTER_X50);
            strListFileName.Add(INTER_X80);
            strListFileName.Add(INTER_X100);
            strListFileName.Add(INTER_X160);
            strListFileName.Add(INTER_X200);
            strListFileName.Add(INTER_X252);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 加载全部滤波器系数文件
        /// </summary>
        /// <returns>是否加载成功</returns>
        public bool CSincInter_LoadAll()
        {
            bool bResult = true;
            foreach (string item in strListFileName)
            {
                bool bReturn = false;
                bReturn = CSincInter_LoadFile(item);
                if (bReturn == false)
                {
                    bResult = false;
                }
            }
            return bResult;
        }
        /// <summary>
        /// 加载指定系数文件
        /// </summary>
        /// <param name="strPath">文件名</param>
        /// <returns>是否加载成功</returns>
        private  bool CSincInter_LoadFile(string strPath)
        {
            /*csv文件操作,读取滤波器系数*/
            bool bResult = true;
            string path = System.IO.Directory.GetCurrentDirectory();
            path += "\\DataFirCoeffcient\\" + strPath;
            FileStream csvFs = new FileStream(path, FileMode.Open);
            StreamReader csvSr = new StreamReader(csvFs, System.Text.Encoding.Default);
            string strLine = "";
            strLine = csvSr.ReadLine();
            if (strLine == null)
            {
                bResult = false;
                return bResult;
            }
            string[] items = strLine.Split(',');
            CSincFirData cSincTemp = new CSincFirData();
            switch (strPath)
            {
                case INTER_X1:
                    cSincTemp.euMultiple = euInterMutipule.X1;
                    cSincTemp.intMultiple = 1;
                    break;
                case INTER_X2:
                    cSincTemp.euMultiple = euInterMutipule.X2;
                    cSincTemp.intMultiple = 2;
                    break;
                case INTER_X4:
                    cSincTemp.euMultiple = euInterMutipule.X4;
                    cSincTemp.intMultiple = 4;
                    break;
                case INTER_X5:
                    cSincTemp.euMultiple = euInterMutipule.X5;
                    cSincTemp.intMultiple = 5;
                    break;
                case INTER_X8:
                    cSincTemp.euMultiple = euInterMutipule.X8;
                    cSincTemp.intMultiple = 8;
                    break;
                case INTER_X10:
                    cSincTemp.euMultiple = euInterMutipule.X10;
                    cSincTemp.intMultiple = 10;
                    break;
                case INTER_X16:
                    cSincTemp.euMultiple = euInterMutipule.X16;
                    cSincTemp.intMultiple = 16;
                    break;
                case INTER_X20:
                    cSincTemp.euMultiple = euInterMutipule.X20;
                    cSincTemp.intMultiple = 20;
                    break;
                case INTER_X25:
                    cSincTemp.euMultiple = euInterMutipule.X25;
                    cSincTemp.intMultiple = 25;
                    break;
                case INTER_X32:
                    cSincTemp.euMultiple = euInterMutipule.X32;
                    cSincTemp.intMultiple = 32;
                    break;
                case INTER_X40:
                   cSincTemp.euMultiple = euInterMutipule.X40;
                    cSincTemp.intMultiple = 40;
                    break;
                case INTER_X50:
                    cSincTemp.euMultiple = euInterMutipule.X50;
                    cSincTemp.intMultiple = 50;
                    break;
                case INTER_X80:
                    cSincTemp.euMultiple = euInterMutipule.X80;
                    cSincTemp.intMultiple = 80;
                    break;
                case INTER_X100:
                    cSincTemp.euMultiple = euInterMutipule.X100;
                    cSincTemp.intMultiple = 100;
                    break;
                case INTER_X160:
                    cSincTemp.euMultiple = euInterMutipule.X160;
                    cSincTemp.intMultiple = 160;
                    break;
                case INTER_X200:
                    cSincTemp.euMultiple = euInterMutipule.X200;
                    cSincTemp.intMultiple = 200;
                    break;
                case INTER_X252:
                    cSincTemp.euMultiple = euInterMutipule.X252;
                    cSincTemp.intMultiple = 252;
                    break;
                default:
                    break;
            }
            cSincTemp.interCoefficient = new int[sincFirOrder * cSincTemp.intMultiple];
            for (int i = 0; i < items.Length; i++)
            {
                cSincTemp.interCoefficient[i] = Convert.ToInt32(items[i]);
            }
            //cSincTemp.interCoefMax = cSincTemp.interCoefficient.Max();
            cSincDataList.Add(cSincTemp);
            //关闭流
            csvSr.Close();
            csvFs.Close();
            return bResult;
        }
        /// <summary>
        /// 卷积计算插值数据-多项式计算
        /// </summary>
        /// <param name="Xn">内插倍数</param>
        /// <param name="inData">输入数据</param>
        /// <param name="outData">输出数据</param>
        public double[] sincConvolut_Cal(euInterMutipule Xn, ref int[] inData)
        {
            int index = cSincDataList.FindIndex(item => (item.euMultiple == Xn));
            int intMultiple = cSincDataList[index].intMultiple;
            int lengthIn = inData.Length;
            /*插值后的数据量为（原始点数+滤波器阶数（窗大小）-1）*插值倍数
             * 对应卷积结果= L+M-1
             */
            double[] waveSincResult = new double[(lengthIn + sincFirOrder - 1) * cSincDataList[index].intMultiple];
            //最终输出的长度=原始数据长度+(内插倍数-1)*（原始数据长度 - 1）
            double[] outData = new double[lengthIn*intMultiple];
            int addVal = 0;
            int matrix_Row = intMultiple;
            int matrix_Colum = sincFirOrder;
            int[] tempFir = new int[sincFirOrder];
            int[,] sincFirGroup = new int[matrix_Row, matrix_Colum];
            int[,] resultAfterInter = new int[matrix_Row, lengthIn + sincFirOrder - 1];
            //先将一维的转换到二维
            for (int i = 0; i < matrix_Row; i++)
            {
                for (int j = 0; j < matrix_Colum; j++)
                {
                    sincFirGroup[i, j] = cSincDataList[index].interCoefficient[i + j * matrix_Row];
                }
            }
            //卷积计算
            try 
            {
                for (int row = 0; row < matrix_Row; row++)
                {
                    /*多项滤波器滤波*/
                    for (int i = 0; i < inData.Length + matrix_Colum - 1; i++)
                    {
                        /*移位计算窗口的系数*/
                        for (int j = 0; j < matrix_Colum - 1; j++)
                        {
                            tempFir[matrix_Colum - 1 - j] = tempFir[matrix_Colum - 2 - j];
                        }
                        if (i < inData.Length)
                        {
                            tempFir[0] = inData[i];
                        }
                        else
                        {
                            tempFir[0] = 0;
                        }
                        /*每个窗口的计算结果*/
                        for (int k = 0; k < matrix_Colum; k++)
                        {
                            addVal += tempFir[k] * sincFirGroup[row, k];
                        }
                        resultAfterInter[row, i] = addVal;
                        addVal = 0;
                    }
                }
            }
            catch(Exception e)
            {
            }
            /*转换为一维的数组*/
            for (int i = 0; i < inData.Length + matrix_Colum - 1; i++)
            {
                for (int j = 0; j < intMultiple; j++)
                {
                    waveSincResult[j + i * intMultiple] = (double)resultAfterInter[j, i] /32767.0 ;
                }
            }
            /*转化为实际插值的输出数据*/
            int offset = cSincDataList[index].interCoefficient.Length/2;//截取中间的数据
            for (int i = offset; i < offset + outData.Length; i++)
            {
                outData[i-offset] = waveSincResult[i];
            }
            return outData;
        }
        #endregion
    }
}
