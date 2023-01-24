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
using System.Diagnostics;

namespace japroj
{
    public partial class Form1 : Form
    {
        
        [DllImport(@"C:\Users\Admin\source\repos\japroj\x64\Debug\DllC.dll")]
        public static extern void convertCPP(byte[] leftRgbValues, byte[] rightRgbValues, int bytesLength);

        [DllImport(@"C:\Users\Admin\source\repos\japroj\x64\Debug\DllAsm.dll")]
        static extern int convertASM(byte[] leftRgbValues, byte[] rightRgbValues, int bytesLength);

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
                    leftBitmap = new Bitmap(dialog.FileName);//dynamic cast to do

                    if (radioButton1.Checked || radioButton2.Checked)
                        convertButton.Enabled = true;
                }
            }
            catch (Exception) {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //pictureBox1.ImageLocation = dialog.FileName;
                    rightBitmap = new Bitmap(dialog.FileName);//dynamic cast to do

                    if (radioButton1.Checked || radioButton2.Checked)
                        convertButton.Enabled = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Bitmap originalBitmap;
        Bitmap rightBitmap;//cyjan
        Bitmap leftBitmap;//red
        //Bitmap resultBitmap;
        bool asmConvert = false;
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
                /*
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
                        //IntPtr f;
                    }

                }
                pictureBox2.Image = resultBitmap;
                */
                lockUnlockBits();

            }
            catch (ArgumentException) 
            {
                MessageBox.Show("There was an error." +
                "Check the path to the image file.");
            }
        }

        private void lockUnlockBits()
        {
            // Lock the bitmap's bits.
            Rectangle leftRect = new Rectangle(0, 0, leftBitmap.Width, leftBitmap.Height);
            Rectangle rightRect = new Rectangle(0, 0, rightBitmap.Width, rightBitmap.Height);
            
            System.Drawing.Imaging.BitmapData leftBmpData =
                leftBitmap.LockBits(leftRect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                leftBitmap.PixelFormat);

            System.Drawing.Imaging.BitmapData rightBmpData =
               rightBitmap.LockBits(rightRect, System.Drawing.Imaging.ImageLockMode.WriteOnly,
               rightBitmap.PixelFormat);

            IntPtr leftPtr = leftBmpData.Scan0;// Get the address of the first line.
            IntPtr righttPtr = rightBmpData.Scan0;

            int leftBytes = Math.Abs(leftBmpData.Stride) * leftBitmap.Height;// Declare an array to hold the bytes of the bitmap.
            byte[] leftRgbValues = new byte[leftBytes];
            int rightBytes = Math.Abs(rightBmpData.Stride) * rightBitmap.Height;
            byte[] rightRgbValues = new byte[rightBytes];

            System.Runtime.InteropServices.Marshal.Copy(leftPtr, leftRgbValues, 0, leftBytes);// Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(righttPtr, rightRgbValues, 0, rightBytes);

            // (asmConvert == true) ? convertAssembly() : convertCPP(); 
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            if (asmConvert == true)
                convertASM(leftRgbValues, rightRgbValues, leftRgbValues.Length);
            else convertCPP(leftRgbValues, rightRgbValues, leftRgbValues.Length);
            stopwatch.Stop();

            cConversionTime.Text = stopwatch.ElapsedMilliseconds.ToString()+" ms";
            
            System.Runtime.InteropServices.Marshal.Copy(leftRgbValues, 0, leftPtr, leftBytes);// Copy the RGB values back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(rightRgbValues, 0, righttPtr, rightBytes);

            leftBitmap.UnlockBits(leftBmpData);// Unlock the bits.
            rightBitmap.UnlockBits(rightBmpData);

            pictureBox2.Image = rightBitmap;// Draw the modified image.
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation != null)
                convertButton.Enabled = true; asmConvert = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (pictureBox1.ImageLocation != null)
                convertButton.Enabled = true; asmConvert = false;
        }

        
    }
}
