using System;
using System.Threading;

namespace AsynDelegation
{
    //自定义计算类
    class Calculator
    {
        public int Add(int x, int y)
        {
            if (Thread.CurrentThread.IsThreadPoolThread)
            {
                Thread.CurrentThread.Name = "Pool Thread";
            }
            Console.WriteLine("Method invoked!");

            // 执行某些事情，模拟需要执行2秒钟
            for (int i = 1; i <= 2; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(i));
                Console.WriteLine("{0}:Add executed {1} second(s).", Thread.CurrentThread.Name, i);
            }
            Console.WriteLine("Method complete!");
            return x + y;
        }
    }

    class Delegate
    {
        //定义委托
        public delegate int AddDelegate(int x, int y);
        static void Main(string[] args)
        {
            Console.WriteLine("Client application started! ");
            Thread.CurrentThread.Name = "Main Thread";

            //实例化计算类
            Calculator cal = new Calculator();

            //定义委托变量
            AddDelegate del = new AddDelegate(cal.Add);

            //委托的异步调用
            IAsyncResult asyncResult = del.BeginInvoke(2, 5, null, null);

            //做某些其它的事情，模拟需要执行3秒钟
            for (int i = 1; i <= 3; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(i));
                Console.WriteLine("{0}:Client executed {1} second(s).",
                   Thread.CurrentThread.Name, i);
            }

            //调用异步委托的结果
            int rtn = del.EndInvoke(asyncResult);
            Console.WriteLine("Result: {0} ", rtn);

            //等待
            Console.ReadKey();
        }
    }
}