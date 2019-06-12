using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExamProcess
{
  public  class ExamPaper
    {
        public string name;
       public object _id;
       public int mixOrder;
       public int mixSelection;
       public string dt;  // 依据这个字串生成密钥
       public string key; // 公开密钥，在C#里面解开，跟dt一起生成内部密钥，用于解开题目内容。

       public List<OneSelection> single, multi;
       public List<string> TrueFalse, filling, shortQuestion;

      public  class OneSelection
        {
            public string stem;
            public string[] candidates;
        }
        public class ExamOneTime
        {

        }
        [DataContract]
        public class AnswerSheet
        {
            [DataMember]
            public string tester;
            [DataMember]
            public string id;
            [DataMember]
            public string dt;
            [DataMember]
            public string examPaperId;
            [DataMember]
            public List<SingleOrTF> singles = new List<SingleOrTF>();
            [DataMember]
            public List<SingleOrTF> TFs = new List<SingleOrTF>();
            [DataMember]
            public List<Multi> multis = new List<Multi>();
            [DataMember]
            public List<Filling> fillings = new List<Filling>();
            [DataMember]
            public List<ShortQuestion> shorts = new List<ShortQuestion>();

            [DataContract]
            public class SingleOrTF
            {
                [DataMember]
                public int answer;
                [DataMember]
                public int[] miniseconds;
            }

            [DataContract]
            public class Multi
            {
                [DataMember]
                public int[] answer;
                [DataMember]
                public int[] miniseconds;
            }
            [DataContract]
            public class Filling
            {
                [DataMember]
                public string[] answer;
                [DataMember]
                public int[] miniseconds;
            }

            [DataContract]
            public class ShortQuestion
            {
                [DataMember]
                public string answer;
                [DataMember]
                public int[] miniseconds;
            }

        }
        public class AnswerCheck
        {
            int singleBackOff;  // 是否单项选择题要倒扣分 0表示不倒扣，值表示倒扣的分所占得分值的百分比
            int multiBackOff;  // 是否多项选择题要倒扣分，值同上。只有multiPatial为0的情况下才有用。
            int TFBackOff;  // truefalse 判断题 是否要倒扣分，值同上
            int multiPartial; // 是否多项选择题计算部分得分，0表示不计算 1表示按答案的个数为分母计算得分，2表示按照总的候选项为分母计算得分 

            #region 填空题 Blank Filling 
            StringComparison theComparision;
            int scoreType;  // 0: 表示每个空是一样的得分，scoreValue为每个空的分值， 
                            //1:表示scoreValue为总得分，在weights里面按百分制每个小空所占分值的百分比 
                            //4:只要有一个空没对，则为0分， scoreValue为每个空的分值

            public int CheckAnswer(string[] answers, string[][] theCorrects, int scoreValue, int[] weights)
            {
            //    string[] answers = answer.Split('|');
                int len = theCorrects.Length;
                int[] scoreArray = new int[len];

                for (int i = 0; i < answers.Length; i++)
                {
                    // i表示第几个空
                    string[] oneCorrect = theCorrects[i];
                    string indicator = oneCorrect[0];
                    if (indicator == null || indicator == "")  // 意味着没有关联
                    {
                        if (CheckAnswer(answers[i], oneCorrect) == 1)
                            scoreArray[i] = 1;
                    }
                    else  // 意味着被关联了
                    {
                        // 此处采用“hit”的算法，只要候选的那个空被hit了，则会记录它的分数。
                        foreach (char c in indicator)
                        {
                            int order = c - '1'; // 里面的字符以1开始，在数组中则是从0开始 
                            if (CheckAnswer(answers[i], theCorrects[order]) == 1)
                                scoreArray[order] = 1;

                        }
                    }
                }
                // score 记录着每个空的结果 
                int total = 0;
                for (int i = 0; i < len; i++)
                {
                    int s = scoreArray[i];
                    if (s > 0)
                    {
                        if (scoreType == 0)
                            total += scoreValue;
                        else if (scoreType == 1)
                            total += weights[i];
                    }
                    else if (scoreType == 4) // ==4 表示只要有一个空没对，得分为0
                        return 0;
                }
                if (scoreType == 1)
                    total = total * scoreValue / 100;

                return 0;
            }

            int CheckAnswer(string answer, string[] theCorrect)
            {

                int candidateOrder = 1;

                while (candidateOrder < theCorrect.Length)
                {
                    string one = theCorrect[candidateOrder];
                    candidateOrder++;
                    if (one == null) continue;
                    if (one[0] == '*')
                    {
                        if (Regex.IsMatch(answer, one.Substring(1))) // 也可以不取substring，因*表示为任意字符
                            return 1;
                    }
                    else
                    {
                        if (one.Equals(answer, theComparision))
                            return 1;
                    }
                }
                return 0;
            }
            #endregion

            public int SingleCheck(int answer, int theCorrect, int scoreValue)
            {
                if (answer == (theCorrect-1))
                    return scoreValue;
                else if (singleBackOff != 0)
                    return scoreValue * (-1) * singleBackOff / 100;
                else
                    return 0;
            }

            public int MultiCheck(int[] answer, int[] theCorrect, int totalOption, int scoreValue)
            {
                int[] scoreBoard = new int[totalOption];

                foreach (int c in theCorrect)
                    scoreBoard[c] = -1;

                foreach (int c in answer)
                    scoreBoard[c] += 2;

                if (multiPartial == 0) // 最通常的情况
                {
                    foreach (int c in scoreBoard)
                        if (c < 0 || c == 2) // 有没有选上的则为-1， 有不是答案里面的，则为2
                        {
                            if (multiBackOff != 0)
                                return scoreValue * (-1) * multiBackOff / 100;
                            else
                                return 0;
                        }
                    return scoreValue;
                }
                else
                {
                    int hit = 0; // 误报1个则没有分
                    foreach (int c in scoreBoard)
                    {
                        if (c == 1) hit += 1;
                        else if (c == 2)
                        {
                            hit = 0;
                            break;
                        }
                    }
                    if (hit == 0)
                        return 0;
                    else if (hit == theCorrect.Length)
                        return scoreValue;
                    else
                    {
                        if (multiPartial == 1) // 按照答案个数做分母来算分
                            return hit * scoreValue / theCorrect.Length;
                        else  // 按照候选的个数做分母得分
                            return hit * scoreValue / totalOption;
                    }
                }

                if (answer == theCorrect)
                    return scoreValue;
                else if (singleBackOff != 0)
                    return scoreValue * (-1) * singleBackOff / 100;
                else
                    return 0;
            }

            public int TFCheck(int answer, int theCorrect, int scoreValue)
            {
                if (answer == theCorrect)
                    return scoreValue;
                else if (TFBackOff != 0)
                    return scoreValue * (-1) * TFBackOff / 100;
                else
                    return 0;
            }

            public int ShortCheck(string answer, string policy, int scoreValue)
            {
                // 依据复杂的判断策略对简答题进行判卷，这是一个需要研究的课题
                // 当前留有人工判卷的接口 

                return 0;
            }
        }
    }
}

