using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using Microsoft.VisualBasic;

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
	      bool isAdmin;
		WindowsIdentity identity = WindowsIdentity.GetCurrent();
		WindowsPrincipal principal = new WindowsPrincipal(identity);
		isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
		if (File.Exists ("c:\\windows\\system32\\drivers\\etc\\hosts")) {
			if (isAdmin != false) {

				if (args.Length > 1) {
					Var.IP = args [0];
					Var.Dest = args [1];
				} else {
					Var.IP =	Microsoft.VisualBasic.Interaction.InputBox ("Where redirect? (IP)", Var.VERSION, Var.IP);
					Var.Dest =	Microsoft.VisualBasic.Interaction.InputBox ("Website? (URL)", Var.VERSION, Var.Dest);
				}

				File.SetAttributes ("c:\\windows\\system32\\drivers\\etc\\hosts", FileAttributes.Normal);
				File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Environment.NewLine + Var.IP + " " + Var.Dest + Environment.NewLine);
				File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Var.IP + " " + Var.Dest.Remove (0, 4) + Environment.NewLine);
				MessageBox.Show ("Successfully", Var.VERSION, MessageBoxButtons.OK);

			} else {
				MessageBox.Show ("You must run it as Adminstrator", Var.VERSION, MessageBoxButtons.OK,MessageBoxIcon.Warning);

			}
		} else
			MessageBox.Show("File not found", Var.VERSION,MessageBoxButtons.OK, MessageBoxIcon.Error);
  }
}
