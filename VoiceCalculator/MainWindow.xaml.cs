using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Text.RegularExpressions;

namespace VoiceCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected SpeechRecognizer sr;
        private GrammarBuilder gb = new GrammarBuilder();

        Double lastValue = 0, firstOperand = 0, secondOperand = 0;
        OpeartionType operationToDo;
        char lastOperationSign;
        bool shouldReplaceValue = false;

        public MainWindow()
        {
            InitializeComponent();
            sr = new SpeechRecognizer();
            Choices choices = new Choices();
            string[] commands = new string[]  { "Plus", "Minus", "Divide", "Time",
                                                "Add", "Substract", "Multiply",
                                                "CE", "C", "Clear Entry", "Clear",
                                                "Modulo", "Square Root", "Equal",
                                                "X Squared"
                                                };
            choices.Add(commands);
            for (int i = 0; i < 1000; i++)
            {
                choices.Add("" + i);
            }
            gb.Append(choices);
            Grammar gr = new Grammar(gb);
            gb.Append(choices);
            sr.LoadGrammar(gr);
            sr.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);
            sr.Enabled = true;
        }
        void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            MessageBox.Show("Speech recognized: " + e.Result.Text);
        }


        private void txtResult_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ibtnVoiceCommand_TouchDown(object sender, TouchEventArgs e)
        {
            
        }
        private void ibtnVoiceCommand_TouchLeave(object sender, TouchEventArgs e)
        {
            //sr.Enabled = false;
        }

        private void btnStartListening_Click(object sender, RoutedEventArgs e)
        {
            sr.Enabled = true;
        }

        private void btnStopListening_Click(object sender, RoutedEventArgs e)
        {
            sr.Enabled = false;
        }



        //end number buttons

        //start control buttons


        private void AddCharToTextBox(char value)
        {
            if (Double.Parse(txtResult.Text) == 0 || shouldReplaceValue)
            {
                txtResult.Text = value.ToString();
            }
            else
            {
                txtResult.AppendText(value.ToString());
            }
        }


        private void DeleteLastCharacter()
        {
            string s = txtResult.Text;

            if (s.Length > 1)
            {
                s = s.Substring(0, s.Length - 1);
            }
            else
            {
                s = "0";
            }

            txtResult.Text = s;
        }

        private void ClearAllDisplay()
        {
            txtResult.Text = "0";
            lblHistory.Content = "";
            lastValue = 0;
        }

        private void txtResult_TextChanged(object sender, RoutedEventArgs e)
        {
            if (shouldReplaceValue)
                shouldReplaceValue = false;
        }

        private double Calculate(OpeartionType ot, double firstOperand, double secondOperand)
        {
            double result = 0;

            switch (ot)
            {
                case OpeartionType.ADDITION:
                    result = firstOperand + secondOperand;
                    break;
                case OpeartionType.SUBSTRACTION:
                    result = firstOperand - secondOperand;
                    break;
                case OpeartionType.MULTIPLICATION:
                    result = firstOperand * secondOperand;
                    break;
                case OpeartionType.DIVISION:
                    result = firstOperand / secondOperand;
                    break;
                case OpeartionType.MODULO:
                    result = firstOperand % secondOperand;
                    break;
                case OpeartionType.SQUARE_ROOT:
                    result = Math.Sqrt(firstOperand);
                    break;

            }

            return result;
        }

        public enum OpeartionType
        {
            ADDITION,
            SUBSTRACTION,
            MULTIPLICATION,
            DIVISION,
            MODULO,
            SQUARE_ROOT
        }


        //start number buttons
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('0');
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('1');
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('2');
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('3');
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('4');
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('5');
        }

        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('6');
        }

        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('7');
        }

        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('8');
        }

        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            AddCharToTextBox('9');
        }

        private void btnDot_Click(object sender, RoutedEventArgs e)
        {
            txtResult.AppendText(".");
        }


        //end number buttons

        //start control buttons

 
        private void ClearEntries()
        {
            lastOperationSign = ' ';
            firstOperand = secondOperand = lastValue = 0;
            ClearAllDisplay();
        }

        private void btnPlusMinus_Click(object sender, RoutedEventArgs e)
        {
        }
        //end control buttons


        //start operation buttons
        private void btnBackspace_Click(object sender, RoutedEventArgs e)
        {
            DeleteLastCharacter();
        }

        private void btnEqual_Click(object sender, RoutedEventArgs e)
        {
            secondOperand = GetValueFromTextBox();
            lastValue = Calculate(operationToDo, lastValue, secondOperand);
            txtResult.Text = "" + lastValue;
            lblHistory.Content = "";
            shouldReplaceValue = true;
            lastValue = 0;
        }

        private void btnOneDivideX_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "" + 1 / GetValueFromTextBox();
        }

        private void btnDivide_Click(object sender, RoutedEventArgs e)
        {
            CheckOperation();
            operationToDo = OpeartionType.DIVISION;
            lastOperationSign = OpeartoinSigns.DIVISION;
            lblHistory.Content = GenerateHistory();
        }

        private void btnMultiply_Click(object sender, RoutedEventArgs e)
        {
            CheckOperation();
            operationToDo = OpeartionType.MULTIPLICATION;
            lastOperationSign = OpeartoinSigns.MULTIPLICATION;
            lblHistory.Content = GenerateHistory();
        }

        private void btnModulo_Click(object sender, RoutedEventArgs e)
        {
            CheckOperation();
            operationToDo = OpeartionType.MODULO;
            lastOperationSign = OpeartoinSigns.MODULO;
            lblHistory.Content = GenerateHistory();
        }

        private void btnClearEntry_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "0";
            lastValue = 0;
        }

        private void btnSqrt_Click(object sender, RoutedEventArgs e)
        {
            CheckOperation();
            lastOperationSign = OpeartoinSigns.SQUARE_ROOT;
            operationToDo = OpeartionType.SQUARE_ROOT;
            lblHistory.Content = GenerateHistory();
        }

        private void btnXSuper2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtResult.Text = "0";
            lastValue = 0;
        }

        private void btnSubstract_Click(object sender, RoutedEventArgs e)
        {
            CheckOperation();
            operationToDo = OpeartionType.SUBSTRACTION;
            lastOperationSign = OpeartoinSigns.SUBSTRACTION;
            lblHistory.Content = GenerateHistory();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CheckOperation();
            operationToDo = OpeartionType.ADDITION;
            lastOperationSign = OpeartoinSigns.ADDITION;
            lblHistory.Content = GenerateHistory();
        }
        private void CheckOperation()
        {
            if (lastValue == 0 && string.IsNullOrEmpty(lblHistory.Content.ToString()))
            {
                lastValue = GetValueFromTextBox();
            }
            else
            {
                DoOperation(operationToDo);
            }
        }
        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            CheckOperation();
            operationToDo = OpeartionType.SUBSTRACTION;
            lastOperationSign = OpeartoinSigns.SUBSTRACTION;
            lblHistory.Content = GenerateHistory();
        }


  
 


        //end operation buttons

        private void DoOperation(OpeartionType ot)
        {
            switch (operationToDo)
            {
                case OpeartionType.ADDITION:
                    lastValue = lastValue + GetValueFromTextBox();
                    break;
                case OpeartionType.SUBSTRACTION:
                    lastValue = lastValue - GetValueFromTextBox();
                    break;
                case OpeartionType.MULTIPLICATION:
                    lastValue = lastValue * GetValueFromTextBox();
                    break;
                case OpeartionType.DIVISION:
                    lastValue = lastValue / GetValueFromTextBox();
                    break;
                case OpeartionType.MODULO:
                    lastValue = lastValue % GetValueFromTextBox();
                    break;
                case OpeartionType.SQUARE_ROOT:
                    lastValue = Math.Sqrt(GetValueFromTextBox());
                    break;
            }
        }

        private string GenerateHistory()
        {
            shouldReplaceValue = true;
            string h = "";
            if (String.IsNullOrEmpty(lblHistory.Content.ToString()))
                h = lastValue + " " + lastOperationSign;
            else
            {
                h =  lblHistory.Content.ToString() +  " " + GetValueFromTextBox() + " " + lastOperationSign ;
            }
            return h;
        }
        private Double GetValueFromTextBox()
        {
            Double value = 0;
            try
            {
                value = Double.Parse(txtResult.Text.Trim());
            }
            catch (Exception e)
            {

            }
            return value;
        }

        private void DisplayHistory(string newOperation)
        {
            if (String.IsNullOrEmpty(lblHistory.Content.ToString()))
                lblHistory.Content = newOperation;
            else
                lblHistory.Content = lblHistory.Content.ToString() + newOperation;
        }

        

        struct OpeartoinSigns
        {
            public static char ADDITION = '+';
            public static char SUBSTRACTION = '-';
            public static char MULTIPLICATION = '*';
            public static char DIVISION = '÷';
            public static char MODULO = '%';
            public static char SQUARE_ROOT = '√';
        }

        private void txtResult_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
