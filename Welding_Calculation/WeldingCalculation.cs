
using Autodesk.Revit.UI;

namespace Welding_Calculation
{
    public class WeldingCalculation : IExternalApplication

    {
        
        public Result OnShutdown(UIControlledApplication application)
        {
        return  Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                application.CreateRibbonTab("Welding Quantities");
                application.CreateRibbonPanel("Welding Quantities", "Welding Panel");

            }
            catch(Exception ex)
            {

            }
            return Result.Succeeded;
        }
    }

}
