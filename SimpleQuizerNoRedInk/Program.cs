using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;

namespace SimpleQuizerNoRedInk
{
    public class Strand
    {
        public int Id;
        public string Name;
        public IEnumerable<Standard> Standards;
    }

    public class Standard
    {
        public string Name;
        public IEnumerable<Question> Questions;
    }

    public class Question
    {
        public float Difficulty;
    }

    class Program
    {
        static void Main(string[] args)
        {
            var mainParser =
                from _ in ConsumeHeader()
                from lines in ParseLines()
                select lines;

            var r = mainParser.Parse(GetQuestions());

        }

        public static Parser<IEnumerable<NonParsedLine>> ParseLines()
        {
            return ParseLine().Many();
        }

        public static Parser<Double> ParseDif()
        {
            return
                from n1 in Parse.Number
                from dot in Parse.Char('.')
                from n2 in Parse.Number
                select toDouble(n1 + dot + n2);
        }

            public static Func<IEnumerable<char>,string> toString =
                cs=> new string(cs.ToArray());

            public static Func<IEnumerable<char>,int> toInt =
                cs=> int.Parse(toString(cs));

            public static Func<IEnumerable<char>,double> toDouble =
                cs=> Double.Parse(toString(cs));

        public static Parser<NonParsedLine> ParseLine()
        {


            return
                from strandId in Parse.Numeric.Until(Parse.Char(','))
                from strandName in Parse.Letter.Until(Parse.Char(','))
                from standardId in Parse.Numeric.Until(Parse.Char(','))
                from standardName in Parse.LetterOrDigit.Or(Parse.WhiteSpace)
                                     .Until(Parse.Char(','))
                from questionId in Parse.Numeric.Until(Parse.Char(','))
                from dif in ParseDif()
                from _ in Parse.String(Environment.NewLine)
                select new NonParsedLine
                           {
                               StrandId = toInt(strandId),
                               StrandName = toString(strandName),
                               StandardId = toInt(standardId),
                               StandardName = toString(strandName),
                               QuestionId = toInt(questionId),
                               Difficulty = dif
                           };
        }


        public static Parser<IEnumerable<char>> ConsumeHeader()
        {
            return Parse.String(
                @"strand_id,strand_name,standard_id,standard_name,question_id,difficulty"
                + Environment.NewLine
                );
        }

        public static string GetQuestions()
        {
            return
             System.IO.File.ReadAllText(@"questions.csv");
        }
    }

    public class NonParsedLine
    {
        public int StrandId;
        public string StrandName;
        public int StandardId;
        public string StandardName;
        public int QuestionId;
        public double Difficulty;
    }
}
