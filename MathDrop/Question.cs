using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathDrop
{
    public class Question
    {
        private string _theQuestion;
        public string TheQuestion
        {
            get { return _theQuestion; }
            set { _theQuestion = value; }
        }

        private int _theAnswer;
        public int TheAnswer
        {
            get { return _theAnswer; }
            set { _theAnswer = value; }
        }

        // Override ToString() so when adding array to ListBox the ListBox will
        // call this version of ToString to display the question
        public override string ToString()
        {
            return _theQuestion;
        }
    }
}