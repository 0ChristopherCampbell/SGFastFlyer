using System;
using System.Collections.Generic;
using System.IO;


namespace SGFastFlyers.Utility
{

    /// <summary>
    /// IO class
    /// </summary> 
    public static class IO
        {
            /// <summary>
            /// Gets one filename from folder
            /// </summary>
            /// <param name="folderPath">folder path</param>
            /// <returns>file name</returns>
            public static string GetOneFileNameFromFolder(string folderPath)
            {
                return GetOneFileNameFromFolder(folderPath, "*");
            }

            /// <summary>
            /// Gets one filename from folder
            /// </summary>
            /// <param name="folderPath">folder path</param>
            /// <param name="fileName">file name filter</param>
            /// <returns>file name</returns>
            public static string GetOneFileNameFromFolder(string folderPath, string fileName)
            {
                List<string> acceptableExtensions = new List<string>();
                acceptableExtensions.Add("*");
                return GetOneFileNameFromFolder(folderPath, fileName, acceptableExtensions);
            }

            /// <summary>
            /// Gets one filename from folder
            /// </summary>
            /// <param name="folderPath">folder path</param>
            /// <param name="fileName">file name filter</param>
            /// <param name="acceptableExtensions">allowable extensions</param>
            /// <returns>file name</returns>
            public static string GetOneFileNameFromFolder(string folderPath, string fileName, List<string> acceptableExtensions)
            {
                string retStr = string.Empty;

                if (Directory.Exists(folderPath))
                {
                    DirectoryInfo di = new DirectoryInfo(folderPath);
                    if (fileName == string.Empty)
                    {
                        fileName = "*";
                    }

                    if (fileName == "*")
                    {
                        try
                        {
                            FileInfo[] theseFiles = di.GetFiles();
                            if (theseFiles.Length > 0)
                            {
                                foreach (FileInfo file in theseFiles)
                                {
                                    if (acceptableExtensions.Contains(file.Extension.ToLower()) || acceptableExtensions[0] == "*")
                                    {
                                        retStr = file.Name;
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            retStr = string.Empty;
                        }
                    }
                    else
                    {
                        if (File.Exists(folderPath + fileName))
                        {
                            retStr = fileName;
                        }
                    }

                    if (folderPath.Substring(folderPath.Length - 1, 1) != "\\")
                    {
                        folderPath = folderPath + "\\";
                    }

                    if (retStr != string.Empty && File.Exists(folderPath + retStr))
                    {
                        return retStr;
                    }

                    return string.Empty;
                }
                else
                {
                    return string.Empty;
                }
            }

            /// <summary>
            /// Path = physical folder on server path.
            /// </summary>
            /// <param name="path">folder path</param>
            public static void CreateFolder(string path)
            {
                FileInfo info = new FileInfo(path);
                if (info.Attributes != FileAttributes.Directory)
                {
                    path = info.Directory.FullName;
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            /// <summary>
            /// Deletes files in a folder folder.
            /// MAKE SURE YOU TEST THAT IT ACTUALLY DELETES WHAT YOU EXPECT!!!!
            /// Path = physical folder on server path.
            /// </summary>
            /// <param name="path">folder path</param>
            /// <returns>success indicator</returns>
            public static bool DeleteFilesInFolder(string path)
            {
                if (Directory.Exists(path))
                {
                    DirectoryInfo d = new DirectoryInfo(path);
                    foreach (FileInfo file in d.GetFiles())
                    {
                        file.Delete();
                    }

                    return true;
                }

                return true;
            }

            /// <summary>
            /// Deletes a folder.. Caters for where the folder is already deleted..
            /// MAKE SURE YOU TEST THAT IT ACTUALLY DELETES WHAT YOU EXPECT!!!!
            /// Path = physical folder on server path.
            /// </summary>
            /// <param name="path">folder path</param>
            /// <returns>success indicator</returns>
            public static bool DeleteFolder(string path)
            {
                if (Directory.Exists(path))
                {
                    if (DeleteFilesInFolder(path))
                    {
                        Directory.Delete(path);
                    }

                    return true;
                }

                return true;
            }

            /// <summary>
            /// Delete folder recursively
            /// </summary>
            /// <param name="targetDir">target folder</param>
            public static void DeleteFolderRecursive(string targetDir)
            {
                string[] files = Directory.GetFiles(targetDir);
                string[] dirs = Directory.GetDirectories(targetDir);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteFolderRecursive(dir);
                }

                try
                {
                    Directory.Delete(targetDir, false);
                }
                catch (Exception exception)
                {
                   //
                }
            }

            /// <summary>
            /// Makes filename valid
            /// </summary>
            /// <param name="name">proposed filename</param>
            /// <returns>corrected filename</returns>
            public static string MakeFileNameValid(string name)
            {
                string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
                string invalidReStr = string.Format(@"[{0}]", invalidChars);
                return System.Text.RegularExpressions.Regex.Replace(name, invalidReStr, "_");
            }
        }
    }
 