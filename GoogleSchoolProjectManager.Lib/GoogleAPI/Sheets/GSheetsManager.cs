using Google.Apis.Sheets;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace GoogleSchoolProjectManager.Lib.Google
{
    public class GSheetsManager
    {
        public SheetsService _service { get; set; }

        public GSheetsManager(SheetsService service)
        {
            this._service = service;
        }

        public GSheetsManager(GoogleConnector googleConnector)
        {
            this._service = googleConnector.Sheets;
        }
    }
}