using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOTander.Models
{
    class Captcha
    {
        public Bitmap Image { get; set; }
        public string Text { get; set; }
        public Captcha(int Width, int Height)
        {
            Random rnd = new Random();

            Bitmap img = new Bitmap(Width, Height);

            int Xpos = rnd.Next(0, Width - 85);
            int Ypos = rnd.Next(0, Height - 30);

            Brush[] colors = { Brushes.White,
                     Brushes.Red,
                     Brushes.DeepSkyBlue,
                     Brushes.GreenYellow };

            Graphics g = Graphics.FromImage((Image)img);
            g.Clear(Color.Gray);

            string text = string.Empty;
            string ALF = "123456789QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            g.DrawString(text,
                         new Font("Arial", 18),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            g.DrawLine(Pens.Black,
                       new Point(0, rnd.Next(0, Height/2)),
                       new Point(Width, rnd.Next(Height / 2, Height)));
            g.DrawLine(Pens.Black,
                       new Point(0, rnd.Next(Height / 2, Height)),
                       new Point(Width, rnd.Next(0, Height/2)));
            g.DrawLine(Pens.Black,
                       new Point(rnd.Next(0, Width), Height),
                       new Point(rnd.Next(Xpos - (Width/10), Xpos + (Width / 10)), 0));

            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        img.SetPixel(i, j, Color.White);

            Image = img;
            Text = text;
        }
    }
}
