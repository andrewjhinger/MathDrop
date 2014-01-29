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
    public partial class MathDrop : Form
    {
        private int _squaresCount = 5;                  // Number of squares (horizontal and vertical)
        private int _numberOfQuestions = 15;            // Number of questions
        private Button[,] _buttons;                     // Buttons array
        private Question[] _questions;                  // Questions array
        private Question[][] _answers;                  // Answers jagged array

        //to access array in results form (AH)

        public MathDrop()
        {
            // Create the Button controls and arrays
            createButtonsAndArrays();

            InitializeComponent();
            // Clear questions and answers, create questions and populate ListBox
            resetProgram();
        }

        private void createButtonsAndArrays()
        {
            // Allocate a two-dimensional array (grid) for our squares using Button controls
            _buttons = new Button[_squaresCount, _squaresCount];

            // Allocate one-dimensional array for our questions
            _questions = new Question[_numberOfQuestions];

            // Allocate a two-dimensional jagged array for our answers
            _answers = new Question[_squaresCount * _squaresCount][];

            // Sequential answer
            int buttonAnswer = 0;

            // Create each Button control in our array
            for (int row = 0; row < _squaresCount; row++)
            {
                for (int col = 0; col < _squaresCount; col++)
                {
                    // Create and assign new Button
                    Button button = new Button()
                    {
                        Text = buttonAnswer.ToString(),
                        BackColor = Color.White,
                        FlatStyle = FlatStyle.Popup,
                        Cursor = Cursors.Hand,
                        AllowDrop = true
                    };

                    _buttons[row, col] = button;

                    // Increment answer value
                    buttonAnswer++;

                    // Add the same event handler for the DragEnter event for each Button
                    _buttons[row, col].DragEnter += new DragEventHandler(buttonDragEnterHandler);

                    // Add the same event handler for the DragDrop event for each Button
                    _buttons[row, col].DragDrop += new DragEventHandler(buttonDragDropHandler);
                }
            }

        }

        void buttonDragEnterHandler(object sender, DragEventArgs e)
        {
            // Only show drag effect for data of the correct type
            if (!e.Data.GetDataPresent(typeof(Question)))
                e.Effect = DragDropEffects.None;
            else
                // Show the Move effect cursor
                e.Effect = DragDropEffects.Move;
        }

        void buttonDragDropHandler(object sender, DragEventArgs e)
        {
            // Only process drag operation if correct data type
            if (e.Data.GetDataPresent(typeof(Question)))
            {
                // Extract question from drag data, and make sure the question is not null
                Question question = (Question)e.Data.GetData(typeof(Question));
                if (question != null)
                {
                    Button button = sender as Button;
                    MessageBox.Show(button.Text + " selected");

                    // Store all of the answers in the _answers jagged array
                    int answer;
                    if (!int.TryParse(button.Text, out answer))
                        answer = 0;
                    if (_answers[answer] == null)
                        // The jagged array hasn't been created, so create the first entry
                        _answers[answer] = new Question[1];
                    else
                        // Already have entries in jagged array, resize to length + 1
                        Array.Resize(ref _answers[answer], _answers[answer].Length + 1);

                    // Add the question to the jagged array
                    _answers[answer][_answers[answer].Length - 1] = question;

                    // Test if correct answer, display dialog box, remove from ListBox
                    if (button.Text == question.TheAnswer.ToString())
                    {
                        // Remove the question from the ListBox
                        questionsListBox.Items.RemoveAt(questionsListBox.SelectedIndex);
                        MessageBox.Show(button.Text + " is correct!");
                    }
                    else
                        MessageBox.Show("Wrong answer, try again.");
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Exit the application
            this.Close();
        }

        private void MathDrop_Layout(object sender, LayoutEventArgs e)
        {
            // Get the height and width for each Button using the inside height and
            // width of the Panel control
            int buttonWidth = mainSplitContainer.Panel2.ClientRectangle.Width / _squaresCount;
            int buttonHeight = mainSplitContainer.Panel2.ClientRectangle.Height / _squaresCount;

            // Loop through and set the location and dimensions of each Button control
            for (int row = 0; row < _squaresCount; row++)
            {
                for (int col = 0; col < _squaresCount; col++)
                    _buttons[row, col].SetBounds(buttonWidth * col, buttonHeight * row, buttonWidth, buttonHeight);
            }

            // Add the Button controls to the Panel Controls collection
            if (mainSplitContainer.Panel2.Controls.Count == 0)
            {
                for (int row = 0; row < _squaresCount; row++)
                {
                    for (int col = 0; col < _squaresCount; col++)
                        mainSplitContainer.Panel2.Controls.Add(_buttons[row, col]);
                }
            }
        }

        private void resetProgram()
        {
            Array.Clear(_questions, 0, _questions.Length);

            // Clear all of the answers from the jagged array and set the array index to null
            for (int i = 0; i < _answers.Length; i++)
            {
                if (_answers[i] != null)
                {
                    Array.Clear(_answers[i], 0, _answers[i].Length);
                    _answers[i] = null;
                }
            }

            int maxAnswer = _squaresCount * _squaresCount - 1;

            Random random = new Random();
            for (int i = 0; i < _numberOfQuestions; i++)
            {
                int randomLeft = random.Next(0, maxAnswer + 1);
                int randomRight = random.Next(0, maxAnswer - randomLeft + 1);
                int answer = randomLeft + randomRight;
                string question = randomLeft.ToString() + " + " + randomRight.ToString();
                _questions[i] = new Question()
                {
                    TheQuestion = question,
                    TheAnswer = answer
                };
            }

            // Reset the questions in ListBox
            questionsListBox.Items.Clear();
            for (int i = 0; i < _questions.Length; i++)
                questionsListBox.Items.Add(_questions[i]);
        }

        private void mainSplitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            // Respond to the Reset menu option being selected
            resetProgram();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Respond to the Reset menu option being selected
            resetProgram();
        }

        private void questionsListBox_MouseMove(object sender, MouseEventArgs e)
        {
            // Make sure we have the left mouse button down, and a valid list view item under mouse
            if (e.Button == MouseButtons.Left)
            {
                if (questionsListBox.SelectedIndex != ListBox.NoMatches)
                    questionsListBox.DoDragDrop((Question)questionsListBox.Items[questionsListBox.SelectedIndex], DragDropEffects.All);
            }
        }

        private void resultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResultsForm resultsForm = new ResultsForm(_answers);
            resultsForm.Show();



        }
    }
}
