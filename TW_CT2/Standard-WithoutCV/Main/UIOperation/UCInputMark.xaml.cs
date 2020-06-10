using BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// UCInputMark.xaml 的交互逻辑
    /// </summary>
    public partial class UCInputMark : UserControl
    {
        public UCInputMark()
        {
            InitializeComponent();
        }

        public void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] arr = tbIndex.Text.Trim().Split(new char[] { ' ', ',', '/', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    if (!int.TryParse(item, out int index))
                    {
                        MessageBox.Show("存在无效数据，请重新输入");
                        return;
                    }
                }
                ModelParams.DumpList_Active.Clear();
                ModelParams.DumpList_Active = new List<string>(arr);
            }
            catch (Exception ex)
            {
                Log.L_I.WriteError("MainWindow", ex);
            }
            //this.Close();
        }
    }
}
