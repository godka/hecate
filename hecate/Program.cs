//select * from data where name="AAAAAAAA"
//select time,like where data="XXXXXXXXXX" to tmp
//graphic tmp as width=xx,height=xx
//date,username,like,score:in redis, key[username:date:like] = score
//so we just bind one unknown symbols,it will like that key[bind:date:like]=score
//so the result must be x=date ,y = score,and z = like num
//x must be binded as date,and y must be binded as score,but z can be much more answers.
//usernameA	 05-07 05-08 05-09 ...
//like
//like
//like
//like
//...
//or
//like		 05-07 	05-08 	05-09 ...
//usernameA
//usernameB
//usernameC
//usernameD
//...
//select username from './data' whemre like = 'XX' and user_region='*(1980-1990),*(8000-12000),*(0-4)' and time_region = '05-07 to 08-12' to tmp
//select like from './data' where username = 'XX' or user_region= '*(1980-1990),*(8000-12000),*(0-4)' to tmp

using System;
using System.Collections.Generic;
using System.IO;
using mythGraStepFour;
using mythGraStepTwo;
using mythTestR;
namespace hecate
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			for (;;) {
				Console.Write (">");
				string str = Console.ReadLine ();
				int t = Environment.TickCount;
				var splitstr = str.Split (' ');
				switch (splitstr [0]) {
				case "appendall":
					appendall (splitstr);
					break;
				case "append":
					append (splitstr);
					break;
				case "sort":
					sort (splitstr);
					break;
				case "combine":
					combine (splitstr);
					break;
				default:
					Console.WriteLine("Expression not found");
					break;
				case "exit":
					Environment.Exit (0);
					break;
				case "quit":
					Environment.Exit (0);
					break;
				case "k-means":
					kmeans (splitstr);
					break;
				case "classify":
					mythClassifyOpr (splitstr);
					break;
				}
				Console.WriteLine ("All ok," + (Environment.TickCount - t).ToString() + "ms");

			}
			//PraseOpr prase = new PraseOpr ("select username from './data' where like='XX' and user_region='*(1980-1990),*(8000-12000),*(0-4)' and time_region='05/07-08/12' to tmp");
		//	Console.WriteLine ("Hello World!");
		}
		public  static void kmeans(string[] args){
			if(args.Length > 2){
				mythResult result = new mythResult(args[1]);
				result.StartLoop();
			}
		}

		public  static void mythClassifyOpr(string[] args){
			if(args.Length > 1){
				mythClassifyOpr result = new mythClassifyOpr(args[1]);
				result.StartLoop();
			}
		}
		public static void combine(string[] args){
			if (args.Length > 1) {
				Combine com = new Combine (args [1]);
				com.StartLoop ();
			}
		}
		public static void appendall(string[] args){
			if (args.Length > 1) {
				appendall al = new hecate.appendall (args [1]);
				al.StartLoop ();
			}
		}
		public static void append(string[] args){
			if (args.Length > 1) {
				if (Directory.Exists (args [1])) {
					var mlist = Directory.GetDirectories (args [1]);
					foreach (string str in mlist) {
						analyzer ana = new analyzer (str);
						ana.StartLoop ();
						Console.WriteLine ("write success:" + str);
					}
				} else {
					Console.WriteLine ("Directory not found or no Directory");
				}
			}
		}
		public static void sort(string[] args){		
			if (args.Length > 1) {
				if (Directory.Exists (args [1])) {
					List<string> mlist = Global.FindFile (args [1]);
					foreach (string str in mlist) {
						mythSortOpr mythsortopr = new mythSortOpr (str);
						if (args.Length > 2) {
							mythsortopr.StartLoop (int.Parse (args [2]));
						} else {
							mythsortopr.StartLoop ();
						}
						Console.WriteLine ("write success:" + str);
						//string dir = Path.GetDirectoryName(str);
						//if(Directory.GetFiles(dir).Length == 0){
						//	Directory.Delete(dir);
						//}
					}
				}
			} else {
				Console.WriteLine ("Directory not found or no Directory");
			}
		}
	}
}