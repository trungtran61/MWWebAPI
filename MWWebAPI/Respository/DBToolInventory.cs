﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using MWWebAPI.Models;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace MWWebAPI.DBRepository
{
    public class DBToolInventoryRepository : DBRepositoryBase, IDisposable
    {
        public string[] GetCuttingMethodTemplate(string cuttingMethod)
        {
            List<string> retTemplate = new List<string>();
            
            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetCuttingMethodTemplate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CuttingMethod", SqlDbType.VarChar, 50).Value = cuttingMethod;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string Template = reader["Template"].ToString();
                        string[] lines = Template.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                        
                        StringBuilder sbLines = new StringBuilder();
                        for (int i = 0; i < lines.Count(); i++)
                        {
                            sbLines.AppendLine(lines[i]);
                        }
                        retTemplate.Add(sbLines.ToString());
                    }
                }
                con.Close();
            }
            return retTemplate.ToArray();
        }

        public DBResponse UpdateCuttingMethodTemplate(CuttingMethodTemplate cuttingMethodTemplate)
        {
            DBResponse dbResponse = new DBResponse();

            try
            {
                using (SqlConnection con = new SqlConnection(MWConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spUpdateCuttingMethodTemplate", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CuttingMethod", SqlDbType.VarChar,50).Value = cuttingMethodTemplate.CuttingMethod;
                        cmd.Parameters.Add("@Template", SqlDbType.VarChar).Value = cuttingMethodTemplate.Template;
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        dbResponse.RecordsAffected = rowsAffected;

                        if (rowsAffected > 0)
                        {
                            dbResponse.ReturnCode = 0;
                        }
                        else
                        {
                            dbResponse.ReturnCode = -1;
                            dbResponse.Message = "No update";
                        }
                    }
                    con.Close();
                }
            }
            catch
            {
                dbResponse.ReturnCode = -1;
                throw;
            }

            return dbResponse;         
        }

        public List<CuttingMethodTemplate> GetCuttingMethodsWithTemplate(string term)
        {
            List<CuttingMethodTemplate> cuttingMethodTemplates = new List<CuttingMethodTemplate>();

            using (SqlConnection conn = new SqlConnection(MWConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spGetCuttingMethodsWithTemplate";
                    cmd.Parameters.Add("@searchterm", SqlDbType.VarChar, 50).Value = term;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    conn.Open();

                    using (SqlDataReader rdLookup = cmd.ExecuteReader())
                    {
                        while (rdLookup.Read())
                        {
                            CuttingMethodTemplate cuttingMethodTemplate = new CuttingMethodTemplate
                            {
                                CuttingMethod = rdLookup["CuttingMethod"].ToString(),
                                Template = rdLookup["Template"].ToString()
                            };
                            cuttingMethodTemplates.Add(cuttingMethodTemplate);
                        }
                    }
                }
            }
            return cuttingMethodTemplates;
        }
        public List<Lookup> GetLookupByCategory(string category, string term)
        {
            List<Lookup> lookups = new List<Lookup>();

            using (SqlConnection conn = new SqlConnection(MWConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spGetLookUp";
                    cmd.Parameters.Add("@category", SqlDbType.VarChar, 50).Value = category;
                    cmd.Parameters.Add("@searchterm", SqlDbType.VarChar, 50).Value = term;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    conn.Open();

                    using (SqlDataReader rdLookup = cmd.ExecuteReader())
                    {
                        while (rdLookup.Read())
                        {
                            Lookup lookup = new Lookup
                            {
                                Text = rdLookup["mText"].ToString(),
                                Value = rdLookup["mValue"].ToString()
                            };
                            lookups.Add(lookup);
                        }
                    }
                }
            }
            return lookups;
        }

        public ToolSetupSheet GetToolSetupSheet(int setupSheetId)
        {
            ToolSetupSheet toolSetupSheet = new ToolSetupSheet();
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = MWConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "spGetToolSetupSheet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SetupSheetID", SqlDbType.Int).Value = setupSheetId;
                        cmd.Connection = conn;
                        conn.Open();
                        DataSet dsSetUpSheet = new DataSet();
                        SqlDataAdapter daSetUpSheet = new SqlDataAdapter();
                        daSetUpSheet.SelectCommand = cmd;
                        daSetUpSheet.Fill(dsSetUpSheet);
                        DataRow drSetupSheet = dsSetUpSheet.Tables[0].Rows[0];
                        DataTable dtToolSetup = dsSetUpSheet.Tables[1];
                        toolSetupSheet.SetUpSheetID = setupSheetId;
                        toolSetupSheet.PartNumber = drSetupSheet["PartNumber"].ToString();
                        toolSetupSheet.Revision = drSetupSheet["Revision"].ToString();
                        toolSetupSheet.Operation = drSetupSheet["Operation"].ToString();
                        toolSetupSheet.InputDate = Convert.ToDateTime(drSetupSheet["InputDate"].ToString()).ToShortDateString();
                        toolSetupSheet.Machine = drSetupSheet["Machine"].ToString();
                        toolSetupSheet.ProgramNumber = drSetupSheet["ProgramNumber"].ToString();
                        toolSetupSheet.ProgramLocation = drSetupSheet["ProgramLocation"].ToString();
                        toolSetupSheet.UOM = drSetupSheet["UOM"].ToString();
                        toolSetupSheet.MaterialType = drSetupSheet["MaterialType"].ToString();
                        toolSetupSheet.MaterialHeatTreated = drSetupSheet["MaterialHeatTreated"].ToString();
                        toolSetupSheet.MaterialForm = drSetupSheet["MaterialForm"].ToString();
                        toolSetupSheet.MaterialSize = drSetupSheet["MaterialSize"].ToString();
                        toolSetupSheet.MachineWorkHoldingTo = drSetupSheet["MachineWorkHoldingTo"].ToString();
                        toolSetupSheet.CutWorkHoldingTo = drSetupSheet["CutWorkHoldingTo"].ToString();
                        toolSetupSheet.workHolding1ItemNumber = drSetupSheet["workHolding1ItemNumber"].ToString();
                        toolSetupSheet.workHolding2ItemNumber = drSetupSheet["workHolding2ItemNumber"].ToString();
                        toolSetupSheet.workHolding3ItemNumber = drSetupSheet["workHolding3ItemNumber"].ToString();
                        toolSetupSheet.workHolding1ImagePath = drSetupSheet["workHolding1ImagePath"].ToString();
                        toolSetupSheet.workHolding2ImagePath = drSetupSheet["workHolding2ImagePath"].ToString();
                        toolSetupSheet.workHolding3ImagePath = drSetupSheet["workHolding3ImagePath"].ToString();
                        toolSetupSheet.workHolding1MWID = drSetupSheet["workHolding1MWID"].ToString();
                        toolSetupSheet.workHolding2MWID = drSetupSheet["workHolding2MWID"].ToString();
                        toolSetupSheet.workHolding3MWID = drSetupSheet["workHolding3MWID"].ToString();
                        toolSetupSheet.workHolding1Location = drSetupSheet["workHolding1Location"].ToString();
                        toolSetupSheet.workHolding2Location = drSetupSheet["workHolding2Location"].ToString();
                        toolSetupSheet.workHolding3Location = drSetupSheet["workHolding3Location"].ToString();
                        toolSetupSheet.workHoldingComments = drSetupSheet["workHoldingComments"].ToString();
                        toolSetupSheet.workHoldingImageNoPart = drSetupSheet["workHoldingImageNoPart"].ToString();
                        toolSetupSheet.workHoldingImageWithPart = drSetupSheet["workHoldingImageWithPart"].ToString();
                        toolSetupSheet.workHoldingImageComplete = drSetupSheet["workHoldingImageComplete"].ToString();
                        toolSetupSheet.Torque = drSetupSheet["Torque"].ToString();
                        toolSetupSheet.HoldPartOn = drSetupSheet["HoldPartOn"].ToString();
                        toolSetupSheet.Z0 = drSetupSheet["Z0"].ToString();
                        toolSetupSheet.BarStickOutBefore = drSetupSheet["BarStickOutBefore"].ToString();
                        toolSetupSheet.FaceOff = drSetupSheet["FaceOff"].ToString();
                        toolSetupSheet.CutOffToolThickness = drSetupSheet["CutOffToolThickness"].ToString();
                        toolSetupSheet.OAL = drSetupSheet["OAL"].ToString();
                        toolSetupSheet.BarStickOutAfter = drSetupSheet["BarStickOutAfter"].ToString();
                        toolSetupSheet.BarPullOut = drSetupSheet["BarPullOut"].ToString();
                        toolSetupSheet.OAL = drSetupSheet["OAL"].ToString();
                        toolSetupSheet.PartStickOutMinimum = drSetupSheet["PartStickOutMinimum"].ToString();
                        toolSetupSheet.Comments = drSetupSheet["Comments"].ToString();
                        toolSetupSheet.Program = drSetupSheet["Program"].ToString();

                        List<ToolSetUp> lstToolSetup = new List<ToolSetUp>();

                        foreach (DataRow drToolSetup in dtToolSetup.Rows)
                        {
                            ToolSetUp toolSetup = new ToolSetUp();
                            toolSetup.ID = Convert.ToInt32(drToolSetup["Id"].ToString());
                            toolSetup.Sequence = drToolSetup["Sequence"].ToString();
                            toolSetup.N = drToolSetup["N"].ToString();
                            toolSetup.ToolNumber = drToolSetup["ToolNumber"].ToString();
                            toolSetup.TONumber = drToolSetup["TONumber"].ToString();
                            //toolSetup.CuttingMethodId = drToolSetup["CuttingMethodId"].ToString();
                            toolSetup.CuttingMethod = drToolSetup["CuttingMethod"].ToString();
                            toolSetup.SpecialComment = drToolSetup["SpecialComment"].ToString();
                            toolSetup.PartsPerCorner = Convert.ToInt32(drToolSetup["PartsPerCorner"].ToString());
                            toolSetup.SecondsPerTool = Convert.ToInt32(drToolSetup["SecondsPerTool"].ToString());
                            toolSetup.Comment = drToolSetup["Comment"].ToString();
                            toolSetup.Snippet = drToolSetup["Snippet"].ToString();
                            toolSetup.ToolDesc = drToolSetup["ToolDesc"].ToString();
                            toolSetup.ToolName = drToolSetup["ToolName"].ToString();
                            toolSetup.ToolHolder1Item = drToolSetup["ToolHolder1Item"].ToString();
                            toolSetup.ToolHolder2Item = drToolSetup["ToolHolder2Item"].ToString();
                            toolSetup.ToolHolder3Item = drToolSetup["ToolHolder3Item"].ToString();
                            toolSetup.ToolHolder1Name = drToolSetup["ToolHolder1Name"].ToString();
                            toolSetup.ToolHolder2Name = drToolSetup["ToolHolder2Name"].ToString();
                            toolSetup.ToolHolder3Name = drToolSetup["ToolHolder3Name"].ToString();
                            toolSetup.ToolHolder1MWID = drToolSetup["ToolHolder1MWID"].ToString();
                            toolSetup.ToolHolder2MWID = drToolSetup["ToolHolder2MWID"].ToString();
                            toolSetup.ToolHolder3MWID = drToolSetup["ToolHolder3MWID"].ToString();
                            toolSetup.ToolHolder1Loc = drToolSetup["ToolHolder1Loc"].ToString();
                            toolSetup.ToolHolder2Loc = drToolSetup["ToolHolder2Loc"].ToString();
                            toolSetup.ToolHolder3Loc = drToolSetup["ToolHolder3Loc"].ToString();
                            toolSetup.ToolID = drToolSetup["ToolID"].ToString();
                            toolSetup.ToolHolder1ID = drToolSetup["ToolHolder1ID"].ToString();
                            toolSetup.ToolHolder2ID = drToolSetup["ToolHolder2ID"].ToString();
                            toolSetup.ToolHolder3ID = drToolSetup["ToolHolder3ID"].ToString();
                            lstToolSetup.Add(toolSetup);
                        }
                        toolSetupSheet.ToolsSetUp = lstToolSetup;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return toolSetupSheet;
        }

        public DBResponse UpdateToolSetupSheet(ToolSetupSheet toolSetupSheet)
        {
            DBResponse dbResponse = new DBResponse();
            string sMachine = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = MWConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "spSaveToolSetupSheet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SetupSheetID", SqlDbType.Int).Value = toolSetupSheet.SetUpSheetID;
                        cmd.Parameters.Add("@PartNumber", SqlDbType.VarChar, 20).Value = toolSetupSheet.PartNumber;
                        cmd.Parameters.Add("@Revision", SqlDbType.VarChar, 20).Value = toolSetupSheet.Revision;
                        if (toolSetupSheet.Operation != string.Empty)
                            cmd.Parameters.Add("@Operation", SqlDbType.Int).Value = toolSetupSheet.Operation;
                        cmd.Parameters.Add("@InputDate", SqlDbType.DateTime, 20).Value = toolSetupSheet.InputDate;

                        switch (toolSetupSheet.Machine.ToLower())
                        {
                            case "lathe":
                                sMachine = "L01";
                                break;
                            case "mill":
                                sMachine = "M01";
                                break;
                            case "wireEDM":
                                sMachine = "E01";
                                break;
                            default:
                                sMachine = toolSetupSheet.Machine;
                                break;
                        }
                        
                        cmd.Parameters.Add("@Machine", SqlDbType.VarChar, 50).Value = sMachine;
                        cmd.Parameters.Add("@ProgramNumber", SqlDbType.VarChar, 20).Value = toolSetupSheet.ProgramNumber;
                        cmd.Parameters.Add("@ProgramLocation", SqlDbType.VarChar, 50).Value = toolSetupSheet.ProgramLocation;
                        cmd.Parameters.Add("@UOM", SqlDbType.VarChar, 20).Value = toolSetupSheet.UOM;
                        cmd.Parameters.Add("@MaterialType", SqlDbType.VarChar, 50).Value = toolSetupSheet.MaterialType;
                        cmd.Parameters.Add("@MaterialHeatTreated", SqlDbType.VarChar, 50).Value = toolSetupSheet.MaterialHeatTreated;
                        cmd.Parameters.Add("@MaterialForm", SqlDbType.VarChar, 50).Value = toolSetupSheet.MaterialForm;
                        cmd.Parameters.Add("@MaterialSize", SqlDbType.VarChar, 20).Value = toolSetupSheet.MaterialSize;
                        cmd.Parameters.Add("@MachineWorkHoldingTo", SqlDbType.VarChar, 20).Value = toolSetupSheet.MachineWorkHoldingTo;
                        cmd.Parameters.Add("@workHolding1ItemNumber", SqlDbType.VarChar, 20).Value = toolSetupSheet.workHolding1ItemNumber;
                        cmd.Parameters.Add("@workHolding2ItemNumber", SqlDbType.VarChar, 20).Value = toolSetupSheet.workHolding2ItemNumber;
                        cmd.Parameters.Add("@workHolding3ItemNumber", SqlDbType.VarChar, 20).Value = toolSetupSheet.workHolding3ItemNumber;
                        cmd.Parameters.Add("@workHolding1ImagePath", SqlDbType.VarChar, 100).Value = toolSetupSheet.workHolding1ImagePath;
                        cmd.Parameters.Add("@workHolding2ImagePath", SqlDbType.VarChar, 100).Value = toolSetupSheet.workHolding2ImagePath;
                        cmd.Parameters.Add("@workHolding3ImagePath", SqlDbType.VarChar, 100).Value = toolSetupSheet.workHolding3ImagePath;
                        cmd.Parameters.Add("@workHolding1MWID", SqlDbType.VarChar, 20).Value = toolSetupSheet.workHolding1MWID;
                        cmd.Parameters.Add("@workHolding2MWID", SqlDbType.VarChar, 20).Value = toolSetupSheet.workHolding2MWID;
                        cmd.Parameters.Add("@workHolding3MWID", SqlDbType.VarChar, 20).Value = toolSetupSheet.workHolding3MWID;
                        cmd.Parameters.Add("@workHolding1Location", SqlDbType.VarChar, 50).Value = toolSetupSheet.workHolding1Location;
                        cmd.Parameters.Add("@workHolding2Location", SqlDbType.VarChar, 50).Value = toolSetupSheet.workHolding2Location;
                        cmd.Parameters.Add("@workHolding3Location", SqlDbType.VarChar, 50).Value = toolSetupSheet.workHolding3Location;
                        cmd.Parameters.Add("@workHoldingComments", SqlDbType.VarChar, 200).Value = toolSetupSheet.workHoldingComments;
                        cmd.Parameters.Add("@workHoldingImageNoPart", SqlDbType.VarChar, 200).Value = toolSetupSheet.workHoldingImageNoPart;
                        cmd.Parameters.Add("@workHoldingImageWithPart", SqlDbType.VarChar, 200).Value = toolSetupSheet.workHoldingImageWithPart;
                        cmd.Parameters.Add("@workHoldingImageComplete", SqlDbType.VarChar, 200).Value = toolSetupSheet.workHoldingImageComplete;
                        cmd.Parameters.Add("@Torque", SqlDbType.VarChar, 20).Value = toolSetupSheet.Torque;
                        cmd.Parameters.Add("@HoldPartOn", SqlDbType.VarChar, 20).Value = toolSetupSheet.HoldPartOn;
                        cmd.Parameters.Add("@Z0", SqlDbType.VarChar, 20).Value = toolSetupSheet.Z0;
                        cmd.Parameters.Add("@BarStickOutBefore", SqlDbType.VarChar, 20).Value = toolSetupSheet.BarStickOutBefore;
                        cmd.Parameters.Add("@FaceOff", SqlDbType.VarChar, 20).Value = toolSetupSheet.FaceOff;
                        cmd.Parameters.Add("@CutOffToolThickness", SqlDbType.VarChar, 20).Value = toolSetupSheet.CutOffToolThickness;
                        cmd.Parameters.Add("@OAL", SqlDbType.VarChar, 20).Value = toolSetupSheet.OAL;
                        cmd.Parameters.Add("@BarStickOutAfter", SqlDbType.VarChar, 20).Value = toolSetupSheet.BarStickOutAfter;
                        cmd.Parameters.Add("@BarPullOut", SqlDbType.VarChar, 20).Value = toolSetupSheet.BarPullOut;
                        cmd.Parameters.Add("@PartStickOutMinimum", SqlDbType.VarChar, 20).Value = toolSetupSheet.PartStickOutMinimum;
                        cmd.Parameters.Add("@Comments", SqlDbType.VarChar, 200).Value = toolSetupSheet.Comments;
                        cmd.Parameters.Add("@Program", SqlDbType.VarChar).Value = toolSetupSheet.Program;
                        cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = toolSetupSheet.ModifiedBy;
                        cmd.Parameters.Add("@CutWorkHoldingTo", SqlDbType.VarChar, 20).Value = toolSetupSheet.CutWorkHoldingTo;
                        // sql 2005 doesn't have table type
                        //cmd.Parameters.Add("@ToolsList", SqlDbType.Structured).Value = dtToolSetup;                        
                        //var pList = new SqlParameter("@ToolSetupList", SqlDbType.Structured);
                        //pList.TypeName = "dbo.ToolSetup";
                        //pList.Value = dtToolSetup;
                        //cmd.Parameters.Add(pList);
                        cmd.Connection = conn;
                        conn.Open();
                        //cmd.ExecuteNonQuery();
                        string sSetupSheetId = cmd.ExecuteScalar().ToString();
                        conn.Close();
                        dbResponse.ReturnCode = 0;
                        RefreshLookupCaches();
                    }
                }
                foreach (ToolSetUp toolSetUp in toolSetupSheet.ToolsSetUp)
                {
                    if (toolSetUp.N != string.Empty)
                        SaveToolsSetup(toolSetupSheet.SetUpSheetID, toolSetUp, toolSetupSheet.ModifiedBy);
                }
            }
            catch (Exception ex)
            {
                dbResponse.ReturnCode = -1;
                dbResponse.Message = ex.Message;
                throw;
            }

            return dbResponse;
        }

        private void SaveToolsSetup(int SetUpSheetID, ToolSetUp toolSetUp, string userName)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = MWConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spSaveToolSetupSheetToolSetups";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SetupSheetID", SqlDbType.Int).Value = SetUpSheetID;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = toolSetUp.ID;
                    cmd.Parameters.Add("@Sequence", SqlDbType.Int).Value = toolSetUp.Sequence;
                    cmd.Parameters.Add("@N", SqlDbType.Int).Value = toolSetUp.N;
                    cmd.Parameters.Add("@TONumber", SqlDbType.Int).Value = toolSetUp.TONumber;
                    cmd.Parameters.Add("@ToolNumber", SqlDbType.Int).Value = toolSetUp.ToolNumber;
                    cmd.Parameters.Add("@CuttingMethod", SqlDbType.VarChar, 50).Value = toolSetUp.CuttingMethod;
                    cmd.Parameters.Add("@PartsPerCorner", SqlDbType.Int).Value = toolSetUp.PartsPerCorner;
                    cmd.Parameters.Add("@SecondsPerTool", SqlDbType.Int).Value = toolSetUp.SecondsPerTool;
                    cmd.Parameters.Add("@Comment", SqlDbType.VarChar, 50).Value = toolSetUp.Comment;
                    cmd.Parameters.Add("@Snippet", SqlDbType.VarChar).Value = toolSetUp.Snippet;
                    if (toolSetUp.ToolID != string.Empty)
                        cmd.Parameters.Add("@ToolID", SqlDbType.Int).Value = toolSetUp.ToolID;
                    if (toolSetUp.ToolHolder1ID != string.Empty)
                        cmd.Parameters.Add("@ToolHolder1ID", SqlDbType.Int).Value = toolSetUp.ToolHolder1ID;
                    if (toolSetUp.ToolHolder2ID != string.Empty)
                        cmd.Parameters.Add("@ToolHolder2ID", SqlDbType.Int).Value = toolSetUp.ToolHolder2ID;
                    if (toolSetUp.ToolHolder3ID != string.Empty)
                        cmd.Parameters.Add("@ToolHolder3ID", SqlDbType.Int).Value = toolSetUp.ToolHolder3ID;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = userName;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        public DBResponse AddToolSetupToSetupSheet(AddToolSetupRequest addToolSetupRequest)
        {
            DBResponse dbResponse = new DBResponse();
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = MWConnectionString;
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "spAddSelectedToolSetupToSetupSheet";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ToolSetupSheetID", SqlDbType.Int).Value = addToolSetupRequest.SetUpSheetID;
                        string ID_GroupTypes = string.Join(",", addToolSetupRequest.ID_GroupType.Select(x => x.ToString()).ToArray());
                        cmd.Parameters.Add("@ID_GroupTypes", SqlDbType.VarChar, 100).Value = ID_GroupTypes;
                        cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = addToolSetupRequest.ModifiedBy;
                        cmd.Connection = conn;
                        conn.Open();
                        dbResponse.RecordsAffected = cmd.ExecuteNonQuery();
                        conn.Close();
                        dbResponse.ReturnCode = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return dbResponse;
        }
        public DBResponse DeleteConversionRule(ConversionRule conversionRule)
        {
            DBResponse dbResponse = new DBResponse();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = MWConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spDeleteConversionRule";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = conversionRule.ID;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = conversionRule.ModifiedBy;
                    cmd.Connection = conn;
                    conn.Open();
                    dbResponse.RecordsAffected = cmd.ExecuteNonQuery();
                    dbResponse.ReturnCode = 0;
                }
            }
            return dbResponse;
        }

        public DBResponse DeleteToolSetup(DeleteToolSetupRequest deleteToolSetupRequest)
        {
            DBResponse dbResponse = new DBResponse();
            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                string query = "spDeleteToolSetup";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SetupSheetID", SqlDbType.Int).Value = deleteToolSetupRequest.SetupSheetID;
                    cmd.Parameters.Add("@ToolSetupId", SqlDbType.Int).Value = deleteToolSetupRequest.ToolSetupID;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = deleteToolSetupRequest.ModifiedBy;
                    con.Open();
                    dbResponse.RecordsAffected = cmd.ExecuteNonQuery();
                    con.Close();
                    dbResponse.ReturnCode = 0;
                }               
            }
            return dbResponse;
        }

        public List<ConversionRule> GetConversionRules (string FromMachineId, string ToMachineId)
        {
            List<ConversionRule> ConversionRules = new List<ConversionRule>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = MWConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spGetConversionRules";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FromMachineId", SqlDbType.VarChar, 20).Value = FromMachineId;
                    cmd.Parameters.Add("@ToMachineId", SqlDbType.VarChar, 20).Value = ToMachineId;
                    cmd.Connection = conn;
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        ConversionRule oConversionRule = new ConversionRule();
                        oConversionRule.ID = Convert.ToInt32(reader["ID"].ToString());
                        oConversionRule.FromSnippet = reader["FromSnippet"].ToString();
                        oConversionRule.ToSnippet = reader["ToSnippet"].ToString();
                        ConversionRules.Add(oConversionRule);
                    }                    
                }
            }

            return ConversionRules;
        }

        public List<string> GetMachines(string machinePrefix)
        {
            List<string> retMachine = new List<string>();
            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                string query = "spGetMachines";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MachinePrefix", SqlDbType.Char, 1).Value = machinePrefix;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        retMachine.Add(reader["MachineId"].ToString());
                    }
                }
                con.Close();
            }
            return retMachine;
        }

        public DBResponse SaveConversionRule(ConversionRule conversionRule)
        {
            DBResponse dbResponse = new DBResponse();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = MWConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spSaveConversionRule";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@RuleId", SqlDbType.VarChar, 20).Value = conversionRule.ID;
                    cmd.Parameters.Add("@FromMachineId", SqlDbType.VarChar, 20).Value = conversionRule.FromMachineId;
                    cmd.Parameters.Add("@ToMachineId", SqlDbType.VarChar, 20).Value = conversionRule.ToMachineId;
                    cmd.Parameters.Add("@FromSnippet", SqlDbType.VarChar, 1000).Value = conversionRule.FromSnippet;
                    cmd.Parameters.Add("@ToSnippet", SqlDbType.VarChar, 1000).Value = conversionRule.ToSnippet;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = conversionRule.ModifiedBy;
                    cmd.Connection = conn;
                    conn.Open();
                    dbResponse.RecordsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    dbResponse.ReturnCode = 0;
                    RefreshLookupCaches();
                }
            }
            return dbResponse;
        }

        public int SaveConvertedProgram(ProgramSaveRequest convertProgramSaveRequest)
        {
            int newSetUpSheetID = 0;

            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spCopyToolSetupSheet", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = convertProgramSaveRequest.ModifiedBy;
                    cmd.Parameters.Add("@ToolSetupSheetID", SqlDbType.Int).Value = convertProgramSaveRequest.SetUpSheetID;
                    newSetUpSheetID = Convert.ToInt32(cmd.ExecuteScalar());
                }               
            }

            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spUpdateConvertedToolSetupSheet", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = convertProgramSaveRequest.ModifiedBy;
                    cmd.Parameters.Add("@Program", SqlDbType.VarChar).Value = convertProgramSaveRequest.Program;
                    cmd.Parameters.Add("@MachineId", SqlDbType.VarChar).Value = convertProgramSaveRequest.SetUpSheetID;
                    cmd.Parameters.Add("@ToolSetupSheetID", SqlDbType.Int).Value = newSetUpSheetID;
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }

            return newSetUpSheetID;
        }

        public void SaveProgram(ProgramSaveRequest programSaveRequest)
        {           
            string filePath = 
                string.Format("{0}\\tss_{1}.txt", HostingEnvironment.MapPath("~\\UnprovenPrograms"), programSaveRequest.SetUpSheetID);
            File.WriteAllText(filePath, programSaveRequest.Program);
        }

        public DBResponse SaveToolSetupGroup(ToolSetupGroupRequest tooSetupGroupRequest)
        {
            DBResponse dbResponse = new DBResponse();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = MWConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "spSaveToolSetupGroup";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MainID", SqlDbType.Int).Value = tooSetupGroupRequest.MainID;
                    string IDs = string.Join(",", tooSetupGroupRequest.IDs.Select(x => x.ToString()).ToArray());
                    cmd.Parameters.Add("@IDs", SqlDbType.VarChar, 100).Value = IDs;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 20).Value = tooSetupGroupRequest.ModifiedBy;
                    cmd.Connection = conn;
                    conn.Open();
                    dbResponse.RecordsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                    dbResponse.ReturnCode = 0;
                }
            }
            return dbResponse;
        }

        public List<ToolSetupSearchResult> SearchToolSetups(string searchTerm)
        {
            List<ToolSetupSearchResult> retToolSetups = new List<ToolSetupSearchResult>();
            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                string query = "spSearchToolSetups";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SearchTerm", SqlDbType.VarChar, 50).Value = searchTerm;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        retToolSetups.Add(new ToolSetupSearchResult
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            SpecialComment = reader["SpecialComment"].ToString(),
                            GroupType = reader["GroupType"].ToString()
                        }
                            );
                    }
                }
                con.Close();
            }
            return retToolSetups;
        }

        public List<string> GetSearchResults(string term, string category)
        {
            List<string> retResults = new List<string>();
            string sStoredProc = "spGetLookupValues";
            
                using (SqlConnection con = new SqlConnection(MWConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sStoredProc, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Category", SqlDbType.VarChar, 100).Value = category;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retResults.Add(string.Format("{0}|{1}", reader["Name"], reader["Description"]));
                        }
                    }
                    con.Close();                    
                }
            
            return retResults;
        }

        public List<string> GetSSPPartNumbers(string term)
        {
            List<string> retPartNumbers = new List<string>();
            if (HttpRuntime.Cache["PartNumber"] == null)
            {
                using (SqlConnection con = new SqlConnection(MWConnectionString))
                {
                    string query = "spGetSSPPartNumbers";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SearchTerm", SqlDbType.VarChar, 100).Value = term.Replace("'", "''");
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            retPartNumbers.Add(reader["PartNumber"].ToString());
                            //retPartNumbers.Add(string.Format("{0}|{1}|{2}", reader["PartNumber"], reader["Revision"], reader["HID"]));
                        }
                    }
                    con.Close();
                    if (term == "")
                        HttpRuntime.Cache["PartNumber"] = retPartNumbers;
                }
            }
            return retPartNumbers;
        }

        public List<string> GetMaterialSize(string term, string materialtype)
        {
            List<string> retMaterialSize = new List<string>();

            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                string query = "spGetMaterialSize";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SearchTerm", SqlDbType.VarChar, 100).Value = term.Replace("'", "''");
                    cmd.Parameters.Add("@MaterialType", SqlDbType.VarChar, 100).Value = materialtype.Replace("'", "''");
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        retMaterialSize.Add(reader["mwSize"].ToString());
                    }
                }
                con.Close();
            }
            return retMaterialSize;
        }

        public List<string> GetSSPOperations(string term, string partid)
        {
            List<string> retOperations = new List<string>();

            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                string query = "spGetSSPOperations";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@SearchTerm", SqlDbType.VarChar, 100).Value = term.Replace("'", "''");
                    cmd.Parameters.Add("@PartId", SqlDbType.Int).Value = partid;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        retOperations.Add(reader["Operation"].ToString());
                    }
                }
                con.Close();
            }
            return retOperations;
        }

        public List<string> GetCuttingMethodTemplate(string cuttingMethod, string N)
        {
            List<string> retTemplate = new List<string>();
            string twoDigitN = N.PadLeft(2, '0');

            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetCuttingMethodTemplate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CuttingMethod", SqlDbType.VarChar, 50).Value = cuttingMethod;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string Template = reader["Template"].ToString();
                        string[] lines = Template.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
                        int iTindex = 0;
                        StringBuilder sbLines = new StringBuilder();
                        for (int i = 0; i < lines.Count(); i++)
                        {
                            //if (i == 0) lines[0] = "N" + twoDigitN;
                            Match match = Regex.Match(lines[i], @"[T][0-9]{4}");
                            if (match.Success)
                            {
                                iTindex++;
                                if (iTindex == 1 || iTindex == 3) lines[i] = Regex.Replace(lines[i], @"[T][0-9]{4}", "T" + twoDigitN + "00");
                                if (iTindex == 2) lines[i] = Regex.Replace(lines[i], @"[T][0-9]{4}", "T" + twoDigitN + twoDigitN);
                            }
                            sbLines.AppendLine(lines[i]);
                        }
                        retTemplate.Add(sbLines.ToString());
                        //retTemplate.Add(reader["Template"].ToString());
                    }
                }
                con.Close();
            }
            return retTemplate;
        }
        public List<ToolSetupSheetHeader> GetSetupSheets(string partnumber, string revision, string operation)
        {
            List<ToolSetupSheetHeader> setupSheetHeaders = new List<ToolSetupSheetHeader>();
            try
            {
                using (SqlConnection con = new SqlConnection(MWConnectionString))
                {
                    string query = "spGetSetupSheets";
                    if (partnumber == "recent")
                        query = "spGetRecentSetupSheets";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (partnumber != "recent")
                        {
                            cmd.Parameters.Add("@partnumber", SqlDbType.VarChar, 100).Value = partnumber.Replace("'", "''");
                            cmd.Parameters.Add("@revision", SqlDbType.VarChar, 20).Value = revision;
                            if (operation != string.Empty)
                                cmd.Parameters.Add("@operation", SqlDbType.VarChar, 10).Value = operation;
                        }
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            setupSheetHeaders.Add(
                                new ToolSetupSheetHeader
                                {
                                    ID = Convert.ToInt32(reader["ID"].ToString()),
                                //PartName = reader["partname"].ToString(),
                                PartNumber = reader["partnumber"].ToString(),
                                    Revision = reader["revision"].ToString(),
                                    Operation = reader["operation"].ToString()
                                });
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return setupSheetHeaders;            
        }
        public string ConvertProgram(ConvertProgramRequest convertProgramRequest)
        {
            //Get all rules then cache
            List<ConversionRule> lstAllRules = new List<ConversionRule>();
            if (HttpRuntime.Cache["ConversionRules"] == null)
            {
                using (SqlConnection con = new SqlConnection(MWConnectionString))
                {
                    string query = "spGetConversionRules";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            lstAllRules.Add(new ConversionRule
                            {
                                FromMachineId = reader["FromMachineId"].ToString(),
                                ToMachineId = reader["ToMachineId"].ToString(),
                                FromSnippet = reader["FromSnippet"].ToString(),
                                ToSnippet = reader["ToSnippet"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
                HttpRuntime.Cache["ConversionRules"] = lstAllRules;
            }
            lstAllRules = (List<ConversionRule>)HttpRuntime.Cache["ConversionRules"];
            StringBuilder newLines = new StringBuilder();

            try
            {
                List<ConversionRule> lstRules = lstAllRules.FindAll(
                    rule => rule.FromMachineId == convertProgramRequest.FromMachineID && rule.ToMachineId == convertProgramRequest.ToMachineID);
                string[] lines = convertProgramRequest.Program.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                for (int i = 0; i < lines.Length; i++)
                {
                    string newLine = ConvertLine(lines[i], lstRules);
                    newLines.AppendLine(newLine);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return newLines.ToString();
        }

        private string ConvertLine(string line, List<ConversionRule> lstRules)
        {
            foreach (ConversionRule rule in lstRules)
            {
                line = line.Replace(rule.FromSnippet, rule.ToSnippet);
            }
            return line;
        }
        public void RefreshLookupCaches()
        {            
            RefreshLookupCache("Unit");
            RefreshLookupCache("MaterialType");
            RefreshLookupCache("MaterialForm");
            RefreshLookupCache("MaterialSize");
            RefreshLookupCache("MaterialHeatTreated");
            RefreshLookupCache("MachineWorkHoldingTo");
            RefreshLookupCache("Torque");
            RefreshLookupCache("HoldPartOn");
            RefreshLookupCache("Z0");
            RefreshLookupCache("BarStickOutBefore");
            RefreshLookupCache("FaceOff");
            RefreshLookupCache("CutOffToolThickness");
            RefreshLookupCache("BarStickOutAfter");
            RefreshLookupCache("OAL");
            RefreshLookupCache("PartStickOutMinimum");
            RefreshLookupCache("MachineType");
        }
        public void RefreshLookupCache(string category)
        {

            List<string> retResults = new List<string>();

            using (SqlConnection con = new SqlConnection(MWConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetLookupValues", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Category", SqlDbType.VarChar, 100).Value = category;
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        retResults.Add(string.Format("{0}|{1}", reader["Name"], reader["Description"]));
                    }
                }
                con.Close();
                HttpRuntime.Cache.Remove(category);
                HttpRuntime.Cache[category] = retResults;
            }

        }

        public void Dispose()
        {

        }
    }

}