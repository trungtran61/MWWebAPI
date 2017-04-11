using MWWebAPI.Models;
//using MWWebAPI.Repository;
using System.Collections.Generic;
using System.Web.Http;
using MWWebAPI.DBRepository;
using System.Web;
using System.Linq;
using System;
using System.IO;

namespace MWWebAPI.Controllers
{
    //[Authorize]
    [RoutePrefix("api")]
    public class ToolInventoryController : ApiController
    {
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
    } 
}
