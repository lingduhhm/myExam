<%@ WebHandler Language="C#" Class="putexam" %>

using System;
using System.Web;
using System.IO;
    using MongoDB.Bson;
//将excel文件数据导入到数据库
public class putexam : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            var filepath = context.Request.Files[0];
            var examname = filepath.FileName;
            var filename = filepath.InputStream;
            if(!filepath.FileName.EndsWith(".xls")&&!filepath.FileName.EndsWith(".xlsx"))
            {
                context.Response.Write("define file is with incorrect extension");
                return;
            }
            else
            {
                ExamProcess.putexam newexam = new ExamProcess.putexam(filename,examname);
                exammess newmess = new exammess();
                newmess.id = newexam.newexam._id;
                newmess.name = examname;
                context.Response.Write(newmess.ToJson());

            }

        }
        catch
        {
            context.Response.Write("Import fail!Please check");
        }
    }
    public class exammess
    {
        public string name;
        public string id;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }

}