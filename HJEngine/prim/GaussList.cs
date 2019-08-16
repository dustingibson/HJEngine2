using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HJEngine
{
    class ProbValue
    {
        public double p;
        public double v;
        public double k;

        public ProbValue(double p, double v)
        {
            this.p = p;
            this.v = v;
        }
    }

    class GaussianList
    {
        private double sd;
        private double m;
        private int n;
        public List<double> values;
        public List<ProbValue> items;

        public GaussianList(double m, double sd, int n)
        {
            this.m = m;
            this.sd = sd;
            this.values = new List<double>();
            this.items = new List<ProbValue>();
            this.n = n;
            double min = this.m - (3 * this.sd);
            double max = this.m + (3 * this.sd);
            double delta = (max - min) / this.n;

            for (int i = 0; i < n; i++)
            {
                double curVal = min + i * delta;
                ProbValue pValue = new ProbValue(GaussProb(curVal, delta), curVal);
                items.Add(pValue);
            }
            this.items = this.items.OrderByDescending(i => i.p).ToList();
            double sum = 0;
            foreach (ProbValue item in items)
            {
                double rVal = 10000 * item.p;
                item.k = sum + rVal;
                sum += rVal;
            }
        }

        public double SumTest()
        {
            double sum = 0.0;
            for (int i = 0; i < this.n; i++)
            {
                sum += items[i].p;
            }
            return sum;
        }

        public double GetValue(Random rand)
        {
            int ps = 10000;
            double r = rand.NextDouble() * ps;
            //values.Add(binSearch(r, 0, this.n).v);
            return binSearch(r, 0, this.n).v;
            //Console.WriteLine(search(r,ps).v);
            //Console.WriteLine(binSearch(r, 0, this.n).v);
        }

        public ProbValue search(double v, double ps)
        {
            double sum = 0.0;
            for (int i = 0; i < this.n; i++)
            {
                if (v >= sum && v <= sum + ps * this.items[i].p)
                    return this.items[i];
                sum += ps * this.items[i].p;
            }
            return this.items[this.n - 1];
        }

        public ProbValue binSearch(double v, int low, int high)
        {
            int mid = (low + high) / 2;
            if (mid + 1 >= this.n)
                return items[this.n - 1];
            if (mid == 0)
                return items[0];
            if (v >= this.items[mid].k && v <= this.items[mid + 1].k)
                return this.items[mid];
            if (v < this.items[mid].k)
                return binSearch(v, low, mid);
            else
                return binSearch(v, mid, high);
        }

        public double Gauss(double v)
        {
            double ePower = -1.0 * (Math.Pow(v - this.m, 2)) / (2.0 * this.sd * this.sd);
            return (1.0 / (this.sd * Math.Sqrt(2 * 3.14159))) * Math.Exp(ePower);
        }

        public double GaussProb(double v, double delta)
        {
            return Gauss(v) * delta;
        }

    }
    //class Program
    //{
    //    static void Main(string[] args)
    //    {
    //        var stopwatch = new Stopwatch();
    //        stopwatch.Start();
    //        GaussianList gl = new GaussianList(65, 3.5, 100);
    //        //GaussianList gl = new GaussianList(0.0, 1.0, 100);
    //        Random rand = new Random();
    //        for (int i = 0; i < 500000; i++)
    //        {
    //            gl.AddValue(rand);
    //        }
    //        stopwatch.Stop();
    //        Console.WriteLine(stopwatch.ElapsedMilliseconds);
    //        //GaussianList g2 = new GaussianList(65, 3.5, 100);
    //    }
    //}
}