<%@ WebHandler Language="C#" Class="logincheck" %>

using System;
using System.Web;
using BizATWebCommon;
using MongoDB.Driver;
using MongoDB.Bson;
using BizSmooth;
using ExamProcess;
public class logincheck : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            var id = context.Request["_id"];
            var pw = context.Request["password"];
            var user = DBConnect.LookupID("Bizexam", "Userinfo", id);
            if (user == null)
            {
                context.Response.Write("{\"fail\":1,\"description\":\"Could not find the user\"}");
            }
            else if (!pw.Equals(user["password"].AsString, StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Write("{\"fail\":2,\"description\":\"Wrong password\"}");
            }
            else
            {
                    context.Response.Write("{\"testerid\":\"" + id +"\",\"testername\":\""+user["name"].AsString+ "\",\"examid\":\""+user["examid"].AsString+"\",\"isfinash\":\""+user["isfinash"].AsBoolean+"\"}");
            
            }

        }
        catch(Exception e)
        {
            context.Response.Write("{\"fail\":3,\"description\":"+e+"}");
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}