using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AndjelijaMladenovicMaturski
{
    class Mnogougao
    {
        List<Point> m;
        public Mnogougao()
        {
            m = new List<Point>();
        }
        public int Duzina()
        {
            return m.Count;
        }
        public Point Poslednji()
        {
            return m.Last();
        }
        public Point TackaNa(int i)
        {
            return m.ElementAt(i);
        }
        public void DodajTacku(Point t)
        {
            m.Add(t);
        }
        public void Obrisi()
        {
            m.Clear();
        }
        public void ObrisiPoslednju()
        {
            m.RemoveAt(m.Count - 1);
        }
        public void Crtaj(Graphics g, Pen p, bool zatvori = true)
        {
            int d = m.Count;
            if (d == 1)
                g.DrawEllipse(p, m.ElementAt(0).X, m.ElementAt(0).Y, 5, 5);
            else if (d>1)
            {
                for (int i = 0; i < d-1; i++)
                {
                    g.DrawLine(p, m.ElementAt(i), m.ElementAt(i + 1));
                }
                if (d > 2 && zatvori)
                    g.DrawLine(p, m.First(), m.Last());
            }
        }
        public void CrtajDijagonale(Graphics g, Pen p)
        {
            for (int i = 2; i < Duzina() - 1; i++)
            {
                g.DrawLine(p, TackaNa(0), TackaNa(i));
            }
        }        
        static int orijentacija(Point t1, Point t3, Point t2)
        {
            int d = (t3.X - t1.X) * (t2.Y - t1.Y) - (t2.X - t1.X) * (t3.Y - t1.Y);
            if (d == 0)
                return 0;//kolinearnost
            else if (d > 0)
                return 1;//pozitivna orijentacija
            else
                return -1;//negativna orijentacija
        }
        public bool Konveksan()
        {
            int d = m.Count;
            int orj;
            for (int i = 0; i < d; i++)
            {
                orj = orijentacija(m.ElementAt(i), m.ElementAt((i + 2)%d), m.ElementAt((i + 1)%d));
                for (int j = 3; j < d ; j++)
                {
                    if (orijentacija(m.ElementAt(i), m.ElementAt((i + j) % d), m.ElementAt((i + 1) % d)) != orj)
                        return false;
                }
            }
            return true;
        }

        static void razmeni(List<Point> l, int i1, int i2)
        {
            Point a = l[i1];
            l[i1] = l[i2];
            l[i2] = a;
        }
        static int podeli(List<Point> t, int l, int d)
        {
            int k = l;
            for (int i = l+1; i <= d; i++)
            {
                if ((t.ElementAt(0).Y - t.ElementAt(i).Y) * (t.ElementAt(0).X - t.ElementAt(l).X) 
                    <= (t.ElementAt(0).Y - t.ElementAt(l).Y) * (t.ElementAt(0).X - t.ElementAt(i).X))
                {
                    //zamene mesta elementi na i i na k+1
                    razmeni(t, i, k + 1);
                    k++;

                    if ((t.ElementAt(0).Y - t.ElementAt(k).Y) * (t.ElementAt(0).X - t.ElementAt(l).X) 
                    == (t.ElementAt(0).Y - t.ElementAt(l).Y) * (t.ElementAt(0).X - t.ElementAt(k).X))
                    {
                        if (Math.Pow(t.ElementAt(0).X - t.ElementAt(k).X, 2) + Math.Pow(t.ElementAt(0).Y - t.ElementAt(k).Y, 2)
                            > Math.Pow(t.ElementAt(0).X - t.ElementAt(l).X, 2) + Math.Pow(t.ElementAt(0).Y - t.ElementAt(l).Y, 2))
                            razmeni(t, l, k);
                    }
                }
            }
            razmeni(t, k, l);
            return k;
        }
        static void QSort(List<Point> t, int l, int d)
        {
            if (l < d)
            {
                int k = podeli(t, l, d);
                QSort(t, l, k - 1);
                QSort(t, k + 1, d);
            }
        }
        static void prostMnogougao(List<Point> t)
        {
            int n = t.Count;
            int ind = 0;
            //trazimo tacku sa najvecom x koordinatom
            //ukoliko ih ima vise, biramo onu sa najmanjom y koordinatom
            for (int i = 1; i < n; i++)
            {
                if ((t.ElementAt(i).X > t.ElementAt(ind).X) || (t.ElementAt(i).X == t.ElementAt(ind).X && t.ElementAt(i).Y < t.ElementAt(ind).Y))
                    ind = i;
            }
            razmeni(t, 0, ind);
            //sortiranje tacaka
            QSort(t, 1, n - 1);
        }
        public Mnogougao KonveksniOmotac()
        {
            Mnogougao omotac = new Mnogougao();
            int a = m.Count;
            if(Konveksan())
            {
                for (int i = 0; i < a; i++)
                {
                    omotac.DodajTacku(m.ElementAt(i));
                }
                return omotac;
            }
            else
            {
                int i;
                prostMnogougao(m);
                omotac.DodajTacku(m.ElementAt(0));
                omotac.DodajTacku(m.ElementAt(1));
                i = 2;
                int b = omotac.Duzina();
                while (i < a)
                    if (b >= 2 && !(orijentacija(m.ElementAt(i), omotac.TackaNa(b - 2), omotac.TackaNa(b - 1)) >= 0))
                        //proverava ima li tacka M levo skretanje u odnosu na pravu odredjenu tackama A i B
                    {
                        b--;
                        omotac.ObrisiPoslednju();
                    }
                    else
                    {
                        omotac.DodajTacku(m.ElementAt(i));
                        b++;
                        i++;
                    }
                return omotac;
            }
            
        }
    }
}
