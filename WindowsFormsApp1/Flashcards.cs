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
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Flashcards : Form
    {
        
        //private FolderBrowserDialog folderBrowserDialog1;
        private OpenFileDialog openFileDialog1;

        private RichTextBox richTextBox1;

        private MainMenu mainMenu1;
        private MenuItem fileMenuItem, openMenuItem;
        private MenuItem folderMenuItem, closeMenuItem;

        private string openFileName, folderName;

        private bool fileOpened = false;

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

        public Flashcards()
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

        protected void getNextCardEnglishToChinese(int n)
        {
            Random rdm = new Random();
            if (numbers.Count > 0)
            {
                int randomIndex = rdm.Next(0, numbers.Count);
                int cardNumber = numbers[randomIndex];

                if (deck[cardNumber].noCorrect >= n)
                {
                    numbers.Remove(cardNumber);
                    getNextCardEnglishToChinese(n);
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
                    lblChineseToEnglishHint.Text = deck[cardNumber].pinyin;
                    lblTimesAnsweredCorrectly2.Text = deck[cardNumber].noCorrect.ToString();
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
                if (path != "" && path != null)
                {
                    lblCardNumberHeader.Visible = true;
                    lblTimesAnsweredCorrectlyHeader.Visible = true;
                    lblCardNumberHeader2.Visible = true;
                    lblTimesAnsweredCorrectlyHeader2.Visible = true;
                    btnEnglishToChinese.Visible = true;
                    btnReviewEnglishToChinese.Visible = true;
                    
                    btnChineseToEnglish.Visible = true;
                    //btnChineseToEnglishHint.Visible = true;
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

            // This line calls the folder diag

            // This is what will execute if the user selects a folder and hits OK (File if you change to FileBrowserDialog)
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string folder = folderBrowserDialog1.SelectedPath;
                try
                {
                    System.IO.File.WriteAllText(folder + @"\" + tbDeckName.Text + ".txt", contents);
                }
                catch
                {

                }
            }
            else
            {
                // This prevents a crash when you close out of the window with nothing
            }

            //System.IO.File.WriteAllText(@"C:\Users\alexn\Desktop\WindowsFormsApp1\Decks" + tbDeckName.Text + ".txt", contents);
        }

        /********************************************************
         *  3. Test English to Chinese                          *
         * *****************************************************/
         /* Start the test */
        private void btnEnglishToChinese_Click(object sender, EventArgs e)
        {
            btnGetFile.Visible = false;
            lblDeckName.Visible = false;
            lblEnglishToChinese.Visible = true;
            lblEnglishToChineseHintHeader.Visible = true;
            tbEnglishToChineseTest.Visible = true;
            tbEnglishToChineseAnswer.Visible = true;
            btnEnglishToChineseCorrect.Visible = true;
            btnEnglishToChineseSeeCorrect.Visible = true;
            btnEnglishToChineseHint.Visible = true;

            if (btnReviewEnglishToChinese.Visible == true)
            {
                btnReviewEnglishToChinese.Visible = false;
            }
            if (btnChineseToEnglish.Visible == true)
            {
                btnChineseToEnglish.Visible = false;
            }
            
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

            getNextCardEnglishToChinese(5);
        }

        /* Mark answer as correct and get new card */
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
            lblEnglishToChineseHint.Visible = false;
            getNextCardEnglishToChinese(5);
        }

        /* View the correct answer */
        private void btnEnglishToChineseSeeCorrect_Click(object sender, EventArgs e)
        {
            if (tbEnglishToChineseAnswer.Text != deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].chinese.ToLower())
            {
                tbEnglishToChineseAnswer.Text = deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].chinese.ToLower();
            }
            else
            {
                tbEnglishToChineseAnswer.Text = String.Empty;
            }
            //if (tbEnglishToChineseAnswer.Text.ToLower() != deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].chinese.ToLower())
            //{
            //    deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].noIncorrect++;
            //}
            //tbEnglishToChineseAnswer.Text = String.Empty;
            //lblEnglishToChineseHint.Text = String.Empty;
            //getNextCardEnglishToChinese();
        }

        /* Show the pinyin for the current card */
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
         /* Start the test */
        private void btnChineseToEnglish_Click(object sender, EventArgs e)
        {
            btnGetFile.Visible = false;
            lblDeckName.Visible = false;
            lblCardNumberHeader2.Visible = true;
            lblTimesAnsweredCorrectlyHeader2.Visible = true;
            btnChineseToEnglish.Visible = true;
            lblChineseToEnglish.Visible = true;
            lblChineseToEnglishHintHeader.Visible = true;
            tbChineseToEnglishTest.Visible = true;
            tbChineseToEnglishAnswer.Visible = true;
            btnChineseToEnglishCorrect.Visible = true;
            btnChineseToEnglishSeeCorrect.Visible = true;
            btnChineseToEnglishHint.Visible = true;

            if (btnEnglishToChinese.Visible == true)
            {
                btnEnglishToChinese.Visible = false;
            }
            if (btnReviewEnglishToChinese.Visible == true)
            {
                btnReviewEnglishToChinese.Visible = false;
            }

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

        /* Mark answer as correct and get new card */
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
            lblChineseToEnglishHint.Visible = false;
            getNextCardChineseToEnglish();
        }

        /* Show the correct answer for the current card */
        private void btnChineseToEnglishSeeCorrect_Click(object sender, EventArgs e)
        {
            if (tbChineseToEnglishAnswer.Text != deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].english.ToLower())
            {
                tbChineseToEnglishAnswer.Text = deck[Convert.ToInt32(lblCardNumberChineseToEnglish.Text)].english.ToLower();
            }
            else
            {
                tbChineseToEnglishAnswer.Text = String.Empty;
            }
        }

        /* Show the pinyin for the current card */
        private void btnChineseToEnglishHint_Click(object sender, EventArgs e)
        {
            if (lblChineseToEnglishHint.Visible != true)
            {
                lblChineseToEnglishHint.Visible = true;
            }
            else
            {
                lblChineseToEnglishHint.Visible = false;
            }
        }

        private void tbEnglishToChineseAnswer_TextChanged(object sender, EventArgs e)
        {

        }

        // REVIEW
        private void btnReviewEnglishToChinese_Click(object sender, EventArgs e)
        {
            btnGetFile.Visible = false;
            lblDeckName.Visible = false;
            lblEnglishToChinese.Visible = true;
            lblEnglishToChineseHintHeader.Visible = true;
            tbEnglishToChineseTest.Visible = true;
            tbEnglishToChineseAnswer.Visible = true;
            btnEnglishToChineseReviewCorrect.Visible = true;
            btnEnglishToChineseSeeCorrect.Visible = true;
            btnEnglishToChineseHint.Visible = true;

            if (btnEnglishToChinese.Visible == true)
            {
                btnEnglishToChinese.Visible = false;
            }
            if (btnChineseToEnglish.Visible == true)
            {
                btnChineseToEnglish.Visible = false;
            }

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

            getNextCardEnglishToChinese(1);
        }

        private void btnEnglishToChineseReviewCorrect_Click(object sender, EventArgs e)
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
            lblEnglishToChineseHint.Visible = false;
            getNextCardEnglishToChinese(1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }
    }

    ///** IN ORDER
    //private void button1_Click(object sender, EventArgs e)
    //{
    //    int counterEnglish = 0;
    //    int counterChinese = counterEnglish + 1;
    //    int counterPinyin = counterChinese + 1;
    //    string pinyinLine = "";
    //    string chineseLine = "";
    //    string englishLine = "";
    //    populateNumbers(numbers);

    //    // Read the file and display it line by line.  
    //    //System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\alexn\Desktop\test.txt");
    //    System.IO.StreamReader file = new System.IO.StreamReader(@"" + loadedDeck);

    //    for (int i = 0; i < 60; i = i + 3)
    //    {
    //        Card card = new Card();
    //        pinyinLine = File.ReadLines(@"" + loadedDeck).Skip(i).Take(1).First();
    //        chineseLine = File.ReadLines(@"" + loadedDeck).Skip(i + 1).Take(1).First();
    //        englishLine = File.ReadLines(@"" + loadedDeck).Skip(i + 2).Take(1).First();

    //        card.pinyin = counterEnglish.ToString(pinyinLine);
    //        card.chinese = counterChinese.ToString(chineseLine);
    //        card.english = counterPinyin.ToString(englishLine);

    //        deck.Add(card);
    //    }

    //    file.Close();
    //    System.Console.ReadLine();

    //    lblCardNumberEnglishToChinese.Text = 0.ToString();
    //    tbEnglishToChineseTest.Text = deck[0].english;
    //    lblEnglishToChineseHint.Text = deck[0].pinyin;
    //    lblTimesAnsweredCorrectly.Text = deck[0].noCorrect.ToString();
    //}

    //private void btnInOrderEnglishToChinese_Click(object sender, EventArgs e)
    //{
    //    int damnNumber = Convert.ToInt32(lblCardNumberEnglishToChinese.Text);
    //    lblCardNumberEnglishToChinese.Text = (damnNumber + 1).ToString();
    //    tbEnglishToChineseTest.Text = deck[Convert.ToInt32(lblCardNumberEnglishToChinese.Text)].english;
    //    lblEnglishToChineseHint.Text = deck[damnNumber + 1].pinyin;
    //} **/       

    

}



