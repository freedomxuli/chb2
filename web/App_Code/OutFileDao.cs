using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using Aspose.Cells;

/// <summary>
///OutFileDao 的摘要说明
/// </summary>
public class OutFileDao
{
    public OutFileDao()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 测试程序
    /// </summary>
    public static void testOut()
    {

        DataTable dt = new DataTable();
        dt.Columns.Add("下单时间");
        dt.Columns.Add("商品名称");
        dt.Columns.Add("商品规格");
        dt.Columns.Add("商品单位");
        dt.Columns.Add("产地");
        dt.Columns.Add("订货数量");
        dt.Columns.Add("配送数量");
        dt.Columns.Add("收货数量");
        DataRow dr = dt.NewRow();
        dr["下单时间"] = "2011-11-20 15:35:34";
        dr["商品名称"] = "花生油";
        dr["商品规格"] = "500g";
        dr["商品单位"] = "瓶";
        dr["产地"] = "地产";
        dr["订货数量"] = "200";
        dr["配送数量"] = "200";
        dr["收货数量"] = "200";
        dt.Rows.Add(dr);

        OutFileToDisk(dt, "测试标题", @"d:\测试.xls");
    }

    /// <summary>
    /// 导出数据到本地
    /// </summary>
    /// <param name="dt">要导出的数据</param>
    /// <param name="tableName">表格标题</param>
    /// <param name="path">保存路径</param>
    public static void OutFileToDisk(DataTable dt, string tableName, string path)
    {


        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 18;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 14;//文字大小
        style2.Font.IsBold = true;//粗体
        style2.IsTextWrapped = true;//单元格内容自动换行
        style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 12;//文字大小
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue(tableName);//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        //生成行2 列名行
        for (int i = 0; i < Colnum; i++)
        {
            cells[1, i].PutValue(dt.Columns[i].ColumnName);
            cells[1, i].SetStyle(style2);
            cells.SetRowHeight(1, 25);
        }

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[2 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[2 + i, k].SetStyle(style3);
            }
            cells.SetRowHeight(2 + i, 24);
        }

        workbook.Save(path);
    }


    public static MemoryStream OutFileToStream(DataTable dt, string tableName)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 18;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 14;//文字大小
        style2.Font.IsBold = true;//粗体
        style2.IsTextWrapped = true;//单元格内容自动换行
        style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 12;//文字大小
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue(tableName);//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        //生成行2 列名行
        for (int i = 0; i < Colnum; i++)
        {
            cells[1, i].PutValue(dt.Columns[i].ColumnName);
            cells[1, i].SetStyle(style2);
            cells.SetRowHeight(1, 25);
        }

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[2 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[2 + i, k].SetStyle(style3);
            }
            cells.SetRowHeight(2 + i, 24);
        }

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamSpjgbd(DataTable dt, string tableName, string rq)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 26;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Left;//文字靠左
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 12;//文字大小
        style2.Font.IsBold = true;//粗体

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 10;//文字大小
        style3.Font.IsBold = true;//粗体
        style3.IsTextWrapped = true;//单元格内容自动换行
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式4
        Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style4.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style4.Font.Name = "宋体";//文字字体
        style4.Font.Size = 11;//文字大小
        style4.IsTextWrapped = true;//单元格内容自动换行
        style4.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;


        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue(tableName);//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        cells.Merge(1, 0, 1, Colnum);
        cells[1, 0].PutValue(rq);
        cells[1, 0].SetStyle(style2);
        cells.SetRowHeight(1, 36);

        //生成行2 列名行
        cells.SetRowHeight(2, 80);
        for (int i = 0; i < Colnum; i++)
        {
            cells[2, i].PutValue(dt.Columns[i].Caption);
            cells[2, i].SetStyle(style3);
            cells.SetColumnWidth(i, 8);
        }

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[3 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[3 + i, k].SetStyle(style4);
            }
            cells.SetRowHeight(3 + i, 24);
        }

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamForDd(DataTable dt, string tableName)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 18;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 14;//文字大小
        style2.Font.IsBold = true;//粗体
        style2.IsTextWrapped = true;//单元格内容自动换行
        style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 12;//文字大小
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue(tableName);//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        //生成行2 列名行
        cells.SetRowHeight(1, 38);
        for (int i = 0; i < Colnum; i++)
        {
            cells[1, i].PutValue(dt.Columns[i].ColumnName);
            cells[1, i].SetStyle(style2);
            if (dt.Columns[i].ColumnName == "下单时间")
            {
                cells.SetColumnWidth(i, 24);
            }
            else if (dt.Columns[i].ColumnName == "商品名称")
            {
                cells.SetColumnWidth(i, 18);
            }
            else if (dt.Columns[i].ColumnName == "商品规格" || dt.Columns[i].ColumnName == "商品单位")
            {
                cells.SetColumnWidth(i, 12);
            }
            else
            {
                cells.SetColumnWidth(i, 8.5);
            }
        }

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[2 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[2 + i, k].SetStyle(style3);
            }
            cells.SetRowHeight(2 + i, 24);
        }

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamForKsj(DataTable dt, string dwmc, string yh, string rq)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 18;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 14;//文字大小
        style2.Font.IsBold = true;//粗体
        style2.IsTextWrapped = true;//单元格内容自动换行
        style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 12;//文字大小
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式4
        Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style4.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style4.Font.Name = "宋体";//文字字体
        style4.Font.Size = 12;//文字大小
        style4.Font.IsBold = true;//粗体

        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue("泰州市区平价农副产品零售价格日报表");//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        //生成行2  
        cells.Merge(1, 0, 1, Colnum);//合并单元格
        cells[1, 0].PutValue("报价单位：" + dwmc + "  报价日期： " + rq);//填写内容
        cells[1, 0].SetStyle(style4);
        cells.SetRowHeight(1, 25);

        //生成行3 列名行
        cells.SetRowHeight(2, 38);
        for (int i = 0; i < Colnum; i++)
        {
            cells[2, i].PutValue(dt.Columns[i].Caption);
            cells[2, i].SetStyle(style2);
            if (dt.Columns[i].Caption == "序号")
            {
                cells.SetColumnWidth(i, 8);
            }
            else if (dt.Columns[i].Caption == "商品名称")
            {
                cells.SetColumnWidth(i, 25);
            }
            else if (dt.Columns[i].Caption == "规格等级")
            {
                cells.SetColumnWidth(i, 16);
            }
            else if (dt.Columns[i].Caption == "计量单位")
            {
                cells.SetColumnWidth(i, 12);
            }
            else if (dt.Columns[i].Caption == "平均零售价")
            {
                cells.SetColumnWidth(i, 15);
            }
            else
            {

            }
        }

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[3 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[3 + i, k].SetStyle(style3);
            }
            cells.SetRowHeight(3 + i, 24);
        }

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamForRxs(DataTable dt, string dwmc, string rq)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 18;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 14;//文字大小
        style2.Font.IsBold = true;//粗体
        style2.IsTextWrapped = true;//单元格内容自动换行
        style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 12;//文字大小
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式4
        Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style4.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style4.Font.Name = "宋体";//文字字体
        style4.Font.Size = 12;//文字大小
        style4.Font.IsBold = true;//粗体

        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue("泰州市区平价农副产品日销售表");//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        //生成行2  
        cells.Merge(1, 0, 1, Colnum);//合并单元格
        cells[1, 0].PutValue("报价单位：" + dwmc + "  报价日期： " + rq);//填写内容
        cells[1, 0].SetStyle(style4);
        cells.SetRowHeight(1, 25);

        //生成行3 列名行
        cells.SetRowHeight(2, 38);
        for (int i = 0; i < Colnum; i++)
        {
            cells[2, i].PutValue(dt.Columns[i].Caption);
            cells[2, i].SetStyle(style2);
            if (dt.Columns[i].Caption == "序号")
            {
                cells.SetColumnWidth(i, 8);
            }
            else if (dt.Columns[i].Caption == "商品名称")
            {
                cells.SetColumnWidth(i, 20);
            }
            else if (dt.Columns[i].Caption == "规格等级")
            {
                cells.SetColumnWidth(i, 16);
            }
            else if (dt.Columns[i].Caption == "计量单位")
            {
                cells.SetColumnWidth(i, 12);
            }
            else if (dt.Columns[i].Caption == "产地")
            {
                cells.SetColumnWidth(i, 8);
            }
            else
            {
                cells.SetColumnWidth(i, 12);
            }
        }

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[3 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[3 + i, k].SetStyle(style3);
            }
            cells.SetRowHeight(3 + i, 24);
        }

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamPJSDKH(DataTable dt, string tableName)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 18;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 14;//文字大小
        style2.Font.IsBold = true;//粗体
        style2.IsTextWrapped = true;//单元格内容自动换行
        style2.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style2.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 12;//文字大小
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue(tableName);//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        //生成行2 列名行
        for (int i = 0; i < Colnum; i++)
        {
            cells[1, i].PutValue(dt.Columns[i].ColumnName);
            cells[1, i].SetStyle(style2);
        }
        cells.SetRowHeight(1, 50);
        cells.SetColumnWidth(0, 50);

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[2 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[2 + i, k].SetStyle(style3);
                if (k == 0)
                {
                    cells[2 + i, k].Style.HorizontalAlignment = TextAlignmentType.Left;
                }
            }
            cells.SetRowHeight(2 + i, 24);
        }

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamWB(DataTable dt, string tableName, string rq)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 26;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Left;//文字靠左
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 12;//文字大小
        style2.Font.IsBold = true;//粗体

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 10;//文字大小
        style3.Font.IsBold = true;//粗体
        style3.IsTextWrapped = true;//单元格内容自动换行
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式4
        Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style4.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style4.Font.Name = "宋体";//文字字体
        style4.Font.Size = 11;//文字大小
        style4.IsTextWrapped = true;//单元格内容自动换行
        style4.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;


        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, 18);//合并单元格
        cells[0, 0].PutValue(tableName + "(" + rq + ")");//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        int rowNum = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Rownum / 3.00)));



        //生成行2 列名行
        cells.SetRowHeight(1, 80);
        for (int i = 0; i < 18; i = i + 6)
        {
            for (int j = 0; j < Colnum; j++)
            {
                cells[1, i + j].PutValue(dt.Columns[j].Caption);
                cells[1, i + j].SetStyle(style3);
                cells.SetColumnWidth(i + j, 8);
            }
        }

        int dtnum = 0;
        //生成数据行
        for (int i = 0; i < 18; i = i + 6)
        {
            for (int j = 0; j < rowNum; j++)
            {
                for (int k = 0; k < Colnum; k++)
                {
                    if (dtnum < Rownum)
                    {
                        cells[2 + j, i + k].PutValue(dt.Rows[dtnum][k].ToString());
                        cells[2 + j, i + k].SetStyle(style4);
                    }
                }
                dtnum++;
                cells.SetRowHeight(2 + j, 26);
                cells.SetRowHeight(2 + j, 26);
            }
        }

        //生成说明
        cells.Merge(rowNum + 2, 0, 1, 18);//合并单元格
        cells[rowNum + 2, 0].PutValue("注：本栏目所公布的价格行情由泰州市物价局价格监测中心提供，供广大市民购买时参考，更多价格行情请登陆泰州价格信息网平价商店专栏查询。");//填写内容
        cells[rowNum + 2, 0].SetStyle(style4);
        cells.SetRowHeight(rowNum + 2, 20);

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamGSLNFCPLSJGXQ(DataTable dt, string tableName, string rq)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 26;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字靠左
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 12;//文字大小
        style2.Font.IsBold = true;//粗体

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 10;//文字大小
        style3.Font.IsBold = true;//粗体
        style3.IsTextWrapped = true;//单元格内容自动换行
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式4
        Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style4.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style4.Font.Name = "宋体";//文字字体
        style4.Font.Size = 11;//文字大小
        style4.IsTextWrapped = true;//单元格内容自动换行
        style4.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式5
        Style style5 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style5.HorizontalAlignment = TextAlignmentType.Right;//文字靠左
        style5.Font.Name = "宋体";//文字字体
        style5.Font.Size = 12;//文字大小
        style5.Font.IsBold = true;//粗体


        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue(tableName);//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        cells.Merge(1, 0, 1, Colnum);
        cells[1, 0].PutValue("监测日期：" + rq);
        cells[1, 0].SetStyle(style2);
        cells.SetRowHeight(1, 36);

        //生成行2 列名行
        cells.SetRowHeight(2, 80);
        for (int i = 0; i < Colnum; i++)
        {
            cells[2, i].PutValue(dt.Columns[i].Caption);
            cells[2, i].SetStyle(style3);
            cells.SetColumnWidth(i, 28);
        }

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[3 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[3 + i, k].SetStyle(style4);
            }
            cells.SetRowHeight(3 + i, 24);
        }

        //生成说明
        cells.Merge(Rownum + 4, 0, 1, Colnum);//合并单元格
        cells[Rownum + 4, 0].PutValue("说明：1、本表公布的平价商店食品零售价格行情，为各平价商店和大型超市平价大型超市的实际销售价，供广大市民和有关方面参考。");//填写内容
        cells[Rownum + 4, 0].SetStyle(style4);
        cells.SetRowHeight(Rownum + 4, 20);

        //生成说明
        cells.Merge(Rownum + 5, 0, 2, Colnum);//合并单元格
        cells[Rownum + 5, 0].PutValue("泰州市价格监测中心发布");//填写内容
        cells[Rownum + 5, 0].SetStyle(style5);
        cells.SetRowHeight(Rownum + 5, 36);


        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamGSLPJSDSPLSJGXQ(DataTable dt, string tableName, string rq)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 26;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字靠左
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 12;//文字大小
        style2.Font.IsBold = true;//粗体

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 10;//文字大小
        style3.Font.IsBold = true;//粗体
        style3.IsTextWrapped = true;//单元格内容自动换行
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式4
        Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style4.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style4.Font.Name = "宋体";//文字字体
        style4.Font.Size = 11;//文字大小
        style4.IsTextWrapped = true;//单元格内容自动换行
        style4.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;


        //样式5
        Style style5 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style5.HorizontalAlignment = TextAlignmentType.Right;//文字靠左
        style5.Font.Name = "宋体";//文字字体
        style5.Font.Size = 12;//文字大小
        style5.Font.IsBold = true;//粗体

        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 标题行 
        cells.Merge(0, 0, 1, 15);//合并单元格
        cells[0, 0].PutValue(tableName);//填写内容
        cells[0, 0].SetStyle(styleTitle);
        cells.SetRowHeight(0, 38);

        cells.Merge(1, 0, 1, 15);
        cells[1, 0].PutValue("监测日期：" + rq);
        cells[1, 0].SetStyle(style2);
        cells.SetRowHeight(1, 36);

        int rowNum = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(Rownum / 2.00)));



        //生成行2 列名行
        cells.SetRowHeight(2, 30);
        for (int i = 0; i < 15; i = i + 8)
        {
            for (int j = 0; j < Colnum; j++)
            {
                cells[2, i + j].PutValue(dt.Columns[j].Caption);
                cells[2, i + j].SetStyle(style3);
                cells.SetColumnWidth(i + j, 12);
            }
        }

        int dtnum = 0;
        //生成数据行
        for (int i = 0; i < 15; i = i + 8)
        {
            for (int j = 0; j < rowNum; j++)
            {
                for (int k = 0; k < Colnum; k++)
                {
                    if (dtnum < Rownum)
                    {
                        cells[3 + j, i + k].PutValue(dt.Rows[dtnum][k].ToString());
                        cells[3 + j, i + k].SetStyle(style4);
                    }
                }
                dtnum++;
                cells.SetRowHeight(3 + j, 26);
                cells.SetRowHeight(3 + j, 26);
            }
        }

        //生成说明
        cells.Merge(rowNum + 4, 0, 1, 15);//合并单元格
        cells[rowNum + 4, 0].PutValue("说明：本表公布的平价商店食品零售价格行情，为各平价商店和大型超市平价大型超市的实际销售价，更多行情请登录“泰州价格信息网”查阅");//填写内容
        cells[rowNum + 4, 0].SetStyle(style4);
        cells.SetRowHeight(rowNum + 4, 20);

        //生成说明
        cells.Merge(rowNum + 5, 0, 1, 15);//合并单元格
        cells[rowNum + 5, 0].PutValue("泰州市价格监测中心发布");//填写内容
        cells[rowNum + 5, 0].SetStyle(style5);
        cells.SetRowHeight(rowNum + 5, 30);

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

    public static MemoryStream OutFileToStreamJGJCPJSDZK(DataTable dt, string tableName, string rq)
    {
        Workbook workbook = new Workbook(); //工作簿
        Worksheet sheet = workbook.Worksheets[0]; //工作表
        Cells cells = sheet.Cells;//单元格

        //为专刊头设置样式  
        Style styleTitle1 = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle1.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle1.Font.Name = "宋体";//文字字体
        styleTitle1.Font.Size = 50;//文字大小
        styleTitle1.Font.IsBold = true;//粗体
        styleTitle1.Font.Color = System.Drawing.Color.Red; //字体红色

        //为专刊期数设置样式  
        Style styleTitle2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle2.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle2.Font.Name = "宋体";//文字字体
        styleTitle2.Font.Size = 12;//文字大小



        //为标题设置样式  
        Style styleTitle = workbook.Styles[workbook.Styles.Add()];//新增样式
        styleTitle.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        styleTitle.Font.Name = "宋体";//文字字体
        styleTitle.Font.Size = 26;//文字大小
        styleTitle.Font.IsBold = true;//粗体

        //样式2
        Style style2 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style2.HorizontalAlignment = TextAlignmentType.Center;//文字靠左
        style2.Font.Name = "宋体";//文字字体
        style2.Font.Size = 12;//文字大小
        style2.Font.IsBold = true;//粗体

        //样式3
        Style style3 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style3.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style3.Font.Name = "宋体";//文字字体
        style3.Font.Size = 10;//文字大小
        style3.Font.IsBold = true;//粗体
        style3.IsTextWrapped = true;//单元格内容自动换行
        style3.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style3.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        //样式4
        Style style4 = workbook.Styles[workbook.Styles.Add()];//新增样式
        style4.HorizontalAlignment = TextAlignmentType.Center;//文字居中
        style4.Font.Name = "宋体";//文字字体
        style4.Font.Size = 11;//文字大小
        style4.IsTextWrapped = true;//单元格内容自动换行
        style4.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
        style4.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;

        int Colnum = dt.Columns.Count;//表格列数
        int Rownum = dt.Rows.Count;//表格行数

        //生成行1 专刊头 
        cells.Merge(0, 0, 1, Colnum);//合并单元格
        cells[0, 0].PutValue("泰州价格监测");//填写内容
        cells[0, 0].SetStyle(styleTitle1);
        cells.SetRowHeight(0, 63.75);

        //生成行2 专刊期数
        cells.Merge(1, 0, 1, Colnum);//合并单元格
        cells[1, 0].PutValue("平价商店专刊（第  期）");//填写内容
        cells[1, 0].SetStyle(styleTitle2);
        cells.SetRowHeight(1, 25);


        //生成行3 标题行 
        cells.Merge(2, 0, 1, Colnum);//合并单元格
        cells[2, 0].PutValue(tableName);//填写内容
        cells[2, 0].SetStyle(styleTitle);
        cells.SetRowHeight(2, 38);

        //生成日期
        cells.Merge(3, 0, 1, 4);
        cells[3, 0].PutValue("时间：" + rq);
        cells[3, 0].SetStyle(style2);
        cells.SetRowHeight(3, 30);

        //生成单位
        cells.Merge(3, 6, 1, 5);
        cells[3, 6].PutValue("计价单位：元/500克");
        cells[3, 6].SetStyle(style2);
        cells.SetRowHeight(3, 30);

        //生成行2 列名行
        cells.SetRowHeight(4, 38.25);
        for (int i = 0; i < Colnum; i++)
        {
            cells[4, i].PutValue(dt.Columns[i].Caption);
            cells[4, i].SetStyle(style3);
            cells.SetColumnWidth(i, 8.38);
        }

        //生成数据行
        for (int i = 0; i < Rownum; i++)
        {
            for (int k = 0; k < Colnum; k++)
            {
                cells[5 + i, k].PutValue(dt.Rows[i][k].ToString());
                cells[5 + i, k].SetStyle(style4);
            }
            cells.SetRowHeight(5 + i, 28.5);
        }

        MemoryStream ms = workbook.SaveToStream();
        return ms;
    }

}
