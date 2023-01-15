using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace japroj
{
    public partial class Form1 : Form
    {
        String safeFileName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String imageLocation = "";
            
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg files(.*jpg)|*.jpg| PNG files(.*png)|*.png| All Files(*.*)|*.*";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocation = dialog.FileName;
                    pictureBox1.ImageLocation = imageLocation;
                    safeFileName = dialog.SafeFileName;

                    System.Drawing.Image imag = System.Drawing.Image.FromFile(imageLocation);
                    imag.Save(@"C:\Users\Admin\source\repos\japroj\"+safeFileName+".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
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
            try
            {
                //retrieve the image
                originalBitmap = new Bitmap(@"C:\Users\Admin\source\repos\japroj\" + safeFileName + ".bmp", true);
                rightBitmap = new Bitmap(@"C:\Users\Admin\source\repos\japroj\" + safeFileName + ".bmp", true);
                leftBitmap = new Bitmap(@"C:\Users\Admin\source\repos\japroj\" + safeFileName + ".bmp", true);
                resultBitmap = new Bitmap(@"C:\Users\Admin\source\repos\japroj\" + safeFileName + ".bmp", true);


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

                        if (x < 5)
                             rightBitmap.SetPixel(x, y, Color.FromArgb(0, 0, 0));
                            
                        if((x+5) < originalBitmap.Width)
                            rightBitmap.SetPixel(x+5, y, rightColor);

                        resultBitmap.SetPixel(x, y, Color.FromArgb(leftBitmap.GetPixel(x, y).R, rightBitmap.GetPixel(x, y).G, rightBitmap.GetPixel(x, y).B));
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


    }
}
