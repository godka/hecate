using System;
using System.IO;
using System.Collections.Generic;
namespace hecate
{
	public class appendall
	{
		private Dictionary<string,int> dict;
		private string mstr;
		public appendall (string str)
		{
			dict = new Dictionary<string, int> ();
			mstr = str;
		}
		private void StartSort(){
			Console.WriteLine ("Sorting...");
			List<KeyValuePair<string,int>> mylist = new List<KeyValuePair<string, int>> (dict);
			mylist.Sort (delegate(KeyValuePair<string, int> x, KeyValuePair<string, int> y) {
				return y.Value.CompareTo (x.Value);
			});
			for (int i = 0; i < 100; i++) {
				Console.WriteLine ((i+1).ToString() + ":" + mylist [i].Key + ":" + mylist [i].Value);
			}
		}
		public void StartLoop(){
			var dirs = Directory.GetDirectories (mstr);
			foreach (string s in dirs) {
				if (File.Exists (s + "/append.txt")) {
					var str = File.ReadLines (s + "/append.txt");
					foreach (string ps in str) {
						var splitstr = ps.Split (':');
						if (splitstr.Length > 1) {
							string key = splitstr [0];
							int value = int.Parse(splitstr [1]);
							if (dict.ContainsKey (key)) {
								dict [key] += value;
							} else {
								dict [key] = value;
							}
						}
					}
					Console.WriteLine (s);
				}
			}
			StartSort ();
			//for (int i = 0; i < 100; i++) {
			//	Console.WriteLine
			//}
		}
	}
}

