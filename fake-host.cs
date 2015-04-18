using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Principal;
using System.Drawing;
// "global" vars, default IP and Dest
struct Var
{
	public const string VERSION= "Fake-host Beta";
	public static string IP = "";
	public static string Dest = "";
	public const string example_IP="127.0.0.1";
	public const string example_Dest="www.google.com";
};

class HostChanger : Form
{
	//must change var, not only value
	public static DialogResult InputBox(string title, string promptText, ref string value)
	{
		//forms for box with text, button etc
		Form form = new Form();
		Label label = new Label();
		TextBox textBox = new TextBox();
		Button buttonOk = new Button();

		//set title from arguments, labels etc
		form.Text = title;
		label.Text = promptText;
		textBox.Text = value;

		//create text on button
		buttonOk.Text = "OK";
		buttonOk.DialogResult = DialogResult.OK;
		//set bounds 
		label.SetBounds(9, 20, 372, 13);
		textBox.SetBounds(12, 36, 372, 20);
		buttonOk.SetBounds(228, 72, 75, 23);


		label.AutoSize = true;
		textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
		buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

		//position, size, can't minimize and Maximize
		form.ClientSize = new Size(396, 107);
		form.Controls.AddRange(new Control[] { label, textBox, buttonOk });
		form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
		form.FormBorderStyle = FormBorderStyle.FixedDialog;
		form.StartPosition = FormStartPosition.CenterScreen;
		form.MinimizeBox = false;
		form.MaximizeBox = false;
		form.AcceptButton = buttonOk;
		//write textBox.text to value(arguments) 
		DialogResult dialogResult = form.ShowDialog();
		value = textBox.Text;
		return dialogResult;
	}
	public HostChanger(){
		//Main window title bar
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

		new_rule.Text = "Create new rule";
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
	//Changing attributes for file "hosts", then write to vars
	private void Button_Rule (object sender, EventArgs e)
	{

		try{	
			while (Var.IP == "" ){

				InputBox(Var.VERSION, "Where redirect? (IP)",ref Var.IP );
			}

			while(Var.Dest == ""){
				InputBox(Var.VERSION, "Website? (URL with WWW)",ref Var.Dest );
			}
			File.SetAttributes ("c:\\windows\\system32\\drivers\\etc\\hosts", FileAttributes.Normal);
			File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Environment.NewLine + Var.IP + " " + Var.Dest + Environment.NewLine);
			File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Var.IP + " " + Var.Dest.Remove (0, 4) + Environment.NewLine);
			MessageBox.Show ("Successfull", Var.VERSION, MessageBoxButtons.OK);
			File.SetAttributes ("c:\\windows\\system32\\drivers\\etc\\hosts", FileAttributes.ReadOnly);
			Var.IP="";
			Var.Dest="";
		}
		catch(System.ArgumentOutOfRangeException error){
		    Var.IP="";
		    Var.Dest="";
		}
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
		File.WriteAllText("c:\\windows\\system32\\drivers\\etc\\hosts", " ");
		MessageBox.Show ("Successfull");
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
					File.SetAttributes ("c:\\windows\\system32\\drivers\\etc\\hosts", FileAttributes.Normal);
					File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Environment.NewLine + Var.IP + " " + Var.Dest + Environment.NewLine);
					File.AppendAllText ("c:\\windows\\system32\\drivers\\etc\\hosts", Var.IP + " " + Var.Dest.Remove (0, 4) + Environment.NewLine);
					if (File.ReadAllText ("c:\\windows\\system32\\drivers\\etc\\hosts") != null) {
						Console.WriteLine ("Successfull");
						File.SetAttributes ("c:\\windows\\system32\\drivers\\etc\\hosts", FileAttributes.ReadOnly);
					}
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
