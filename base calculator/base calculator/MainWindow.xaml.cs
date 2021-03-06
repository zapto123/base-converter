﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace base_calculator
{
    public partial class MainWindow
    {
        public struct IOResultField
        {
            public int numberBase;
            public Regex baseRegex;
            public TextBox baseTextBox;

            public IOResultField(int _numberbase, Regex _baseRegex, TextBox _baseTextbox)
            {
                numberBase = _numberbase;
                baseRegex = _baseRegex;
                baseTextBox = _baseTextbox;
            }
        }

        Dictionary<string, IOResultField> IOFields = new Dictionary<string, IOResultField>();
        IOResultField focusedIOResultField;
        long result;


        public MainWindow()
        {
            InitializeComponent();
            IOFields.Add("bin", new IOResultField(2, new Regex("[0-1]"), BinInput));
            IOFields.Add("oct", new IOResultField(8, new Regex("[0-7]"), OctInput));
            IOFields.Add("dec", new IOResultField(10, new Regex("[0-9]"), DecInput));
            IOFields.Add("hex", new IOResultField(16, new Regex("[0-9a-fA-F]"), HexInput));
        }

        private void BtnClick(object sender, RoutedEventArgs e) 
        {
            var focusedTextBox = focusedIOResultField.baseTextBox;

            if (focusedTextBox != null && ((Button)sender).Tag != null && focusedIOResultField.baseRegex.IsMatch(((Button)sender).Tag.ToString()))
            {
                focusedTextBox.Text += ((Button)sender).Tag;
                focusedTextBox.Select(focusedTextBox.Text.Length, 0);
            }
        }

        private void TxtBoxGotFocus(object sender, RoutedEventArgs e) 
        {
            foreach(var field in IOFields)
            {
                if (field.Value.baseTextBox == (TextBox)sender)
                {
                    focusedIOResultField = field.Value;
                    //Console.WriteLine("focused " + field.Key);
                    break;
                }
            }
        }

        private void Backspace(object sender, RoutedEventArgs e) 
        {
            var focusedTextBox = focusedIOResultField.baseTextBox; 
            if (!focusedIOResultField.Equals(null) && focusedTextBox.Text.Length != 0) //can't directly compare struct type vars via == and !=
            {
                focusedTextBox.Text = focusedTextBox.Text.Remove(focusedTextBox.CaretIndex - 1, 1);
                focusedTextBox.Select(focusedTextBox.Text.Length, 0);
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e) 
        {
            e.Handled = !focusedIOResultField.baseRegex.IsMatch(e.Text);
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (focusedIOResultField.baseTextBox.Text == "")
                result = 0;
            else
                result = Convert.ToInt64(focusedIOResultField.baseTextBox.Text, focusedIOResultField.numberBase);

            //Console.WriteLine("changed textbox " + focusedIOResultField.numberBase);
            foreach (var field in IOFields)
            {
                field.Value.baseTextBox.Text = Convert.ToString(result, field.Value.numberBase);
            }

            focusedIOResultField.baseTextBox.Select(focusedIOResultField.baseTextBox.Text.Length, 0);
        }
    }
}