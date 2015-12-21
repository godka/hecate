using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace hecate
{
	public static class Global
	{
		public static int remain = 0;
		public static List<string> FindFile(string sSourcePath)
		{
			List<String> list = new List<string>();
			//遍历文件夹
			DirectoryInfo theFolder = new DirectoryInfo(sSourcePath);
			FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
			foreach (FileInfo NextFile in thefileInfo)  //遍历文件
				list.Add(NextFile.FullName);
			//遍历子文件夹
			DirectoryInfo[] dirInfo = theFolder.GetDirectories();
			foreach (DirectoryInfo NextFolder in dirInfo)
			{
				FileInfo[] fileInfo = NextFolder.GetFiles("*.*", SearchOption.AllDirectories);
				foreach (FileInfo NextFile in fileInfo)  //遍历文件
					list.Add(NextFile.FullName);
			}
			return list;
		}
	}
}

