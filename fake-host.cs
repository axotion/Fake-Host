using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using Microsoft.VisualBasic;
using System.Drawing;

// "global" vars, default IP and Dest
struct Var
{
	public const string VERSION= "Fake-host Beta";
	public static string IP = "127.0.0.1";
	public static string Dest = "www.bbc.com";
};
	
class HostChanger : Form
{
	public HostChanger(){
		//Window title bar
		this.Text = Var.VERSION;
		//Size of window
		this.Width=150;
		this.Height=160;
		// Create new buttons
		Button new_rule = new Button ();
		Button show_hosts = new Button ();
		Button exit = new Button ();
		Button fix_hosts = new Button ();

		// definitions of buttons positions

		new_rule.Top=10;
		new_rule.Left=30;

		show_hosts.Top=40;
		show_hosts.Left=30;

		fix_hosts.Top=70;
		fix_hosts.Left=30;

		exit.Top=100;
		exit.Left=30;
		// create attributes for buttons (like name, event and control)

	    new_rule.Text = "Create new rule to host";
		new_rule.Click += new EventHandler (Button_Rule);
		Controls.Add (new_rule);

		show_hosts.Text = "Show hosts";
		show_hosts.Click += new EventHandler (Button_Show);
		Controls.Add (show_hosts);

		fix_hosts.Text = "Fix hosts";
		fix_hosts.Click += new EventHandler (Button_Fix);
		Controls.Add (fix_hosts);

		exit.Text = "Exit";
		exit.Click += new EventHandler (Button_Close);
		Controls.Add (exit);

	}
	//Changing attributes for file "hosts", then write vars
	private void Button_Rule (object sender, EventArgs e)
	{
		Var.IP = Microsoft.VisualBasic.Interaction.InputBox ("Where redirect? (IP)", Var.VERSION, Var.IP);
		Var.Dest = Microsoft.VisualBasic.Interaction.InputBox ("Website? (URL)", Var.VERSION, Var.Dest);
		File.SetAttributes ("c:\\windows\\system32\\drivers\\etc\\hosts", FileAttributes.Normal);
		File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Environment.NewLine + Var.IP + " " + Var.Dest + Environment.NewLine);
		File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Var.IP + " " + Var.Dest.Remove (0, 4) + Environment.NewLine);
		MessageBox.Show ("Successfully", Var.VERSION, MessageBoxButtons.OK);
	}
	//show the contents of the file
	private void Button_Show (object sender, EventArgs e)
	{
		MessageBox.Show(File.ReadAllText("c:\\windows\\system32\\drivers\\etc\\hosts"));
	}
	private void Button_Close (object sender, EventArgs e)
	{
		Application.Exit();
	}
	//overwrite file
	private void Button_Fix (object sender, EventArgs e)
	{
		File.WriteAllText("/home/axotion/hosts", " ");
		MessageBox.Show ("Successfully");
	}
	//   MAIN!
	public static void Main(string[] args)
	{
		//check if run as admin
	    bool isAdmin;
		WindowsIdentity identity = WindowsIdentity.GetCurrent();
		WindowsPrincipal principal = new WindowsPrincipal(identity);
		isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
		//check if file exist
		if (File.Exists ("c:\\windows\\system32\\drivers\\etc\\hosts")) {
			if (isAdmin != false) {
				//if arguments exist then write directly to hosts
				if (args.Length > 1) {
					Var.IP = args [0];
					Var.Dest = args [1];
				} else {

					Application.Run(new HostChanger());

				}



			} else {
				MessageBox.Show ("You must run it as Adminstrator", Var.VERSION, MessageBoxButtons.OK,MessageBoxIcon.Warning);

			}
		} else
			MessageBox.Show("File not found", Var.VERSION,MessageBoxButtons.OK, MessageBoxIcon.Error);
  }
}
