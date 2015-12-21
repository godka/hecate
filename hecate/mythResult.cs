using System;
using System.IO;
using System.Collections.Generic;
namespace mythTestR
{
	public class mythResult
	{
		string mstr;
		public mythResult (string str)
		{
			mstr = str;
		}
		public void StartLoop(){
			if(this.mstr == string.Empty){
				return;
			}
			if (!Directory.Exists ("./result")) {
				Directory.CreateDirectory ("./result");
			}
			mythKmeans kmeans = new mythKmeans ();
			if (File.Exists (mstr + "/outall.txt")) {
				var tmps = File.ReadAllLines (mstr + "/outall.txt", System.Text.Encoding.GetEncoding ("gb2312"));
				foreach (string s in tmps) {
					kmeans.Add (s);
				}
				/*
				StreamWriter sw = new StreamWriter ("./out2.txt");
				for (int i = 5; i < 33; i++) {
					var t = Environment.TickCount;
					sw.WriteLine (tmp.ToString ());
					Console.WriteLine (i.ToString() + ":" + (Environment.TickCount - t).ToString() + "ms:" + tmp.ToString());
				}
				*/
				//sw.Close ();

				var tmp = kmeans.StartLoop ();
				for (int i = 0; i < kmeans.Count; i++) {
					List<string> lst = new List<string> ();
					var retlist = kmeans.GetIndex (i);
					foreach (int t in retlist) {
						lst.Add (kmeans.GetName (t));
					}
					Console.WriteLine (retlist.Count.ToString ());
					File.WriteAllLines ("./result/" + i.ToString () + ".txt", lst.ToArray (), System.Text.Encoding.GetEncoding ("gb2312"));
					//Console.WriteLine ("write:" + "./result/" + i.ToString () + ".txt");
				}


			}
		}
	}
}

