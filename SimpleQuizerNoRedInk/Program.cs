using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleQuizerNoRedInk
{
    public class Strand
    {
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
        }

        public static string GetQuestions()
        {
            return
             System.IO.File.ReadAllText(@"questions.csv");
        }
    }
}
