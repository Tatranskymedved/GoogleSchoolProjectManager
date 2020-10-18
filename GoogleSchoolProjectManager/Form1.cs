using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using GoogleSchoolProjectManager.Lib.Google;
using GoogleSchoolProjectManager.Lib.Google.Drive;
using System.Collections.ObjectModel;

namespace GoogleSchoolProjectManager
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            const string diskName = "OSTODISK";


            using (var con = new GoogleConnector())
            {
                Do(con);

                var man = new GDriveManager(con);
                man.DriveName = diskName;
                var tree = man.GetTree();
                Add(new GFolder()
                {
                    Files = new ObservableCollection<GFile>(tree.FindAllSpreadSheets())
                    //Items = new ObservableCollection<GFile>(tree.findAllSpreadSheets())
                });
                
                //Add(tree);
            };
        }

        private void Add(GFolder collection, string prefix = "")
        {
            var sortedFolders = collection.Folders.OrderBy(a => a.FileInfo.Name);
            foreach (GFolder item in sortedFolders)
            {
                a(prefix + item.ToString());
                Add(item, prefix + "------------");
            }

            var sortedFiles = collection.Files.OrderBy(a => a.FileInfo.Name);
            foreach (GFile item in sortedFiles)
            {
                a(prefix + item.ToString());
            }
        }

        private void a(string child)
        {
            treeView1.Nodes.Add(child.ToString());
        }

        public void Do(GoogleConnector con)
        {
            //con.Sheets.Spreadsheets.Get();

        }
    }
}
