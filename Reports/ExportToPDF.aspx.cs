using ASP;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SanLib;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_ExportToPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnExpWord_Click(object sender, EventArgs e)
    {
        Response.AddHeader("content-disposition", "attachment;filename=Export.doc");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = "application/vnd.word";
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter((TextWriter)stringWriter);
        HtmlForm htmlForm = new HtmlForm();
        gv.Parent.Controls.Add((Control)htmlForm);
        htmlForm.Attributes["runat"] = "server";
        htmlForm.Controls.Add((Control)gv);
        htmlForm.RenderControl(writer);
        Response.Write(stringWriter.ToString());
        Response.End();
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        string str = "attachment; filename=Export.xls";
        Response.ClearContent();
        Response.AddHeader("content-disposition", str);
        Response.ContentType = "application/ms-excel";
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter((TextWriter)stringWriter);
        HtmlForm htmlForm = new HtmlForm();
        gv.Parent.Controls.Add((Control)htmlForm);
        htmlForm.Attributes["runat"] = "server";
        htmlForm.Controls.Add((Control)gv);
        htmlForm.RenderControl(writer);
        Response.Write(stringWriter.ToString());
        Response.End();
    }

    protected void btnExpPDF_Click(object sender, EventArgs e)
    {
        gv.DataSource = new clsDAL().GetDataTableQry("select * from dbo.Acts_AccountHeads");
        gv.DataBind();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=Export.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter((TextWriter)stringWriter);
        HtmlForm htmlForm = new HtmlForm();
        new StringBuilder().Append("Test");
        gv.Parent.Controls.Add((Control)htmlForm);
        htmlForm.Attributes["runat"] = "server";
        htmlForm.Controls.Add((Control)gv);
        htmlForm.RenderControl(writer);
        StringReader stringReader = new StringReader(stringWriter.ToString());
        Document document = new Document((Rectangle)PageSize.A4, 10f, 10f, 10f, 0.0f);
        HTMLWorker htmlWorker = new HTMLWorker((IDocListener)document);
        PdfWriter.GetInstance(document, Response.OutputStream);
        document.Open();
        htmlWorker.Parse((TextReader)stringReader);
        document.Close();
        Response.Write(document);
        Response.End();
    }

    protected void btnExpPDFString_Click(object sender, EventArgs e)
    {
        gv.DataSource = new clsDAL().GetDataTableQry("select * from dbo.Acts_AccountHeads");
        gv.DataBind();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=Export.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter((TextWriter)stringWriter);
        HtmlForm htmlForm = new HtmlForm();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" <table>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append(" <td><b>Col1</b></td>");
        stringBuilder.Append(" <td><b>Col2</b></td>");
        stringBuilder.Append(" <td><b>Col3</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append(" <td><b>x</b></td>");
        stringBuilder.Append(" <td><b>y</b></td>");
        stringBuilder.Append(" <td><b>z</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append(" </table>");
        stringWriter.WriteLine(stringBuilder.ToString());
        htmlForm.Attributes["runat"] = "server";
        StringReader stringReader = new StringReader(stringWriter.ToString());
        Document document = new Document((Rectangle)PageSize.A4, 10f, 10f, 10f, 0.0f);
        HTMLWorker htmlWorker = new HTMLWorker((IDocListener)document);
        PdfWriter.GetInstance(document, Response.OutputStream);
        document.Open();
        htmlWorker.Parse((TextReader)stringReader);
        document.Close();
        Response.Write(document);
        Response.End();
    }
}