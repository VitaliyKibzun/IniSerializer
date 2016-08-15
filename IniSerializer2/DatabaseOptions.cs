namespace IniSerializer2
{
    public class DatabaseOptions
    {
        private string name;
        private string organization;
        private string server;
        private string port;
        private string file;

        [IniSection(ElementName = "owner")]
        [IniKey(ElementName = "name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [IniSection(ElementName = "owner")]
        [IniKey(ElementName = "organization")]
        public string Organization
        {
            get { return organization; }
            set { organization = value; }
        }

        [IniSection(ElementName = "database")]
        [IniKey(ElementName = "server")]
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        [IniSection(ElementName = "database")]
        [IniKey(ElementName = "port")]
        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        [IniSection(ElementName = "database")]
        [IniKey(ElementName = "file")]
        public string File
        {
            get { return file; }
            set { file = value; }
        }
    }
}