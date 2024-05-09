using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Welding_Calculation.Command
{
    [Transaction(TransactionMode.Manual)]
    internal class WeldingQuantityCalculation : IExternalCommand
    {
        public static Dictionary<string, double> result;

        public static Stopwatch stopwatch;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                stopwatch = new Stopwatch();
                Autodesk.Revit.DB.Document actDoc = commandData.Application.ActiveUIDocument.Document;
                result = new Dictionary<string, double>();
                stopwatch.Start();

                //Gussetplate_SingleBrace
                Welding_GussetPlateSingleBrace(actDoc);


                //Gussetplate_DoubleBrace
                Welding_GussetPlateDoubleBrace(actDoc);

                //FinPlate
                Welding_FinPlate(actDoc);

                Welding_WebCoverPlate(actDoc);

                Welding_FlangeCoverPlate(actDoc);

                Welding_RibPlateSingleBrace(actDoc);

                Welding_RibPlateDoubleBrace(actDoc);

                Welding_BeamStiffnerplate(actDoc);

                Welding_BeamStiffnerplateFacade(actDoc);

                //Welding_BeamStiffnerPlateRoof(actDoc);


                Welding_SingleBraceLeganchorBolt(actDoc);

                Welding_DoubleBraceLeganchorBolt(actDoc);

            }
            catch (Exception ex)
            {

            }
            return Result.Succeeded;
        }



        //[Gusset Plate - Single Brace]
        public static void Welding_GussetPlateSingleBrace(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance1 = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Gusset Plate - Single Brace")).ToList();

            IList<Element> IlstEqpFamInstance = elem
    .Where(f => f is FamilyInstance && (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Gusset Plate - Single Brace") && (f as FamilyInstance).Symbol.Name.Contains("Gusset Plate - Single Brace"))
    .ToList();

            double GussetPlateSingleBrace_Formula = 0;
            double GussetPlateSingleBrace_Length = 0;
            double GussetPlateSingleBrace_Thickness = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)
                {
                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Gusset Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("Gusset Plate"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Length");
                        if (lengthpara != null)
                        {
                            GussetPlateSingleBrace_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter thcknessPara = ele.LookupParameter("Thickness");
                        if (thcknessPara != null)
                        {
                            GussetPlateSingleBrace_Thickness += Convert.ToDouble(thcknessPara.AsValueString());
                        }

                    }
                }


            }
            GussetPlateSingleBrace_Formula = ((GussetPlateSingleBrace_Length + GussetPlateSingleBrace_Thickness) * 2) * 2;
            result.Add("GussetPlateSingleBrace_Formula", GussetPlateSingleBrace_Formula);
        }

        //[Gusset Plate - Double Brace]
        public static void Welding_GussetPlateDoubleBrace(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();
            //List<FamilySymbol> filteredFamilySymbols = elem.Cast<FamilySymbol>().Where(symbol => symbol.Family.Name.Contains("Gusset Plate - Double Brace")).ToList();
            //IList<Element> IlstEqpFamInstance = elem.OfClass(typeof(FamilyInstance)).Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("Gusset Plate - Double Brace")).ToList();
            IList<Element> IlstEqpFamInstance = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Gusset Plate - Double Brace")).ToList();


            double gussetPlateDoubleBrace_Formula = 0;
            double gussetPlate_Length = 0;
            double gussetPlate_Thickness = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {


                if (ele != null)
                {
                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Gusset Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("Gusset Plate"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Length");
                        if (lengthpara != null)
                        {
                            gussetPlate_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter thicknessPara = ele.LookupParameter("Thickness");
                        if (thicknessPara != null)
                        {
                            gussetPlate_Thickness += Convert.ToDouble(thicknessPara.AsValueString());
                        }
                    }
                }

            }
            gussetPlateDoubleBrace_Formula = ((gussetPlate_Length + gussetPlate_Thickness) * 2) * 2;
            result.Add("GussetPlateDoubleBrace_Formula", gussetPlateDoubleBrace_Formula);
        }

        //[Fin plate]
        public static void Welding_FinPlate(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Fin Plate")).ToList();


            double finPlate_Formula = 0;
            double finPlate_Length = 0;
            double finPlate_Thickness = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {

                if (ele != null)
                {
                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Fin Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("Gusset Plate"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Length");
                        if (lengthpara != null)
                        {
                            finPlate_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter thicknessPara = ele.LookupParameter("NS_Thickness");
                        if (thicknessPara != null)
                        {
                            finPlate_Thickness += Convert.ToDouble(thicknessPara.AsValueString());
                        }

                    }
                }


            }
            finPlate_Formula = ((finPlate_Length + finPlate_Thickness) * 2) * 2;
            result.Add("finPlate_Formula", finPlate_Formula);
        }

        //[Web cover plate]
        public static void Welding_WebCoverPlate(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Web Cover Plate")).ToList();


            double webCoverPlate_Formula = 0;
            double webCoverPlate_Length = 0;
            double webCoverPlate_Width = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)
                {

                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Web Cover Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("First Brace"))
                    {
                        ElementId typeId = ele.GetTypeId();
                        ElementType elementType = actDoc.GetElement(typeId) as ElementType;
                        Parameter lengthpara = elementType.LookupParameter("L");

                        if (lengthpara != null)
                        {
                            webCoverPlate_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter widthPara = elementType.LookupParameter("W");
                        if (widthPara != null)
                        {
                            webCoverPlate_Width += Convert.ToDouble(widthPara.AsValueString());
                        }

                    }
                }


            }
            webCoverPlate_Formula = ((webCoverPlate_Length + webCoverPlate_Width) * 2) * 1;
            result.Add("webCoverPlate_Formula", webCoverPlate_Formula);
        }

        //[FlangeCoverPlate]
        public static void Welding_FlangeCoverPlate(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Flange Cover Plate")).ToList();


            double flangeCoverPlate_Formula = 0;
            double flangeCoverPlate_Length = 0;
            double flangeCoverPlate_Width = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)
                {
                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Flange Cover Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("First Brace"))
                    {
                        ElementId typeId = ele.GetTypeId();
                        ElementType elementType = actDoc.GetElement(typeId) as ElementType;
                        Parameter lengthpara = elementType.LookupParameter("Length");

                        if (lengthpara != null)
                        {
                            flangeCoverPlate_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter widthPara = elementType.LookupParameter("Width");
                        if (widthPara != null)
                        {
                            flangeCoverPlate_Width += Convert.ToDouble(widthPara.AsValueString());
                        }

                    }
                }


            }
            flangeCoverPlate_Formula = ((flangeCoverPlate_Length + flangeCoverPlate_Width) * 2) * 1;
            result.Add("flangeCoverPlate_Formula", flangeCoverPlate_Formula);
        }


        // [Rib Plate- Brace Leg Single Brace Assembly]
        public static void Welding_RibPlateSingleBrace(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Rib Plate")).ToList();


            double RibPlateSingleBrace_Formula = 0;
            double RibPlateSingleBrace_Length = 0;
            double RibPlateSingleBrace_Height = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)
                {

                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Rib Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("Brace Leg") && ele.LookupParameter("NS_Assembly Information").AsValueString().Equals("Brace Leg Single Brace Assembly"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Length");
                        if (lengthpara != null)
                        {
                            RibPlateSingleBrace_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter heightPara = ele.LookupParameter("Height");
                        if (heightPara != null)
                        {
                            RibPlateSingleBrace_Height += Convert.ToDouble(heightPara.AsValueString());
                        }

                    }
                }


            }
            RibPlateSingleBrace_Formula = (RibPlateSingleBrace_Length + RibPlateSingleBrace_Height) * 2;
            result.Add("RibPlateSingleBrace_Formula", RibPlateSingleBrace_Formula);
        }

        //[ Rib Plate- Brace Leg Single Brace Assembly]
        public static void Welding_RibPlateDoubleBrace(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Rib Plate")).ToList();


            double RibPlateDoubleBrace_Formula = 0;
            double RibPlateDoubleBrace_Length = 0;
            double RibPlateDoubleBrace_Height = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)
                {

                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Rib Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("Brace Leg") && ele.LookupParameter("NS_Assembly Information").AsValueString().Equals("Brace Leg Double Brace Assembly"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Length");
                        if (lengthpara != null)
                        {
                            RibPlateDoubleBrace_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter heightPara = ele.LookupParameter("Height");
                        if (heightPara != null)
                        {
                            RibPlateDoubleBrace_Height += Convert.ToDouble(heightPara.AsValueString());
                        }

                    }
                }


            }
            RibPlateDoubleBrace_Formula = (RibPlateDoubleBrace_Length + RibPlateDoubleBrace_Height) * 2;
            result.Add("RibPlateDoubleBrace_Formula", RibPlateDoubleBrace_Formula);
        }

        //[Beam stiffner plate-beam flange and beam web]
        public static void Welding_BeamStiffnerplate(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance1 = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Beam Stiffener")).ToList();

            IList<Element> IlstEqpFamInstance = elem
 .Where(f => f is FamilyInstance && (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Beam Stiffener") && (f as FamilyInstance).Symbol.Name.Contains("Beam Stiffener Plate - Beam Flange"))
 .ToList();
            IList<Element> IlstEqpFamInstance2 = elem
 .Where(f => f is FamilyInstance && (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Beam Stiffener") && (f as FamilyInstance).Symbol.Name.Contains("Beam Stiffener Plate - Beam Web"))
 .ToList();
            double BeamStiffnerplate_Formula = 0;
            double BeamStiffnerplate_Length = 0;
            double BeamStiffnerplate_Height = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)
                {

                    if (ele.LookupParameter("NS_P_Filter").AsValueString() != null && ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Shiguchi Beam Stiffener Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("First Brace"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Length");
                        if (lengthpara != null)
                        {
                            BeamStiffnerplate_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter heightPara = ele.LookupParameter("Height");
                        if (heightPara != null)
                        {
                            BeamStiffnerplate_Height += Convert.ToDouble(heightPara.AsValueString());
                        }

                    }
                }

            }

            foreach (FamilyInstance ele in IlstEqpFamInstance2)
            {
                if (ele != null)
                {
                    if (ele.LookupParameter("NS_P_Filter").AsValueString() != null && ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Shiguchi Beam Stiffener Plate") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("First Brace"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Length");
                        if (lengthpara != null)
                        {
                            BeamStiffnerplate_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter heightPara = ele.LookupParameter("Height");
                        if (heightPara != null)
                        {
                            BeamStiffnerplate_Height += Convert.ToDouble(heightPara.AsValueString());
                        }

                    }
                }

            }


            BeamStiffnerplate_Formula = (BeamStiffnerplate_Length + BeamStiffnerplate_Height) * 2;
            result.Add("BeamStiffnerplate_Formula", BeamStiffnerplate_Formula);
        }


        //[Beam stiffner plate-facade]
        public static void Welding_BeamStiffnerplateFacade(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance1 = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Beam Stiffener")).ToList();

            IList<Element> IlstEqpFamInstance = elem
    .Where(f => f is FamilyInstance && (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Beam Stiffener") && (f as FamilyInstance).Symbol.Name.Contains("Beam Stiffener Plate_Facade"))
    .ToList();

            double BeamStiffnerplateFacade_Formula = 0;
            double BeamStiffnerplateFacade_Length = 0;
            double BeamStiffnerplateFacade_Height = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)
                {
                    Parameter lengthpara = ele.LookupParameter("Length");
                    if (lengthpara != null)
                    {
                        BeamStiffnerplateFacade_Length += Convert.ToDouble(lengthpara.AsValueString());
                    }
                    Parameter heightPara = ele.LookupParameter("Height");
                    if (heightPara != null)
                    {
                        BeamStiffnerplateFacade_Height += Convert.ToDouble(heightPara.AsValueString());
                    }
                }


            }
            BeamStiffnerplateFacade_Formula = (BeamStiffnerplateFacade_Length + 2 * BeamStiffnerplateFacade_Height) * 2;
            result.Add("BeamStiffnerplateFacade_Formula", BeamStiffnerplateFacade_Formula);
        }

        //[Beam Stiffener Plate Roof]
        //    public static void Welding_BeamStiffnerPlateRoof(Document actDoc)
        //    {
        //        FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

        //        IList<Element> IlstEqpFamInstance1 = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Beam Stiffener")).ToList();

        //        IList<Element> IlstEqpFamInstance = elem
        //.Where(f => f is FamilyInstance && (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Beam Stiffener") && (f as FamilyInstance).Symbol.Name.Contains("Beam Stiffener Plate Roof"))
        //.ToList();

        //        double BeamStiffnerplateRoof_Formula = 0;
        //        double BeamStiffnerplateRoof_Length = 0;
        //        double BeamStiffnerplateRoof_Height = 0;
        //        foreach (FamilyInstance ele in IlstEqpFamInstance)
        //        {
        //            if (ele != null)
        //            {
        //                if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Roof Column Top Plate (Type-B)") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("Steel Connection"))
        //                {
        //                    Parameter lengthpara = ele.LookupParameter("Length");
        //                    if (lengthpara != null)
        //                    {
        //                        BeamStiffnerplateRoof_Length += Convert.ToDouble(lengthpara.AsValueString());
        //                    }
        //                    Parameter heightPara = ele.LookupParameter("Height");
        //                    if (heightPara != null)
        //                    {
        //                        BeamStiffnerplateRoof_Height += Convert.ToDouble(heightPara.AsValueString());
        //                    }
        //                }
        //            }


        //        }
        //        BeamStiffnerplateRoof_Formula = (BeamStiffnerplateRoof_Length + 2 * BeamStiffnerplateRoof_Height) * 2;
        //        result.Add("BeamStiffnerplateRoof_Formula", BeamStiffnerplateRoof_Formula);
        //    }



        //Anchor Bolt - Brace Leg Double Brace Assembly
        public static void Welding_SingleBraceLeganchorBolt(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Brace Leg Anchor Bolt")).ToList();

            //        IList<Element> IlstEqpFamInstance = elem
            //.Where(f => f is FamilyInstance && (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Gusset Plate - Single Brace") && (f as FamilyInstance).Symbol.Name.Contains("Gusset Plate - Single Brace"))
            //.ToList();

            double singleBraceLeganchorBolt_Formula = 0;
            double BraceLeganchorBolt_Length = 0;
            double BraceLeganchorBolt_Width = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)

                {
                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Anchor Bolt") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("Brace Leg") && ele.LookupParameter("NS_Assembly Information").AsValueString().Equals("Brace Leg Single Brace Assembly"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Fixing Plate Length");
                        if (lengthpara != null)
                        {
                            BraceLeganchorBolt_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter widthPara = ele.LookupParameter("Fixing Plate Width");
                        if (widthPara != null)
                        {
                            BraceLeganchorBolt_Width += Convert.ToDouble(widthPara.AsValueString());
                        }

                    }
                }


            }
            singleBraceLeganchorBolt_Formula = ((BraceLeganchorBolt_Length + BraceLeganchorBolt_Width) * 2) * 2;
            result.Add("singleBraceLeganchorBolt_Formula", singleBraceLeganchorBolt_Formula);

        }

        //[Anchor Bolt - Brace Leg Double Brace Assembly]
        public static void Welding_DoubleBraceLeganchorBolt(Document actDoc)
        {
            FilteredElementCollector elem = new FilteredElementCollector(actDoc).OfClass(typeof(FamilyInstance)).WhereElementIsNotElementType();

            IList<Element> IlstEqpFamInstance = elem.Where(f => (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Brace Leg Anchor Bolt")).ToList();

            //        IList<Element> IlstEqpFamInstance = elem
            //.Where(f => f is FamilyInstance && (f as FamilyInstance).Symbol.FamilyName.Contains("NS_Gusset Plate - Single Brace") && (f as FamilyInstance).Symbol.Name.Contains("Gusset Plate - Single Brace"))
            //.ToList();

            double doubleBraceLeganchorBolt_Formula = 0;
            double BraceLeganchorBolt_Length = 0;
            double BraceLeganchorBolt_Width = 0;
            foreach (FamilyInstance ele in IlstEqpFamInstance)
            {
                if (ele != null)
                {
                    if (ele.LookupParameter("NS_P_Filter").AsValueString().Equals("Anchor Bolt") && ele.LookupParameter("NS_P_Group").AsValueString().Equals("Brace Leg") && ele.LookupParameter("NS_Assembly Information").AsValueString().Equals("Brace Leg Double Brace Assembly"))
                    {
                        Parameter lengthpara = ele.LookupParameter("Fixing Plate Length");
                        if (lengthpara != null)
                        {
                            BraceLeganchorBolt_Length += Convert.ToDouble(lengthpara.AsValueString());
                        }
                        Parameter widthPara = ele.LookupParameter("Fixing Plate Width");
                        if (widthPara != null)
                        {
                            BraceLeganchorBolt_Width += Convert.ToDouble(widthPara.AsValueString());
                        }

                    }
                }


            }
            doubleBraceLeganchorBolt_Formula = ((BraceLeganchorBolt_Length + BraceLeganchorBolt_Width) * 2) * 2;
            result.Add("doubleBraceLeganchorBolt_Formula", doubleBraceLeganchorBolt_Formula);
            stopwatch.Stop();
            result.Add(@"\nTotal time Required", stopwatch.ElapsedMilliseconds / 1000);
            JsonConvert.SerializeObject(result);
            string filePath = "output.json";
            File.WriteAllText(filePath, JsonConvert.SerializeObject(result));
        }

    }
}
