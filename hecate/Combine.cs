using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
namespace hecate
{
	public class Combine
	{
		private string mstr = string.Empty;
		public Combine (string str)
		{
			mstr = str;
		}
		private bool SuperCompare(string a,string b){
			string[] split_a = a.Split (',');
			string[] split_b = b.Split (',');
			if (split_a.Length != split_b.Length)
				return false;
			for (int i = 1; i < split_a.Length; i++) {
				if (!split_a [i].Equals (split_b [i])) {
					return false;
				}
			}
			return true;
		}
		private void SingleStep(string file){
			string[] s = File.ReadAllLines (file,Encoding.Default);
			bool[] lists = new bool[s.Length];
			for (int i = 0; i < lists.Length; i++) {
				lists [i] = false;
			}
			if (s.Length > 1) {
				for (int i = 1; i < s.Length - 1; i++) {
					if (lists [i] == true)
						continue;
					for (int j = i + 1; j < s.Length - 1; j++) {
						if (SuperCompare (s [i], s [j]) == false) {
							Console.WriteLine (s [j]);
							lists [j] = true;
						}
					}
				}
				List<string> liststr = new List<string> ();
				for (int i = 0; i < s.Length; i++) {
					if (lists [i] == false) {
						liststr.Add (s[i]);
					}
				}
				File.WriteAllLines (file,liststr.ToArray (),Encoding.Default);
			}
		}
		public void StartLoop(){
			var files = Global.FindFile(mstr);
			foreach (string s in files) {
				SingleStep (s);
				Console.WriteLine ("Processing:" + s);
			}
		}
	}
}

