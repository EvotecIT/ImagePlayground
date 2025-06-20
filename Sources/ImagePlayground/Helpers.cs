﻿using System.Diagnostics;
using System.IO;

namespace ImagePlayground;
public static partial class Helpers {
    /// <summary>
    /// Converts Color to Hex Color
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static string ToHexColor(this SixLabors.ImageSharp.Color c) {
        return c.ToHex().Remove(6);
    }

    /// <summary>
    /// Opens up any file using assigned Application
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="open"></param>
    public static void Open(string filePath, bool open) {
        if (open) {
            ProcessStartInfo startInfo = new ProcessStartInfo(filePath) {
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }
    }

    public static bool IsFileLocked(this FileInfo file) {
        try {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None)) {
                stream.Close();
            }
        } catch (IOException) {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }

    public static bool IsFileLocked(this string fileName) {
        try {
            var file = new FileInfo(fileName);
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None)) {
                stream.Close();
            }
        } catch (IOException) {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }
}
