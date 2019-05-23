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
            string objIdStr = context.Request["_id"];
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objIdStr);
            var collection = DBConnect.GetCollection("Bizexam", "exampaper");
            var results = collection.Find<BsonDocument>(filter);
            if (results.CountDocuments() == 0)
            {
                context.Response.Write("{\"fail\":1, \"description\": \"No such exampaper " + objIdStr + "\"}");
            }
            else
            {
                context.Response.Write(results.First().ToJson());
            }
        }
        catch (Exception e)
        {
            context.Response.Write("{\"fail\":4, \"description\": \"Exception: " + e + "\"}");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}