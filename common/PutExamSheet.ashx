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
            if (answerSheet == null)
            {
                flag = false;
                context.Response.Write(flag);
                return;
            }
            else
            {
                DBConnect.Initialize();
                var col = DBConnect.GetCollection("Bizexam","answersheet");
                DBConnect.SaveBson(col, answerSheet.ToBsonDocument());
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