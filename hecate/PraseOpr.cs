using System;
using System.IO;
using System.Collections.Generic;
//select username from './data' where like='XX' and user_region='*(1980-1990),*(8000-12000),*(0-4)' and time_region='05/07-08/12' to tmp
//select like from './data' where username='XX' and user_region='*(1980-1990),*(8000-12000),*(0-4)' to tmp
namespace hecate
{
	public class PraseOpr
	{
		private string select_str = string.Empty;
		private string from_path = string.Empty;
		private string to_path = string.Empty;
		private bool startwhere;
		private bool and_or;
		private Dictionary<string,string> mywhere = null;
		private bool ismythSql;
		private string mstr = string.Empty;
		public PraseOpr (string str)
		{
			mywhere = new Dictionary<string, string> ();
			startwhere = false;
			and_or = false;
			mstr = str;
			ismythSql = Parse (str.Split (' '));
		}
		//private List<string> getTimeRegion(){
			
		//}
		public bool Export(){
			if (!ismythSql) {
				return false;	
			} else {
				return true;
			}
		}
		private bool Parse(string[] args){
			for (int i = 0; i < args.Length; i++) {
				switch (args [i]) {
				case "select":
					select_str = args [++i];
					break;
				case "from":
					from_path = args [++i];
					break;				
				case "where":
					startwhere = true;
					break;
				case "to":
					startwhere = false;
					to_path = args [++i];
					break;
				default:
					if (startwhere) {
						var splitstr = args[i].Split ('=');
						if (splitstr.Length == 2) {
							mywhere [splitstr [0]] = splitstr [1];
						}
					}
					break;
				}
			}
			if (this.select_str.Equals (string.Empty) ||
			    this.from_path.Equals (string.Empty) ||
			    this.to_path.Equals (string.Empty)) {
				return false;
			} else {
				return true;
			}
		}
	}
}

