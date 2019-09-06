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

namespace Library
{
    public partial class Form1 : Form
    {
        private string Path = @"C:\untitled\books.txt";

        public BindingList<string> Books;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Books = new BindingList<string>(File.ReadLines(Path).OrderBy(x => x[0]).ToList());

            txtSearch.Text = "Search a book...";
            txtSearch.ForeColor = Color.Gray;

            lstBooks.DataSource = Books;
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.ForeColor = Color.Black;

            if (txtSearch.Text == "Search a book...")
            {
                txtSearch.Text = "";
            }
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                txtSearch.Text = "Search a book...";
                txtSearch.ForeColor = Color.Gray;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Books.Add($"{txtBook.Text} - {txtAuthor.Text}");
            File.WriteAllLines(Path, Books);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstBooks.SelectedIndex == -1) return;

            Books.Remove(lstBooks.SelectedItem.ToString());
            File.WriteAllLines(Path, Books);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lstBooks.DataSource = (txtSearch.Text != "Search a book...") ? new BindingList<string>(Books.Where(x => x[0].ToString().ToLower().Contains(txtSearch.Text)).ToList()) : Books;    
        }

        private void btnBorrow_Click(object sender, EventArgs e)
        {
            string Text = File.ReadAllText(Path);

            if (Text.Contains(lstBooks.SelectedItem.ToString()) && lstBooks.SelectedItem.ToString().Contains("*"))
            {
                MessageBox.Show("That book is already borrowed!");
                return;
            }
            else
            {
                Text = Text.Replace(lstBooks.SelectedItem.ToString(), lstBooks.SelectedItem.ToString() + "*");
                File.WriteAllText(Path, Text);

                MessageBox.Show("That book is now borrowed!");

                Books[lstBooks.SelectedIndex] = lstBooks.SelectedItem.ToString() + "*";
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            string Text = File.ReadAllText(Path);

            if (Text.Contains(lstBooks.SelectedItem.ToString()) && lstBooks.SelectedItem.ToString().Contains("*"))
            {
                Text = Text.Replace(lstBooks.SelectedItem.ToString(), lstBooks.SelectedItem.ToString().Replace("*", ""));
                File.WriteAllText(Path, Text);

                MessageBox.Show("That book is now returned!");

                Books[lstBooks.SelectedIndex] = lstBooks.SelectedItem.ToString().Replace("*", "");
            }
            else
            {
                MessageBox.Show("That book is not borrowed.");
            }
        }
    }
}
