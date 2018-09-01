using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;

using DevExpress.XtraTreeList;


using System.IO;
namespace Scheme
{
    public partial class Frmsceme : Form
    {
        /// <summary>
        /// 方案的顶级路径
        /// </summary>
        private string schemeTopPath="";
        List<string> foldersNameList;
        List<string> foldersUrlList;
        List<string> filesNameList;
        List<string> filesUrlLsit;
        public Frmsceme()
        {
            
            InitializeComponent();
            schemeTopPath = Application.StartupPath + @"\category";
        }


        /// <summary>
        /// 遍历文件夹
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="treelist"></param>
        void TraverseFolder(string filepath, TreeListNode treelist = null)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(filepath); //文件夹信息
            if (!TheFolder.Exists) return;
            foreach (DirectoryInfo childFolder  in TheFolder.GetDirectories())//获取文件夹的子文件夹
            {
                TreeListNode FileNode = this.treeClass.AppendNode(null, treelist);
                FileNode.SetValue(this.treeClass.Columns["FolderName"], childFolder.Name);
                TraverseFolder(filepath + childFolder.Name + "\\", FileNode);
            }
        }

        public Frmsceme(string spath)
        {
            schemeTopPath = spath;


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //add folders
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            if (path != null) {
                TraverseFolder(path);
            }
        }

        /// <summary>
        /// 遍历文件
        /// </summary>
        /// <param name="filepath"></param>
        private void TraverseFild(string filepath)
        {
            this.treeFile.ClearNodes();
            //this.clearAll();
            //this.currentpackeol = null;
            DirectoryInfo TheFolder = new DirectoryInfo(filepath);
            if (!TheFolder.Exists) return;
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                TreeListNode ParentNode = this.treeFile.AppendNode(null, null);
                ParentNode.SetValue(this.treeFile.Columns["FileName"], NextFile.Name.Replace(".eol", ""));
            }

        }
        /// <summary>
        /// 获取选中文件夹的路径
        /// </summary>
        /// <param name="node"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        private  string getfolder(TreeListNode node, string folder)
        {
            if (node.ParentNode == null)
            {
                return this.schemeTopPath + folder;
            }
            else
            {
                string newfolder = node.GetValue(this.treeClass.Columns["FolderName"]).ToString() + "\\" + folder;
                return getfolder(node.ParentNode, newfolder);
            }
        }

        private void Frmsceme_Load(object sender, EventArgs e)
        {
            ToolHeight();
            TreeListNode FileNode = this.treeClass.AppendNode(null, null);
            FileNode.SetValue(this.treeClass.Columns["FolderName"], "scheme");
            TraverseFolder(this.schemeTopPath, FileNode);
            treeClass.FocusedNode = FileNode;
            TraverseFild(getfolder(FileNode, ""));
        }

        private void ToolHeight()
        {
            scemePanel.Height = toolboxControl1.Height - 39;
        }
        private void Frmsceme_Resize(object sender, EventArgs e)
        {
            ToolHeight();
        }

        private void tabNNarmalTest_Paint(object sender, PaintEventArgs e)
        {

        }



        private void contextMenuStripResult_Opening(object sender, CancelEventArgs e)
        {

        }

        private void buttonResult_Click(object sender, EventArgs e)
        {

         
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
