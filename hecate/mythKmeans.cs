using System;
using System.Collections.Generic;
namespace mythTestR
{
	public class mythKmeans
	{
		private List<points> lp;// = new List<points> (); //n条数据
		private List<List<int>> clusters;// = new List<List<int>>();
		//add like this
		//XXXX:1,2,3,4,5
		public void Add(string SingleLine){
			var tmps = SingleLine.Split (':');
			string tmpname = string.Empty;
			List<double> lst = new List<double> ();
			if (tmps.Length > 1) {
				tmpname = tmps [0];
				string s = tmps [1];
				var sps = s.Split (',');
				foreach (string t in sps) {
					lst.Add (double.Parse (t));
				}
			}
			Add (tmpname, lst.ToArray ());
		}
		public void Add(string fName,params double[] data){
			lp.Add (new points (fName,data));
		}
		private bool CheckData(){
			if (lp.Count > 0) {
				int count = lp [0].GetCount ();
				foreach (points p in lp) {
					if (p.GetCount () != count)
						return false;
				}
				return true;
			}
			return false;
		}
		public int Count{
			get{
				return clusters.Count;
			}
		}
		public String GetName(int index){
			return lp [index].Name.ToString ();
		}
		public List<int> GetIndex(int index){
			return clusters [index];
		}
		public double StartLoop(){
			return StartLoop ((int)Math.Sqrt (lp.Count) + 1);
		}
		public double StartLoop(int k)
		{
			int n = lp.Count;
			if (n == 0)
				return 0;
			if (!CheckData ())
				return 0;
			points[] centers = new points[k]; //k个簇的中心
			points[] preCenters = new points[k];//前一次计算的簇中心

			for (int i = 0; i < k; i++) { //取前k个对象作为初始的簇中心
				centers [i] = new points(lp[i]);
			}
			bool stop = false;
			while (!stop) {
				clusters.Clear();
				for (int i = 0; i < k; i++) {
					clusters.Add (new List<int> ());
				}
				for (int i = 0; i < n; i++) {
					double distance = lp [i].DistanceBetween (centers [0]);
					double temp;
					int num = 0;

					for (int j = 0; j < k; j++) {
						if ((temp = lp [i].DistanceBetween (centers [j])) < distance) { 
							num = j; 
							distance = temp; 
						}
					}
					clusters [num].Add(i);
				}
				for (int i = 0; i < k; i++) {
					preCenters [i] = new points (centers[i]);//保存先前的簇中心
				}

				for (int i = 0; i < k; i++) {
					centers [i].Reset ();
					if (clusters [i] != null) {
						//string[] pointsNum = clusters [i].Split(',');
						var pointsNum = clusters[i];
						for (int j = 0; j < pointsNum.Count; j++) {
							for (int index = 0; index < centers [i].GetCount (); index++) {
								centers [i].mlists[index] = centers [i].mlists[index] + lp [pointsNum [j]].mlists[index];
							}
						}
						for (int index = 0; index < centers [i].GetCount (); index++) {
							centers [i].mlists [index] = (double)centers [i].mlists [index] / (pointsNum.Count);
						}
					}
				}
				stop = true;
				for (int i = 0; i < k; i++) {
					for (int index = 0; index < centers [i].GetCount (); index++) {
						if(preCenters[i].mlists[index] != centers [i].mlists[index]){
							//Console.WriteLine ("working...");
							stop = false;
							break;
						}
					}
				}
			}
			//min distance
			double m_distance = 0;
			for (int i = 0; i < k - 1; i++) {
				m_distance += centers [i].DistanceBetween (centers [i + 1]);
			}
			m_distance /= k;
			return m_distance;
		}

		public class points:ICloneable
		{
			public string Name;
			public List<double> mlists;
			public points Clone(){
				return (points)this.MemberwiseClone ();
			}
			public int Reset(){
				for (int i = 0; i < mlists.Count; i++) {
					mlists [i] = 0;
				}
				return 0;
			}

			object ICloneable.Clone() 
			{ 
				return this.Clone(); 
			} 
			public int GetCount(){
				return mlists.Count;
			}
			public points(List<double> lst)
			{
				mlists = new List<double>(lst);
			}
			public points(string fname,params double[] t){
				Name = fname;
				mlists = new List<double>();
				foreach(double d in t){
					mlists.Add(d);
				}
			}
			public points(points p){
				mlists = new List<double>(p.mlists);
			}
			public points(params double[] t)
			{
				mlists = new List<double>();
				foreach(double d in t){
					mlists.Add(d);
				}
			}
			public double DistanceBetween(points p2)
			{
			//double distance;
				double ret = 0;
				for (int i = 0; i < mlists.Count; i++) {
					ret += Math.Pow (mlists [i] - p2.mlists [i], 2.0);
				}
				return Math.Sqrt(ret);
			}
			public override string ToString ()
			{
				string tmp = "(";
				int s = 0;
				foreach (double t in mlists) {
					if (s == 0) {
						s = 1;
						tmp += t.ToString ();
					}else
						tmp += "," + t.ToString ();
				}
				tmp+=")";
				return tmp;
			}
		}
		public mythKmeans ()
		{
			lp = new List<points> ();
			clusters = new List<List<int>> ();
		}
	}
}

