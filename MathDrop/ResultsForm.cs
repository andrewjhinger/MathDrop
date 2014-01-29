using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MathDrop
{
    public partial class ResultsForm : Form
    {
        private Question[][] answers;            // Questions array


        public ResultsForm(Question [][] _answers)
        {
            this.answers = _answers;
            
            InitializeComponent();
            displayResults();

        }

        private void displayResults()
        {
            string[] stringAnswers = Array.ConvertAll(answers, element => element.ToString());


            //for (int i = 0; i < answers.Length; i++)
            foreach (string i in stringAnswers)
            {



                resultsListBox.Items.Add(i);
            }
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
