using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace japroj
{
    public partial class Form1 : Form
    {
        
        [DllImport(@"C:\Users\Admin\source\repos\japroj\x64\Debug\DllC.dll")]
        public static extern int addNumbers(int a, int b);

        [DllImport(@"C:\Users\Admin\source\repos\japroj\x64\Debug\DllAsm.dll")]
        static extern int MyProc1(int a, int b);

        public Form1()
        {
            InitializeComponent();
        }

        private void chooseFileButton_Click(object sender, EventArgs e)
        {            
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    pictureBox1.ImageLocation = dialog.FileName;
                    originalBitmap = new Bitmap(dialog.FileName);//dynamic cast to do

                    if (radioButton1.Checked || radioButton2.Checked)
                        convertButton.Enabled = true;
                }
            }
            catch (Exception) {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        Bitmap originalBitmap;
        Bitmap rightBitmap;//cyjan
        Bitmap leftBitmap;//red
        Bitmap resultBitmap;
        private void convertButton_Click(object sender, EventArgs e)
        {
            // Display the ProgressBar control.
            progressBar1.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            progressBar1.Minimum = 1;
            // Set the initial value of the ProgressBar.
            progressBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            progressBar1.Step = 1;

            try
            {
                //retrieve the image
                rightBitmap = new Bitmap(originalBitmap);
                leftBitmap = new Bitmap(originalBitmap);
                resultBitmap = new Bitmap(originalBitmap);

                // Set Maximum to the total number of files to copy.
                progressBar1.Maximum = originalBitmap.Width * originalBitmap.Height;

                int x, y;

                //loop through the images pixels to reset color
                for(x=0; x< originalBitmap.Width; x++)
                {
                    for(y=0; y< originalBitmap.Height; y++)
                    {
                        Color pixelColor = originalBitmap.GetPixel(x, y);
                        Color leftColor = Color.FromArgb(pixelColor.R, 0, 0);
                        Color rightColor = Color.FromArgb(0, pixelColor.G, pixelColor.B);

                        leftBitmap.SetPixel(x, y, leftColor);

                        if (x < 20)
                             rightBitmap.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                            
                        if((x+20) < originalBitmap.Width)
                            rightBitmap.SetPixel(x+20, y, rightColor);

                        resultBitmap.SetPixel(x, y, Color.FromArgb(leftBitmap.GetPixel(x, y).R, rightBitmap.GetPixel(x, y).G, rightBitmap.GetPixel(x, y).B));
                        progressBar1.PerformStep();
                    }

                }

                pictureBox2.Image = resultBitmap;

            }
            catch (ArgumentException) 
            {
                MessageBox.Show("There was an error." +
                "Check the path to the image file.");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            int x = 5, y = 3;
            int retVal = MyProc1(x, y);

           // label1.Text = addNumbers(1, 5).ToString();

            label1.Text = retVal.ToString();


        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation != null)
                convertButton.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation != null)
                convertButton.Enabled = true;

        }

       
    }
}
