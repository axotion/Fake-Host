using System;
using System.IO;
using System.Security.Principal;

/* For example
 * 127.0.0.1 google.com
 * Where 127.0.0.1 is your server/whatever
 * Also you can run it with parametr like xxx.xxx.xxx.xxx www.something.com
 * */

struct Var{
public static string IP = "127.0.0.1";
public static string Dest = "www.google.com";
};
	class HostChanger
	{
	public static void Main (string[] args)
	{
	    if (args.Length > 1) {
			Var.IP = args [0];
			Var.Dest = args[1];
		                         }
			bool isAdmin;
			WindowsIdentity identity = WindowsIdentity.GetCurrent ();
			WindowsPrincipal principal = new WindowsPrincipal (identity);
			isAdmin = principal.IsInRole (WindowsBuiltInRole.Administrator);
		    if(File.Exists("c:\\windows\\system32\\drivers\\etc\\hosts")){
		    if (isAdmin != false) {
			File.SetAttributes ("c:\\windows\\system32\\drivers\\etc\\hosts", FileAttributes.Normal);
			File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Environment.NewLine + Var.IP+" "+Var.Dest+Environment.NewLine);
			File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Var.IP+" "+Var.Dest.Remove (0,4)+Environment.NewLine);
				} else {
				Console.WriteLine ("You must run it as admin");
			    Console.ReadKey ();
			           }
		        }
		}
}
