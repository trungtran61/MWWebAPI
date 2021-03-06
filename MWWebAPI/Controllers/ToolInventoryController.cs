﻿using MWWebAPI.Models;
//using MWWebAPI.Repository;
using System.Collections.Generic;
using System.Web.Http;
using MWWebAPI.DBRepository;
using System.Web;
using System.Linq;
using System;
using System.IO;
using System.Configuration;

namespace MWWebAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("api")]
    public class ToolInventoryController : ApiController
    {
        private static string imageLibrary = ConfigurationManager.AppSettings["imageLibrary"];
        private static string imageUrl = ConfigurationManager.AppSettings["imageUrl"];
        DBToolInventoryRepository ToolInventoryRepo = new DBToolInventoryRepository();
        /*
        [Route("")]
        public IEnumerable<Lookup> GetAll()
        {
            return LookupRepo.GetAll();
        }
        */
        [Route("SearchToolSetups")]
        [HttpPost]
        public List<ToolSetupSearchResult> SearchToolSetups(ToolSetupSearchRequest toolSetupSearchRequest)
        {
            List<ToolSetupSearchResult> retSearchResult = ToolInventoryRepo.SearchToolSetups(toolSetupSearchRequest.SearchTerm);
            return retSearchResult;
        }

        [Route("lookup")]
        [HttpPost]
        public IEnumerable<Lookup> GetByCategory(LookUpRequest lookUpRequest)
        {
            return ToolInventoryRepo.GetLookupByCategory(lookUpRequest.Category, lookUpRequest.SearchTerm);
        }

        [Route("GetToolCategoryNames")]       
        public List<Lookup> GetToolCategoryNames()
        {
            return ToolInventoryRepo.GetToolCategoryNames();
        }

        [Route("GetCuttingMethodsWithTemplate")]
        [HttpPost]
        public IEnumerable<CuttingMethodTemplate> GetCuttingMethodsWithTemplate(ToolSetupSearchRequest toolSetupSearchRequest)
        {
            return ToolInventoryRepo.GetCuttingMethodsWithTemplate(toolSetupSearchRequest.SearchTerm);
        }

        [Route("cuttingmethodtemplate/update")]
        [HttpPost]
        public APIResponse UpdateCuttingMethodTemplate(CuttingMethodTemplate cuttingMethodTemplate)
        {
            DBResponse dbResponse = ToolInventoryRepo.UpdateCuttingMethodTemplate(cuttingMethodTemplate);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = "Template updated"
            };
        }

        [Route("getCuttingMethodtemplate/{cuttingMethod}")]       
        public string[] GetTemplate(string cuttingMethod)
        {
            // URL can't handle period so use | instead
            cuttingMethod = cuttingMethod.Replace('|', '.');
            return ToolInventoryRepo.GetCuttingMethodTemplate(cuttingMethod);
        }

        [Route("getToolSetupSheet/{id}")]
        public ToolSetupSheet GetToolSetupSheet(int id)
        {
            return ToolInventoryRepo.GetToolSetupSheet(id);
        }

        [Route("ToolSetupSheet/Update")]
        [HttpPost]
        public APIResponse UpdateToolSetupSheet(ToolSetupSheet toolSetupSheet)
        {
            DBResponse dbResponse = ToolInventoryRepo.UpdateToolSetupSheet(toolSetupSheet);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = "Setup Sheet updated"
            };
        }

        [Route("AddToolSetupToSetupSheet")]
        [HttpPost]
        public APIResponse AddToolSetupToSetupSheet(AddToolSetupRequest addToolSetupRequest)
        {
            DBResponse dbResponse = ToolInventoryRepo.AddToolSetupToSetupSheet(addToolSetupRequest);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = "Setup Sheet updated"
            };
        }

        [Route("DeleteConversionRule")]
        [HttpPost]
        public APIResponse DeleteConversionRule(ConversionRule conversionRule)
        {

            DBResponse dbResponse = ToolInventoryRepo.DeleteConversionRule(conversionRule);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = "Setup Sheet updated"
            };
        }

        [Route("DeleteToolSetup")]
        [HttpPost]
        public APIResponse DeleteToolSetup(DeleteToolSetupRequest deleteToolSetupRequest)
        {

            DBResponse dbResponse = ToolInventoryRepo.DeleteToolSetup(deleteToolSetupRequest);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = "Setup Sheet updated"
            };
        }

        [Route("SaveToolSetupGroup")]
        [HttpPost]
        public APIResponse SaveToolSetupGroup(ToolSetupGroupRequest toolSetupGroupRequest)
        {

            DBResponse dbResponse = ToolInventoryRepo.SaveToolSetupGroup(toolSetupGroupRequest);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = "Tool Setup Group updated"
            };
        }

        [Route("SaveConvertedProgram")]
        [HttpPost]
        public int SaveConvertedProgram(ProgramSaveRequest convertProgramSaveRequest)
        {

            int newSetupSheetID = ToolInventoryRepo.SaveConvertedProgram(convertProgramSaveRequest);
            return newSetupSheetID;            
        }

        [Route("UploadProvenProgram")]
        [HttpPost]
        public int UploadProvenProgram(UploadProgramRequest uploadProgramRequest)
        {

            int newSetupSheetID = ToolInventoryRepo.UploadProvenProgram(uploadProgramRequest);
            return newSetupSheetID;
        }

        [Route("SaveProgram")]
        [HttpPost]
        public int SaveProgram(ProgramSaveRequest convertProgramSaveRequest)
        {
            ToolInventoryRepo.SaveProgram(convertProgramSaveRequest);
            return 0;          
        }

        [Route("GetSearchResults/{category}/{term?}")]
        public string[] GetSearchResults(string category, string term = "")
        {
            List<string> retResults = ToolInventoryRepo.GetSearchResults(term, category);

            var resultList = retResults.Where(x => x.IndexOf(term, StringComparison.OrdinalIgnoreCase) > -1);
            return resultList.ToArray();
        }

        [Route("GetMachines/{prefix?}")]
        public string[] GetMachines(string prefix = "")
        {
            List<string> retPartNumbers = ToolInventoryRepo.GetMachines(prefix);
            return retPartNumbers.ToArray();
        }

        [Route("GetLookUpCategories/{searchTerm?}")]
        public string[] GetLookUpCategories(string searchTerm = "")
        {
            List<string> getCategories = ToolInventoryRepo.GetLookUpCategories(searchTerm);
            return getCategories.ToArray();
        }

        [Route("GetSSPPartNumbers/{partId?}")]
        public string[] GetSSPPartNumbers(string partId = "")
        {
            List<string> retPartNumbers = ToolInventoryRepo.GetSSPPartNumbers(partId);
            var resultList = retPartNumbers.Where(x => x.IndexOf(partId, StringComparison.OrdinalIgnoreCase) > -1);
            return resultList.ToArray();
        }

        [Route("GetMaterialSize/{materialType}/{term}")]
        public string[] GetSSPPartNumbers(string materialType, string term)
        {
            List<string> retMaterialSize = ToolInventoryRepo.GetMaterialSize(term, materialType);
            var resultList = retMaterialSize.Where(x => x.IndexOf(term, StringComparison.OrdinalIgnoreCase) > -1);
            return resultList.ToArray();
        }

        [Route("GetSSPOperations/{partId}/{operation?}")]
        public string[] GetSSPOperations(string partId, string operation = "")
        {
            List<string> retOperations = ToolInventoryRepo.GetSSPOperations(operation, partId);
            var resultList = retOperations.Where(x => x.IndexOf(operation, StringComparison.OrdinalIgnoreCase) > -1);
            return resultList.ToArray();
        }

        [Route("GetCuttingMethodTemplate/{cuttingMethod}/{n}")]
        public string[] GetCuttingMethodTemplate(string cuttingMethod, string n)
        {
            List<string> retTemplate = ToolInventoryRepo.GetCuttingMethodTemplate(cuttingMethod, n);
            return retTemplate.ToArray();
        }

        [Route("GetSetupSheets/{partNumber}/{revision}/{operation?}")]
        public List<ToolSetupSheetHeader> GetSetupSheets(string partnumber, string revision, string operation = "")
        {
            List<ToolSetupSheetHeader> setupSheetHeaders = ToolInventoryRepo.GetSetupSheets(partnumber, revision, operation);
            return setupSheetHeaders;
        }

        [Route("ConvertProgram")]
        [HttpPost]
        public string ConvertProgram(ConvertProgramRequest convertProgramRequest)
        {
            return ToolInventoryRepo.ConvertProgram(convertProgramRequest);
        }

        [Route("UploadProgram")]
        [HttpPost]
        public void UploadProgram()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];

                if (httpPostedFile != null)
                {
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/ProvenPrograms"), httpPostedFile.FileName);
                    httpPostedFile.SaveAs(fileSavePath);
                }
            }
        }

        [Route("GetToolInventoryColumns")]
        public List<ToolInventoryColumn> GetToolInventoryColumns(string code)
        {
            return ToolInventoryRepo.GetToolInventoryColumns();
        }

        [Route("GetToolInventoryColumns/{code}")]
        public List<ToolInventoryCodeColumn> GetToolInventoryColumnsByCode(string code)
        {
            return ToolInventoryRepo.GetToolInventoryColumnsByCode(code);
           
        }

        [Route("SaveToolInventoryCodeColumns")]
        [HttpPost]
        public DBResponse SaveToolInventoryCodeColumns(SaveCodeColumnsRequest saveCodeColumnsRequest)
        {
            return ToolInventoryRepo.SaveToolInventoryCodeColumns(saveCodeColumnsRequest);
        }

        [Route("CopyToolInventoryCodeColumns")]
        [HttpPost]
        public DBResponse CopyToolInventoryCodeColumns(CopyCodeColumnsRequest copyCodeColumnsRequest)
        {
            return ToolInventoryRepo.CopyToolInventoryCodeColumns(copyCodeColumnsRequest);
        }

        [Route("GetSelectedToolInventoryColumns/{codes?}")]
        public List<ToolInventoryColumn> GetSelectedToolInventoryColumns(string codes = "")
        {
            return ToolInventoryRepo.GetSelectedToolInventoryColumns(codes);
        }

        [Route("GetToolNames")]
        [HttpPost]
        public List<string> GetToolNames(LookUpRequest lookUpRequest)
        {
            return ToolInventoryRepo.GetToolNames(lookUpRequest.SearchTerm);
        }

        [Route("GetSearchableToolInventoryColumns/{codes?}")]
        public List<ToolInventoryColumn> GetSearchableToolInventoryColumns(string codes = "")
        {
            return ToolInventoryRepo.GetSelectedToolInventoryColumns(codes, true);
        }

        [Route("GetToolDetails/{ToolID}")]
        public ToolInventorySearchResult GetToolDetails(int ToolID)
        {
            return ToolInventoryRepo.GetToolDetails(ToolID);
        }

        [Route("CopyTool")]
        [HttpPost]
        public int CopyTool(ToolInventorySaveRequest copyToolRequest)
        {
            return ToolInventoryRepo.CopyTool(copyToolRequest.ID);
        }

        [Route("CreateTool")]
        [HttpPost]
        public int CreateTool(ToolInventorySaveRequest toolInventorySaveRequest)
        {
            return ToolInventoryRepo.CreateTool(toolInventorySaveRequest);
        }


        [Route("DeleteTool")]
        [HttpPost]
        public int DeleteTool(ToolInventorySaveRequest toolInventorySaveRequest)
        {
            return ToolInventoryRepo.DeleteTool(toolInventorySaveRequest.ID);
        }

        [Route("SaveToolDetails")]
        [HttpPost]
        public int SaveToolDetails(ToolInventorySaveRequest toolInventorySaveRequest)
        {
            return ToolInventoryRepo.SaveToolDetails(toolInventorySaveRequest);
        }
        //

        [Route("UpdateToolVendor")]
        [HttpPost]
        public int UpdateToolVendor(ToolInventorySearchResult toolInventorySearchResult)
        {
            return ToolInventoryRepo.UpdateToolVendor(toolInventorySearchResult);
        }

        [Route("ToolInventorySearch")]
        [HttpPost]
        public ToolInventorySearchResults ToolInventorySearch(ToolInventorySearch toolInventorySearch)
        {
            return ToolInventoryRepo.ToolInventorySearch(toolInventorySearch);
        }
        
        [Route("GetLookUpCategory")]
        [HttpPost]
        public LookupCategories GetLookUpCategory(LookupCategorySearch lookupCategorySearch)
        {
            return ToolInventoryRepo.GetLookUpCategory(lookupCategorySearch);
        }

        [Route("GetToolCuttingMethods/{ToolID}/{AllMethods}")]
        public List<ToolCuttingMethod> GetToolCuttingMethods(int ToolID, bool allMethods = true)
        {
            return ToolInventoryRepo.GetToolCuttingMethods(ToolID, allMethods);
        }

        [Route("GetVendors/{categoryID?}/{searchTerm?}")]
        public List<Company> GetVendors(string searchTerm = "", int categoryID = 0)
        {
            return ToolInventoryRepo.GetVendors(searchTerm, categoryID);
        }

        [Route("GetVendorInfo/{ID}")]
        public VendorInfo GetVendorInfo(int ID)
        {
            return ToolInventoryRepo.GetVendorInfo(ID);
        }
        //public LookupCategories GetLookUpCategory(LookupCategorySearch lookupCategorySearch)

        [Route("ToolInventorySearchSelected")]
        [HttpPost]
        public ToolInventorySearchResults ToolInventorySearchSelected(ToolInventorySearch toolInventorySearch)
        {
            return ToolInventoryRepo.ToolInventorySearchSelected(toolInventorySearch);
        }

        [Route("LinkTool")]
        [HttpPost]
        public APIResponse LinkTool(LinkToolRequest linkToolRequest)
        {
            DBResponse dbResponse = ToolInventoryRepo.LinkTool(linkToolRequest);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = linkToolRequest.Action + " successful."
            };
        }

        [Route("CheckOutCheckIn")]
        [HttpPost]
        public APIResponse CheckOutCheckIn(CheckOutCheckInRequest checkOutCheckInRequest)
        {
            DBResponse dbResponse = ToolInventoryRepo.CheckOutCheckIn(checkOutCheckInRequest);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = checkOutCheckInRequest.Action + " successful."
            };
        }

        [Route("SaveLookupCategory")]
        [HttpPost]
        public APIResponse SaveLookupCategory(SaveLookupCategoryRequest saveLookupCategoryRequest)
        {
            DBResponse dbResponse = ToolInventoryRepo.SaveLookupCategory(saveLookupCategoryRequest);
            return new APIResponse
            {
                ResponseCode = 0,
                ResponseText = "Save LookupCategory successful."
            };
        }
        [Route("UploadToolImage")]
        [HttpPost]
        public void UploadToolImage()
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                // Get the uploaded image from the Files collection
                var httpPostedFile = HttpContext.Current.Request.Files["UploadedImage"];

                if (httpPostedFile != null)
                {
                    int toolID = Convert.ToInt32(HttpContext.Current.Request["ToolID"]);
                    string fileFormat = httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf('.')+1);
                    string fileName = string.Format("{0}.{1}", toolID, fileFormat);
                    var fileSavePath = 
                        string.Format("{0}\\ToolInventory\\{1}.{2}", imageLibrary+"", toolID, fileFormat);

                    httpPostedFile.SaveAs(fileSavePath);
                    ToolInventoryRepo.SaveToolImageInfo(toolID, fileName);
                }
            }
        }
    } 
}
