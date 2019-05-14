<%@ WebHandler Language="C#" Class="PutExamSheet" %>

using System;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Shared;
using BizATWebCommon;
using BizExamDLL;
using BizSmooth;

public class PutExamSheet : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        try
        {
            string email = context.Request["email"];
            string wechat = context.Request["wechat"];
            string jsonStr = context.Request["jsonstr"];

            var examSheet = JSON.parse<AnswerSheet>(jsonStr);

            if (examSheet == null)
            {
                context.Response.Write("Fail to parse the exam sheet from the string from the front ");
                return;
            }
           
            // put it in the database
            var filter = Builders<BsonDocument>.Filter.And(Builders<BsonDocument>.Filter.Eq("id", examSheet.id), Builders<BsonDocument>.Filter.Eq("dt",examSheet.dt));
            var col = DBConnect.GetCollection("bizexam","answersheet");
            var result = col.Find(filter);
            string objIDStr;
            string info = "";
            if (result.CountDocuments() != 0)
            {
                objIDStr = result.First()["_id"].ToString();
                col.ReplaceOne(filter, examSheet.ToBsonDocument());
                info =   "The the exam sheet has been exist, and it was overrited successfully.";
            }
            else
            {
                col.InsertOne(examSheet.ToBsonDocument());
                result = col.Find(filter);
                objIDStr = result.First()["_id"].ToString();
                info = "The the exam sheet was submitted successfully.";
            }
            // 这里需要发个邮件或微信给Tester。 对方的wechat必须加入我们的公众号。
            if (email != null && email != "")
            {
                // 如果发送失败，则info后面加入信息
            }
            if (wechat != null && wechat != "")
            {
                // 如果发送失败，则info后面加入信息
            }

            context.Response.Write(WebLib.BuildFailStr(0, info + " Please remember the your examsheet ID: " + objIDStr + ", You could check your score by using this ID."));
        }
        catch (Exception e)
        {
            context.Response.Write(WebLib.BuildFailStr(4, "Exception: " + e));
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}