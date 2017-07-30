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
            //This part was not fully tested might not work 100%
            Console.WriteLine(
                string.Join(",",
            r.OrderBy(l => l.Difficulty)
                .Take(int.Parse(args[0]))
                .Select(l=> l.QuestionId)));
        }

        public static Parser<IEnumerable<CsvLine>> ParseLines()
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

        public static Parser<CsvLine> ParseLine()
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
                select new CsvLine
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

    public class CsvLine
    {
        public int StrandId;
        public string StrandName;
        public int StandardId;
        public string StandardName;
        public int QuestionId;
        public double Difficulty;
    }
}
