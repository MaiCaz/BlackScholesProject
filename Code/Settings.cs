using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesModelisation
{
    public class Settings
    {
        private double? _T_value;
        private double? _n_value;
        private double? _S0_value;
        private double? _mu_value;
        private double? _sigma_value;
        private int _precision;

        /// <summary>
        /// getter and setter of settings
        /// </summary>
        public int precision
        {
            get { return _precision; }
            set { _precision = value; }
        } 

        public double? T_value {
            get { return _T_value; }
            set { _T_value = value; } 
        } 

        public double? n_value
        {
            get { return _n_value; }
            set { _n_value = value; }
        }

        public double? S0_value
        {
            get { return _S0_value; }
            set { _S0_value = value; }
        }

        public double? mu_value
        {
            get { return _mu_value; }
            set { _mu_value = value; }
        }

        public double? sigma_value
        {
            get { return _sigma_value; }
            set { _sigma_value = value; }
        }

        public double? teta_value(){
            return (mu_value - ((sigma_value * sigma_value) / 2));
        }

        public Settings() {
            this._T_value = null;
            this._n_value = null;
            this._S0_value = null;
            this._mu_value = null;
            this._sigma_value = null;
        }

        public Boolean valid() {
            if (this._T_value != null &&
                this._n_value != null &&
                this._S0_value != null &&
                this._mu_value != null &&
                this._sigma_value != null &&
                (this._T_value > 0) &&
                (this._n_value > 0) &&
                (this._S0_value > 0) &&
                (this._sigma_value >= 0) 
                ) {
                return true;
            } 
            return false;
        }

    }
}
