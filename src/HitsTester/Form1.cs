using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HitsTester
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      textBox3.Text = HaagsTranslator.Translator.Translate(textBox1.Text);
      textBox2.Text = HaagsTranslator.Translator.GetHits(textBox1.Text)
        .Aggregate("", (current, item) => current += item.Item1 + " => hit: " + item.Item2 + Environment.NewLine);
    }

    private void label2_Click(object sender, EventArgs e)
    {

    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
  }
}
