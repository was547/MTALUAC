namespace MTALUAC
{
    public partial class Form1 : Form
    {
        public static string SelectedFolderPath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();

            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;

            folderBrowser.FileName = "Select a folder.";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                SelectedFolderPath = Path.GetDirectoryName(folderBrowser.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(SelectedFolderPath))
            {
                MessageBox.Show("Select a valid folder");
                return;
            }

            var metaFile = $"{SelectedFolderPath}/meta.xml";
            var BackupMetaFile = $"{SelectedFolderPath}/bkp.meta.xml";

            if (!File.Exists(metaFile))
            {
                MessageBox.Show("meta.xml file not found in the selected folder!");
                return;
            }

            var metaData = File.ReadAllText(metaFile);

            //Save a backup before make changes.
            File.WriteAllText(BackupMetaFile, metaData);

            metaData = metaData.Replace(".lua", ".luac");
            
            File.Delete(metaFile);

            File.WriteAllText(metaFile, metaData);

            MessageBox.Show("meta.xml updated!");
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedFolderPath))
            {
                MessageBox.Show("Select a valid folder");
                return;
            }

            var luaFiles = Directory.GetFiles(SelectedFolderPath, "*.lua");

            foreach (var luaFile in luaFiles)
            {
                await LUACAPI.CompileFile(luaFile);
            }

            MessageBox.Show("All files were compiled successfully!");
        }
    }
}