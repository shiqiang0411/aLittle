using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CommonTools
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 获取自定义宽度的一个背景图片（模拟进度条）
        /// </summary>
        /// <param name="maxWidth">背景宽度</param>
        /// <param name="maxHeight">背景高度</param>
        /// <param name="isShade">是否渐变</param>
        /// <param name="width">自定义的宽度</param>
        /// <returns></returns>
        public static Image GetAssignWidthImage(int maxWidth, int maxHeight, bool isShade, int width)
        {
            Bitmap bmp = new Bitmap(maxWidth, maxHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.SkyBlue);
            if (isShade)
            {
                //颜色渐变
                g.FillRectangle(new LinearGradientBrush(new Point(maxHeight, 2), new Point(maxHeight, maxHeight), Color.Green, Color.GreenYellow), 2, 2, width, maxHeight - 4);
            }
            else
            {
                //普通
                g.FillRectangle(Brushes.Green, 2, 2, width, maxHeight - 4);
            }
            return bmp;
        }
    }
}
