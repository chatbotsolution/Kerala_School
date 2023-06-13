using Classes.DA;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace Classes.UI
{
    public sealed class clsPopulateControls
    {
        public static void PopulateDropDownList(DropDownList ddlControl, string strStoredProcedureName, string strFieldText, string strFieldValue, Hashtable ht)
        {
            clsData clsData = new clsData();
            DataSet dataSet = clsData.GetDataSet(strStoredProcedureName, ht);
            ddlControl.DataSource = (object)dataSet;
            ddlControl.DataTextField = strFieldText;
            ddlControl.DataValueField = strFieldValue;
            ddlControl.DataBind();
            ddlControl.Items.Insert(0, "Select");
            clsData.Dispose();
        }

        public static void FillDropDown(DropDownList ddlControl, string strStoredProcedureName, string TextField, string ValueField)
        {
            clsData clsData = new clsData();
            DataTable dataTable = clsData.GetDataTable(strStoredProcedureName);
            ddlControl.DataSource = (object)dataTable;
            ddlControl.DataTextField = TextField;
            ddlControl.DataValueField = ValueField;
            ddlControl.DataBind();
            ddlControl.Items.Insert(0, "Select");
            clsData.Dispose();
        }

        public static void PopulateCountyDropDownList(DropDownList ddlControl)
        {
            Hashtable ht = new Hashtable();
            clsPopulateControls.PopulateDropDownList(ddlControl, "uspPopulateCountyDropDownList", "CountyName", "CountyId", ht);
        }

        public static void PopulateCityDropDownList(DropDownList ddlControl, string intSelectedValue)
        {
            clsPopulateControls.PopulateDropDownList(ddlControl, "uspPopulateCity", "CityName", "CityId", new Hashtable()
      {
        {
          (object) "CountyId",
          (object) intSelectedValue.ToString()
        }
      });
        }

        public static void PopulateZipDropDownList(DropDownList ddlControl, string intSelectedValue)
        {
            clsPopulateControls.PopulateDropDownList(ddlControl, "uspPopulateZip", "PostCode", "PostCodeId", new Hashtable()
      {
        {
          (object) "CityId",
          (object) intSelectedValue.ToString()
        }
      });
        }

        public static void PopulateCountyDropDownListAll(DropDownList ddlControl)
        {
            Hashtable ht = new Hashtable();
            clsPopulateControls.PopulateDropDownList(ddlControl, "uspPopulateCountyDropDownListAll", "CountyName", "CountyId", ht);
        }

        public static void PopulateCityDropDownListAll(DropDownList ddlControl, string intSelectedValue)
        {
            clsPopulateControls.PopulateDropDownList(ddlControl, "uspPopulateCityAll", "CityName", "CityId", new Hashtable()
      {
        {
          (object) "CountyId",
          (object) intSelectedValue.ToString()
        }
      });
        }

        public static void PopulateZipDropDownListAll(DropDownList ddlControl, string intSelectedValue)
        {
            clsPopulateControls.PopulateDropDownList(ddlControl, "uspPopulateZipAll", "PostCode", "PostCodeId", new Hashtable()
      {
        {
          (object) "CityId",
          (object) intSelectedValue.ToString()
        }
      });
        }
    }
}