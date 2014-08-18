using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;

using Ini;

namespace SocketAgentC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter your command here:");
            string cmd = Console.ReadLine();
            switch (cmd)
            {

                case "send":
                    SendMessage();
                    Console.WriteLine("Send data");
                    break;
                //case "receive":
                //    ReceiveMessage();
                //    Console.WriteLine("Receive data:");
                //    break;
                default:
                    Console.WriteLine("You enter a wrong argument.  Press Enter to Exit");
                    break;

            }

        }
        static void SendMessage()
        {
            try
            {
                IniFile ini = new IniFile(Environment.CurrentDirectory + "\\param.ini");
                int port = Int32.Parse(ini.IniReadValue("Config","PORT"));
                string host = ini.IniReadValue("Config", "IP");

                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);

                Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
                Console.WriteLine("Connecting ...");
                c.Connect(ipe);//连接到服务器

                ///向服务器发送信息
                string sendStr = "Hello! This is a socket test";
                byte[] bs = Encoding.ASCII.GetBytes(sendStr);//把字符串编码为字节
                Console.WriteLine("Send Message");
                c.Send(bs, bs.Length, 0);//发送信息


                ///接受从服务器返回的信息
                string recvStr = "";
                byte[] recvBytes = new byte[1024];
                int bytes;
                bytes = c.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
                recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
                Console.WriteLine("client get message: {0}", recvStr);//显示服务器返回信息

                ///一定记着用完socket后要关闭
                c.Close();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("argumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }
/*
        static void ReceiveMessage()
        {
            int port = 8080;
            string host = "127.0.0.1";
            try
            {
                ///创建终结点（EndPoint）
                IPAddress ip = IPAddress.Parse(host);//把ip地址字符串转换为IPAddress类型的实例
                IPEndPoint ipe = new IPEndPoint(ip, port);//用指定的端口和ip初始化IPEndPoint类的新实例


                ///创建socket并开始监听
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个socket对像，如果用udp协议，则要用SocketType.Dgram类型的套接字
                s.Bind(ipe);//绑定EndPoint对像（2000端口和ip地址）
                s.Listen(0);//开始监听
                Console.WriteLine("Waiting for the client connection ...");//等待客户端连接...


                ///接受到client连接，为此连接建立新的socket，并接受信息
                Socket temp = s.Accept();//为新建连接创建新的socket
                Console.WriteLine("Connection succeeds.");//与客户端成功建立连接
                string recvStr = "";
                byte[] recvBytes = new byte[1024];
                int bytes;
                bytes = temp.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
                //recvStr = System.Text.Encoding.Default.GetString(recvBytes);
                recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
                //recvStr += Encoding.ASCII.GetString(recvBytes);

                //recvStr=reverse(recvStr, 0, 1024);
                ///给client端返回信息
                Console.WriteLine("Server get message from client : {0}", recvStr);//把客户端传来的信息显示出来
                string sendStr = "Ok! Client sends message success .";
                byte[] bs = Encoding.ASCII.GetBytes(sendStr);
                temp.Send(bs, bs.Length, 0);//返回信息给客户端
                temp.Close();
                s.Close();
                Console.ReadLine();
            }
            catch (Exception Ex)
            {
                ErrorLog(Ex);
                //throw;
            }
        }
*/
        public static void ErrorLog(Exception Ex)
        {
            string ErrTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string ErrSource = Ex.Source;
            string ErrTargetSite = Ex.TargetSite.ToString();
            string ErrMsg = Ex.Message;
            string ErrStackTrace = Ex.StackTrace;

            Console.WriteLine("SocketServer有错误:");
            Console.WriteLine("错误时间 : " + ErrTime);
            Console.WriteLine("错误对象 : " + ErrSource);
            Console.WriteLine("异常方法 : " + ErrTargetSite);
            Console.WriteLine("错误信息 : " + ErrMsg);
            Console.WriteLine("堆栈内容 : ");
            Console.WriteLine(ErrStackTrace);
            Console.WriteLine("\r\n******************Error*Report*****************\r\n");
            Console.WriteLine("按Enter键继续 : ");
            if (Console.ReadKey().KeyChar == '\r')
            { }
        }
        
    }
}
