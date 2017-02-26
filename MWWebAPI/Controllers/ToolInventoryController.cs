using MWWebAPI.Models;
//using MWWebAPI.Repository;
using System.Collections.Generic;
using System.Web.Http;
using MWWebAPI.DBRepository;
using System.Web;
using System.Linq;
using System;

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

        [Route("lookup/{category}/{term?}")]
        public IEnumerable<Lookup> GetByCategory(string category, string term = "")
        {
            // URL can't handle period so use | instead
            term = term.Replace('|', '.');

            return ToolInventoryRepo.GetLookupByCategory(category, term);
        }

        [Route("GetCuttingMethodsWithTemplate/{term?}")]
        public IEnumerable<CuttingMethodTemplate> GetCuttingMethodsWithTemplate(string term = "")
        {
            return ToolInventoryRepo.GetCuttingMethodsWithTemplate(term);
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
        public int SaveConvertedProgram(ConvertProgramSaveRequest convertProgramSaveRequest)
        {

            int newSetupSheetID = ToolInventoryRepo.SaveConvertedProgram(convertProgramSaveRequest);
            return newSetupSheetID;            
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
    } 
}
