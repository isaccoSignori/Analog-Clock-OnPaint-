using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace WF_AnalogClock_OnPaint
{
    public partial class Form1 : Form
    {
        private Image picture;
        Image clock;
        int L, WIDTH, HEIGHT;
        float secHAND , minHAND, hrHAND;
        Bitmap bmp;
        Graphics g;
        Point center;

        bool exc = false;
        public Form1()
        {
            InitializeComponent();
            timer.Start(); //insert the function  t_Tick on the timer's tick event 
            getBackground();
        }

        private void getBackground()
        {
            //insert the name of the bmp saved in your folder for background image 
            try
            {
                string fileName = "yourBackGround.bmp";
                FileInfo f = new FileInfo(fileName);
                string fullname = f.FullName;
                clock = Image.FromFile(fullname);
            }
            catch (Exception)
            {

                exc = true;
            }
            
        }

        private void create_hands()
        {
            WIDTH = pictureBox1.Width;
            HEIGHT = pictureBox1.Height;
            L = WIDTH;
            if (WIDTH != HEIGHT)
                L = WIDTH > HEIGHT ? HEIGHT : WIDTH;
           
           
            secHAND = L / 2 - L / 30;
            minHAND = secHAND - L / 10;
            hrHAND = minHAND - L / 10;
            center = new Point((L / 2), (L / 2));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            create_hands();
            base.OnPaint(e);
            bmp = new Bitmap(L+ 1, L +1 );
            
       
            g = Graphics.FromImage(bmp);
            if(exc == false)
            {
                pictureBox1.BackgroundImage = clock;
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                //if you don't want to use a background immage, you can draw it 
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                
                Rectangle R = new Rectangle(0, 0, L , L );
                g.DrawEllipse(new Pen(Brushes.Black, 1),R);
                g.FillEllipse(Brushes.Coral, R);

            }

            //get time
            int ss = DateTime.Now.Second;
            int mm = DateTime.Now.Minute;
            int hh = DateTime.Now.Hour;

            float[] handCoord = new float[2];

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //SECOND HAND
            handCoord = msCoord(ss, secHAND);
            g.DrawLine(new Pen(Color.White, 1f), center, new PointF(handCoord[0], handCoord[1]));

            //MINUTE HAND
            handCoord = msCoord(mm, minHAND);
            g.DrawLine(new Pen(Color.White, 2f), center, new PointF(handCoord[0], handCoord[1]));

            //HOUR HAND
            handCoord = hrCoord(hh % 12, mm, hrHAND);
            g.DrawLine(new Pen(Color.White, 3f), center, new PointF(handCoord[0], handCoord[1]));

            //load bmp in picturebox1
            pictureBox1.Image = bmp;

            //Digital clock con form name
            this.Text = DateTime.Now.ToString("HH:mm:ss");
            //dispose 
            g.Dispose();
            if (picture != null) picture.Dispose();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        //coord for minute and second hand
        private float[] msCoord(float val, float hlen)
        {
            float[] coord = new float[2];
            val *= 6;   //each minute and second make 6 degree

            if (val >= 0 && val <= 180)
            {
                coord[0] = (L / 2) + (float)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = (L / 2) - (float)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = (L / 2) - (float)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = (L / 2) - (float)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        //coord for hour hand
        private float[] hrCoord(float hval, float mval, float hlen)
        {
            float[] coord = new float[2];

            //each hour makes 30 degree
            //each min makes 0.5 degree
            float val = (float)((hval * 30) + (mval * 0.5));

            if (val >= 0 && val <= 180)
            {
                coord[0] = (L / 2) + (float)(hlen * Math.Sin(Math.PI * val / 180));
                coord[1] = (L / 2) - (float)(hlen * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = (L / 2) - (float)(hlen * -Math.Sin(Math.PI * val / 180));
                coord[1] = (L / 2) - (float)(hlen * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

    }
}
