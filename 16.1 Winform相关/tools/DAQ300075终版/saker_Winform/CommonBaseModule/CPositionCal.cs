using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saker_Winform.CommonBaseModule
{
    class CPositionCal
    {
        private double k = 0.0;
        private double b = 0.0;

        public List<stPoint> listPoint = new List<stPoint>();

        private int xStart;
        public int m_xStart
        {
            get { return xStart; }
            set { xStart = value; }
        }

        private int xStop;
        public int m_xStop
        {
            get { return xStop; }
            set { xStop = value; }
        }

        private int yStart;
        public int m_yStart
        {
            get { return yStart; }
            set { yStart = value; }
        }

        private int yStop;
        public int m_yStop
        {
            get { return yStop; }
            set { yStop = value; }
        }
        /// <summary>
        /// 定义坐标点
        /// </summary>
        public struct stPoint
        {
            public int point_X;
            public int point_Y;
        }
        /// <summary>
        /// 计算y=kx+b的系数
        /// </summary>
        private void calKandB()
        {
            k = (double)(yStop - yStart) / (xStop - xStart);
            b = yStop - k * xStop;
        }
        /// <summary>
        /// 计算两点之间插入的10个点
        /// </summary>
        public void calPointBetwXY(int numPoint)
        {
            listPoint.Clear();
            if (xStart == xStop)
            {
                if (yStop > yStart)
                {
                    double yPerPoint = (double)((yStop - yStart) / numPoint);
                    for (int i = 0; i < 10; i++)
                    {
                        stPoint stPointTemp = new stPoint();
                        stPointTemp.point_X = xStart;
                        stPointTemp.point_Y = (int)(yStop - i * yPerPoint);
                        listPoint.Add(stPointTemp);
                    }
                }
                else
                {
                    double yPerPoint = (double)((yStart - yStop) / numPoint);
                    for (int i = 0; i < 10; i++)
                    {
                        stPoint stPointTemp = new stPoint();
                        stPointTemp.point_X = xStart;
                        stPointTemp.point_Y = (int)(yStop + i * yPerPoint);
                        listPoint.Add(stPointTemp);
                    }
                }

            }
            else
            {
                /*先计算kb*/
                calKandB();
                if (xStop > xStart)
                {
                    double xPerPoint = (double)((xStop - xStart) / numPoint);
                    for (int i = 0; i < 10; i++)
                    {
                        stPoint stPointTemp = new stPoint();
                        stPointTemp.point_X = (int)(xStop - i * xPerPoint);
                        stPointTemp.point_Y = (int)(k * (xStop - i * xPerPoint) + b);
                        listPoint.Add(stPointTemp);
                    }
                }
                else
                {
                    double xPerPoint = (double)((xStart - xStop) / numPoint);
                    for (int i = 0; i < 10; i++)
                    {
                        stPoint stPointTemp = new stPoint();
                        stPointTemp.point_X = (int)(xStop + i * xPerPoint);
                        stPointTemp.point_Y = (int)(k * (xStop + i * xPerPoint) + b);
                        listPoint.Add(stPointTemp);
                    }
                }

            }
        }
    }
}
