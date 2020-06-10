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
    /// UCGridMark.xaml 的交互逻辑
    /// </summary>
    public partial class UCGridMark : UserControl
    {
        List<string> labelList = new List<string>();
        List<string> indexList = new List<string>();
        static ArraySort_Enum sortEnum = ArraySort_Enum.Y升序X降序;

        public UCGridMark()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!InitRow(ModelParams.PickArrayRows, ModelParams.confGlassY / 3, gdArray))
                return;
            if (!InitColumn(ModelParams.PickArrayCols, ModelParams.confGlassX / 3, gdArray))
                return;
            InitArray(ModelParams.PickArrayRows, ModelParams.PickArrayCols, gdArray);
        }

        bool InitRow(int num, double height, Grid grid)
        {
            try
            {
                if (num > 100 || num < 1)
                    return false;
                
                for (int i = 0; i < num; ++i)
                {
                    RowDefinition rowDefinition = new RowDefinition
                    {
                        Height = new GridLength(height)
                    };
                    grid.RowDefinitions.Add(rowDefinition);
                }
            }
            catch(Exception ex)
            {

            }

            return true;
        }

        bool InitColumn(int num, double width, Grid grid)
        {
            try
            {
                if (num > 100 || num < 1)
                    return false;
                for (int i = 0; i < num; ++i)
                {
                    ColumnDefinition colDefinition = new ColumnDefinition
                    {
                        Width = new GridLength(width)
                    };
                    grid.ColumnDefinitions.Add(colDefinition);                    
                }
            }
            catch(Exception ex)
            {

            }

            return true;
        }

        void InitArray(int rows, int cols, Grid grid)
        {
            int x = (int)sortEnum & 2;
            int y = (int)sortEnum & 1;
            int indexx = 0, indexy = 0;
            if (((int)sortEnum & 4) > 0)
            {
                for (int j = 0; j < cols; ++j)
                {
                    for (int i = 0; i < rows; ++i)
                    {

                        int row = i;
                        if (x > 0)
                            indexx = cols - j - 1;
                        else
                            indexx = j;

                        if (y > 0)
                            indexy = rows - i - 1;
                        else
                            indexy = i;
                        System.Windows.Controls.Primitives.ToggleButton button =
                            new System.Windows.Controls.Primitives.ToggleButton
                            {
                                Tag = (indexy + 1).ToString() + '-' + (indexx + 1).ToString(),
                                Content = j * rows + row + 1
                            };
                        button.Checked += new RoutedEventHandler(BtnChecked_event);
                        button.Unchecked += new RoutedEventHandler(BtnUnChecked_event);
                        grid.Children.Add(button);
                        Grid.SetColumn(button, indexx);
                        Grid.SetRow(button, indexy);
                    }
                }
            }
            else
            {
                for (int i = 0; i < rows; ++i)
                {
                    for (int j = 0; j < cols; ++j)
                    {
                        int col = j;

                        if (x > 0)
                            indexx = cols - j - 1;
                        else
                            indexx = j;

                        if (y > 0)
                            indexy = rows - i - 1;
                        else
                            indexy = i;
                        System.Windows.Controls.Primitives.ToggleButton button =
                            new System.Windows.Controls.Primitives.ToggleButton
                            {
                                Tag = (indexy + 1).ToString() + '-' + (indexx + 1).ToString(),
                                Content = i * cols + col + 1
                            };
                        button.Checked += new RoutedEventHandler(BtnChecked_event);
                        button.Unchecked += new RoutedEventHandler(BtnUnChecked_event);
                        grid.Children.Add(button);
                        Grid.SetColumn(button, indexx);
                        Grid.SetRow(button, indexy);
                    }
                }
            }
        }

        void BtnChecked_event(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Primitives.ToggleButton btn = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (!labelList.Contains(btn.Tag.ToString()))
            {
                labelList.Add(btn.Tag.ToString());                
            }
            if(!indexList.Contains(btn.Content.ToString()))
            {
                indexList.Add(btn.Content.ToString());
            }
            UpdateUI();
        }

        void BtnUnChecked_event(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Primitives.ToggleButton btn = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (labelList.Contains(btn.Tag.ToString()))
            {
                labelList.Remove(btn.Tag.ToString());
            }
            if(indexList.Contains(btn.Content.ToString()))
            {
                indexList.Remove(btn.Content.ToString());
            }
            UpdateUI();
        }

        void UpdateUI()
        {
            lblList.Content = string.Empty;
            foreach(string item in labelList)
            {
                lblList.Content += item.Split('-')[0] + "行" + item.Split('-')[1] + "列" + '\n';
            }
        }

        public void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            ModelParams.DumpList_Active.Clear();
            foreach(string item in indexList)
            {
                int num = int.Parse(item);
                ModelParams.DumpList_Active.Add(num.ToString());
            }
        }
    }
}
