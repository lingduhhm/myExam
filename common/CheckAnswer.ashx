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
            List<Exam.Singles> singles = newexam.singles; 
            List<Exam.Multi> Multi = newexam.multis;
            List<Exam.TrueFalse> TFs = newexam.TFs;
            List<Exam.Filling> fillings = newexam.fillings;
            List<Exam.ShortQuestion> Shorts = newexam.shorts;
            if (testanswer.singles!=null&& testanswer.singles.Count > 0)
            {
                for(int i=0;i<testanswer.singles.Count;i++)
                {
                    if (testanswer.singles[i].answer != -1)
                    {
                        int score = newcheck.SingleCheck(testanswer.singles[i].answer, singles[i].theAnswer, singles[i].scoreValue);
                        sumvalue = sumvalue + score;
                    }
                }
            }

            if (testanswer.multis != null&&testanswer.multis.Count > 0)
            {
                for (int i = 0; i < testanswer.multis.Count; i++)
                {
                    if (testanswer.multis[i].answer.Length != 0)
                    {
                        int score = newcheck.MultiCheck(testanswer.multis[i].answer, Multi[i].theAnswer, Multi[i].theAnswer.Length, Multi[i].scoreValue);
                        sumvalue = sumvalue + score;
                    }
                }
            }

            if(testanswer.TFs!=null&&testanswer.TFs.Count>0)
            {
                for(int i=0;i<testanswer.TFs.Count;i++)
                {
                    if (testanswer.TFs[i].answer != -1)
                    {
                        int score = newcheck.TFCheck(testanswer.TFs[i].answer, TFs[i].theAnswer, TFs[i].scoreValue);
                        sumvalue = sumvalue + score;
                    }
                }
            }
            if(testanswer.fillings !=null&&testanswer.fillings.Count>0)
            {
                for(int i=0;i<testanswer.fillings.Count;i++)
                {
                    if (testanswer.fillings[i].answer.Length!=0)
                    {
                        int score = newcheck.CheckAnswer(testanswer.fillings[i].answer, fillings[i].evaluation, fillings[i].scoreValue, fillings[i].weights);
                        sumvalue = sumvalue + score;
                    }
                }
            }
            if(testanswer.shorts!=null&&testanswer.shorts.Count>0)
            {
                for(int i=0;i<testanswer.shorts.Count;i++)
                {
                    if (testanswer.shorts[i] .answer!= null)
                    {
                        int score = newcheck.ShortCheck(testanswer.shorts[i].answer, Shorts[i].evaluation, Shorts[i].scoreValue);
                        sumvalue = sumvalue + score;
                    }
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