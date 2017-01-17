using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesModelisation
{
    public static class Calcul
    {
        private static Random rnd = new Random();

        public static double SampleGaussian(double mean, double stddev) {
            double x1 = rnd.NextDouble();
            double x2 = rnd.NextDouble();
            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Sin(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }

        public static double[] brownian_mouvement(Settings settings) {
            double[] b = new double[(int)settings.n_value+1];
            double[] xi = new double[(int)settings.n_value+1];
            b[0] = 0;
            for (int i = 1; i < (int)settings.n_value; i++) {
                xi[i] = SampleGaussian(0, 1); 
                b[i] = b[i - 1] + Math.Sqrt((double)settings.T_value/(double)settings.n_value) * xi[i];
            }
            return b;
        }

        public static double[] equation_stochastic(Settings settings) {
            double[] b = new double[(int)settings.n_value + 1];
            double[] xi = new double[(int)settings.n_value + 1];
            double[] s = new double[(int)settings.n_value + 1];
            b[0] = 0;
            for (int i = 1; i <= (int)settings.n_value; i++) {
                xi[i] = SampleGaussian(0, 1);
                b[i] = b[i - 1] + Math.Sqrt((double)settings.T_value / (double)settings.n_value) * xi[i];
                s[i] = (double)settings.S0_value * Math.Exp((double)(settings.teta_value() * (i * (settings.T_value / settings.n_value))) + (double)(settings.sigma_value * (b[i])));
            }
            return s;
        }

        public static double[] equation_teta(Settings settings,double[] s) {
            double[] t = new double[(int)settings.n_value + 1];
            for (int i = 1; i <= (int)settings.n_value; i++) {
                t[i] =(double)(Math.Log(s[i]) - Math.Log((double)settings.S0_value))/(double)(i*(settings.T_value/settings.n_value));
            }
            return t;
        }

        public static double[] equation_x(Settings settings, double[] s) {
            double[] x = new double[(int)settings.n_value + 1];
            x[0] = Math.Log((double)settings.S0_value);
            for (int i = 1; i <= (int)settings.n_value; i++) {
                x[i] = (double)(Math.Log(s[i]));
            }
            return x;
        }

        public static double[] equation_sigma(Settings settings, double[] x) {
            double[] si = new double[(int)settings.n_value + 1];
            double add = (double)((x[(int)settings.n_value]-x[0]) * (x[(int)settings.n_value]-x[0]))/(double)settings.T_value;
            si[0] = 0;
            si[1] = 0;
            for (int i = 2; i <= (int)settings.n_value; i++) {
                double concaten = 0;
                for (int j = 0; j < i; j++) {
                    concaten = concaten + ((double)(x[j + 1] - x[j]) * (x[j + 1] - x[j]) / (double)(settings.T_value / settings.n_value));
                }
                si[i] = Math.Sqrt(Math.Abs((1.0/((double)i-1.0))) *(concaten-add));
            }
            
            return si;
        }
        public static double erreur_sigma(Settings settings, double[] si) {

            double erreur = Math.Abs((double)settings.sigma_value - si[(int)settings.n_value]);
            return (double)erreur;
        }
        public static double erreur_teta(Settings settings, double[] t)
        {
            //double erreur = t[(int)settings.n_value] - (double)settings.teta_value();
           double erreur = Math.Abs((double)settings.teta_value()-t[(int)settings.n_value] );

            return (double)erreur;
        }
    }
}
