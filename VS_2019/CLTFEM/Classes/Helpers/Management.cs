using CLTFEM.Windows;
using CLTFEM.Classes.Structural;
using System.Windows;

namespace CLTFEM.Classes.Helpers
{
    class Management
    {
        public Management()
        {

        }

        public static void CleanCurrentStrucure()
        {
            //cleans the current LISTs of objects in the project (nodes, elements, materials, sections, etc.)
            if (MainWindow.materialList != null)
            {
                MainWindow.materialList.Clear();
            }
            if (MainWindow.nodeList != null)
            {
                MainWindow.nodeList.Clear();
            }
            if (MainWindow.springList != null)
            {
                MainWindow.springList.Clear();
            }
            if (MainWindow.shellList != null)
            {
                MainWindow.shellList.Clear();
            }
            if (MainWindow.loadList != null)
            {
                MainWindow.loadList.Clear();
            }
            if (MainWindow.massList != null)
            {
                MainWindow.massList.Clear();
            }
            if (MainWindow.supportList != null)
            {
                MainWindow.supportList.Clear();
            }
            if (MainWindow.dispList != null)
            {
                MainWindow.dispList.Clear();
            }
            if (MainWindow.seriesDispList != null)
            {
                MainWindow.seriesDispList.Clear();
            }
            if (MainWindow.seriesLoadList != null)
            {
                MainWindow.seriesLoadList.Clear();
            }
            if (MainWindow.modalDispList != null)
            {
                MainWindow.modalDispList.Clear();
            }
            if (MainWindow.natFreqs != null)
            {
                MainWindow.natFreqs.Clear();
            }
            if (MainWindow.deformed)
            {
                MainWindow.deformed = false;
            }
            if (MainWindow.analysis != null)
            {
                MainWindow.analysis.Clear(); 
            }
            if (MainWindow.seismicLoad != null)
            {
                MainWindow.seismicLoad.Clear();
            }
            if (MainWindow.impulseLoad != null)
            {
                MainWindow.impulseLoad.Clear();
            }

            MainWindow.iteration = 1;
            MainWindow.scale = 1;
            MainWindow.numberLoadSteps = 0;

            //cleans the current 3D elements on screen
            DrawingHelper.ClearScreen(ref MainWindow.myModel3DGroup);

        }

        public static void ReorganizeLoadList()
        {
            int count = 1;
            foreach (Load l in MainWindow.loadList)
            {
                l.ID = count;
                count++;
            }
        }

        public static void ReorganizeMassList()
        {
            int count = 1;
            foreach (Mass l in MainWindow.massList)
            {
                l.ID = count;
                count++;
            }
        }

        public static void ReorganizeMaterialList()
        {
            int count = 1;
            foreach (Material mat in MainWindow.materialList)
            {
                mat.ID = count;
                count++;
            }
        }

        public static void ReorganizeSupportList()
        {
            int count = 1;
            foreach (Support sup in MainWindow.supportList)
            {
                sup.ID = count;
                count++;
            }
        }

        public static void ReorganizeShellList()
        {
            int count = 1;
            foreach (ShellElement shell in MainWindow.shellList)
            {
                shell.ID = count;
                count++;
            }
        }

        public static void ReorganizeNodeList()
        {
            int count = 1;
            foreach (Node node in MainWindow.nodeList)
            {
                node.ID = count;
                count++;
            }
        }

        public static void ReorganizeSpring3DList()
        {
            int count = 1;
            foreach (Spring3D spring in MainWindow.springList)
            {
                spring.ID = count;
                count++;
            }
        }

    }
}
