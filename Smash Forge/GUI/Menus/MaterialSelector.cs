﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Smash_Forge
{
    public partial class MaterialSelector : Form
    {
        public static int Running = 0;
        public static int Opened = 1;
        public static int Cancelled = 2;

        public string path = null;
        ImageList matPresetRenders = new ImageList();
        public int exitStatus = 0; //0 - not done, 1 - one is selected, 2 - cancelled

        public MaterialSelector()
        {
            InitializeComponent();
            matPresetRenders.ImageSize = new Size(64, 64);
            matPresetRenders.ColorDepth = ColorDepth.Depth32Bit;
            addPresetImages();

            treeView1.ImageList = matPresetRenders;
        }


        public void populateTreeNodes()
        {
            if (Directory.Exists(MainForm.executableDir + "\\materials"))
            {
                Console.WriteLine(Path.GetFullPath(MainForm.executableDir + "\\materials"));
                foreach (string folder in Directory.EnumerateDirectories(Path.GetFullPath(MainForm.executableDir + "\\materials")))
                {
                    DirectoryInfo dir = new DirectoryInfo(folder);
               
                    TreeNode folderNode = new TreeNode(dir.Name) { Tag = "folder" };
                    if (dir.Name == "Character Mats")
                    {
                        folderNode.ImageKey = "Character";
                        folderNode.SelectedImageKey = "Character";
                    }
                    if (dir.Name == "Stage Mats")
                    {
                        folderNode.ImageKey = "Stage";
                        folderNode.SelectedImageKey = "Stage";
                    }


                    foreach (string file in Directory.EnumerateFiles(folder))
                    {
                        if (Path.GetExtension(file) == ".nmt")
                        {
                            string filename = Path.GetFileNameWithoutExtension(file);
                            TreeNode node = new TreeNode(filename) { Tag = file };
                            if (matPresetRenders.Images.ContainsKey(filename))
                            {
                                node.ImageKey = filename;
                                node.SelectedImageKey = filename;

                            }
                            folderNode.Nodes.Add(node);
                        }
                    }
                    if (dir.Name != "Preview Images")
                        treeView1.Nodes.Add(folderNode);
                }
            }
            treeView1.Refresh();
        }

        public void addPresetImages()
        {
            // should use a proper folder for preset images eventually 

            if (Directory.Exists(MainForm.executableDir + "//Preview Images"))
            {
                if (File.Exists(MainForm.executableDir + "//Preview Images//Dummy.png"))
                    matPresetRenders.Images.Add("Dummy", Image.FromFile(MainForm.executableDir + "//Preview Images//Dummy.png"));
                if (File.Exists(MainForm.executableDir + "//Preview Images//Character.png"))
                    matPresetRenders.Images.Add("Character", Image.FromFile(MainForm.executableDir + "//Preview Images//Character.png"));
                if (File.Exists(MainForm.executableDir + "//Preview Images//Stage.png"))
                    matPresetRenders.Images.Add("Stage", Image.FromFile(MainForm.executableDir + "//Preview Images//Stage.png"));

                foreach (string file in Directory.EnumerateFiles(MainForm.executableDir + "//Preview Images"))
                {
                    if (Path.GetExtension(file) == ".png")
                    {
                        string filename = Path.GetFileNameWithoutExtension(file);
                        matPresetRenders.Images.Add(filename, Image.FromFile(file));
                    }
                }
            }
        }

        private void openButton()
        {
            if (treeView1.SelectedNode != null && ((string)treeView1.SelectedNode.Tag).EndsWith(".nmt"))
            {
                path = ((string)treeView1.SelectedNode.Tag);
                exitStatus = Opened;
                Close();
            }
        }

        private void openButton(object sender, EventArgs e)
        {
            openButton();
        }

        private void closeButton(object sender, EventArgs e)
        {
            exitStatus = Cancelled;
            Close();
        }

        private void MaterialSelector_Load(object sender, EventArgs e)
        {
            populateTreeNodes();
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                openButton();
                e.Handled = true;
            }
        }
    }
}
