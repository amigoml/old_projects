using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class InputForm : Form
    {
        public int dem = 0;
        private Controller myContr;
        public InputForm( Controller myController )
        {
            InitializeComponent();
            textBox1.Text = 1.ToString();
            myContr = myController;
        }

        private void InputForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                dem = int.Parse(textBox1.Text);
                myContr.ProjectInPCA(dem);
                this.Close();
            }
            catch (Exception)
            {
               
            }
        }
    }
}
