using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;

namespace Classes.DA
{
    public class gp_add_file
    {
        private static int min_width = 500;
        private static int min_height = 500;
        private static int small_width = 180;
        private static int small_height = 180;
        private static int large_width = 500;
        private static int large_height = 500;
        private static string File_name;
        private static string image_name;
        private static FileReturnParameter objreturn;
        private Hashtable hsh_bannedContentTypes;
        private Hashtable hsh_SupportedContentTypes;

        public gp_add_file()
        {
            this.hsh_SupportedContentTypes = new Hashtable();
            this.hsh_SupportedContentTypes.Add((object)"image/jpe", (object)"image/jpe");
            this.hsh_SupportedContentTypes.Add((object)"image/jpg", (object)"image/jpg");
            this.hsh_SupportedContentTypes.Add((object)"image/jpeg", (object)"image/jpeg");
            this.hsh_SupportedContentTypes.Add((object)"image/pjpeg", (object)"image/pjpeg");
            this.hsh_SupportedContentTypes.Add((object)"image/gif", (object)"image/gif");
            this.hsh_SupportedContentTypes.Add((object)"image/pnf", (object)"image/png");
            this.hsh_SupportedContentTypes.Add((object)"Text/pdf", (object)"Text/pdf");
            this.hsh_bannedContentTypes = new Hashtable();
            this.hsh_bannedContentTypes.Add((object)"application/octet-stream", (object)"application/octet-stream");
        }

        private bool bannedContentType(string Contenttype)
        {
            return this.hsh_bannedContentTypes.ContainsKey((object)Contenttype);
        }

        private bool CompatibleContentType(string Contenttype)
        {
            return this.hsh_SupportedContentTypes.ContainsKey((object)Contenttype);
        }

        public static void DeleteFile(string strFileName)
        {
            if (strFileName.Trim().Length <= 0)
                return;
            FileInfo fileInfo = new FileInfo(strFileName);
            if (!fileInfo.Exists)
                return;
            fileInfo.Delete();
        }

        public static FileReturnParameter UploadImage(string filepath, string filename, FileUpload Files, int minwidth, int minheight, int smallheight, int smallwidth)
        {
            gp_add_file.objreturn = new FileReturnParameter();
            if (filename.Trim().Length > 0)
            {
                if (Files.PostedFile != null)
                {
                    if (Files.PostedFile.ContentLength > 0)
                    {
                        try
                        {
                            if (new gp_add_file().CompatibleContentType(Files.PostedFile.ContentType))
                            {
                                gp_add_file.image_name = filename + DateTime.Now.ToString("dd_MM_yyyy_T_hhmmss") + Path.GetExtension(Files.PostedFile.FileName);
                                Files.PostedFile.SaveAs(filepath + gp_add_file.image_name);
                                Bitmap bitmap = new Bitmap(filepath + gp_add_file.image_name);
                                gp_add_file.objreturn.Filename = gp_add_file.image_name;
                                gp_add_file.objreturn.Fileuploaded = true;
                                bitmap.Dispose();
                                return gp_add_file.objreturn;
                            }
                            gp_add_file.objreturn.Errormessage = "Uploaded File Format Not Supported";
                            return gp_add_file.objreturn;
                        }
                        catch
                        {
                            gp_add_file.DeleteFile(filepath + gp_add_file.image_name);
                            gp_add_file.objreturn.Errormessage = "Error While Uploading Image";
                            return gp_add_file.objreturn;
                        }
                    }
                }
                gp_add_file.objreturn.Errormessage = "Please Select File To Upload Or File Size Is Not Valid ";
                return gp_add_file.objreturn;
            }
            gp_add_file.objreturn.Errormessage = "Not A Valid Filename";
            return gp_add_file.objreturn;
        }

        public static FileReturnParameter UploadFile(string filepath, string filename, FileUpload Files)
        {
            gp_add_file.objreturn = new FileReturnParameter();
            if (filename.Trim().Length > 0)
            {
                if (Files.PostedFile != null)
                {
                    if (Files.PostedFile.ContentLength > 0)
                    {
                        try
                        {
                            gp_add_file gpAddFile = new gp_add_file();
                            gp_add_file.image_name = filename + DateTime.Now.ToString("dd_MM_yyyy_T_hhmmss") + Path.GetExtension(Files.PostedFile.FileName);
                            Files.PostedFile.SaveAs(filepath + gp_add_file.image_name);
                            gp_add_file.objreturn.Filename = gp_add_file.image_name;
                            gp_add_file.objreturn.Fileuploaded = true;
                            return gp_add_file.objreturn;
                        }
                        catch
                        {
                            gp_add_file.DeleteFile(filepath + gp_add_file.image_name);
                            gp_add_file.objreturn.Errormessage = "Error While Uploading File";
                            return gp_add_file.objreturn;
                        }
                    }
                }
                gp_add_file.objreturn.Errormessage = "Please Select File To Upload Or File Size Is Not Valid ";
                return gp_add_file.objreturn;
            }
            gp_add_file.objreturn.Errormessage = "Not A Valid Filename";
            return gp_add_file.objreturn;
        }
    }
}
