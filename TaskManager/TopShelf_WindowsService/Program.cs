using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace TopShelf_WindowsService
{
    /*
     * 本控制台项目和整个解决方案没有任何关系，仅测试TopShelf把Quartz安装到Windows服务里，做到开机启动。
     */

    /*
     * 以Windows服务运行程序步骤：
     * 1.打开控制台（cmd）
     * 2.D:\code\study\TaskManager\TaskManager\TopShelf_WindowsService\bin\Debug\TopShelf_WindowsService.exe install
     * 3.D:\code\study\TaskManager\TaskManager\TopShelf_WindowsService\bin\Debug\TopShelf_WindowsService.exe start
     * 4.D:\code\study\TaskManager\TaskManager\TopShelf_WindowsService\bin\Debug\TopShelf_WindowsService.exe uninstall
     * 
     * 或者直接运行debug目录下的bat文件安装和卸载服务
     */

    /*
     * 参考资料：
     * http://www.cnblogs.com/jys509/p/4614975.html
     * http://www.cnblogs.com/jys509/p/4628926.html
     */

    class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseNLog();
                x.Service<ServiceRunner>();
                x.RunAsLocalSystem();

                x.SetDescription("TopShelf服务简单测试");
                x.SetDisplayName("TopShelfDemo");
                x.SetServiceName("TopShelfService");//批处理文件根据这个打开和关闭服务

                x.EnablePauseAndContinue();
            });
        }
    }
}
