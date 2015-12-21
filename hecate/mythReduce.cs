using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace mythTestR
{
	public class mythReduce
	{
		private string mpath;
		private List<string> mlist;
		public mythReduce (string path)
		{
			mpath = path;
			mlist = new List<string> ();
		}
		private int SingleStep(string path){
			string name = Path.GetFileName(path);
			string ret = name + ":";
			if (File.Exists (path + "/out.csv")) {
				var files = File.ReadAllLines (path + "/out.csv", Encoding.GetEncoding ("gb2312"));
				for (int i = 0; i < files.Length; i++) {
					if (i != 0) {
						ret += ",";
					}	
					int tmp = int.Parse (files [i].Split (',') [1]);
					if (tmp > 10000000)
						tmp = 10000000;
					ret += tmp.ToString();
				}
			}
			mlist.Add (ret);
			return 0;
		}
		public int StartLoop()
		{
			int t = 0;
			if (mpath == string.Empty)
				return 0;
			var dirs = Directory.GetDirectories(mpath);
			foreach (string s in dirs)
			{
				t += SingleStep(s);
				Console.WriteLine("write:" + s.ToString());
			}
			File.WriteAllLines (mpath + "/outall.txt", mlist.ToArray(), Encoding.GetEncoding ("gb2312"));
			return t;
		}
	}
}

