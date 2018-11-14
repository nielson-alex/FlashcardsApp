using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Card> deck = new List<Card>();
        List<int> numbers = new List<int>();
        string loadedDeck = "";

        public class Card
        {
            public string english { get; set; }
            public string chinese { get; set; }
            public string pinyin { get; set; }
            public int noCorrect { get; set; }
            public int noIncorrect { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
        }

        /*******************************************************
         *  Global Functions                                   *
         * *****************************************************/
        private void populateNumbers(List<int> numbers)
        {
            for (int i = 0; i < 20; i++)
            {
                numbers.Add(i);
            }
        }

        private List<Card> buildDeck(string english, string chinese, string pinyin)
        {
            List<Card> deck = new List<Card>();
            Card card = new Card();
            card.english = english;
            card.chinese = chinese;
            card.pinyin = pinyin;
            deck.Add(card);
            return deck;
        }

        protected void getNextCardEnglishToChinese()
        {
            Random rdm = new Random();
            if (numbers.Count > 0)
            {
                int randomIndex = rdm.Next(0, numbers.Count);
                int cardNumber = numbers[randomIndex];

                if (deck[cardNumber].noCorrect >= 5)
                {
                    numbers.Remove(cardNumber);
                    getNextCardEnglishToChinese();
                }
                else
                {
                    lblCardNumberEnglishToChinese.Text = cardNumber.ToString();
                    tbEnglishToChineseTest.Text = deck[cardNumber].english;
                    lblEnglishToChineseHint.Text = deck[cardNumber].pinyin;
                    lblTimesAnsweredCorrectly.Text = deck[cardNumber].noCorrect.ToString();
                }
            }
            else
            {
                lblCardNumberEnglishToChinese.Text = "Congratulations! You have mastered the English to Chinese version this deck";
                tbEnglishToChineseTest.Text = String.Empty;
            }
        }

        protected void getNextCardChineseToEnglish()
        {
            Random rdm = new Random();
            if (numbers.Count > 0)
            {
                int randomIndex = rdm.Next(0, numbers.Count);
                int cardNumber = numbers[randomIndex];

                if (deck[cardNumber].noCorrect >= 5)
                {
                    numbers.Remove(cardNumber);
                    getNextCardChineseToEnglish();
                }
                else
                {
                    lblCardNumberChineseToEnglish.Text = cardNumber.ToString();
                    tbChineseToEnglishTest.Text = deck[cardNumber].chinese;
                    lblEnglishToChineseHint.Text = deck[cardNumber].pinyin;
                    lblTimesAnsweredCorrectly.Text = deck[cardNumber].noCorrect.ToString();
                }
            }
            else
            {
                lblCardNumberChineseToEnglish.Text = "Congratulations! You have mastered the English to Chinese version this deck";
                tbChineseToEnglishTest.Text = String.Empty;
            }
        }

        /********************************************************
         *  1. Load Deck                                        *
         * *****************************************************/
        private void btnGetFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files | *.txt"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                String path = dialog.FileName; // get name of file
                loadedDeck = path;
                lblDeckName.Text = loadedDeck;
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), new UTF8Encoding())) // do anything you want, e.g. read it
                {
                    // ...
                }
            }
        }

        /********************************************************
         *  2. Save Deck                                        *
         * *****************************************************/
        private void btnSave_Click(object sender, EventArgs e)
        {
            string contents = "";
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i].GetType().ToString() == "System.Windows.Forms.TextBox" &&
                    this.Controls[i].Name != "tbEnglishToChineseTest" &&
                    this.Controls[i].Name != "tbEnglishToChineseAnswer" &&
                    this.Controls[i].Name != "tbChineseToEnglishTest" &&
                    this.Controls[i].Name != "tbChineseToEnglishAnswer" &&
                    this.Controls[i].Name != "tbDeckName" )
                {
                    if (i == 0)
                    {
                        contents = this.Controls[i].Text + Environment.NewLine;
                    }
                    else if (i == (this.Controls.Count - 1))
                    {
                        contents = contents + this.Controls[i].Text;
                    }
                    else
                    {
                        contents = contents + this.Controls[i].Text + Environment.NewLine;
                    }
                }
            }
            
            System.IO.File.WriteAllText(@"C:\Users\alexn\Desktop\" + tbDeckName.Text + ".txt", contents);
        }

        /********************************************************
         *  3. Test English to Chinese                          *
         * *****************************************************/
        private void btnEnglishToChinese_Click(object sender, EventArgs e)
        {
            int counterEnglish = 0;
            int counterChinese = counterEnglish + 1;
            int counterPinyin = counterChinese + 1;
            string pinyinLine = "";
            string chineseLine = "";
            string englishLine = "";
            populateNumbers(numbers);

            // Read the file and display it line by line.  
            //System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\alexn\Desktop\test.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(@"" + loadedDeck);

            for (int i = 0; i < 60; i = i + 3)
            {
                Card card = new Card();
                pinyinLine = File.ReadLines(@"" + loadedDeck).Skip(i).Take(1).First();
                chineseLine = File.ReadLines(@"" + loadedDeck).Skip(i + 1).Take(1).First();
                englishLine = File.ReadLines(@"" + loadedDeck).Skip(i + 2).Take(1).First();

                card.pinyin = counterEnglish.ToString(pinyinLine);
                card.chinese = counterChinese.ToString(chineseLine);
                card.english = counterPinyin.ToString(englishLine);
                
                deck.Add(card);
            }

            file.Close();
            System.Console.ReadLine();

            getNextCardEnglishToChinese();
        }

        private void btnEnglishToChineseCorrect_Click(object sender, EventArgs e)
        {
            if (tbEnglishToChineseAnswer.Text.ToLower() == deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].chinese.ToLower())
            {
                deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].noCorrect++;
                lblEnglishToChineseRightOrWrong.Text = "Correct!";
            }
            else
            {
                deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].noIncorrect++;
                lblEnglishToChineseRightOrWrong.Text = "Incorrect";
            }
            tbEnglishToChineseAnswer.Text = String.Empty;
            lblEnglishToChineseHint.Text = String.Empty;
            getNextCardEnglishToChinese();
        }

        private void btnEnglishToChineseIncorrect_Click(object sender, EventArgs e)
        {
            tbEnglishToChineseAnswer.Text = deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].chinese.ToLower();
            //if (tbEnglishToChineseAnswer.Text.ToLower() != deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].chinese.ToLower())
            //{
            //    deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].noIncorrect++;
            //}
            //tbEnglishToChineseAnswer.Text = String.Empty;
            //lblEnglishToChineseHint.Text = String.Empty;
            //getNextCardEnglishToChinese();
        }

        private void btnEnglishToChineseHint_Click(object sender, EventArgs e)
        {
            if (lblEnglishToChineseHint.Visible != true)
            {
                lblEnglishToChineseHint.Visible = true;
            }
            else
            {
                lblEnglishToChineseHint.Visible = false;
            }
        }

        /********************************************************
         *  4. Test Chinese to English                          *
         * *****************************************************/
        private void btnChineseToEnglish_Click(object sender, EventArgs e)
        {
            int counterEnglish = 0;
            int counterChinese = counterEnglish + 1;
            int counterPinyin = counterChinese + 1;
            string pinyinLine = "";
            string chineseLine = "";
            string englishLine = "";
            populateNumbers(numbers);

            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(@"" + loadedDeck);

            for (int i = 0; i < 60; i = i + 3)
            {
                Card card = new Card();
                pinyinLine = File.ReadLines(@"" + loadedDeck).Skip(i).Take(1).First();
                chineseLine = File.ReadLines(@"" + loadedDeck).Skip(i + 1).Take(1).First();
                englishLine = File.ReadLines(@"" + loadedDeck).Skip(i + 2).Take(1).First();

                card.pinyin = counterEnglish.ToString(pinyinLine);
                card.chinese = counterChinese.ToString(chineseLine);
                card.english = counterPinyin.ToString(englishLine);

                deck.Add(card);
            }

            file.Close();
            System.Console.ReadLine();

            getNextCardChineseToEnglish();
        }

        private void btnChineseToEnglishCorrect_Click(object sender, EventArgs e)
        {
            if (tbChineseToEnglishAnswer.Text.ToLower() == deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].english.ToLower())
            {
                deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].noCorrect++;
                lblChineseToEnglishRightOrWrong.Text = "Correct!";
            }
            else
            {
                deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].noIncorrect++;
                lblChineseToEnglishRightOrWrong.Text = "Incorrect";
            }
            tbChineseToEnglishAnswer.Text = String.Empty;
            lblChineseToEnglishHint.Text = String.Empty;
            getNextCardChineseToEnglish();
        }

        private void btnChineseToEnglishIncorrect_Click(object sender, EventArgs e)
        {
            tbChineseToEnglishAnswer.Text = deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].english.ToLower();
            //if (tbChineseToEnglishAnswer.Text.ToLower() != deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].english.ToLower())
            //{
            //    deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].noIncorrect++;
            //}
            //tbChineseToEnglishAnswer.Text = String.Empty;
            //lblChineseToEnglishHint.Text = String.Empty;
            //getNextCardChineseToEnglish();
        }

        private void btnChineseToEnglishHint_Click(object sender, EventArgs e)
        {
            lblChineseToEnglishHint.Text = deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].pinyin;
        }

        private void tbEnglishToChineseAnswer_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
