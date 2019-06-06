<%@ WebHandler Language="C#" Class="CheckAnswer" %>

using System;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using BizATWebCommon;
using BizSmooth;
using ExamProcess;
using System.Collections.Generic;

public class CheckAnswer : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        try
        {
            ExamPaper.AnswerCheck newcheck=new ExamPaper.AnswerCheck();
            int sumvalue = 0;
            string testerid = context.Request["_id"];
            var examid = context.Request["examid"];
            var testeranswer = DBConnect.LookupID("Bizexam", "answersheet", testerid);
            var examinfo = DBConnect.LookupID("Bizexam", "exam", examid);
            Exam newexam = JSON.parse<Exam>(examinfo.ToString());
            ExamPaper.AnswerSheet testanswer = JSON.parse<ExamPaper.AnswerSheet>(testeranswer.ToString());
            List<Exam.Singles> singles = newexam.singles; ;
            List<Exam.Multi> Multi = newexam.multis;
            List<Exam.TrueFalse> TFs = newexam.TFs;
            List<Exam.Filling> fillings = newexam.fillings;
            List<Exam.ShortQuestion> Shorts = newexam.shorts;
            if (singles!=null&& singles.Count > 0)
            {
                for(int i=0;i<singles.Count;i++)
                {
                    int score=newcheck.SingleCheck(testanswer.singles[i].answer, singles[i].theAnswer, singles[i].scoreValue);
                    sumvalue = sumvalue + score;
                }
            }
          
                if (Multi != null&&Multi.Count > 0)
                {
                    for (int i = 0; i < Multi.Count; i++)
                    {
                        int score = newcheck.MultiCheck(testanswer.multis[i].answer, Multi[i].theAnswer, Multi[i].theAnswer.Length, Multi[i].scoreValue);
                        sumvalue = sumvalue + score;
                    }
                }
            
            if(TFs!=null&&TFs.Count>0)
            {
                for(int i=0;i<TFs.Count;i++)
                {
                    int score= newcheck.TFCheck(testanswer.TFs[i].answer, TFs[i].theAnswer, TFs[i].scoreValue);
                    sumvalue = sumvalue + score;
                }
            }
            if(fillings !=null&&fillings.Count>0)
            {
                for(int i=0;i<fillings.Count;i++)
                {
                    int score=  newcheck.CheckAnswer(testanswer.fillings[i].answer, fillings[i].evaluation, fillings[i].scoreValue, fillings[i].weights);
                    sumvalue = sumvalue + score;
                }
            }
            if(Shorts!=null&& Shorts.Count>0)
            {
                for(int i=0;i<Shorts.Count;i++)
                {
                    int score=  newcheck.ShortCheck(testanswer.shorts[i].answer, Shorts[i].evaluation,Shorts[i].scoreValue);
                    sumvalue = sumvalue + score;
                }
            }
            context.Response.Write("{\"Score\":" + sumvalue + "}");
        }
        catch (Exception e)
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