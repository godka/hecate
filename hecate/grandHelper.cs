using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
namespace hecate
{
	public class grandHelper
	{
		private Dictionary<string,int> fakeredis = null;
		private static grandHelper mgrandHelper = null;
		public static grandHelper GetInstance(){
			if(mgrandHelper == null){
				mgrandHelper = new grandHelper();
			}
			return mgrandHelper;
		}
		public grandHelper ()
		{
			fakeredis = new Dictionary<string, int> ();
		}
		public int FindValue(string username,string date,string like){
			return FindValue ("./data", username, date, like);
		}
		public int FindValue(string root,string username,string date,string like){
			//simulate like redis
			string str = username + ":" + date + ":" + like;
			if (fakeredis.ContainsKey (str)) {
				return fakeredis [str];
			} else {
				if (Directory.Exists (String.Format ("{0}/{1}/{2}/", root,username, date))) {
					var lines = File.ReadAllLines (String.Format ("{0}/{1}/{2}/{3}", root,username, date, "single_append.log"));
					foreach (string l in lines) {
						var splitstr = l.Split (':');
						if (splitstr.Length > 1) {
							if (splitstr [0].Equals (like)) {
								int t = int.Parse (splitstr [1]);
								fakeredis [str] = t;
								return t;
							}
						}
					}
					return 0;
				} else {
					fakeredis [str] = 0;
					return 0;
				}
			}

		}
	}
}

