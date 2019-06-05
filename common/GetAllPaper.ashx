<%@ WebHandler Language="C#" Class="GetAllPaper" %>

using System;
using System.Web;
using MongoDB.Driver;
using MongoDB.Bson;
using BizATWebCommon;
using System.Collections.Generic;
    //获取所有的exam中的试卷id+name信息
public class GetAllPaper : IHttpHandler {
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            DBConnect.Initialize();
            var filter = new BsonDocument();
            var listbson = DBConnect.GetCollection("Bizexam", "exam");
            List<BsonDocument> results = new List<BsonDocument>();
            results = listbson.Find(filter).ToListAsync().Result;
            List<Papermess> listmess = new List<Papermess>();
            Papermess mess;
            foreach(BsonDocument bd in results)
            {
                if(!bd.IsBsonNull)
                {
                    mess = new Papermess();
                    mess.id = bd["_id"].AsString;
                    mess.name = bd["name"].AsString;
                    listmess.Add(mess);
                }
            }
            context.Response.Write(listmess.ToJson());
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
    public class Papermess
    {
        public string id;
        public string name;
    }

}