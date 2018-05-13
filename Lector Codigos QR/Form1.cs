using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.DirectShow;
using BarcodeLib.BarcodeReader;
using System.IO;

namespace Lector_Codigos_QR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FilterInfoCollection dispositivos;
        VideoCaptureDevice fuentevideo;

        private void Form1_Load(object sender, EventArgs e)
        {
            dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo x in dispositivos)
            {
                comboBox1.Items.Add(x.Name);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
            fuentevideo = new VideoCaptureDevice(dispositivos[comboBox1.SelectedIndex].MonikerString);
            videoSourcePlayer1.VideoSource = fuentevideo;
            videoSourcePlayer1.Start();
        }

        private void btnTerminar_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            videoSourcePlayer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (videoSourcePlayer1.GetCurrentVideoFrame() != null)
            {
                Bitmap img = new Bitmap(videoSourcePlayer1.GetCurrentVideoFrame());
                string[] resultados = BarcodeReader.read(img, BarcodeReader.QRCODE);
                img.Dispose();
                if ((resultados != null) && (resultados.Length > 0))
                    listBox1.Items.Add(resultados[0]);
            }
        }
    }
}
