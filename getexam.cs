using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using OfficeOpenXml;

namespace ExamProcess
{
   public class getexam
    {
       public  ExcelPackage excel = new ExcelPackage();
        public getexam(BsonDocument bd)
        {

            excel.Workbook.Worksheets.Add(bd["_id"].AsString);
            var sheet = excel.Workbook.Worksheets[1];
            sheet.Cells.AutoFitColumns(9);
            sheet.Cells.Style.Font.Name = "微软雅黑";
            sheet.Cells.Style.Font.Size = 9;
            sheet.Column(6).Width = 48;
            sheet.Column(7).Width = 13;
            sheet.Column(8).Width = 50;
            sheet.Column(9).Width = 12;
            sheet.Column(10).Width = 17;
            sheet.Column(11).Width = 12;
            sheet.Cells.Style.ShrinkToFit = true;
            sheet.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
            sheet.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheet.Cells.Style.WrapText = true;
            int row = 1;
            BsonArray singles, multis, TFs, fillings, shorts;
            if (!bd["singles"].IsBsonNull)
            {
                singles = bd["singles"].AsBsonArray;
                if (singles.Count() > 0)
                {
                    sheet.Cells[row, 1].Value = "Single";
                    sheet.Cells[row, 1].Style.Font.Bold = true;
                    row++;
                    sheet.Cells[row, 1].Value = "order";
                    sheet.Cells[row, 2].Value = "category";
                    sheet.Cells[row, 3].Value = "Min";
                    sheet.Cells[row, 4].Value = "Max";
                    sheet.Cells[row, 5].Value = "Average";
                    sheet.Cells[row, 6].Value = "stem";
                    sheet.Cells[row, 7].Value = "ScoreValue";
                    sheet.Cells[row, 8].Value = "Candidates";
                    sheet.Cells[row, 9].Value = "TheAnswer";
                    int i = 1;
                    foreach (BsonDocument single in singles)
                    {
                        row++;
                        sheet.Cells[row, 1].Value = i;
                        string category = getcategory(single["c1"].AsInt32, single["c11"].AsInt32, single["c2"].AsInt32, single["c22"].AsInt32);
                        sheet.Cells[row, 2].Value = category;
                        sheet.Cells[row, 3].Value = single["min"].AsInt32;
                        sheet.Cells[row, 4].Value = single["max"].AsInt32;
                        sheet.Cells[row, 5].Value = single["average"].AsInt32;
                        sheet.Cells[row, 6].Value = single["stem"].AsString;
                        sheet.Cells[row, 7].Value = single["scoreValue"].AsInt32;
                        var bsoncandi = single["candidates"].AsBsonArray;
                        StringBuilder candidates = process(bsoncandi);
                        sheet.Cells[row, 8].Value = candidates.ToString();
                        sheet.Cells[row, 9].Value = single["theAnswer"].AsString;
                        i++;
                    }
                    row = row + 2;
                }

            }
            if (!bd["multis"].IsBsonNull)
            {
                multis = bd["multis"].AsBsonArray;
                if (multis.Count() > 0)
                {
                    sheet.Cells[row, 1].Value = "Mulit";
                    sheet.Cells[row, 1].Style.Font.Bold = true;
                    row++;
                    sheet.Cells[row, 1].Value = "order";
                    sheet.Cells[row, 2].Value = "category";
                    sheet.Cells[row, 3].Value = "Min";
                    sheet.Cells[row, 4].Value = "Max";
                    sheet.Cells[row, 5].Value = "Average";
                    sheet.Cells[row, 6].Value = "stem";
                    sheet.Cells[row, 7].Value = "ScoreValue";
                    sheet.Cells[row, 8].Value = "Candidates";
                    sheet.Cells[row, 9].Value = "TheAnswer";
                    int i = 1;
                    foreach (BsonDocument mulit in multis)
                    {
                        row++;
                        sheet.Cells[row, 1].Value = i;
                        string category = getcategory(mulit["c1"].AsInt32, mulit["c11"].AsInt32, mulit["c2"].AsInt32, mulit["c22"].AsInt32);
                        sheet.Cells[row, 2].Value = category;
                        sheet.Cells[row, 3].Value = mulit["min"].AsInt32;
                        sheet.Cells[row, 4].Value = mulit["max"].AsInt32;
                        sheet.Cells[row, 5].Value = mulit["average"].AsInt32;
                        sheet.Cells[row, 6].Value = mulit["stem"].AsString;
                        sheet.Cells[row, 7].Value = mulit["scoreValue"].AsInt32;
                        var bsoncandi = mulit["candidates"].AsBsonArray;
                        StringBuilder candidates = process(bsoncandi);
                        sheet.Cells[row, 8].Value = candidates;
                        sheet.Cells[row, 9].Value = mulit["theAnswer"].AsString;
                        i++;
                    }
                    row = row + 2;
                }
            }
            if (!bd["TFs"].IsBsonNull)
            {
                TFs = bd["TFs"].AsBsonArray;
                if (TFs.Count() > 0)
                {
                    sheet.Cells[row, 1].Value = "TrueFalse";
                    sheet.Cells[row, 1].Style.Font.Bold = true;
                    row++;
                    sheet.Cells[row, 1].Value = "order";
                    sheet.Cells[row, 2].Value = "category";
                    sheet.Cells[row, 3].Value = "Min";
                    sheet.Cells[row, 4].Value = "Max";
                    sheet.Cells[row, 5].Value = "Average";
                    sheet.Cells[row, 6].Value = "stem";
                    sheet.Cells[row, 7].Value = "ScoreValue";
                    sheet.Cells[row, 8].Value = "TheAnswer";
                    int i = 1;
                    foreach (BsonDocument tf in TFs)
                    {
                        row++;
                        sheet.Cells[row, 1].Value = i;
                        string category = getcategory(tf["c1"].AsInt32, tf["c11"].AsInt32, tf["c2"].AsInt32, tf["c22"].AsInt32);
                        sheet.Cells[row, 2].Value = category;
                        sheet.Cells[row, 3].Value = tf["min"].AsInt32;
                        sheet.Cells[row, 4].Value = tf["max"].AsInt32;
                        sheet.Cells[row, 5].Value = tf["average"].AsInt32;
                        sheet.Cells[row, 6].Value = tf["stem"].AsString;
                        sheet.Cells[row, 7].Value = tf["scoreValue"].AsInt32;
                        sheet.Cells[row, 8].Value = tf["theAnswer"].AsInt32;
                        i++;
                    }
                    row = row + 2;
                }
            }
            if (!bd["fillings"].IsBsonNull)
            {
                fillings = bd["fillings"].AsBsonArray;
                if (fillings.Count() > 0)
                {
                    sheet.Cells[row, 1].Value = "Filling";
                    sheet.Cells[row, 1].Style.Font.Bold = true;
                    row++;
                    sheet.Cells[row, 1].Value = "order";
                    sheet.Cells[row, 2].Value = "category";
                    sheet.Cells[row, 3].Value = "Min";
                    sheet.Cells[row, 4].Value = "Max";
                    sheet.Cells[row, 5].Value = "Average";
                    sheet.Cells[row, 6].Value = "stem";
                    sheet.Cells[row, 7].Value = "ScoreValue";
                    sheet.Cells[row, 8].Value = "TotalBlank";
                    sheet.Cells[row, 9].Value = "OneValue";
                    sheet.Cells[row, 10].Value = "TheAnswer";
                    sheet.Cells[row, 11].Value = "Evaluation";
                    sheet.Cells[row, 12].Value = "Connection";
                    sheet.Cells[row, 13].Value = "Weights";
                    int i = 1;
                    foreach (BsonDocument filling in fillings)
                    {
                        row++;
                        sheet.Cells[row, 1].Value = i;
                        string category = getcategory(filling["c1"].AsInt32, filling["c11"].AsInt32, filling["c2"].AsInt32, filling["c22"].AsInt32);
                        sheet.Cells[row, 2].Value = category;
                        sheet.Cells[row, 3].Value = filling["min"].AsInt32;
                        sheet.Cells[row, 4].Value = filling["max"].AsInt32;
                        sheet.Cells[row, 5].Value = filling["average"].AsInt32;
                        sheet.Cells[row, 6].Value = filling["stem"].AsString;
                        sheet.Cells[row, 7].Value = filling["scoreValue"].AsInt32;
                        sheet.Cells[row, 8].Value = filling["totalBlank"].AsInt32;
                        sheet.Cells[row, 9].Value = filling["oneValue"].AsInt32;
                        var bsonanswer = filling["theAnswer"].AsBsonArray;
                        StringBuilder theanswer = process(bsonanswer);
                        sheet.Cells[row, 10].Value = theanswer;
                        var evaluations = filling["evaluation"].AsBsonArray;
                        var weigths = filling["weights"].AsBsonArray;
                        sheet.Cells[row, 13].Value = process(weigths);
                        string[] evaluate = getevaluation(evaluations);
                        sheet.Cells[row, 11].Value = evaluate[1];
                        sheet.Cells[row, 12].Value = evaluate[0];
                        i++;
                    }
                    row = row + 2;
                }
            }
            if (!bd["shorts"].IsBsonNull)
            {
                shorts = bd["shorts"].AsBsonArray;
                if (shorts.Count() > 0)
                {
                    sheet.Cells[row, 1].Value = "ShortQuestion";
                    sheet.Cells[row, 1].Style.Font.Bold = true;
                    row++;
                    sheet.Cells[row, 1].Value = "order";
                    sheet.Cells[row, 2].Value = "category";
                    sheet.Cells[row, 3].Value = "Min";
                    sheet.Cells[row, 4].Value = "Max";
                    sheet.Cells[row, 5].Value = "Average";
                    sheet.Cells[row, 6].Value = "stem";
                    sheet.Cells[row, 7].Value = "ScoreValue";
                    sheet.Cells[row, 8].Value = "TheAnswer";
                    sheet.Cells[row, 9].Value = "Evaluation";
                    int i = 1;
                    foreach (BsonDocument Short in shorts)
                    {
                        row++;
                        sheet.Cells[row, 1].Value = i;
                        string category = getcategory(Short["c1"].AsInt32, Short["c11"].AsInt32, Short["c2"].AsInt32, Short["c22"].AsInt32);
                        sheet.Cells[row, 2].Value = category;
                        sheet.Cells[row, 3].Value = Short["min"].AsInt32;
                        sheet.Cells[row, 4].Value = Short["max"].AsInt32;
                        sheet.Cells[row, 5].Value = Short["average"].AsInt32;
                        sheet.Cells[row, 6].Value = Short["stem"].AsString;
                        sheet.Cells[row, 7].Value = Short["scoreValue"].AsInt32;
                        sheet.Cells[row, 8].Value = Short["theAnswer"].AsString;
                        sheet.Cells[row, 9].Value = Short["evaluation"];
                        i++;
                    }
                }

            }
            excel.Save();
    }
        public string getcategory(int c1, int c11, int c2, int c22)
        {
            StringBuilder sb = new StringBuilder();
            if (c1 != 0)
                sb.Append(c1.ToString());
            if (c11 != 0)
                sb.Append("." + c11.ToString());
            if (c2 != 0)
                sb.Append("\n" + c2.ToString());
            if (c22 != 0)
                sb.Append("." + c22.ToString());
            return sb.ToString();
        }
        public string[] getevaluation(BsonArray bson)
        {
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            for (int es = 0; es < bson.Count(); es++)
            {
                if (!bson[es].IsBsonNull)
                {
                    BsonArray one = bson[es].AsBsonArray;
                    if (one[0].AsString != "")
                    {
                        if (!sb1.ToString().Contains(one[0].AsString))
                            sb1.Append(one[0].AsString + "\n");
                    }
                    for (int b = 1; b < one.Count(); b++)
                    {
                        sb2.Append(one[b].AsString + "\n");
                    }
                }
            }
            string connect = sb1.ToString();
            string evaluate = sb2.ToString();
            int cindex = connect.LastIndexOf("\n");
            int eindex = evaluate.LastIndexOf("\n");
            if (cindex > 0)
                connect = connect.Substring(0, cindex);
            if (eindex > 0)
                evaluate = evaluate.Substring(0, eindex);
            string[] all = new string[2];
            all[0] = connect;
            all[1] = evaluate;
            return all;
        }
        public static int[] analycategory(string sentence)
        {
            int[] category = null;
            if (sentence.Contains("\n"))
            {
                int index = sentence.IndexOf(".");
                category[0] = Convert.ToInt32(sentence.Substring(0, index));
                int nindex = sentence.IndexOf("\n");
                category[1] = Convert.ToInt32(sentence.Substring(index + 1, (nindex - index)));
                int lastindex = sentence.LastIndexOf(".");
                category[2] = Convert.ToInt32(sentence.Substring(nindex + 1, (lastindex - nindex)));
                category[3] = Convert.ToInt32(sentence.Substring(lastindex + 1));
            }
            return category;
        }
        public StringBuilder process(BsonArray bson)
        {
            StringBuilder s = new StringBuilder();
            for (int j = 0; j < bson.Count(); j++)
            {
                if (j == 0)
                    s.Append(bson[j]);
                else
                    s.Append("\n" + bson[j]);
            }
            return s;
        }
    }
}
