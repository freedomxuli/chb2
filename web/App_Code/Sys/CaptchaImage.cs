using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.IO;
public class CaptchaImage
{
    #region 变量声明
    int m_nDisturbLine, m_nDisturbPoint;
    int m_strFontSize = 12;
    string m_strFont = "宋体";
    string m_strValidate = null;
    #endregion

    #region 构造函数
    /// <summary>
    /// 
    /// </summary>
    /// <param name="nDisturbLine">干扰线</param>
    /// <param name="nDisturbPoint">干扰点</param>
    public CaptchaImage(int nDisturbLine, int nDisturbPoint)
    {
        this.m_nDisturbLine = nDisturbLine;
        this.m_nDisturbPoint = nDisturbPoint;
    }
    #endregion

    #region  属性
    /// <summary>
    /// 获取验证码字符串 用以保存在Session
    /// </summary>
    public string strValidate
    {
        get { return this.m_strValidate; }
    }

    /// <summary>
    /// 设置或获取验证码字体
    /// </summary>
    public string strFont
    {
        get { return this.m_strFont; }
        set { this.m_strFont = value; }
    }

    /// <summary>
    /// 设置或获取验证码字体大小
    /// </summary>
    public int nFontSize
    {
        get { return this.m_strFontSize; }
        set { this.m_strFontSize = value; }
    }
    #endregion

    /// <summary>
    /// 获取字母数字验证码字符串
    /// </summary>
    /// <param name="nLength">验证码字符个数</param>
    /// <returns>验证码字符串</returns>
    private string GetStrAscii(int nLength)
    {
        int nStrLength = nLength;
        string strString = "1234567890";
        //string  strString   = "1234567890qwertyuioplkjhgfdsazxcvbnmASDFGHJKLMNBVCXZQWERTYUIOP";
        StringBuilder strtemp = new StringBuilder();
        Random random = new Random((int)DateTime.Now.Ticks);
        for (int i = 0; i < nStrLength; i++)
        {
            random = new Random(unchecked(random.Next() * 1000));
            strtemp.Append(strString[random.Next(10)]);
        }
        return strtemp.ToString();
    }


    /// <summary>
    /// 获取中文验证码字符串
    /// </summary>
    /// <param name="nLength">验证码字符中文的个数</param>
    /// <returns>验证码字符串(中文)</returns>
    private string GetStrChinese(int nLength)
    {
        int nStrLength = nLength;
        object[] btStrings = new object[nStrLength];
        string[] strString = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

        Random random = new Random();
        for (int i = 0; i < nStrLength; i++)
        {
            //区位码第1位   
            int nNext1 = random.Next(11, 14);
            string strChar1 = strString[nNext1];

            //区位码第2位   
            random = new Random(nNext1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的   

            int nNext2;
            if (nNext1 == 13)
            {
                nNext2 = random.Next(0, 7);
            }
            else
            {
                nNext2 = random.Next(0, 16);
            }
            string strChar2 = strString[nNext2];

            //区位码第3位   
            random = new Random(nNext2 * unchecked((int)DateTime.Now.Ticks) + i);
            int nNext3 = random.Next(10, 16);
            string strChar3 = strString[nNext3];

            //区位码第4位   
            random = new Random(nNext3 * unchecked((int)DateTime.Now.Ticks) + i);
            int nNext4;
            if (nNext3 == 10)
            {
                nNext4 = random.Next(1, 16);
            }
            else if (nNext3 == 15)
            {
                nNext4 = random.Next(0, 15);
            }
            else
            {
                nNext4 = random.Next(0, 16);
            }
            string strChar4 = strString[nNext4];

            byte bt1 = Convert.ToByte(strChar1 + strChar2, 16);
            byte bt2 = Convert.ToByte(strChar3 + strChar4, 16);
            byte[] btString = new byte[2] { bt1, bt2 };

            btStrings.SetValue(btString, i);
        }

        //转化成汉字
        StringBuilder strReturn = new StringBuilder();
        Encoding edGb2312 = Encoding.GetEncoding("gb2312");
        for (int i = 0; i < nStrLength; i++)
        {
            strReturn.Append(edGb2312.GetString((byte[])Convert.ChangeType(btStrings[i], typeof(byte[]))));
        }
        return strReturn.ToString();
    }

    /// <summary>
    /// 获取验证码图片
    /// </summary>
    /// <param name="nValidateLength">验证码字符个数</param>
    /// <param name="nImgWidth">图片宽度</param>
    /// <param name="nImgHeight">图片高度</param>
    /// <returns>流的图片</returns>
    public MemoryStream GetCode(int nType, int nValidateLength, int nImgWidth, int nImgHeight)
    {
        MemoryStream stream = null;

        if (nType == 0)    //是否启用汉字验证码
        {
            m_strValidate = this.GetStrChinese(nValidateLength);
        }
        else
        {
            m_strValidate = this.GetStrAscii(nValidateLength);
        }

        Bitmap image = new Bitmap(nImgWidth, nImgHeight);
        Graphics grImg = Graphics.FromImage(image);
        try
        {
            Random random = new Random();
            grImg.Clear(Color.White);//清空图片背景色

            //画图片的干扰线
            for (int i = 0; i < m_nDisturbLine; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                grImg.DrawLine(new Pen(Color.SkyBlue), x1, y1, x2, y2);
            }

            Font font = new Font(m_strFont, m_strFontSize, FontStyle.Bold);
            LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Black, 10.2f, false);
            grImg.DrawString(m_strValidate, font, brush, 3, 2);

            //画图片的前景干扰点
            for (int i = 0; i < m_nDisturbPoint; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            grImg.DrawRectangle(new Pen(Color.Olive), 0, 0, image.Width - 1, image.Height - 1);//画图片的边框线
            //保存图片数据
            stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
        }
        finally
        {
            grImg.Dispose();
            image.Dispose();
        }
        return stream;
    }
}
