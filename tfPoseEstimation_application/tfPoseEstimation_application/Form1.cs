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

namespace super_resolution_Application
{
    public partial class Form1 : Form
    {
        bool video_mode = false;
        bool cam_mode = false;
        public Form1()
        {
            InitializeComponent();
            string[] cmds = System.Environment.GetCommandLineArgs();
            if ( cmds.Length >= 2)
            {
                openFileDialog1.FileName = cmds[1];
                pictureBox1.Image = System.Drawing.Image.FromFile(cmds[1]);
            }
        }

        /// <summary>
        /// 指定したファイルをロックせずに、System.Drawing.Imageを作成する。
        /// </summary>
        /// <param name="filename">作成元のファイルのパス</param>
        /// <returns>作成したSystem.Drawing.Image。</returns>
        public static System.Drawing.Image CreateImage(string filename)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            return img;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult res = openFileDialog1.ShowDialog();
            if ( res == DialogResult.OK)
            {
                pictureBox1.Image = System.Drawing.Image.FromFile(openFileDialog1.FileName);
                video_mode = false;
                cam_mode = false;
            }
            else
            {
                openFileDialog1.FileName = "";
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (!cam_mode && openFileDialog1.FileName == "") return;

            string model_size = "432x368";
            string model = comboBox1.Text;
            if ( comboBox1.Text == "cmu")
            {
                model_size = "656x368";
            }
            if (comboBox1.Text == "mobilenet_thin")
            {
                model_size = "432x368";
            }
            if (comboBox1.Text == "mobilenet_v2_large")
            {
                model_size = "432x368";
            }
            if (comboBox1.Text == "mobilenet_v2_small")
            {
                model_size = "432x368";
            }

            var apppath = System.IO.Path.GetDirectoryName(
                System.IO.Path.GetFullPath(
                    Environment.GetCommandLineArgs()[0]));

            //apppath = @"D:\tfPoseEstimation_app\tfPoseEstimation_application\tfPoseEstimation_application\dist";
            System.Environment.CurrentDirectory = apppath + "\\main";

            System.IO.Directory.SetCurrentDirectory(apppath + "\\main");

            if (!video_mode && !cam_mode)
            {
                string newfile1 = "input_.png";
                pictureBox1.Image.Save(newfile1, System.Drawing.Imaging.ImageFormat.Png);

                string newfile2 = "tfpose\\input.png";
                var imagemagick = new System.Diagnostics.ProcessStartInfo();
                imagemagick.FileName = "cmd.exe";
                imagemagick.UseShellExecute = true;
                imagemagick.Arguments = "/c";
                imagemagick.Arguments += " ..\\conv.bat";
                imagemagick.Arguments += " " + newfile1;
                imagemagick.Arguments += " " + model_size;
                imagemagick.Arguments += " " + newfile2;
                System.Diagnostics.Process imagemagick_p = System.Diagnostics.Process.Start(imagemagick);
                imagemagick_p.WaitForExit();
                File.Delete(newfile1);

                var app = new System.Diagnostics.ProcessStartInfo();
                app.FileName = "cmd.exe";
                app.UseShellExecute = true;
                app.Arguments = "/c";
                app.Arguments += " run_image.bat ";
                app.Arguments += model;

                string directoryName = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                string fileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                string extension = System.IO.Path.GetExtension(openFileDialog1.FileName);

                System.Diagnostics.Process p = System.Diagnostics.Process.Start(app);
                p.WaitForExit();

                string outfile = "";
                outfile = "tfpose\\output.jpg";
                pictureBox2.Image = CreateImage(outfile);
            }
            if (video_mode && !cam_mode)
            {
                var app = new System.Diagnostics.ProcessStartInfo();
                app.FileName = "cmd.exe";
                app.UseShellExecute = true;
                app.Arguments = "/c";
                app.Arguments += " run_video.bat ";
                app.Arguments += " " + "\""+openFileDialog1.FileName + "\""+" ";
                app.Arguments += model;

                System.Diagnostics.Process p = System.Diagnostics.Process.Start(app);
                p.WaitForExit();
            }
            if (!video_mode && cam_mode)
            {
                var app = new System.Diagnostics.ProcessStartInfo();
                app.FileName = "cmd.exe";
                app.UseShellExecute = true;
                app.Arguments = "/c";
                app.Arguments += " run_cam.bat ";
                app.Arguments += " " + numericUpDown1.Value.ToString() + " ";
                app.Arguments += model;

                System.Diagnostics.Process p = System.Diagnostics.Process.Start(app);
                p.WaitForExit();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ( openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                openFileDialog1.FileName = "";
                return;
            }
            video_mode = true;
            cam_mode = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            video_mode = false;
            cam_mode = true;
        }
    }
}
