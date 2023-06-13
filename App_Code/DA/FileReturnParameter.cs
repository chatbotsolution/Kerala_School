namespace Classes.DA
{
    public class FileReturnParameter
    {
        public string Filename;
        public string Errormessage;
        public bool Fileuploaded;
        public float fsize;
        public string resolution;

        public FileReturnParameter()
        {
            this.Fileuploaded = false;
        }
    }
}