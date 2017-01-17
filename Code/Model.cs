using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackScholesModelisation
{
    public class Model
    {
        private Controller _controller;
        private Settings _settings; 

        public Controller controller {
            get { return _controller; }
            set { _controller = value; }
        }

        public Settings settings {
            get { return _settings; }
            set { _settings = value; }
        }

        public Model() {
            _settings = new Settings();
        }
    }
}
