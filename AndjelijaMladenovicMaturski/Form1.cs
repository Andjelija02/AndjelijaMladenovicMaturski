using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace AndjelijaMladenovicMaturski
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Mnogougao mnogougao = new Mnogougao();
        int tekX;
        int tekY;
        bool btn_clicked = false;
        private void Form1_MouseClick_1(object sender, MouseEventArgs e)
        {
            tekX = e.X;
            tekY = e.Y;
            mnogougao.DodajTacku(new Point { X = tekX, Y = tekY });
            Refresh();
        }
        private double povrsinaTrougla(Point A, Point B, Point C)
        {
            return Math.Abs((A.X * B.Y + B.X * C.Y + C.X * A.Y - A.X * C.Y - B.X * A.Y - C.X * B.Y) / 2.00);
        }
        private void btn_Click(object sender, EventArgs e)
        {
            btn_clicked = true; 
            Refresh();
            //this.Invalidate();
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black, 3);
            SolidBrush b = new SolidBrush(Color.Red);
            Pen c = new Pen(b);
            if (!btn_clicked)
            {
                mnogougao.Crtaj(g, p, false);
            }
            else
            {
                mnogougao.Crtaj(g, p);
                if (mnogougao.Konveksan())
                {
                    //nacrta dijagonale
                    mnogougao.CrtajDijagonale(g, p);
                    string izraz;
                    double povrsina = 0;
                    double pTrougla = povrsinaTrougla(mnogougao.TackaNa(0), mnogougao.TackaNa(1), mnogougao.TackaNa(2));
                    povrsina += pTrougla;
                    izraz = pTrougla.ToString("0.00");
                    //boji prvi trougao
                    g.FillPolygon(b, new Point[] { mnogougao.TackaNa(0), mnogougao.TackaNa(1), mnogougao.TackaNa(2) });
                    tb.Text = izraz; //ispise njegovu povrsinu
                    Application.DoEvents();
                    Thread.Sleep(500);//ceka 0,5 sec
                    for (int i = 2; i < mnogougao.Duzina() - 1; i++)
                    {
                        pTrougla = povrsinaTrougla(mnogougao.TackaNa(0), mnogougao.TackaNa(i), mnogougao.TackaNa(i + 1));
                        povrsina += pTrougla;
                        izraz += "+" + pTrougla.ToString("0.00");
                        //boji naredni trougao
                        g.FillPolygon(b, new Point[] { mnogougao.TackaNa(0), mnogougao.TackaNa(i), mnogougao.TackaNa(i+1) });
                        tb.Text = izraz; //ispise njegovu povrsinu
                        mnogougao.CrtajDijagonale(g, p);
                        Application.DoEvents();
                        Thread.Sleep(500);//ceka 0,5 sec
                    }
                    tb.Text = izraz + "=" + povrsina.ToString("0.00");
                }
                else
                    mnogougao.KonveksniOmotac().Crtaj(g, c);
            }
        }

        private void btnIzbrisi_Click(object sender, EventArgs e)
        {
            mnogougao.Obrisi();
            btn_clicked = false;
            tekX = 0;
            tekY = 0;
            tb.Text = "";
            this.Invalidate();
        }



    }
}
