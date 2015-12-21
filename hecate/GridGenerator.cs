using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
namespace hecate
{
	public class GridGenerator
	{
		private string mdir = string.Empty;
		public GridGenerator (string dir)
		{
			mdir = dir;
		}

		private List<string> CreateDate(string mdir,string datefrom,string dateto){
			DateTime datef = DateTime.Parse (datefrom);
			DateTime datet = DateTime.Parse (dateto);
			List<string> ret = new List<string> ();
			var dirs = Directory.GetDirectories (mdir);
			foreach (string s in dirs) {
				string t = Path.GetFileNameWithoutExtension (s);
				DateTime dt = DateTime.Parse (t);
				if (datef <= dt && datet >= dt) {
					ret.Add (t);
				}
			}
			return ret;
		}
		private List<string> CreateDate(string mdir){
			List<string> ret = new List<string> ();
			var dirs = Directory.GetDirectories (mdir);
			foreach (string s in dirs) {
				ret.Add (Path.GetFileNameWithoutExtension (s));
			}
			return ret;
		}
		private List<string> CreateDate(string datefrom,string dateto){
			List<string> mlist = new List<string> ();
			DateTime datef = DateTime.Parse (datefrom);
			DateTime dateT = DateTime.Parse (dateto);
			while (!datef.Equals (dateT)) {
				mlist.Add(datef.ToString("yyyy-MM-dd"));
				datef = datef.AddDays (1);
			}
			return mlist;
		}
		public void StartLoop(){
			if (mdir.Equals (string.Empty))
				return;
			if (File.Exists (mdir + "/append.txt")) {
				string username = Path.GetFileNameWithoutExtension (mdir);
				List<string> mlist = CreateDate (mdir);
				var str = File.ReadAllLines (mdir + "/append.txt",System.Text.Encoding.Default);
				StreamWriter sw = new StreamWriter ("./result/" + username + ".csv", false, System.Text.Encoding.Default);
				//write first line
				sw.Write (" ");
				foreach(string l in mlist){
					sw.Write ("," + l.ToString ());
				}
				sw.WriteLine ();
				int i = 0;
				foreach (string s in str) {
					var splittmp = s.Split (':');
					if (splittmp.Length > 1) {
						string tmplike = splittmp [0];
						sw.Write (tmplike);
						foreach (string l in mlist) {
							sw.Write ("," + grandHelper.GetInstance ().FindValue (username, l, tmplike).ToString ());
						}
						sw.WriteLine ();
						i++;
						if (i == 20)
							break;
					}
				}
				sw.Close ();
			}
		}
	}
}

