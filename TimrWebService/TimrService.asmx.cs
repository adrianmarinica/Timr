using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Objects;
using BusinessLogic;

namespace TimrWebService
{
    /// <summary>
    /// Summary description for TimrService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TimrService : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetTimetableForBachelorYear(StudyYear year, HalfYear halfYear)
        {
            FIITimetableParserBL bl = new FIITimetableParserBL();
            return bl.GetXMLTimetableForBachelorYear(year, halfYear);
        }

        [WebMethod]
        public string GetTimetableForMastersYear(StudyYear year)
        {
            FIITimetableParserBL bl = new FIITimetableParserBL();
            return bl.GetXMLTimetableForMastersYear((StudyYear)year);
        }

    }
}
