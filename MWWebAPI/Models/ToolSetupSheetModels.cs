using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace MWWebAPI.Models
{
    /// <summary>
    /// Summary description for ToolSetupSheetModel
    /// </summary>
    public class ToolSetUp
    {
        public int ID { get; set; }
        public string Sequence { get; set; }
        public string N { get; set; }
        public string ToolNumber { get; set; }
        public string TONumber { get; set; }
        public string CuttingMethod { get; set; }
        public string SpecialComment { get; set; }
        public int PartsPerCorner { get; set; }
        public int SecondsPerTool { get; set; }
        public string Snippet { get; set; }
        public string ToolItem { get; set; }
        public string ToolName { get; set; }
        public string ToolMWID { get; set; }
        public string ToolDesc { get; set; }
        public string ToolLoc { get; set; }
        public string ToolHolder1Item { get; set; }
        public string ToolHolder1Name { get; set; }
        public string ToolHolder1MWID { get; set; }
        public string ToolHolder1Loc { get; set; }
        public string ToolHolder2Item { get; set; }
        public string ToolHolder2Name { get; set; }
        public string ToolHolder2MWID { get; set; }
        public string ToolHolder2Loc { get; set; }
        public string ToolHolder3Item { get; set; }
        public string ToolHolder3Name { get; set; }
        public string ToolHolder3MWID { get; set; }
        public string ToolHolder3Loc { get; set; }
        public string ToolID { get; set; }
        public string ToolHolder1ID { get; set; }
        public string ToolHolder2ID { get; set; }
        public string ToolHolder3ID { get; set; }
        public string Comment { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class ToolSetupSheet
    {
        public int SetUpSheetID { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string Revision { get; set; }
        public string Operation { get; set; }
        public string Machine { get; set; }
        public string InputDate { get; set; }
        public string ProgramNumber { get; set; }
        public string ProgramLocation { get; set; }
        public string UOM { get; set; }
        public string MaterialType { get; set; }
        public string MaterialHeatTreated { get; set; }
        public string MaterialForm { get; set; }
        public string MaterialSize { get; set; }
        public string MachineWorkHoldingTo { get; set; }
        public string CutWorkHoldingTo { get; set; }        
        public string workHolding1ItemNumber { get; set; }

        public string workHolding2ItemNumber { get; set; }
        public string workHolding3ItemNumber { get; set; }
        public string workHolding1ImagePath { get; set; }
        public string workHolding2ImagePath { get; set; }
        public string workHolding3ImagePath { get; set; }
        public string workHolding1MWID { get; set; }
        public string workHolding2MWID { get; set; }
        public string workHolding3MWID { get; set; }
        public string workHolding1Location { get; set; }
        public string workHolding2Location { get; set; }
        public string workHolding3Location { get; set; }
        public string workHoldingComments { get; set; }
        public string workHoldingImageNoPart { get; set; }
        public string workHoldingImageWithPart { get; set; }
        public string workHoldingImageComplete { get; set; }
        public string Torque { get; set; }
        public string HoldPartOn { get; set; }
        public string BarStickOutBefore { get; set; }
        public string FaceOff { get; set; }
        public string Z0 { get; set; }
        public string CutOffToolThickness { get; set; }
        public string OAL { get; set; }
        public string BarStickOutAfter { get; set; }
        public string BarPullOut { get; set; }
        public string PartStickOutMinimum { get; set; }
        public string Comments { get; set; }
        public string Program { get; set; }
        public string ModifiedBy { get; set; }
        public List<ToolSetUp> ToolsSetUp { get; set; }

    }

    public class MWJsonResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class ToolSetupSheetHeader
    {
        public int ID { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string Revision { get; set; }
        public string Operation { get; set; }
    }

    public class ConversionRule
    {
        public int ID { get; set; }
        public string FromMachineId { get; set; }
        public string ToMachineId { get; set; }
        public string FromSnippet { get; set; }
        public string ToSnippet { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ConvertedProgramRequest
    {
        public int SetUpSheetID { get; set; }
        public string Program { get; set; }
        public string MachineId { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ToolSetupGroupRequest
    {
        public int MainID { get; set; }
        public int[] IDs { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ToolSetupSearchResult
    {
        public int ID { get; set; }
        public string SpecialComment { get; set; }
        public string GroupType { get; set; }
    }

    public class AddToolSetupRequest
    {
        public int SetUpSheetID { get; set; }
        public string[] ID_GroupType { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class DeleteToolSetupRequest
    {
        public int SetupSheetID { get; set; }
        public int ToolSetupID { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ConvertProgramSaveRequest
    {
        public string Program { get; set; }
        public string MachineID { get; set; }
        public int SetUpSheetID { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ConvertProgramRequest
    {
        public string Program { get; set; }
        public string FromMachineID { get; set; }
        public string ToMachineID { get; set; }
    }

    public class ToolSetupSearchRequest
    {
        public string SearchTerm { get; set; }
    }

}