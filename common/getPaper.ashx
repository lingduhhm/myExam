<%@ WebHandler Language="C#" Class="getPaper" %>

using System;
using System.Web;
using BizATWebCommon;
using BizSmooth;
using MongoDB.Bson;
using MongoDB.Driver;
using OfficeOpenXml;
using System.IO;
//将数据库中exam导出为excel文件
public class getPaper : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            var id = context.Request["_id"];
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
            var collection = DBConnect.GetCollection("Bizexam", "exam");
            var result = collection.Find<BsonDocument>(filter).ToListAsync().Result;
            if(result.Count==0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("this exam is not exist");
            }
            else
            {
                ExamProcess.getexam getexam = new ExamProcess.getexam(result[0]);
                var file = getexam.excel;
                var filename = getexam.filename;
                context.Response.Charset = "UTF8";
                context.Response.AddHeader("content-Disposition", "attachment;filename=" + id + ".xlsx");
                Byte[] bytes = file.GetAsByteArray();
                context.Response.BinaryWrite(bytes);
            }
        }
        catch(Exception e)
        {
            context.Response.Write(e);
        }
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}