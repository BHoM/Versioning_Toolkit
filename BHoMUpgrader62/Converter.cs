/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BH.Upgrader.v62
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "6.1";

            ToNewObject.Add("BH.oM.LadybugTools.SimulationResult", UpgradeSimulationResult);
            ToNewObject.Add("BH.oM.Lighting.Elements.Luminaire", UpgradeLuminaire);
            ToNewObject.Add("BH.oM.Base.Attributes.InputAttribute", UpgradeInputAttribute);
            MessageForDeleted.Add("BH.oM.LadybugTools.ILBTMaterial", "This class has been removed from the installer and has been replaced with an interface called ILadybugToolsMaterial. You may need to manually update your code to be compatible with the new interface.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.UsedBarDiameters(System.Collections.Generic.List<Autodesk.Revit.DB.Structure.Rebar>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RebarShape(Autodesk.Revit.DB.Document, System.String)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RebarBarType(Autodesk.Revit.DB.Document, System.Int32)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.HookType(Autodesk.Revit.DB.Document, System.String)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RebarSchedule(Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.MassSummarySchedule(Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.LengthSummarySchedule(Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetColumnWidths(Autodesk.Revit.DB.ViewSchedule)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.ColumnCount(Autodesk.Revit.DB.ViewSchedule)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.TwoLegStirrupOutlines(System.Collections.Generic.List<System.Collections.Generic.List<BH.oM.Geometry.Point>>, System.Int32, System.Int32, System.Double, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.SingleLegStirrupLines(System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<BH.oM.Geometry.Point>>>, System.Collections.Generic.List<System.Collections.Generic.List<BH.Revit.oM.Rebar.BarProperties>>, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RectangleStirrupOutline(BH.oM.Geometry.PolyCurve, System.Double, System.Double, BH.oM.Geometry.TransformMatrix)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.PrimarySideBarPoints(BH.oM.Geometry.Polyline, System.Double, System.Collections.Generic.List<System.Collections.Generic.List<System.Double>>, System.Double, System.Double[])", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.PrimarySideBarLocations(BH.oM.Geometry.Line, System.Double, System.Double, System.Collections.Generic.List<System.Double>, BH.oM.Geometry.Vector)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.PrimarySideBarLines(System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<BH.oM.Geometry.Point>>>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.PrimaryCornerBarLines(BH.oM.Geometry.Polyline, System.Double, System.Double, System.Collections.Generic.List<System.Double>, System.Double[], BH.Revit.oM.Rebar.Enums.StirrupsForm)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.Mark(Autodesk.Revit.DB.Element)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.IsAnchoredOnTop(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.IsAnchoredOnBottom(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetReinforcementData(System.Collections.Generic.List<Autodesk.Revit.DB.FamilyInstance>, System.Collections.Generic.List<System.String>, BH.Revit.oM.Rebar.Enums.SectionShape)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetPotentialRebarHosts(Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetMaxConstraintDistance(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, System.Double&, System.Int32&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetHostedBars(Autodesk.Revit.DB.Element)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetColumnReinforcementData(BH.oM.Data.Collections.Table)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetColumnExtentionsData(BH.oM.Data.Collections.Table)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetAllMarksOnViews(Autodesk.Revit.DB.ViewSheet)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.VerticalLine(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.UpperEndPoint(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.TryGetThickness(Autodesk.Revit.DB.HostObjAttributes, System.Double&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.TryGetPanelOutlines(Autodesk.Revit.DB.HostObject, System.Collections.Generic.List)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.TryGetColumnHeightDomain(Autodesk.Revit.DB.FamilyInstance, System.Double&, System.Double&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.TryGetColumnBoundary(Autodesk.Revit.DB.FamilyInstance, BH.oM.Geometry.PolyCurve&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RoundRotationAngle(Autodesk.Revit.DB.Structure.Rebar, BH.oM.Geometry.TransformMatrix)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RebarTopElevation(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RebarBottomElevation(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RealBoundingBox(Autodesk.Revit.DB.FamilyInstance)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.LowerEndPoint(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.HorizontalLine(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.Height(Autodesk.Revit.DB.BoundingBoxXYZ)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.GetLinesFromPolyCurve(BH.oM.Geometry.PolyCurve)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.Direction(Autodesk.Revit.DB.Structure.Rebar, System.Collections.Generic.List<BH.oM.Geometry.Line>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.ClosestSide(BH.oM.Geometry.Point, System.Collections.Generic.List<BH.oM.Geometry.Line>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.CheckColumnDimensions(BH.oM.Geometry.PolyCurve, BH.Revit.oM.Rebar.Enums.SectionShape, BH.Revit.oM.Rebar.ColumnReinforcementData)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.CentreLines(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.BoundingBox(Autodesk.Revit.DB.ScheduleSheetInstance, Autodesk.Revit.DB.ViewSheet)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.AngleBetweenSlabAndAnchoring(BH.oM.Geometry.Polyline, Autodesk.Revit.DB.Structure.Rebar, BH.oM.Geometry.Point)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.AdjustmentToMax(Autodesk.Revit.DB.ScheduleSheetInstance, Autodesk.Revit.DB.ViewSheet)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.FindMarkedElements(System.Collections.Generic.IEnumerable<Autodesk.Revit.DB.Element>, System.String)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.FilterRebarByFunction(System.Collections.Generic.IEnumerable<Autodesk.Revit.DB.Structure.Rebar>, BH.Revit.oM.Rebar.Enums.Function)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.StirrupsFormFromString(System.String)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.SectionShape(Autodesk.Revit.DB.FamilyInstance)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.ViewTypeId(Autodesk.Revit.DB.Document, System.String)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.RebarShapeId(Autodesk.Revit.DB.Document, System.String)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.CheckRebarsPairing(System.Collections.Generic.List<Autodesk.Revit.DB.Structure.Rebar>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.CheckBending(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Query.AllBarsUnderScheduleMark(Autodesk.Revit.DB.Structure.Rebar)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.UpdateSheetSchedule(Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.UpdateDetailItem(Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.UnhideRebar(Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.SetSlabCoverRebar(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Floor, System.Double, BH.Revit.oM.Rebar.Enums.SlabFace)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.SetSlabCoverColumn(Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.Floor, System.Double, BH.Revit.oM.Rebar.Enums.SlabFace)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.SetHalftone(Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.ResetHalftone(Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.HideAllButSelected(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.DeleteAllRebarFromElement(System.Collections.Generic.List<Autodesk.Revit.DB.FamilyInstance>, Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.AdjustScheduleSummary(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.ScheduleSheetInstance)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.SetSlabCoverForRebar(Autodesk.Revit.DB.Structure.Rebar, System.Double, System.Double, System.Double, System.Boolean)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.SetColumnCover(Autodesk.Revit.DB.FamilyInstance, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.SetBendDiameter(Autodesk.Revit.DB.Structure.RebarBarType, BH.Revit.oM.Rebar.ColumnReinforcementData)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.UpdateMassSchedule(Autodesk.Revit.DB.ViewSchedule, System.Collections.Generic.List<System.String>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.UpdateLengthSchedule(Autodesk.Revit.DB.ViewSchedule, System.Collections.Generic.List<System.String>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.SetColumnWidths(Autodesk.Revit.DB.ViewSchedule, System.Collections.Generic.List<System.Double>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.RotateVertically(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.RotateOutOfColumn(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, BH.oM.Geometry.PolyCurve, BH.Revit.oM.Rebar.Enums.SectionShape, BH.oM.Geometry.TransformMatrix, System.Boolean)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.RotateHorizontally(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.TryCopyMetaData(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.FamilyInstance)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.TryCopyDimensions(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.FamilyInstance)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.SingleLegStirrupsParameters(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.ElementId, System.Int32, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.RectangleStirrupsParameters(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.ElementId, System.Int32, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.PrimaryBarsParameters(Autodesk.Revit.DB.Structure.Rebar, System.Int32)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.BentRebarParameters(Autodesk.Revit.DB.Structure.Rebar, System.Double, System.Double, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.AnchoringParameters(Autodesk.Revit.DB.Structure.Rebar, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.MoveToElevation(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.Inflate(Autodesk.Revit.DB.BoundingBoxXYZ, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.Inflate(Autodesk.Revit.DB.BoundingBoxXYZ, System.Double, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.Inflate(Autodesk.Revit.DB.BoundingBoxXYZ, System.Double, System.Double, System.Double, System.Double, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.ResetConstraintsInSingleStirrup(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.ResetConstraintsInRectStirrup(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.ResetConstraintsInOverlapStirrup(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, System.Double, System.Int32)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.BendAndRotateSingleRebar(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.ElementId, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.AnchorOnTop(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, BH.oM.Geometry.PolyCurve, Autodesk.Revit.DB.ElementId, BH.oM.Geometry.TransformMatrix, BH.Revit.oM.Rebar.Enums.SectionShape, System.Double, BH.Revit.oM.Rebar.AnchoringReinforcementData, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Modify.AlignBetweenLevels(Autodesk.Revit.DB.Structure.Rebar, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Create.RectangularColumnReinforcementData(System.Boolean, System.Collections.Generic.List<BH.Revit.oM.Rebar.BarProperties>, System.Collections.Generic.List<System.Collections.Generic.List<BH.Revit.oM.Rebar.BarProperties>>, System.Double, BH.Revit.oM.Rebar.BarProperties, BH.Revit.oM.Rebar.BarProperties, System.Double, System.Double, System.Double, System.Double, System.Double, System.String, System.Double, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Create.StarterBar(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.ElementId, BH.oM.Geometry.Vector)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Create.SingleLegStirrup(Autodesk.Revit.DB.Document, System.Int32, System.Double, Autodesk.Revit.DB.ElementId, System.Collections.Generic.List<System.Collections.Generic.List<BH.oM.Geometry.Line>>, System.Double, Autodesk.Revit.DB.Element, Autodesk.Revit.DB.Structure.RebarHookType, Autodesk.Revit.DB.Structure.RebarBarType, System.Collections.Generic.List<System.Collections.Generic.List<System.Double>>, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Create.RectangleStirrup(Autodesk.Revit.DB.Document, System.Int32, System.Double, Autodesk.Revit.DB.Structure.RebarShape, BH.oM.Geometry.Polyline, System.Double, Autodesk.Revit.DB.Element, Autodesk.Revit.DB.Structure.RebarHookType, Autodesk.Revit.DB.Structure.RebarBarType, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Create.PrimaryRebar(Autodesk.Revit.DB.Document, System.Int32, System.Collections.Generic.List<Autodesk.Revit.DB.Curve>, Autodesk.Revit.DB.Element, Autodesk.Revit.DB.Structure.RebarHookType, System.Collections.Generic.List<Autodesk.Revit.DB.Structure.RebarBarType>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Create.AdditionalBar(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Structure.RebarBarType, Autodesk.Revit.DB.Element, BH.oM.Geometry.TransformMatrix, BH.oM.Geometry.PolyCurve, BH.Revit.oM.Rebar.AdditionalBar, System.Double, BH.Revit.oM.Rebar.Enums.SectionShape)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.VectorToCopyBy(BH.Revit.oM.Rebar.Enums.SectionShape, BH.oM.Geometry.PolyCurve, BH.oM.Geometry.Point)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.UpdateBarQuantity(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.FamilyInstance)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.UpdateBarQuantity(Autodesk.Revit.DB.Document, System.Collections.Generic.List<Autodesk.Revit.DB.FamilyInstance>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.TagBarSet(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.XYZ)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.SetAllRebarSolidAndVisibleInView(Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.ReloadExcelData()", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.ReinforceRectangleColumn(Autodesk.Revit.DB.Document, System.Collections.Generic.List<Autodesk.Revit.DB.FamilyInstance>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.LoadTemplates(Autodesk.Revit.DB.Document, System.String)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.LoadExcelData(System.String)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.ExtendRebarByBending(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Floor)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.ExtendRebarByAdditionalBar(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Floor, System.Double, System.Double, System.Double, System.Double, System.Int32)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.ExtendColumnByBending(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.Floor)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.ExtendColumnByAdditionalBar(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.Floor, System.Double, System.Double, System.Double, System.Double, System.Int32)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.DetailItem(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.XYZ)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.CreateViews(Autodesk.Revit.DB.Document, System.Collections.Generic.List<Autodesk.Revit.DB.FamilyInstance>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.CreateSheetSchedule(Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.CheckSheet(Autodesk.Revit.DB.Document)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.AnchorRebarToFloor(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Floor, System.Double, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.AnchorColumnToFloor(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.Floor, System.Double, System.Double, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.StirrupDivision(System.Collections.Generic.List, System.Collections.Generic.List, System.Double[], System.Double, System.Collections.Generic.List, System.Collections.Generic.List)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.CheckNames(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.ViewSheet, System.Collections.Generic.List<Autodesk.Revit.DB.Element>, System.Collections.Generic.List<System.String>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.CheckForNotReinforcedColums(Autodesk.Revit.DB.Document, System.Collections.Generic.List<Autodesk.Revit.DB.Element>, System.Collections.Generic.List<Autodesk.Revit.DB.ElementId>, System.Collections.Generic.List<System.String>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.CheckForNotDetailedRebars(Autodesk.Revit.DB.Document, System.Collections.Generic.List<Autodesk.Revit.DB.ElementId>, System.Collections.Generic.List<System.String>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.CheckBarQuantities(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.ViewSheet, System.Collections.Generic.List<Autodesk.Revit.DB.Element>, System.Collections.Generic.List<System.String>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.ParseExcelSpreadsheet(System.String, System.Int32, System.Int32)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.PairRebars(System.Collections.Generic.List<Autodesk.Revit.DB.Structure.Rebar>, System.Collections.Generic.List<Autodesk.Revit.DB.Structure.Rebar>)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.OrderColumnsTopBottom(Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.FamilyInstance, Autodesk.Revit.DB.FamilyInstance&, Autodesk.Revit.DB.FamilyInstance&, System.String&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.OrderBarsTopBottom(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Structure.Rebar&, Autodesk.Revit.DB.Structure.Rebar&, System.String&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.StirrupReinforcementData(Autodesk.Revit.DB.Document, BH.Revit.oM.Rebar.ColumnReinforcementData, BH.Revit.oM.Rebar.StirrupsDataComputed&, System.String&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.PrimaryReinforcementData(Autodesk.Revit.DB.Document, BH.Revit.oM.Rebar.ColumnReinforcementData, BH.Revit.oM.Rebar.PrimaryBarsDataComputed&, System.String&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.ColumnReinforcementData(Autodesk.Revit.DB.FamilyInstance, BH.Revit.oM.Rebar.ColumnReinforcementData, BH.Revit.oM.Rebar.ColumnReinforcementDataComputed&, System.String&)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.CheckSlabOutlines(Autodesk.Revit.DB.Structure.Rebar, Autodesk.Revit.DB.Document, BH.oM.Geometry.TransformMatrix, System.Collections.Generic.List<BH.oM.Geometry.Polyline>, BH.Revit.oM.Rebar.Enums.SectionShape, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.AddColumnReinforcement(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.FamilyInstance, BH.Revit.oM.Rebar.RectangularColumnReinforcementData, BH.Revit.oM.Rebar.ColumnReinforcementDataComputed, BH.Revit.oM.Rebar.PrimaryBarsDataComputed, BH.Revit.oM.Rebar.StirrupsDataComputed)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.Engine.Rebar.Compute.AddBottomAnchoring(Autodesk.Revit.DB.Document, Autodesk.Revit.DB.Structure.Rebar, BH.oM.Geometry.PolyCurve, Autodesk.Revit.DB.ElementId, BH.oM.Geometry.TransformMatrix, BH.Revit.oM.Rebar.Enums.SectionShape, System.Double, BH.Revit.oM.Rebar.AnchoringReinforcementData, System.Double)", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.StirrupsForm", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.SlabFace", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.SectionShape", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.HookType", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.Function", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.BarShape", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.BarFunction", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.InputData", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.Constants", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.StirrupsDataComputed", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.RectangularColumnReinforcementData", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.PrimaryBarsDataComputed", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.ExtendingReinforcementData", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.ColumnReinforcementDataComputed", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.ColumnReinforcementData", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.AnchoringReinforcementData", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.BarProperties", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.AdditionalBar", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.Query", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.Modify", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.Create", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            MessageForDeleted.Add("BH.Revit.oM.Rebar.Compute", "Revit Rebar project has been discontinued and is not available any more. Please contact the BHoM development team in case of any questions.");
            ToNewObject.Add("BH.oM.DeepLearning.Models.Graph", UpgradeDeepLearningGraph);
            ToNewObject.Add("BH.oM.Forms.InputTree`1[[System.Object]]", UpgradeInputTree);
            ToNewObject.Add("BH.oM.Data.Collections.PointMatrix`1[[System.Object]]", UpgradePointMatrix);
            ToNewObject.Add("BH.oM.Structure.FloorSystem.FloorDesign", UpgradeFloorDesing);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeSimulationResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("GroundMaterial"))
                newVersion["GroundMaterial"] = null;

            if (newVersion.ContainsKey("ShadeMaterial"))
                newVersion["ShadeMaterial"] = null;

            return newVersion;
        }

        private static Dictionary<string, object> UpgradeLuminaire(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (newVersion.ContainsKey("Direction"))
            {
                if (newVersion["Direction"] == null)
                {
                    newVersion["Orientation"] = null;
                    newVersion.Remove("Direction");
                    return newVersion;
                }

                Dictionary<string, object> basis = null;

                try
                {
                    Dictionary<string, object> xVec = new Dictionary<string, object>() { ["X"] = 1.0, ["Y"] = 0.0, ["Z"] = 0.0 };
                    Dictionary<string, object> yVec = new Dictionary<string, object>() { ["X"] = 0.0, ["Y"] = 1.0, ["Z"] = 0.0 };
                    Dictionary<string, object> zVec = new Dictionary<string, object>() { ["X"] = 0.0, ["Y"] = 0.0, ["Z"] = 1.0 };

                    basis = new Dictionary<string, object>() { ["X"] = xVec, ["Y"] = yVec, ["Z"] = zVec };

                    Dictionary<string, object> orientation = Normalise(newVersion["Direction"] as Dictionary<string, object>);
                    double dirX = (double)orientation["X"];
                    double dirY = (double)orientation["Y"];
                    double dirZ = (double)orientation["Z"];

                    if (dirX == 0 && dirY == 0)
                    {
                        if (dirZ == 0)
                        {
                            basis = null;
                        }
                        else if (dirZ > 0)
                        {
                            basis["X"] = xVec;
                            basis["Y"] = yVec;
                            basis["Z"] = zVec;
                        }
                        else
                        {
                            basis["X"] = new Dictionary<string, object>() { ["X"] = -1.0, ["Y"] = 0.0, ["Z"] = 0.0 };
                            basis["Y"] = yVec;
                            basis["Z"] = new Dictionary<string, object>() { ["X"] = 0.0, ["Y"] = 0.0, ["Z"] = -1.0 };
                        }
                    }
                    else
                    {
                        basis["X"] = Normalise(CrossProduct(orientation, zVec));
                        basis["Y"] = Normalise(CrossProduct(orientation, xVec));
                        basis["Z"] = orientation;
                    }
                }
                catch { }

                if (basis != null)
                {
                    foreach (object o in basis.Values)
                    {
                        if (o is Dictionary<string, object> vector)
                            vector["_t"] = "BH.oM.Geometry.Vector";
                    }
                    basis["_t"] = "BH.oM.Geometry.Basis";
                }

                newVersion["Orientation"] = basis;
                newVersion.Remove("Direction");
            }
            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> CrossProduct(Dictionary<string, object> a, Dictionary<string, object> b)
        {
            double aX = (double)a["X"];
            double aY = (double)a["Y"];
            double aZ = (double)a["Z"];

            double bX = (double)b["X"];
            double bY = (double)b["Y"];
            double bZ = (double)b["Z"];

            return new Dictionary<string, object> { ["X"] = aY * bZ - aZ * bY, ["Y"] = aZ * bX - aX * bZ, ["Z"] = aX * bY - aY * bX };
        }

        /***************************************************/

        private static Dictionary<string, object> Normalise(Dictionary<string, object> a)
        {
            double x = (double)a["X"];
            double y = (double)a["Y"];
            double z = (double)a["Z"];
            double d = Math.Sqrt(x * x + y * y + z * z);

            if (d == 0)
                return null;

            return new Dictionary<string, object> { ["X"] = x / d, ["Y"] = y / d, ["Z"] = z / d };

        }

        private static Dictionary<string, object> UpgradeInputAttribute(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            newVersion.Add("Exposure", "Display");

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeDeepLearningGraph(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("Modules"))
            {
                newVersion["Modules"] = UpgradeGraph(newVersion["Modules"] as Dictionary<string, object>, "BH.oM.DeepLearning.IModule");
            }
            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeInputTree(Dictionary<string, object> oldVersion)
        {
            return UpgradeGraph(oldVersion, "System.Object");
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeGraph(Dictionary<string, object> oldVersion, string typeRestriction)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (!newVersion.ContainsKey("_t"))
                newVersion["_t"] = $"BH.oM.Data.Collections.Tree`1[[{typeRestriction}]]";


            if (newVersion.ContainsKey("Children"))
            {
                Dictionary<string, object> children = newVersion["Children"] as Dictionary<string, object>;
                if (children != null)
                {
                    Dictionary<string, object> newChildren = new Dictionary<string, object>();
                    foreach (var item in children)
                    {
                        newChildren[item.Key] = UpgradeGraph(item.Value as Dictionary<string, object>, typeRestriction);
                    }
                    newVersion["Children"] = newChildren;
                }

            }
            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradePointMatrix(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("Data"))
            {
                object[] data = newVersion["Data"] as object[];
                if (data != null)
                {
                    object[] newData = new object[data.Length];

                    for (int i = 0; i < data.Length; i++)
                    {
                        Dictionary<string, object> item = data[i] as Dictionary<string, object>;
                        if (item != null)
                        {
                            object[] localData = item["v"] as object[];

                            if (localData != null)
                            {
                                object[] newLocData = new object[localData.Length];
                                for (int j = 0; j < localData.Length; j++)
                                {
                                    if (localData[j] is Dictionary<string, object> dic)
                                    {
                                        dic["_t"] = "BH.oM.Data.Collections.LocalData`1[[System.Object]]";
                                        newLocData[j] = dic;
                                    }
                                }
                                item["v"] = newLocData;
                            }
                            newData[i] = item;
                        }
                    }

                    newVersion["Data"] = newData;
                }

            }
            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeFloorDesing(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (!newVersion.ContainsKey("ColumnConfiguration"))
                newVersion["ColumnConfiguration"] = null;

            if (newVersion.ContainsKey("FloorConfiguration"))
                newVersion["FloorConfiguration"] = UpgradeFloorConfiguration(newVersion["FloorConfiguration"] as Dictionary<string, object>);

            Dictionary<string, object> customData;
            if (newVersion.ContainsKey("CustomData"))
                customData = newVersion["CustomData"] as Dictionary<string, object>;
            else
                customData = new Dictionary<string, object>();

            MoveToCustomData(newVersion, customData, "Utilisation");
            MoveToCustomData(newVersion, customData, "MinBaysX");
            MoveToCustomData(newVersion, customData, "MinBaysY");
            MoveToCustomData(newVersion, customData, "GlobalWarmingPotential");
            MoveToCustomData(newVersion, customData, "LifeCycleAssessmentNotes");

            newVersion["CustomData"] = customData;

            return newVersion;
        }

        /***************************************************/

        private static void MoveToCustomData(Dictionary<string, object> newVersion, Dictionary<string, object> customData, string prop)
        {
            if (newVersion.ContainsKey(prop))
            {
                if (newVersion[prop] != null)
                    customData[prop] = newVersion[prop];

                newVersion.Remove(prop);
            }
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeFloorConfiguration(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion["_t"].ToString() == "BH.oM.Structure.FloorSystem.SteelInfillBeams" ||
                newVersion["_t"].ToString() == "BH.oM.Structure.FloorSystem.TimberInfillBeams" ||
                newVersion["_t"].ToString() == "BH.oM.Structure.FloorSystem.CompositeSteelInfillBeams")
            {

                bool primSet = newVersion.ContainsKey("PrimaryBeamTopLevel");
                bool secSet = newVersion.ContainsKey("SecondaryBeamTopLevel");

                if (!primSet || !secSet)
                {
                    //Sets the beam top level to the thickness of the slab as that was previously assumed, but now needs to be set explicitly
                    double thickness = TotalThickness(newVersion["Slab"] as Dictionary<string, object>);

                    if (!primSet)
                        newVersion["PrimaryBeamTopLevel"] = thickness;

                    if (!secSet)
                        newVersion["SecondaryBeamTopLevel"] = thickness;
                }
            }

            if (newVersion["_t"].ToString() == "BH.oM.Structure.FloorSystem.RCFlatPlate")
            {
                if (newVersion.ContainsKey("Reinforcement"))
                {
                    Dictionary<string, object> slabReinforcement = new Dictionary<string, object>();
                    slabReinforcement["_t"] = "BH.oM.Structure.FloorSystem.LayoutSlabReinforcement";
                    slabReinforcement["Reinforcement"] = newVersion["Reinforcement"];
                    newVersion["SlabReinforcement"] = slabReinforcement;
                    newVersion.Remove("Reinforcement");
                }
            }


            return newVersion;
        }

        /***************************************************/

        private static double TotalThickness(Dictionary<string, object> slab)
        {
            try
            {
                if (slab["_t"].ToString() == "BH.oM.Structure.SurfaceProperties.ConstantThickness")
                {
                    return (double)slab["Thickness"];
                }
                else if (slab["_t"].ToString() == "BH.oM.Structure.SurfaceProperties.SlabOnDeck")
                {
                    return (double)slab["DeckHeight"] + (double)slab["SlabThickness"];
                }
                else if (slab["_t"].ToString() == "BH.oM.Structure.SurfaceProperties.Layered")
                {
                    double thickness = 0;
                    if (slab.ContainsKey("Layers"))
                    {
                        object[] layers = slab["Layers"] as object[];
                        foreach (object layer in layers)
                        {
                            Dictionary<string, object> lay = layer as Dictionary<string, object>;
                            if (lay != null && lay.ContainsKey("Thickness"))
                            {
                                thickness += (double)lay["Thickness"];
                            }
                        }
                    }
                    return thickness;
                }
            }
            catch (Exception)
            {

                return double.NaN;
            }
            return double.NaN;
        }

        /***************************************************/
    }
}

