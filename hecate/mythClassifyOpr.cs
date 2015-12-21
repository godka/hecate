using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace hecate
{
    class mythClassifyOpr
    {
        private string mstr;
		private Dictionary<string,int> total_dict;
		private Dictionary<string,string[]> filter_list;
        public mythClassifyOpr(string str)
        {
            mstr = str;
			total_dict = new Dictionary<string, int> ();
			filter_list = new Dictionary<string, string[]> ();
			readfilters ();
        }
		private void readfilters()
		{
			var files = File.ReadLines ("./filter.txt");
			foreach (string s in files) {
				var spstr = s.Split (':');
				string mkey = spstr [0];
				List<string> tmplist = new List<string> ();
				var spstr2 = spstr [1].Split (',');
				foreach (string sp in spstr2) 
				{
					tmplist.Add (sp);
				}
				filter_list [mkey] = tmplist.ToArray();
			}
		}

		bool NewMethod (string s)
		{
			foreach (string key in filter_list.Keys) {
				var tmplist = filter_list [key];
				foreach (string t in tmplist) {
					if (s.Contains (t)) {
						//add great keys'
						if (!total_dict.ContainsKey (key))
							total_dict [key] = 0;
						total_dict [key] = total_dict [key] + int.Parse (s.Split (',') [1]);
						break;
					}
				}
			}
			return false;
		}

		private int SingleStep2(string path)
		{
			int t2 = 0;
			var files = File.ReadLines (path + "/data.csv",Encoding.GetEncoding("gb2312"));
			//List<string> listfile = new List<string> (files);

			foreach (string key in filter_list.Keys) {
				total_dict [key] = 0;
			}
			File.Delete (path + "/data.csv");
			StreamWriter sw = new StreamWriter(path + "/data.csv",false,Encoding.GetEncoding("gb2312"));
			foreach (string s in files) 
			{
				int t1 = 0;
				string keys = s.Split (',') [2];
				if (s.Contains ("错误") || keys.Trim().Equals("") || s.Contains("404") || s.Contains("302") || s.Contains("Error")
					|| s.Contains("对不起") || s.Contains("跳转") || s.Contains("LimeSurvey") || s.Contains("不存在")
					|| s.Contains(".") || s.Contains("出错") ||s.Contains("301")) {
					t1 = 1;
				}
				foreach (string key in filter_list.Keys) {
					var tmplist = filter_list [key];
					foreach (string t in tmplist) {
						if (s.Contains (t)) {
							//add great keys'
							if (!total_dict.ContainsKey (key))
								total_dict [key] = 0;
							total_dict [key] = total_dict [key] + int.Parse (s.Split (',') [1]);
							t1 = 1;
							t2++;
							break;
						}
					}
				}
				if(t1 == 0){
					sw.WriteLine(s);
					Global.remain++;
				}
			}
			//File.WriteAllLines (path + "/data.csv", files.ToArray (), Encoding.GetEncoding ("gb2312"));
			sw.Close();
			WriteFiles (path);
			return t2;
		}
		private void WriteFiles(string path){
			string mout = path + "/out.csv";
			if (File.Exists (mout)) {
				var tmpstrs = File.ReadLines (mout, Encoding.GetEncoding ("gb2312"));
				foreach (string s in tmpstrs) {
					var t = s.Split (',');
					if (!total_dict.ContainsKey (t [0])) {
						total_dict [t [0]] = 0;
					}
					total_dict [t [0]] = total_dict [t [0]] + int.Parse (t [1]);
				}
			}
			List<string> output = new List<string> ();
			foreach (string keys in total_dict.Keys) {
				output.Add (keys + "," + total_dict [keys].ToString ());
			}
			File.WriteAllLines (mout,output.ToArray (), Encoding.GetEncoding ("gb2312"));
		}
        private void SingleStep(string path)
        {
            var dirs = Directory.GetDirectories(path);
            List<string> slist = new List<string>();
            foreach (string s in dirs)
            {
                var files = Directory.GetFiles(s);
                foreach (string f in files)
                {
					var strs = File.ReadAllLines(f, Encoding.GetEncoding("gb2312"));
                    foreach (string s2 in strs)
                    {
                        var sps2 = s2.Split(',');
                        if (sps2.Length > 5)
                        {
                            var tmps = sps2[5].Split(':');
                            if (tmps.Length > 1)
                            {
                                if (tmps[0].Equals("ReadHtmlFailed") || tmps[0].Equals("Rejected"))
                                {
                                    // Console.WriteLine("Skip");
                                }
                                else
                                {
                                    slist.Add(string.Format("{0},{1},{2}", sps2[0], sps2[3], sps2[5]));
                                }
                            }
                            else
                            {
                                slist.Add(string.Format("{0},{1},{2}", sps2[0], sps2[3], sps2[5]));
                            }
                        }
                    }
                    File.Delete(f);
                }
                Directory.Delete(s);
            }
			File.WriteAllLines(path + "/data.csv", slist.ToArray(), Encoding.GetEncoding("gb2312"));
        }
        public int StartLoop()
        {
			int t = 0;
            if (mstr == string.Empty)
                return 0;
            var dirs = Directory.GetDirectories(mstr);
            foreach (string s in dirs)
            {
				t += SingleStep2(s);
				Console.WriteLine("write:" + s + "/data.csv");
			}
			return t;
        }
    }
}
