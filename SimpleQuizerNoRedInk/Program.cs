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
            var file = GetQuestions();
            var restult =
                from _ in ConsumeHeader()
                from lines in ParseLines()
                select lines;

        }

        public static Parser<IEnumerable<NonParsedLine>> ParseLines()
        {
            return ParseLine().Many();
        }

        public static Parser<NonParsedLine> ParseLine()
        {

            Func<IEnumerable<char>,string> toString =
                cs=> new string(cs.ToArray());

            Func<IEnumerable<char>,int> toInt =
                cs=> int.Parse(toString(cs));

            Func<IEnumerable<char>,double> toDouble =
                cs=> Double.Parse(toString(cs));

            return
                from strandId in Parse.Numeric.Until(Parse.Char(','))
                from strandName in Parse.Letter.Until(Parse.Char(','))
                from standardId in Parse.Numeric.Until(Parse.Char(','))
                from standardName in Parse.Letter.Until(Parse.Char(','))
                from dif in Parse.Numeric.Until(Parse.LineEnd)

                select new NonParsedLine
                           {
                               StrandId = toInt(strandId),
                               StrandName = toString(strandName),
                               StandardId = toInt(standardId),
                               StandardName = toString(strandName),
                               Difficulty = toDouble(dif)
                           };
        }


        public static Parser<IEnumerable<char>> ConsumeHeader()
        {
            return Parse.String(
                @"strand_id,strand_name,standard_id,standard_name,question_id,difficulty"
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
        public double Difficulty;
    }
}
