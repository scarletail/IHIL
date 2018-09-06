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
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

namespace Scheme
{
    public partial class Frmsceme : Form
    {
        /// <summary>
        /// 方案的顶级路径
        /// </summary>
        private string schemeTopPath = "";
        string fileUrl = null;
        string hiddenEditorType = null;
        TSchem CurrentTSchem = null;
        public Frmsceme()
        {
            InitializeComponent();
            schemeTopPath = Application.StartupPath + @"\category";
        }
        /// <summary>
        /// traverse the folders
        /// </summary>
        /// <param name="ParentNode"></param>
        void TraverseFolder(TreeListNode ParentNode)
        {
            if (ParentNode.Tag == null) return;
            string path = ParentNode.Tag.ToString();
            DirectoryInfo DirInfo = new DirectoryInfo(path);
            if (!DirInfo.Exists) return;
            foreach (DirectoryInfo childFolder in DirInfo.GetDirectories())
            {
                TreeListNode ChildNode = this.treeClass.AppendNode(null, ParentNode);
                ChildNode.SetValue(this.treeClass.Columns["FolderName"], childFolder.Name);
                ChildNode.Tag = childFolder.FullName;
                TraverseFolder(ChildNode);
            }
        }
        public Frmsceme(string spath)
        {
            schemeTopPath = spath;
        }
        /// <summary>
        /// Remove the folder
        /// while treeClass is empty it will throw a NullReferenceException
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //remove the focused folder
            TreeListNode treeListNode = this.treeClass.FocusedNode;
            try
            {
                treeListNode.Remove();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("can't remove the folder because the list is empty!");
                return;
            }

        }
        /// <summary>
        /// Import a new folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //add folders
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = true;
            //set the default folder path
            folderBrowserDialog.SelectedPath = @"F:\GitSync\IHIL\test";
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            if (path != null)
            {
                DirectoryInfo info = new DirectoryInfo(path);
                TreeListNode node = treeClass.AppendNode(null, null);
                node.Tag = path;
                node.SetValue(treeClass.Columns["FolderName"], info.Name);
                TraverseFolder(node);
                treeClass.Columns["FolderName"].SortOrder = SortOrder.Ascending;
                //sorting completed!
            }
        }
        /// <summary>
        /// 遍历文件
        /// </summary>
        /// <param name="filepath"></param>
        private void TraverseFile(string filepath)
        {
            //this.clearAll();
            //this.currentpackeol = null;
            DirectoryInfo TheFolder = new DirectoryInfo(filepath);
            if (!TheFolder.Exists) return;
            foreach (FileInfo childFile in TheFolder.GetFiles())
            {
                TreeListNode fileNode = treeFile.AppendNode(null, null);
                fileNode.Tag = childFile.FullName;
                //add node tag to bulid assosication between the filenode and the filepath
                fileNode.SetValue(treeFile.Columns["FileName"], childFile.Name);
            }

        }
        /// <summary>
        /// 获取选中文件夹的路径
        /// </summary>
        /// <param name="node"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        private string getfolder(TreeListNode node, string folder)
        {
            if (node.ParentNode == null)
            {
                return this.schemeTopPath + folder;
            }
            else
            {
                string newfolder = node.GetValue(treeClass.Columns["FolderName"]).ToString() + "\\" + folder;
                return getfolder(node.ParentNode, newfolder);
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frmsceme_Load(object sender, EventArgs e)
        {
            ToolHeight();
            TreeListNode FileNode = treeClass.AppendNode(null, null);
            FileNode.SetValue(treeClass.Columns["FolderName"], "scheme");
            FileNode.Tag = schemeTopPath;
            TraverseFolder(FileNode);
            treeClass.FocusedNode = FileNode;
            TraverseFile(FileNode.Tag.ToString());
            //gridControlTest.DataSource = null;
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
        private void treeClass_AfterExpand(object sender, NodeEventArgs e)
        {

        }
        private void treeClass_RowStateImageClick(object sender, RowClickEventArgs e)
        {

        }
        private void treeClass_RowSelectImageClick(object sender, RowClickEventArgs e)
        {

        }
        private void treeClass_CustomDrawNodeButton(object sender, CustomDrawNodeButtonEventArgs e)
        {

        }
        /// <summary>
        /// sort the nodes by filename when fucused node is changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeClass_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            treeFile.ClearNodes();
            if (treeClass.AllNodesCount == 0) return;
            try
            {
                TreeListNode node = treeClass.FocusedNode;
                if (node.Tag == null) return;
                string path = node.Tag.ToString();
                TraverseFile(path);
                treeFile.Columns["FileName"].SortOrder = SortOrder.Ascending;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("There is no node focused in the TreeClass!");
            }
        }
        /// <summary>
        /// what's this ?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeFile_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (treeFile.AllNodesCount == 0) return;
            try
            {
                TreeListNode fnode = treeFile.FocusedNode;
                if (fnode.Tag == null) return;
                fileUrl = fnode.Tag.ToString();
            }
            catch (System.NullReferenceException)
            {
                Console.WriteLine("There is no node focused in the TreeFile!");
                return;
            }

        }
        /// <summary>
        /// create a new xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddclass_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeClass.FocusedNode != null)
                {
                    string folderpath = treeClass.FocusedNode.Tag.ToString();
                    TreeListNode newFileNode = treeFile.AppendNode(null, null);
                    newFileNode.Selected = true;
                    hiddenEditorType = "addFile";
                    treeFile.ShowEditor();
                }

            }
            catch (NullReferenceException)
            {
                Console.WriteLine("There're some unreasonable problems!");
                return;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("File created filed,no such folder!");
                return;
            }

        }
        /// <summary>
        /// add a listener to response the hidden of editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeFile_HiddenEditor(object sender, EventArgs e)
        {
            if (hiddenEditorType == "addFile")
            {
                TreeListNode currentNode = treeFile.FocusedNode;
                TreeListNode parentNode = treeClass.FocusedNode;
                if (currentNode == null || parentNode == null) return;
                try
                {
                    string filename = currentNode.GetValue(treeFile.Columns["FileName"]).ToString();
                    string filepath = parentNode.Tag.ToString();
                    string fullname = filepath + '\\' + filename;
                    currentNode.Tag = fullname;
                    //Console.WriteLine(fullname);
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                    xmlDoc.AppendChild(declaration);
                    XmlElement RootElement = xmlDoc.CreateElement("chHIL");
                    xmlDoc.AppendChild(RootElement);
                    xmlDoc.Save(fullname);
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("file name cannot be empty!", "Warning!");
                    currentNode.Remove();
                }
                catch (IOException)
                {
                    MessageBox.Show("Failed to create the file!", "Error");
                    currentNode.Remove();
                }
                finally
                {
                    hiddenEditorType = null;
                }
                return;
            }
            if (hiddenEditorType == "renameFile")
            {
                TreeListNode node = treeFile.FocusedNode;
                if (node == null) return;
                string sourceFile = node.Tag.ToString();
                string targetFile = node.GetValue(treeFile.Columns["FileName"]).ToString();
                if (sourceFile != null)
                {
                    FileInfo sFile = new FileInfo(sourceFile);
                    DirectoryInfo dirInfo = sFile.Directory;
                    targetFile = dirInfo.FullName + "\\" + targetFile;
                    FileInfo tFile = new FileInfo(targetFile);
                    if (sourceFile == targetFile) return;
                    if (tFile.Exists)
                    {
                        MessageBox.Show("Target file already exists!");
                        node.SetValue(treeFile.Columns["FileName"], sFile.Name);
                    }
                    else
                    {
                        sFile.MoveTo(targetFile);
                    }
                }
                hiddenEditorType = null;
                return;
            }
        }
        /// <summary>
        /// delete the node in the treeFile and delete the local file in the meantime
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelClass_Click(object sender, EventArgs e)
        {
            try
            {
                TreeListNode deleteNode = treeFile.FocusedNode;
                string deleteFile = deleteNode.Tag.ToString();
                treeFile.DeleteNode(deleteNode);
                if (File.Exists(deleteFile))
                {
                    File.Delete(deleteFile);
                }
            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (IOException)
            {
                return;
            }
        }
        /// <summary>
        /// create a dialog
        /// and copy the current file to a new file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            TreeListNode node = treeClass.FocusedNode;
            TreeListNode fnode = treeFile.FocusedNode;
            string path = null;
            string name = null;
            if (node != null && fnode != null)
            {
                path = node.Tag.ToString();
                name = fnode.GetValue(treeFile.Columns["FileName"]).ToString();
            }
            dialog.OverwritePrompt = true;
            dialog.InitialDirectory = path;
            dialog.FileName = name;
            dialog.Filter = "All files|*.*|Xml file|*.xml";
            dialog.ShowDialog();
            string sourceFile = path + "//" + name;
            string targetFile = null;
            if (dialog.FileName != "")
            {
                //FileStream fs = (FileStream)dialog.OpenFile();
                targetFile = dialog.FileName;
                File.Copy(sourceFile, targetFile);
            }
        }
        /// <summary>
        /// rename the focused file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReName_Click(object sender, EventArgs e)
        {
            TreeListNode fileNode = treeFile.FocusedNode;
            if (fileNode == null) return;
            fileNode.Selected = true;
            hiddenEditorType = "renameFile";
            treeFile.ShowEditor();
        }
        /// <summary>
        /// no use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //TreeListNode currentNode = treeFile.FocusedNode;
            //if (currentNode == null) return;
            //string path = currentNode.Tag.ToString();
            //MessageBox.Show("Selected file path is " + path);
            //return;
        }
        /// <summary>
        /// show menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeFile_MouseDown(object sender, MouseEventArgs e)
        {
            TreeListHitInfo hitInfo = (sender as TreeList).CalcHitInfo(new Point(e.X, e.Y));
            TreeListNode node = hitInfo.Node;
            if (e.Button == MouseButtons.Right)
            {
                if (node != null)
                {
                    node.TreeList.FocusedNode = node;
                    node.TreeList.ContextMenuStrip = this.FileMenuStripResult;
                }
            }
        }
        /// <summary>
        /// show file in the explorer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_in_explorer_Click(object sender, EventArgs e)
        {
            TreeListNode fileNode = treeFile.FocusedNode;
            if (fileNode != null)
            {
                System.Diagnostics.Process.Start("Explorer", "/select," + fileNode.Tag.ToString());
            }
        }
        /// <summary>
        /// open file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_Click(object sender, EventArgs e)
        {
            TreeListNode fileNode = treeFile.FocusedNode;
            if (fileNode == null)
            {
                MessageBox.Show("Error, no file selected!");
                return;
            }
            else
            {
                string path = fileNode.Tag.ToString();
                XmlSerializer serializer = new XmlSerializer(typeof(TSchem));
                FileStream fs1 = new FileStream(path, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs1);
                CurrentTSchem = (TSchem)serializer.Deserialize(reader);
                fs1.Close();
                //show information included in xml file
                setAllPage();
                return;
            }
        }

        private void setAllPage()
        {
            if (CurrentTSchem == null)
            {
                MessageBox.Show("No file opened!", "Warning");
                return;
            }
            else
            {
                //set CAN page checkbox
                chkCan0.Checked = CurrentTSchem.setCanlist.TSetCans[0].Check == "1" ? true : false;
                chkCan1.Checked = CurrentTSchem.setCanlist.TSetCans[1].Check == "1" ? true : false;
                chkCan2.Checked = CurrentTSchem.setCanlist.TSetCans[2].Check == "1" ? true : false;
                chkCan3.Checked = CurrentTSchem.setCanlist.TSetCans[3].Check == "1" ? true : false;
                //set file name
                beditCan0.Text = CurrentTSchem.setCanlist.TSetCans[0].AgreeMentFile;
                beditCan1.Text = CurrentTSchem.setCanlist.TSetCans[1].AgreeMentFile;
                beditCan2.Text = CurrentTSchem.setCanlist.TSetCans[2].AgreeMentFile;
                beditCan3.Text = CurrentTSchem.setCanlist.TSetCans[3].AgreeMentFile;
                //set baut
                cboxCanbtl11.SelectedIndex = Int32.Parse(CurrentTSchem.setCanlist.TSetCans[0].Baut);
                cboxCanbtl12.SelectedIndex = Int32.Parse(CurrentTSchem.setCanlist.TSetCans[1].Baut);
                cboxCanbtl13.SelectedIndex = Int32.Parse(CurrentTSchem.setCanlist.TSetCans[2].Baut);
                cboxCanbtl14.SelectedIndex = Int32.Parse(CurrentTSchem.setCanlist.TSetCans[3].Baut);
                //set ethernet page
                checkBox1.Checked = CurrentTSchem.SetEthList.TSetEths[0].Check == "1" ? true : false;
                checkBox2.Checked = CurrentTSchem.SetEthList.TSetEths[1].Check == "1" ? true : false;
                buttonEdit1.Text = CurrentTSchem.SetEthList.TSetEths[0].AgreeMentFile;
                buttonEdit2.Text = CurrentTSchem.SetEthList.TSetEths[1].AgreeMentFile;
                NettextEdit1.Text = CurrentTSchem.SetEthList.TSetEths[0].IP;
                NettextEdit2.Text = CurrentTSchem.SetEthList.TSetEths[1].IP;
                TEPortOne1.Text = CurrentTSchem.SetEthList.TSetEths[0].Port;
                TEPortOne2.Text = CurrentTSchem.SetEthList.TSetEths[1].Port;
                //set normal test
                //set test project
                //warning: use list rather than array!
                List<TStep> steps = CurrentTSchem.StepList.TSteps.ToList();
                gridControlTest.DataSource = steps;
                //set Project cmd
                TStep step = steps.FirstOrDefault();
                List<TCMD> tCMDs = step.CmdList.TCMDs.ToList();
                gridControlProject.DataSource = tCMDs;
                //set init omited...
                //set Set omited...
                //set Result judge

            }
        }
        private void GetAllPage()
        {
            if (CurrentTSchem == null) return;
        }

        /// <summary>
        /// some functions may be used
        /// </summary>
        /// <param name="path"></param>
        /// <returns>XmlDocument object</returns>
        public XmlDocument GetXml(string path)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(path);
                return xDoc;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("No such xml file, please retry.");
                return null;
            }
            catch (XmlException)
            {
                MessageBox.Show("Error,wrong file format!");
                return null;
            }
        }
        public void SetValueFromXmlFile(XmlDocument xDoc)
        {
            //set CAN
            XmlNodeList list = xDoc.GetElementsByTagName("canitem");
            string DeviceIndex = null;
            if (list.Count != 0)
            {
                foreach (XmlNode node in list)
                {
                    string chindex = node.Attributes["chindex"].Value;
                    string canFile = node.Attributes["file"].Value;
                    DeviceIndex = node.Attributes["machineindex"].Value;
                    switch (chindex)
                    {
                        case "0":
                            chkCan0.Checked = true;
                            beditCan0.Text = canFile;
                            break;
                        case "1":
                            chkCan1.Checked = true;
                            beditCan1.Text = canFile;
                            break;
                        case "2":
                            chkCan2.Checked = true;
                            beditCan2.Text = canFile;
                            break;
                        case "3":
                            chkCan3.Checked = true;
                            beditCan3.Text = canFile;
                            break;
                    }
                }
                cboxMachineindex.SelectedIndex = Int32.Parse(DeviceIndex);
            }
        }

        private void beditCan0_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileInfo info = GetFile();
            if (info != null)
            {
                beditCan0.Text = info.Name;
            }
            else
            {
                beditCan0.Text = "";
            }
        }

        private void beditCan1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileInfo info = GetFile();
            if (info != null)
            {
                beditCan1.Text = info.Name;
            }
            else
            {
                beditCan1.Text = "";
            }
        }

        private void beditCan2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileInfo info = GetFile();
            if (info != null)
            {
                beditCan2.Text = info.Name;
            }
            else
            {
                beditCan2.Text = "";
            }
        }

        private void beditCan3_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileInfo info = GetFile();
            if (info != null)
            {
                beditCan3.Text = info.Name;
            }
            else
            {
                beditCan3.Text = "";
            }
        }
        private FileInfo GetFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            FileInfo info;
            dialog.ShowDialog();
            try
            {
                string path = Path.GetFullPath(dialog.FileName);
                info = new FileInfo(path);
            }
            catch(ArgumentException)
            {
                info = null;
            }
            return info;
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileInfo info = GetFile();
            if (info != null)
            {
                buttonEdit1.Text = info.Name;
            }
            else
            {
                buttonEdit1.Text = "";
            }
        }

        private void buttonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileInfo info = GetFile();
            if (info != null)
            {
                buttonEdit2.Text = info.Name;
            }
            else
            {
                buttonEdit2.Text = "";
            }
        }
    }

}
