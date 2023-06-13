using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;

public class clsStaticDropdowns
{
    public string GetCurrSessionYr()
    {
        DateTime today = DateTime.Today;
        int month = today.Month;
        int year = today.Year;
        return month <= 3 ? Convert.ToString(year - 1) + "-" + year.ToString().Substring(2, 2) : year.ToString() + "-" + Convert.ToString(year + 1).Substring(2, 2);
    }

    public void FillClass(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("--SELECT --", "0"));
        drp.Items.Insert(1, new ListItem("Praramva", "Praramva"));
        drp.Items.Insert(2, new ListItem("Bodh", "Bodh"));
        drp.Items.Insert(3, new ListItem("I", "I"));
        drp.Items.Insert(4, new ListItem("II", "II"));
        drp.Items.Insert(5, new ListItem("III", "III"));
        drp.Items.Insert(6, new ListItem("IV", "IV"));
        drp.Items.Insert(7, new ListItem("V", "V"));
        drp.Items.Insert(8, new ListItem("VI", "VI"));
        drp.Items.Insert(9, new ListItem("VII", "VII"));
        drp.Items.Insert(10, new ListItem("VIII", "VIII"));
        drp.Items.Insert(11, new ListItem("IX", "IX"));
        drp.Items.Insert(12, new ListItem("X", "X"));
        drp.Items.Insert(13, new ListItem("XI", "XI"));
        drp.Items.Insert(14, new ListItem("XII", "XII"));
    }

    public void FillSessionYr(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("--SELECT--", "0"));
        int year = DateTime.Today.Year;
        for (int index = 0; index < 40; ++index)
        {
            string str = (year - index + 1).ToString().Substring(2);
            string text = (year - index).ToString() + "-" + str;
            drp.Items.Insert(index + 1, new ListItem(text, text));
        }
    }

    public void FillSessYrForAdmn(DropDownList drp)
    {
        drp.Items.Clear();
        int year = DateTime.Today.Year;
        for (int index = 0; index < 40; ++index)
        {
            string str = (year - index + 1).ToString().Substring(2);
            string text = (year - index).ToString() + "-" + str;
            drp.Items.Insert(index, new ListItem(text, text));
        }
    }

    public void FillSessionYr2(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("--SELECT--", "0"));
        int year = DateTime.Today.Year;
        for (int index = 0; index < 2; ++index)
        {
            string str = (year - index + 1).ToString().Substring(2);
            string text = (year - index).ToString() + "-" + str;
            drp.Items.Insert(index + 1, new ListItem(text, text));
        }
    }

    public void FillProspectusType(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("--SELECT --", "0"));
        drp.Items.Insert(1, new ListItem("Praramva & Bodh", "Praramva & Bodh"));
        drp.Items.Insert(2, new ListItem("Class-I to Class-X", "Class-I to Class-X"));
        drp.Items.Insert(3, new ListItem("Class-XI & Class-XII", "Class-XI & Class-XII"));
    }

    public void FillStatus(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("--SELECT --", "0"));
        drp.Items.Insert(1, new ListItem("Active", "1"));
        drp.Items.Insert(2, new ListItem("Passed out", "2"));
        drp.Items.Insert(3, new ListItem("TC", "3"));
        drp.Items.Insert(4, new ListItem("Absent", "4"));
    }

    public void getsections(DropDownList drp)
    {
        int int16 = (int)Convert.ToInt16(ConfigurationManager.AppSettings["TotSec"].ToString());
        int num1 = int16;
        if (int16 <= 0)
            return;
        drp.Items.Clear();
        int num2 = 65;
        int index = 0;
        while (num1 > 0)
        {
            drp.Items.Insert(index, new ListItem(Convert.ToChar(num2).ToString(), Convert.ToChar(num2).ToString()));
            --num1;
            ++index;
            ++num2;
        }
        drp.Items.Insert(0, "Not Allotted");
    }

    public void FillSection(DropDownList drp)
    {
        int int16 = (int)Convert.ToInt16(ConfigurationManager.AppSettings["TotSec"].ToString());
        int num1 = int16;
        if (int16 <= 0)
            return;
        drp.Items.Clear();
        int num2 = 65;
        int index = 0;
        while (num1 > 0)
        {
            drp.Items.Insert(index, new ListItem(Convert.ToChar(num2).ToString(), Convert.ToChar(num2).ToString()));
            --num1;
            ++index;
            ++num2;
        }
        drp.Items.Insert(0, "Not Allotted");
    }

    public DataTable GetSectionDT()
    {
        int int16 = (int)Convert.ToInt16(ConfigurationManager.AppSettings["TotSec"].ToString());
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("section");
        int num1 = 65;
        int num2 = 0;
        while (int16 > 0)
        {
            char ch = Convert.ToChar(num1);
            DataRow row = dataTable.NewRow();
            row["section"] = (object)ch;
            dataTable.Rows.Add(row);
            --int16;
            ++num2;
            ++num1;
        }
        return dataTable;
    }

    public void FillStudAdmnType(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("Existing", "E"));
        drp.Items.Insert(1, new ListItem("New", "N"));
        //drp.Items.Insert(2, new ListItem("TC", "T"));
        //drp.Items.Insert(3, new ListItem("Casual", "C"));
    }

    public void FillCusomerCat(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("--SELECT--", "0"));
        drp.Items.Insert(1, new ListItem("Wholesaler", "WSR"));
        drp.Items.Insert(2, new ListItem("Retailer", "RET"));
        drp.Items.Insert(3, new ListItem("Regular Customer", "RC"));
        drp.Items.Insert(4, new ListItem("Education Institute", "EI"));
        drp.Items.Insert(5, new ListItem("Student", "ST"));
        drp.Items.Insert(6, new ListItem("Others", "OTH"));
    }

    public void FillPartyType(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("--SELECT--", "0"));
        drp.Items.Insert(1, new ListItem("Branch", "Branch"));
        drp.Items.Insert(2, new ListItem("Supplier", "Supplier"));
        drp.Items.Insert(3, new ListItem("Customer", "Customer"));
        drp.Items.Insert(4, new ListItem("Transporter", "Transporter"));
    }

    public void FillMeasuringUnits(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("-SELECT -", "0"));
        drp.Items.Insert(1, new ListItem("gm", "gm"));
        drp.Items.Insert(2, new ListItem("Kg", "Kg"));
        drp.Items.Insert(3, new ListItem("Piece", "Piece"));
        drp.Items.Insert(4, new ListItem("Packet", "Packet"));
        drp.Items.Insert(5, new ListItem("Mtr", "Mtr"));
        drp.Items.Insert(6, new ListItem("Feet", "Feet"));
        drp.Items.Insert(7, new ListItem("Inch", "Inch"));
        drp.Items.Insert(8, new ListItem("Ltr", "Ltr"));
        drp.Items.Insert(9, new ListItem("ml", "ml"));
    }
}
