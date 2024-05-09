using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Welding_Calculation
{
    public class WeldingCalculation : IExternalApplication

    {

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                application.CreateRibbonTab("Welding Quantities");
                RibbonPanel panel= application.CreateRibbonPanel("Welding Quantities", "Welding");

                string assStr=Assembly.GetExecutingAssembly().Location;

                PushButtonData btn1 = new PushButtonData("weldingCalculation", "Welding quantity calculation", assStr, "Welding_Calculation.Command.WeldingQuantityCalculation");
                panel.AddItem(btn1);

            }
            catch (Exception ex)
            {

            }
            return Result.Succeeded;
        }
    }
}
