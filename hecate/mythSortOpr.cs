using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace mythGraStepTwo
{
	public class mythSortOpr
	{
		private List<Atom> alist;
		private class Atom : IComparable{
			public string date = string.Empty;
			public string processname = string.Empty;
			public string url = string.Empty;
			public int spantime = 0;
			public int time = 0;
			private int TryPraseInt(string str){
				int ret;
				if (int.TryParse (str, out ret)) {
					return ret;
				} else {
					return 0;
				}
			}
			public Atom(string str){
				var tmp = str.Split(',');
				if(tmp.Length > 4){
					date = tmp[0];
					processname = tmp[1];
					url = tmp[2];
					spantime = TryPraseInt(tmp[3]);
					time = TryPraseInt(tmp[4]);
				}
			}
			public int CompareTo(object obj)
			{
				Atom p = obj as Atom;
				if (p == null)
				{
					throw new NotImplementedException();
				}
				return p.spantime.CompareTo(this.spantime);
			}
			public override string ToString(){
				return this.date + "," + this.processname + "," + this.url + "," + this.spantime + "," + this.time;
			}
		}
		private string mfilename = string.Empty;
		public mythSortOpr (string filename)
		{
			mfilename = filename;
			alist = new List<Atom> ();
		}
		public void StartLoop(){
			StartLoop (10);
		}
		public void StartLoop(int count){
			if (!mfilename.Equals(string.Empty)){
				var alltxt = File.ReadAllLines (mfilename);
				foreach (string s in alltxt) {
					Atom atom = new Atom (s);
					if (!atom.url.Equals (string.Empty)) {
						alist.Add (atom);
					}
				}
				alist.Sort ();
				List<string> slist = new List<string> ();
				//foreach (Atom a in alist) {
				for (int i = 0; i < count; i++) {
					if (i < alist.Count)
						slist.Add (alist [i].ToString ());
					else
						break;
				}
				if (slist.Count > 0)
					File.WriteAllLines (mfilename, slist.ToArray ());
				else
					File.Delete (mfilename);
			}
		}
	}
}

