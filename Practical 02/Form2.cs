using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataValidation
{
    public partial class Errors : Form
    {

        //get property
        public WebBrowser getBrowser  { get{ return webBrowser1; } }



        public Errors()
        {
            
            InitializeComponent();

           //webBrowser1.DocumentText = "errors";
        }
    }
}
