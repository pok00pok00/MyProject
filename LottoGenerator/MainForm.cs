using System;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace LottoGenerator
{
    public partial class MainForm : Form
    {
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            numberList.GridLines = true;
            numberList.FullRowSelect = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            const int NumCount = 45;
            string sFileName;
            int[,] fullNumberList;
            int[] wholeNumbers = new int[] {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45};
            int[] numberValues = new int[NumCount];
            int[] bonusNumberValues = new int[NumCount];
            int lastRow;
            int tempNumber;
            int[,] luckyNumbers = new int[9,7];

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                sFileName = openFileDialog1.FileName;
            else
                return;
            
            MyApp  = new Excel.Application();
            MyApp.Visible = false;

            MyBook = MyApp.Workbooks.Open(sFileName);
            MySheet = MyBook.Sheets[1];

            lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
            fullNumberList = new int[lastRow, 7];
            for (int index = 1; index<=lastRow; index++)
            {
                Array MyValues = (System.Array)MySheet.get_Range("A" + index.ToString(), "I" + index.ToString()).Cells.Value;
                for (int arrIndex = 0; arrIndex <=6; arrIndex++)
                {
                    fullNumberList[index - 1, arrIndex] = Convert.ToInt16(MyValues.GetValue(1, arrIndex + 3).ToString());
                    if (arrIndex == 6)
                        bonusNumberValues[fullNumberList[index - 1, arrIndex] - 1]++;
                    else
                        numberValues[fullNumberList[index - 1, arrIndex] - 1]++;                                          
                }                                
            }            
            MyBook.Close();
            MyApp.Quit();

            // 1. Pick top 6 frequent numbers, 2. Pick top 6 rare numbers
            Array.Sort(numberValues, wholeNumbers);
            luckyNumbers[0, 0] = wholeNumbers[44];
            luckyNumbers[0, 1] = wholeNumbers[43];
            luckyNumbers[0, 2] = wholeNumbers[42];
            luckyNumbers[0, 3] = wholeNumbers[41];
            luckyNumbers[0, 4] = wholeNumbers[40];
            luckyNumbers[0, 5] = wholeNumbers[39];
            luckyNumbers[1, 0] = wholeNumbers[0];
            luckyNumbers[1, 1] = wholeNumbers[1];
            luckyNumbers[1, 2] = wholeNumbers[2];
            luckyNumbers[1, 3] = wholeNumbers[3];
            luckyNumbers[1, 4] = wholeNumbers[4];
            luckyNumbers[1, 5] = wholeNumbers[5];
            Array.Sort(wholeNumbers);
            Array.Sort(bonusNumberValues, wholeNumbers);
            luckyNumbers[0, 6] = wholeNumbers[44];
            luckyNumbers[1, 6] = wholeNumbers[0];

            // 3. Poisson distribute - it's same to rare numbers

            // 4. Fibonacci numbers with my birthday
            luckyNumbers[2, 0] = 8;
            luckyNumbers[2, 1] = 2;
            for(int index = 2; index<=6; index++)
            {
                tempNumber = luckyNumbers[2, index - 2] + luckyNumbers[2, index - 1];
                if (tempNumber <= 45)
                    luckyNumbers[2, index] = tempNumber;
                else
                {
                    tempNumber = tempNumber - 45;
                    luckyNumbers[2, index] = tempNumber;
                    if (FindValue(luckyNumbers, 2, tempNumber))
                        luckyNumbers[2, index] = tempNumber + luckyNumbers[2, index - 1];                    
                }
            }

            // 5. Fibonacci numbers with Father's birthday
          
            // 6. Fibonacci numbers with Mother's birthday

            // 7. Fibonacci numbers with sister's birthday

            // 8. Fibonacci numbers with Gayoon's birthday

            // 9. Fibonacci numbers with Nayoon's birthday

            // 10. Fibonacci numbers with wife's birthday
            for (int index = 0; index <=8; index++)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(luckyNumbers[index, 0].ToString());
                item.SubItems.Add(luckyNumbers[index, 1].ToString());
                item.SubItems.Add(luckyNumbers[index, 2].ToString());
                item.SubItems.Add(luckyNumbers[index, 3].ToString());
                item.SubItems.Add(luckyNumbers[index, 4].ToString());
                item.SubItems.Add(luckyNumbers[index, 5].ToString());
                item.SubItems.Add(luckyNumbers[index, 6].ToString());
                numberList.Items.Add(item);
            }
        }

        private bool FindValue(int[,] numArray, int arrindex, int value)
        {
            bool bFind = false;
            int colIndex;
            for (colIndex = 0; colIndex <= 5; colIndex++)
            {
                if (numArray[arrindex, colIndex] == value)
                {
                    bFind = true;
                    break;
                }
            }
            return bFind;
        }
    }
}
