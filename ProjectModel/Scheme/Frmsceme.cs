using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using System.IO;

namespace Scheme
{
    public partial class Frmsceme : Form
    {
        private string schemeTopPath = "";
        private string CurrentFileUrl = null;
        private string hiddenEditorType = null;
        private TSchem CurrentTSchem = new TSchem();
        private bool SaveMode = false;
        private FileRW frw = new FileRW();
        private XMLFunction XmlFunc = new XMLFunction();
        private DevExpress.XtraGrid.Views.Grid.GridView CurrentgridView = null;

        public Frmsceme()
        {
            InitializeComponent();
            schemeTopPath = Application.StartupPath + @"\category";
        }
        public Frmsceme(string spath)
        {
            schemeTopPath = spath;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            frw.RemoveFolder(ref treeClass);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            frw.ImportFolder(ref treeClass);
        }
        private void Frmsceme_Load(object sender, EventArgs e)
        {
            ToolHeight();
            TreeListNode FileNode = treeClass.AppendNode(null, null);
            FileNode.SetValue(treeClass.Columns["FolderName"], "scheme");
            FileNode.Tag = schemeTopPath;
            frw.TraverseFolder(FileNode, ref treeClass);
            treeClass.FocusedNode = FileNode;
            frw.TraverseFile(ref treeClass, ref treeFile);
        }
        private void ToolHeight()
        {
            scemePanel.Height = toolboxControl1.Height - 39;
        }
        private void Frmsceme_Resize(object sender, EventArgs e)
        {
            ToolHeight();
        }
        private void treeClass_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            frw.TraverseFile(ref treeClass, ref treeFile);
        }
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

        }
        private void treeFile_HiddenEditor(object sender, EventArgs e)
        {
            frw.FileNameEditorHidden(ref hiddenEditorType, ref treeFile, ref treeClass);
            //reload the file list after hidden editor
            frw.TraverseFile(ref treeClass, ref treeFile);
        }
        private void btnDelClass_Click(object sender, EventArgs e)
        {
            frw.DeleteFile(ref treeFile);
        }
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.OverwritePrompt = true;
            dialog.InitialDirectory = schemeTopPath;
            dialog.Filter = "Test sample|*.hil";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                frw.SaveFile(CurrentTSchem, dialog.FileName);
                TreeListNode node = treeClass.FocusedNode;
                string path = node.Tag.ToString();
                frw.TraverseFile(ref treeClass, ref treeFile);
            }
        }
        private void btnReName_Click(object sender, EventArgs e)
        {
            TreeListNode fileNode = treeFile.FocusedNode;
            if (fileNode == null) return;
            fileNode.Selected = true;
            hiddenEditorType = "renameFile";
            treeFile.ShowEditor();
        }
        private void Show_in_explorer_Click(object sender, EventArgs e)
        {
            TreeListNode fileNode = treeFile.FocusedNode;
            if (fileNode != null)
            {
                System.Diagnostics.Process.Start("Explorer", "/select," + fileNode.Tag.ToString());
            }
        }
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
                try
                {
                    string path = fileNode.Tag.ToString();
                    CurrentFileUrl = path;
                    XmlFunc.LoadXml(path, ref CurrentTSchem);
                    setAllPage();
                    SaveMode = true;
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("File format error", "Error!");
                }
                return;
            }
        }
        private void setAllPage()
        {
            SaveMode = false;
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
            //warning: datasource is a list rather than an array!
            setTestProj();
            //set Project cmd
            setProjCMD(CurrentTSchem.StepList.TSteps.FirstOrDefault().CmdList == null ? new cmdList() : CurrentTSchem.StepList.TSteps.FirstOrDefault().CmdList);
            //set init omited...
            //set Set omited...
            //set Result judge
            //set save panel
            setOther(CurrentTSchem.StepList.TSteps.FirstOrDefault().CmdList.TCMDs.FirstOrDefault() == null ? new TCMD() : CurrentTSchem.StepList.TSteps.FirstOrDefault().CmdList.TCMDs.FirstOrDefault());
            SaveMode = true;

        }
        private void setTestProj()
        {
            List<TStep> steps = CurrentTSchem.StepList.TSteps;
            BindingSource bs = new BindingSource();
            bs.DataSource = steps;
            gridControlTest.DataSource = bs;
        }
        private void setProjCMD(cmdList list)
        {
            List<TCMD> tCMDs = list.TCMDs;
            BindingSource bs = new BindingSource();
            bs.DataSource = tCMDs;
            gridControlProject.DataSource = bs;
        }
        private void setOther(TCMD tcmd)
        {
            BindingSource jdbs = new BindingSource();
            BindingSource svbs = new BindingSource();
            jdbs.DataSource = tcmd.Judgelist.tconditions;
            svbs.DataSource = tcmd.Savelist.TConditions;
            gridControlJudge.DataSource = jdbs;
            gridControlSave.DataSource = svbs;
        }
        private void GetAllPage()
        {
            if (CurrentTSchem == null) return;
            //get CAN page checkbox
            CurrentTSchem.setCanlist.TSetCans[0].Check = chkCan0.Checked ? "1" : "0";
            CurrentTSchem.setCanlist.TSetCans[1].Check = chkCan1.Checked ? "1" : "0";
            CurrentTSchem.setCanlist.TSetCans[2].Check = chkCan2.Checked ? "1" : "0";
            CurrentTSchem.setCanlist.TSetCans[3].Check = chkCan3.Checked ? "1" : "0";
            //get file name
            CurrentTSchem.setCanlist.TSetCans[0].AgreeMentFile = beditCan0.Text;
            CurrentTSchem.setCanlist.TSetCans[1].AgreeMentFile = beditCan1.Text;
            CurrentTSchem.setCanlist.TSetCans[2].AgreeMentFile = beditCan2.Text;
            CurrentTSchem.setCanlist.TSetCans[3].AgreeMentFile = beditCan3.Text;
            //get baut
            CurrentTSchem.setCanlist.TSetCans[0].Baut = cboxCanbtl11.SelectedIndex.ToString();
            CurrentTSchem.setCanlist.TSetCans[1].Baut = cboxCanbtl12.SelectedIndex.ToString();
            CurrentTSchem.setCanlist.TSetCans[2].Baut = cboxCanbtl13.SelectedIndex.ToString();
            CurrentTSchem.setCanlist.TSetCans[3].Baut = cboxCanbtl14.SelectedIndex.ToString();
            //get ethernet page
            CurrentTSchem.SetEthList.TSetEths[0].Check = checkBox1.Checked ? "1" : "0";
            CurrentTSchem.SetEthList.TSetEths[1].Check = checkBox2.Checked ? "1" : "0";
            CurrentTSchem.SetEthList.TSetEths[0].AgreeMentFile = buttonEdit1.Text;
            CurrentTSchem.SetEthList.TSetEths[1].AgreeMentFile = buttonEdit2.Text;
            CurrentTSchem.SetEthList.TSetEths[0].IP = NettextEdit1.Text;
            CurrentTSchem.SetEthList.TSetEths[1].IP = NettextEdit2.Text;
            CurrentTSchem.SetEthList.TSetEths[0].Port = TEPortOne1.Text;
            CurrentTSchem.SetEthList.TSetEths[1].Port = TEPortOne2.Text;
        }
        private void beditCan0_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileInfo info = frw.GetFile();
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
            FileInfo info = frw.GetFile();
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
            FileInfo info = frw.GetFile();
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
            FileInfo info = frw.GetFile();
            if (info != null)
            {
                beditCan3.Text = info.Name;
            }
            else
            {
                beditCan3.Text = "";
            }
        }
        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FileInfo info = frw.GetFile();
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
            FileInfo info = frw.GetFile();
            if (info != null)
            {
                buttonEdit2.Text = info.Name;
            }
            else
            {
                buttonEdit2.Text = "";
            }
        }
        private void gridView3_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            TStep nextTstep = new TStep();
            if (gridView3.RowCount != 0)
            {
                int h = gridView3.GetSelectedRows()[0];
                BindingSource bs = (BindingSource)gridControlTest.DataSource;
                List<TStep> list = (List<TStep>)bs.DataSource;
                nextTstep = list[h];
            }
            setProjCMD(nextTstep.CmdList);
        }
        private void gridView6_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            TCMD nextTCMD = new TCMD();
            if (gridView6.RowCount != 0)
            {
                int h = gridView6.GetSelectedRows()[0];
                BindingSource bs = (BindingSource)gridControlProject.DataSource;
                List<TCMD> tcmds = (List<TCMD>)bs.DataSource;
                nextTCMD = tcmds[h];
            }
            setOther(nextTCMD);
        }
        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            SaveToFile();
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            TreeListNode fileNode = treeFile.FocusedNode;
            if (fileNode == null)
            {
                MessageBox.Show("Error, no file selected!");
                return;
            }
            else
            {
                try
                {
                    SaveMode = false;
                    string path = fileNode.Tag.ToString();
                    CurrentFileUrl = path;
                    XmlFunc.LoadXml(path, ref CurrentTSchem);
                    setAllPage();
                    SaveMode = true;
                }
                catch (InvalidOperationException)
                {
                    MessageBox.Show("File format error", "Error!");
                }
                return;
            }
        }
        private void beditCan0_EditValueChanged(object sender, EventArgs e)
        {
            SaveToFile();
        }

        private void gridView6_MouseUp(object sender, MouseEventArgs e)
        {
            ShowMenu(ref gridView6, sender, e);
        }
        private void gridView3_MouseUp(object sender, MouseEventArgs e)
        {
            ShowMenu(ref gridView3, sender, e);
        }
        private void gridView4_MouseUp(object sender, MouseEventArgs e)
        {
            ShowMenu(ref gridView4, sender, e);
        }
        private void gridView10_MouseUp(object sender, MouseEventArgs e)
        {
            ShowMenu(ref gridView10, sender, e);
        }
        private void Result_judge_subview_MouseUp(object sender, MouseEventArgs e)
        {
            ShowMenu(ref Result_judge_subview, sender, e);
        }
        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            ShowMenu(ref gridView1, sender, e);
        }
        private void gridView7_MouseUp(object sender, MouseEventArgs e)
        {
            ShowMenu(ref gridView7, sender, e);
        }

        private void toolStripMenuItemAdd_Click(object sender, EventArgs e)
        {
            if (CurrentgridView == null) return;
            CurrentgridView.AddNewRow();
        }
        private void toolStripMenuItemDel_Click(object sender, EventArgs e)
        {
            if (CurrentgridView == null) return;
            CurrentgridView.DeleteSelectedRows();
            SaveToFile();
        }
        private void toolStripMenuItemMvUp_Click(object sender, EventArgs e)
        {
            int i = CurrentgridView.GetSelectedRows()[0];
            BindingSource bs = (BindingSource)CurrentgridView.DataSource;
            if (CurrentgridView.Name == "gridView3")
            {
                List<TStep> list = (List<TStep>)bs.DataSource;
                TStep temp = list[i];
                list[i] = list[i - 1];
                list[i - 1] = temp;
            }
            else if (CurrentgridView.Name == "gridView6")
            {
                List<TCMD> list = (List<TCMD>)bs.DataSource;
                TCMD temp = list[i];
                list[i] = list[i - 1];
                list[i - 1] = temp;
            }
            else if (CurrentgridView.Name == "Result_judge_subview")
            {
                List<subCondition> list = (List<subCondition>)bs.DataSource;
                subCondition temp = list[i];
                list[i] = list[i - 1];
                list[i - 1] = temp;
            }
            else
            {
                List<TCondition> list = (List<TCondition>)bs.DataSource;
                TCondition temp = list[i];
                list[i] = list[i - 1];
                list[i - 1] = temp;
            }
            setAllPage();
            SaveToFile();
        }
        private void toolStripMenuItemMvDn_Click(object sender, EventArgs e)
        {
            int i = CurrentgridView.GetSelectedRows()[0];
            BindingSource bs = (BindingSource)CurrentgridView.DataSource;
            if (CurrentgridView.Name == "gridView3")
            {
                List<TStep> list = (List<TStep>)bs.DataSource;
                TStep temp = list[i];
                list[i] = list[i + 1];
                list[i + 1] = temp;
            }
            else if (CurrentgridView.Name == "gridView6")
            {
                List<TCMD> list = (List<TCMD>)bs.DataSource;
                TCMD temp = list[i];
                list[i] = list[i + 1];
                list[i + 1] = temp;
            }
            else if (CurrentgridView.Name == "Result_judge_subview")
            {
                List<subCondition> list = (List<subCondition>)bs.DataSource;
                subCondition temp = list[i];
                list[i] = list[i + 1];
                list[i + 1] = temp;
            }
            else
            {
                List<TCondition> list = (List<TCondition>)bs.DataSource;
                TCondition temp = list[i];
                list[i] = list[i + 1];
                list[i + 1] = temp;
            }
            setAllPage();
            SaveToFile();
        }

        private void SaveToFile()
        {
            if (!SaveMode) return;
            if (CurrentFileUrl == null) return;
            GetAllPage();
            string path = treeFile.FocusedNode.Tag.ToString();
            try
            {
                File.Delete(path);
                XmlFunc.SaveXml(path, CurrentTSchem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
            }
        }
        private void ShowMenu(ref DevExpress.XtraGrid.Views.Grid.GridView gridView, object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitinfo = gridView.CalcHitInfo(e.Location);
            if (e.Button == MouseButtons.Right)
            {
                toolStripMenuItemAdd.Enabled = true;
                toolStripMenuItemDel.Enabled = true;
                toolStripMenuItemMvDn.Enabled = true;
                toolStripMenuItemMvUp.Enabled = true;
                if (!hitinfo.InRow)
                {
                    toolStripMenuItemMvUp.Enabled = false;
                    toolStripMenuItemMvDn.Enabled = false;
                    toolStripMenuItemDel.Enabled = false;
                }
                else
                {
                    int[] index = gridView.GetSelectedRows();
                    if (index[0] == 0)
                    {
                        toolStripMenuItemMvUp.Enabled = false;
                    }
                    if (index[0] == gridView.RowCount - 1)
                    {
                        toolStripMenuItemMvDn.Enabled = false;
                    }
                }
                MenuStripOpera.Show(MousePosition);
                Console.WriteLine(CurrentgridView.Name);
            }
        }

        private void gridControlTest_MouseEnter(object sender, EventArgs e)
        {
            CurrentgridView = (DevExpress.XtraGrid.Views.Grid.GridView)gridControlTest.FocusedView;
        }
        private void gridControlSave_MouseEnter(object sender, EventArgs e)
        {
            CurrentgridView = (DevExpress.XtraGrid.Views.Grid.GridView)gridControlSave.FocusedView;
        }
        private void gridControlJudge_MouseEnter(object sender, EventArgs e)
        {
            CurrentgridView = (DevExpress.XtraGrid.Views.Grid.GridView)gridControlJudge.FocusedView;
        }
        private void gridControlProject_MouseEnter(object sender, EventArgs e)
        {
            CurrentgridView = (DevExpress.XtraGrid.Views.Grid.GridView)gridControlProject.FocusedView;
        }
        private void Result_judge_subview_MouseEnter(object sender, EventArgs e)
        {
            CurrentgridView = Result_judge_subview;
        }
        private void Result_judge_subview_MouseLeave(object sender, EventArgs e)
        {
            CurrentgridView = (DevExpress.XtraGrid.Views.Grid.GridView)gridControlJudge.FocusedView;
        }
        private void gridControlInit_MouseEnter(object sender, EventArgs e)
        {
            CurrentgridView = (DevExpress.XtraGrid.Views.Grid.GridView)gridControlInit.FocusedView;
        }
        private void gridControlSetCMD_MouseEnter(object sender, EventArgs e)
        {
            CurrentgridView = (DevExpress.XtraGrid.Views.Grid.GridView)gridControlSetCMD.FocusedView;
        }

    }

}
