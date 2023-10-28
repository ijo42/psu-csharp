using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using RomeCalc.Utils;

namespace RomeCalc
{
    public partial class MainWindow
    {
        /* первое значение в десятичной СС */
        private int FirstValue { get; set; }
        /* второе значение в десятичной СС */
        private int? SecondValue { get; set; }

        /* ожидающая операция */
        private IOperation CurrentOperation;
        /* режим активных кнопок */
        private ActiveButtons activeButtons;
        /* список кнопок для переключения доступности */
        private readonly List<Button> romeBtns, decimalBtns, otherBtns;

        public MainWindow()
        {
            InitializeComponent();
            btnSum.Tag = new Sum();
            btnSubtraction.Tag = new Subtraction();
            btnDivision.Tag = new Division();
            btnMultiplication.Tag = new Multiplication();
            btnRemainder.Tag = new Remainder();
            btnLe.Tag = new Less();
            btnLE.Tag = new LessEquals();
            btnB.Tag = new Bigger();
            btnBE.Tag = new BiggerEquals();

            decimalBtns = new List<Button> { btn0, btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };
            romeBtns = new List<Button> { btnV, btnC, btnX, btnL, btnI, btnM };
            otherBtns = new List<Button>
            {
                btnLe, btnLE, btnB, btnBE, btnMultiplication, btnDivision, btnRemainder, btnSum, btnSubtraction,
                btnToRome, btnToDecimial
            };
        }

        /* метод применения режима активности кнопок */
        private void BlockKeys()
        {
            if (activeButtons != ActiveButtons.All)
            {
                decimalBtns.ForEach(btn => btn.IsEnabled = activeButtons == ActiveButtons.Decimal);   
                romeBtns.ForEach(btn => btn.IsEnabled = activeButtons == ActiveButtons.Rome);
                if(activeButtons == ActiveButtons.None)
                    otherBtns.ForEach(btn => btn.IsEnabled = false);   
            }
            else
            {
                decimalBtns.ForEach(btn => btn.IsEnabled = true);   
                romeBtns.ForEach(btn => btn.IsEnabled = true);   
                otherBtns.ForEach(btn => btn.IsEnabled = true);   
            }
        }

        /* метод для кнопок ввода цифр */
        private void regularButtonClick(object sender, RoutedEventArgs e)
            => SendToInput(((Button)sender).Content.ToString());
        
        private void SendToInput(string content)
        {
            if (activeButtons == ActiveButtons.All)
            {
                activeButtons = int.TryParse(content, out var k) ? ActiveButtons.Decimal : ActiveButtons.Rome;
                BlockKeys();
            }
            
            // 0 не должен появляться впереди
            if (txtInput.Text == "0")
                txtInput.Text = "";

            txtInput.Text = $"{txtInput.Text}{content}";
        }

        /* перевод в римскую */
        private void btnToRome_Click(object sender, RoutedEventArgs e)
        {
            if(int.TryParse(txtInput.Text, out var k))
            {
                txtInput.Text = RomeUtils.To(k);
                activeButtons = ActiveButtons.Rome;
                BlockKeys();
            }
        }

        /* перевод в десятичную */
        private void btnToDecimial_Click(object sender, RoutedEventArgs e)
        {
            //Prevent from clearing zero
            if (txtInput.Text == "0")
                return;
            if(!int.TryParse(txtInput.Text, out var k))
            {
                txtInput.Text = RomeUtils.From(txtInput.Text).ToString();
                activeButtons = ActiveButtons.Decimal;
                BlockKeys();
            }
            
        }

        /* кнопка операции */
        private void operationButton_Click(object sender, RoutedEventArgs e)
        {
            //if current operation is not null then we already have the FirstValue
            if (CurrentOperation == null) 
                FirstValue = int.TryParse(txtInput.Text, out var k) ? 
                    k : RomeUtils.From(txtInput.Text);
            
            activeButtons = ActiveButtons.All;
            BlockKeys();
            
            CurrentOperation = (IOperation)((Button)sender).Tag;
            SecondValue = null;
            txtInput.Text = "";
        }
        
         /* кнопка равенства - выполенение действия */
        private void btnEquals_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentOperation == null) /* пропуск при пустом операторе */
                return;

            if (txtInput.Text == "") /* пропуск при пустом значении */
                return;
            
            // Если значение парсится в число - используем его для вычисления, иначе - переводим из римской
            // При повторном клике на равно без выбора нового оператора повторяется предыдущая операция
            var val2 = SecondValue ?? (int.TryParse(txtInput.Text, out var k) ? k : RomeUtils.From(txtInput.Text));
            if(val2 is > 3999 or < 0)
            {
                MessageBox.Show("Ввод должен быть менее 3999 и более 0", "Выход за предел", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                btnClearAll.PerformClick();
                return;
            }

            try
            {
                var result = CurrentOperation.DoOperation(FirstValue, (int)(SecondValue = val2));
                if (result < 0)
                {
                    MessageBox.Show("Доступны операции лишь для неотрицательных значений",
                        "Отрицательное значение обнаружено", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    btnClearAll.PerformClick();
                }
                else if (CurrentOperation is Bigger or Less or LessEquals or BiggerEquals)
                {
                    txtInput.Text = result == 0 ? "false" : "true";
                    activeButtons = ActiveButtons.None;
                    BlockKeys();
                }
                else
                {
                    txtInput.Text = (FirstValue = result).ToString();
                    btnToRome.PerformClick();    // перевод в римскую по окончании вычисления
                }
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Делить на 0 нельзя", "Деление на 0", MessageBoxButton.OK, MessageBoxImage.Error);
                btnClearAll.PerformClick();
            }
        }

        private void btnClearAll_Click(object sender, RoutedEventArgs e)
        {
            FirstValue = 0;
            SecondValue = 0;
            CurrentOperation = null;
            txtInput.Text = "0";
            activeButtons = ActiveButtons.All;
            BlockKeys();
        }
    }
}
