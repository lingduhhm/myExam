<%@ WebHandler Language="C#" Class="putexam" %>

using System;
using System.Web;
using System.IO;

public class putexam : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            // var filepath = context.Request["filepath"];
            var filepath = context.Request.Files[0];
            var filename = filepath.InputStream;
            if(!filepath.FileName.EndsWith(".xls")&&!filepath.FileName.EndsWith(".xlsx"))
            {
                context.Response.Write("define file is with incorrect extension");
                return;
            }
            else
            {
                ExamProcess.putexam newexam = new ExamProcess.putexam(filename);
                context.Response.Write("Import Sucess!");
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