using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;

struct Var
{
	public const string VERSION= "Fake-host Beta";
	public static string IP = "127.0.0.1";
	public static string Dest = "www.bbc.com";
};
class HostChanger
{
	public static void Main(string[] args)
	{
		if (args.Length > 1)
		{
			Var.IP = args[0];
			Var.Dest = args[1];
		}
		bool isAdmin;
		WindowsIdentity identity = WindowsIdentity.GetCurrent();
		WindowsPrincipal principal = new WindowsPrincipal(identity);
		isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
		if (File.Exists ("c:\\windows\\system32\\drivers\\etc\\hosts")) {
			if (isAdmin != false) {
				File.SetAttributes ("c:\\windows\\system32\\drivers\\etc\\hosts", FileAttributes.Normal);
				File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Environment.NewLine + Var.IP + " " + Var.Dest + Environment.NewLine);
				File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Var.IP + " " + Var.Dest.Remove (0, 4) + Environment.NewLine);
				MessageBox.Show ("Complete!", Var.VERSION, MessageBoxButtons.OK);

			} else {
				MessageBox.Show ("You must run it as Adminstrator", Var.VERSION, MessageBoxButtons.OK);

			}
		} else
			MessageBox.Show("File not found", Var.VERSION);
	}
}
