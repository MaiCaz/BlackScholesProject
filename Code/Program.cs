using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackScholesModelisation
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Controller controller = new Controller();
            Model model = new Model();
            View view = new View();

            controller.model = model;
            controller.form = view;
            model.controller = controller;
            view.controller = controller;

            Application.Run(view);
        }
    }
}
