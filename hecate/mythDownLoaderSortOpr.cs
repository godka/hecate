using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace mythGraStepThree
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
            public string Detail = string.Empty;
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
                    if(tmp.Length > 5)
                        Detail = tmp[5];
				}
                if (Detail.Contains("ReadHtmlFailed"))
                {
                    if (!url.Equals("NULL") && !url.Equals(string.Empty))
                    {
                        this.Detail = mythDownloader.GetInstance().SingleStep(url);
                        Console.WriteLine(this.Detail);
                    }
                }
                else if (Detail.Equals(string.Empty) && !url.Equals("NULL") && !url.Equals(string.Empty))
                {
                    this.Detail = mythDownloader.GetInstance().SingleStep(url);
                    Console.WriteLine(this.Detail);
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
				return this.date + "," + this.processname + "," + this.url + "," + this.spantime + "," + this.time + "," + this.Detail;
			}
		}
		private string mfilename = string.Empty;
		public mythSortOpr (string filename)
		{
			mfilename = filename;
			alist = new List<Atom> ();
		}

		public void StartLoop(){
			if (!mfilename.Equals(string.Empty)) {
				var alltxt = File.ReadAllLines (mfilename,Encoding.Default);
				foreach (string s in alltxt) {
					Atom atom = new Atom (s);
					alist.Add (atom);
				}
				List<string> slist = new List<string> ();
                foreach (Atom a in alist)
                {
                    slist.Add(a.ToString());
                }
				if (slist.Count > 0)
					File.WriteAllLines (mfilename, slist.ToArray (),Encoding.Default);
				else
					File.Delete (mfilename);
			}
		}
	}
}

