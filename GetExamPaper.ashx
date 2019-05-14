<%@ WebHandler Language="C#" Class="GetExamPaper" %>

using System;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Shared;
using BizATWebCommon;

public class GetExamPaper : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        try
        {
            string objIdStr = context.Request["objid"];

            var objId = ObjectId.Parse(objIdStr);
            var docExam = DBConnect.LookupID("bizexam", "exampaper", objId);

            if (docExam == null)
                context.Response.Write("Fail to get the exam paper with the objID " + objIdStr);
            else
                context.Response.Write(docExam.ToJson(DBConnect.JsonModeStrict));
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