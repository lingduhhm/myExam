<%@ WebHandler Language="C#" Class="PutExamSheet" %>


using System;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Shared;
using BizATWebCommon;
//using BizExamDLL;
using BizSmooth;
using ExamProcess;

public class PutExamSheet : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        bool flag = true;
        try
        {
            string jsonStr = context.Request["Answer"];
            var answerSheet = JSON.parse<ExamPaper.AnswerSheet>(jsonStr);
            var result = DBConnect.LookupID("Bizexam", "Userinfo", answerSheet.id);
            result["isfinash"] = true;
            if (answerSheet == null)
            {
                flag = false;
                context.Response.Write(flag);
                return;
            }
            else
            {
                DBConnect.Initialize();
                var col1= DBConnect.GetCollection("Bizexam","answersheet");
                var col2= DBConnect.GetCollection("Bizexam","Userinfo");
                DBConnect.SaveBson(col1, answerSheet.ToBsonDocument());
                DBConnect.SaveBson(col2, result);
                flag = true;
                context.Response.Write(flag);
            }
        }
        catch (Exception e)
        {
            flag = false;
            context.Response.Write(flag);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}