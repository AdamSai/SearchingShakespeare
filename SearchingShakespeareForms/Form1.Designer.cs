using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SearchingShakespeare;

namespace SearchingShakespeareForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private TextBox _textBox;
        private RichTextBox _resultTextBox;
        private SuffixTree _suffixTree;
        private List<string> _searchResults =new List<string>();
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._searchResults = new List<string>();
            
            this._textBox = new TextBox();
            this._resultTextBox = new RichTextBox();
            
            this._textBox.Enabled = false;
            this._textBox.Text = "Building suffix tree please wait..";
            Cursor.Current = Cursors.WaitCursor;
            new Thread(() =>
            {
                // TODO: Change this to your own file path

                var text = File.ReadAllText(
                    @"D:\Github\Algorithms\SearchingShakespeare\shakespeare-complete-works.txt") + "$";
                text = Regex.Replace(text, @"\s+", " ");
                var lower = text.ToLower();
                this._suffixTree = new SuffixTree(text, lower);
                this._textBox.Text = "Enter text here...";
                this._textBox.Enabled = true;
                Cursor.Current = Cursors.Default;
            }).Start();


            this.SuspendLayout();
            
            this._textBox.Location = new Point(0, 0);
            this._textBox.Size = new Size(1620, 20);
            this._textBox.AcceptsReturn = true;
            this._textBox.Multiline = false;
            this._textBox.Name = "Search";
            this._textBox.TextChanged += TextBox_TextChanged;
            this._textBox.GotFocus += TextBoxRemovetext;
            this._textBox.LostFocus += TextBoxAddtext;
            
            this._resultTextBox.Location = new Point(0, 20);
            this._resultTextBox.BackColor = Color.Tomato;
            this._resultTextBox.Size = new Size(1620, 790);
            this._resultTextBox.Text = "Can you see me ?";
            this._resultTextBox.Multiline = true;
            this._resultTextBox.Enabled = false;
            this._resultTextBox.Text = "";
            this._resultTextBox.ForeColor = Color.Black;
            this._resultTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            this._resultTextBox.Font = new Font(new FontFamily(System.Drawing.Text.GenericFontFamilies.Monospace), 12);

            this.components = new System.ComponentModel.Container();
            this.Controls.Add(_textBox);
            this.Controls.Add(_resultTextBox);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1620, 450);
            this.ResumeLayout(false);
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var count = 0;
            if (this._suffixTree is null) return;
            _searchResults.Clear();
            this._resultTextBox.Text = "";
            
            if (!string.IsNullOrWhiteSpace(_textBox.Text))
            {
                this._searchResults = _suffixTree.Search(_textBox.Text);
            }

            foreach (var searchResult in _searchResults)
            {
                this._resultTextBox.Text += $"[{++count}] {searchResult}\r\n";
            }
        }

        private void TextBoxRemovetext(object sender, EventArgs e)
        {
            if (this._textBox.Text == "Enter text here...")
            {
                this._textBox.Text = "";
            }   
        }
        private void TextBoxAddtext(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this._textBox.Text))
            {
                this._textBox.Text = "Enter text here...";
            }         
        }
        private void RenderText(PaintEventArgs e)
        {
            TextRenderer.DrawText(e.Graphics, "Regular Text", this.Font, new Point(10, 10), SystemColors.ControlText );
        }

        #endregion
    }
}