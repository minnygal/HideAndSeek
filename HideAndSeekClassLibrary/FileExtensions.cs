﻿using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HideAndSeek
{
    /// <summary>
    /// Extension class for IFileSystem interface to store JSON file extension
    /// and methods to get full file name and validate file name
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Extension for JSON file (including period)
        /// </summary>
        public static readonly string JsonFileExtension = ".json";

        /// <summary>
        /// Get full name for JSON file (including extension)
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="fileNameWithoutExtension">Name of file not including extension</param>
        /// <returns>Full name of file including extension</returns>
        /// <exception cref="ArgumentException">Exception thrown if file name is invalid</exception>
        public static string GetFullFileNameForJson(this IFileSystem fileSystem, string fileNameWithoutExtension)
        {
            return GetFullFileNameForJson(fileNameWithoutExtension);
        }

        /// <summary>
        /// Get full name for file (including extension)
        /// </summary>
        /// <param name="fileNameWithoutExtension">Name of file not including extension</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Exception thrown if file name is invalid</exception>
        public static string GetFullFileNameForJson(string fileNameWithoutExtension)
        {
            // If file name is invalid
            if( !(IsValidName(fileNameWithoutExtension)) )
            {
                throw new ArgumentException($"Cannot perform action because file name \"{fileNameWithoutExtension}\" is invalid " +
                                             "(is empty or contains illegal characters, e.g. \\, /, or whitespace)", nameof(fileNameWithoutExtension));
            }

            // Return file name with extension
            return $"{fileNameWithoutExtension}{JsonFileExtension}";
        }

        /// <summary>
        /// Return whether file name is valid (not empty and no illegal characters)
        /// </summary>
        /// <param name="fileSystem"></param>
        /// <param name="name">File name to evaluate</param>
        /// <returns>Whether file name is valid</returns>
        public static bool IsValidName(this IFileSystem fileSystem, string name)
        {
            return IsValidName(name);
        }

        /// <summary>
        /// Return whether file name is valid (not empty and no illegal characters)
        /// </summary>
        /// <param name="name">File name to evaluate</param>
        /// <returns>Whether file name is valid</returns>
        public static bool IsValidName(string name)
        {
            // Return whether file name does NOT contain whitespace, a backslash, or a forward slash
            return !(name.Contains(' ') || name.Contains('\\') || name.Contains('/') || name.Equals(""));
        }
    }
}
