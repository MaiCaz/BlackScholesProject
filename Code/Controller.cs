using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackScholesModelisation
{
    public class Controller
    {
        private Model _model;
        private Form _form;

        public Model model { 
            get { return _model; }
            set { _model = value; }
        }

        public Form form {
            get { return _form; }
            set { _form = value; }
        }

        public Settings settings() {
            return this._model.settings;
        }

        public void settings(Settings settings) {
            this.model.settings = settings;
        }

    }
}
