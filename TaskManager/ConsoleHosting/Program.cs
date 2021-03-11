using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Utility.Admin;
using Utility.Config;
using Utility.ConfigHandler;
using Utility.ConfigHandler.Config;
using Utility.Mef;
using Utility.Quartz;
using Web;

namespace ConsoleHosting
{

    /*
     * 使用须知：
     * 控制台或者类库项目的配置文件.config都需要右键设置属性为“始终复制”，程序跑起来才能放到bin下的debug目录。另，修改了配置文件需要清理并重新生成解决方案
     * 右键ConsoleHosting类库项目 - 属性 - 生成事件 - 编辑后期生成事件 - 键入要复制的内容（把Web项目下的Content和Views真个目录拿到ConsoleHosting项目的bin目录里）
     * 增删改查任务在Task类库下的Task目录下写，写完生成然后把Task.dll拷到此目录：\TaskManager\ConsoleHosting\bin\Debug\Task
     * 
     */

    class Program
    {
        /// <summary>

        /// 获取窗口句柄

        /// </summary>

        /// <param name="lpClassName"></param>

        /// <param name="lpWindowName"></param>

        /// <returns></returns>

        [DllImport("user32.dll", SetLastError = true)]

        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);


        /// <summary>
        /// 设置窗体的显示与隐藏
        /// </summary>

        /// <param name="hWnd"></param>

        /// <param name="nCmdShow"></param>

        /// <returns></returns>

        [DllImport("user32.dll", SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);


        /// <summary>

        /// 隐藏控制台

        /// </summary>

        /// <param name="ConsoleTitle">控制台标题(可为空,为空则取默认值)</param>

        public static void hideConsole(string ConsoleTitle = "")
        {

            ConsoleTitle = String.IsNullOrEmpty(ConsoleTitle) ? Console.Title : ConsoleTitle;

            IntPtr hWnd = FindWindow("ConsoleWindowClass", ConsoleTitle);
            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 0);
            }
        }
        static void Main(string[] args)
        {
            //以管理员身份运行
            AdminRun.Run();

            //MEF初始化
            MefConfig.Init();

            //数据库初始化连接
            ConfigInit.InitConfig();

            //系统参数配置初始化
            ConfigManager configManager = MefConfig.TryResolve<ConfigManager>();
            configManager.Init();

            Console.Title = SystemConfig.ProgramName;
            Console.CursorVisible = false; //隐藏光标

            //任务启动
            QuartzHelper.InitScheduler();
            QuartzHelper.StartScheduler();

            try
            {
                //启动站点
                using (NancyHost host = Startup.Start(SystemConfig.WebPort))
                {
                    //调用系统默认的浏览器   
                    string url = string.Format("http://127.0.0.1:{0}", SystemConfig.WebPort);
                    //Process.Start(url);//自动弹出浏览器
                    Console.WriteLine("系统已启动，当前监听站点地址：{0}", url);
                    hideConsole("TaskScheduler-任务执行容器");
                    while (true)
                    {
                        Console.ReadLine();
                        Console.WriteLine("程序运行中，请勿输入任何内容！");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
          
            Console.ReadLine();
          
        }
    }
}
