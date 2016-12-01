using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form4 : Form
    {

        public Form4(string s1, string s2, string s3, string s4, string s5, string s6, Color cl)
        {
            InitializeComponent();
            label6.Text = s1;
            label7.Text = s2;
            label8.Text = s3;
            label9.Text = s4;
            label10.Text = s5;
            label12.Text = s6;
            label13.Text = "               ";
            label13.BackColor = cl;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
