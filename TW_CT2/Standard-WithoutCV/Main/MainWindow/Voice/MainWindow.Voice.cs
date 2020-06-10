using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DealMedia;
using System.Threading.Tasks;
using BasicClass;

namespace Main
{
    /// <summary>
    /// 声音控制
    /// </summary>
    partial class MainWindow
    {
        /// <summary>
        /// 显示语音
        /// </summary>
        void ShowVoice(int i)
        {
            try
            {                
                switch (i)
                {
                    case 1:
                        ShowVoice("请上插栏卡塞");
                        break;

                    case 2:
                        ShowVoice("请上取栏卡塞");
                        break;

                    case 3:
                        ShowVoice("安全门打开");
                        break;

                    case 4:
                        ShowVoice("请上料");
                        break;

                    case 5:
                        ShowVoice("卡塞满清");
                        break;

                    case 6:
                        ShowVoice("机器人抛料");
                        break;

                    case 7:
                        ShowVoice("皮带线满");
                        break;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        void ShowVoice(string name)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(()=>
                    {
                        UCVoicePlay.U_I.NameVoice = name;
                        new Task(UCVoicePlay.U_I.StartPlay_Task).Start();
                    }
                
                ));
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
