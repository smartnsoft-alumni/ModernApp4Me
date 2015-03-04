﻿// Copyright (C) 2015 Smart&Soft SAS (http://www.smartnsoft.com/) and contributors
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 3 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// Contributors:
//   Smart&Soft - initial API and implementation

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace ModernApp4Me.WP8.Cache
{

    /// <summary>
    /// Enables to store some contents into the <see cref="IsolatedStorageFile"/>.
    /// The classe implements the singleton pattern and is thread safe !
    /// </summary>
    ///
    /// <author>Ludovic ROLAND</author>
    /// <since>2014.03.24</since>
    public sealed class M4MFilePersistence
    {
        private static volatile M4MFilePersistence instance;

        private static readonly object InstanceLock = new Object();

        private M4MFilePersistence()
        {
        }

        public static M4MFilePersistence Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (InstanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new M4MFilePersistence();
                        }
                    }
                }

                return instance;
            }
        }
        
        /// <summary>
        /// Reads a file from the Isolated StorageFile.
        /// </summary>
        /// <param name="fileName">the name of the file to read</param>
        /// <returns>the content of the file in case of success, null otherwise</returns>
        public string ReadFile(string fileName)
        {
            lock (InstanceLock)
            {
                string fileContent = null;

                try
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isf.FileExists(fileName) == true)
                        {
                            using (var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isf))
                            {
                                using (var streamReader = new StreamReader(fileStream))
                                {
                                    fileContent = streamReader.ReadToEnd();
                                }
                            }
                        }
                        else
                        {

                            fileContent = null;
                        }
                    }
                }
                catch (Exception)
                {
                    fileContent = null;
                }

                return fileContent;
            }
        }

        /// <summary>
        /// Writes a <see cref="string"/> into a file located into the Isolated Storage.
        /// </summary>
        /// <param name="fileName">the file name</param>
        /// <param name="fileContent">the file content</param>
        /// <param name="fileMode">the <see cref="FileMode"/></param>
        /// <returns>true in case of success. false otherwise</returns>
        public bool WriteFile(string fileName, string fileContent, FileMode fileMode)
        {
            lock (InstanceLock)
            {
                var isWritten = true;

                try
                {
                    //The file name includes the directory
                    if (fileName.Split('/').Length > 1)
                    {
                        CreateDirectoryFromFilePath(fileName);
                    }

                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var fileStream = new IsolatedStorageFileStream(fileName, fileMode, isf))
                        {
                            using (var streamWriter = new StreamWriter(fileStream))
                            {
                                streamWriter.Write(fileContent);
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    isWritten = false;
                }

                return isWritten;
            }
        }

        /// <summary>
        /// Deletes a file from the Isolated Storage.
        /// </summary>
        /// <param name="fileName">the name of the file to delete.</param>
        /// <returns>true in case of success. false otherwise</returns>
        public bool DeleteFile(string fileName)
        {
            lock (InstanceLock)
            {
                var isDeleted = true;

                try
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isf.FileExists(fileName) == true)
                        {
                            isf.DeleteFile(fileName);
                        }
                        else
                        {
                            isDeleted = false;
                        }
                    }
                }
                catch (Exception)
                {
                    isDeleted = false;
                }

                return isDeleted;
            }
        }

        /// <summary>
        /// Cleans-up the Isolated Storage
        /// </summary>
        /// <returns>true in case of success. false otherwise</returns>
        public bool CleanupIsolatedStorage()
        {
            lock (InstanceLock)
            {
                var isCleaned = true;

                try
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        isf.Remove();
                    }
                }
                catch (Exception)
                {
                    isCleaned = false;
                }

                return isCleaned;
            }
        }

        /// <summary>
        /// Checks if a file exists into the Isolated Storage
        /// </summary>
        /// <param name="fileName">the name of the file</param>
        /// <returns>true if the file exists, false otherwise</returns>
        public bool IsFileExists(string fileName)
        {
            lock (InstanceLock)
            {
                var isFileExists = true;

                try
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        isFileExists = isf.FileExists(fileName);
                    }
                }
                catch (Exception)
                {
                    isFileExists = false;
                }

                return isFileExists;
            }
        }

        /// <summary>
        /// Creates a directory into the Isolated Storage.
        /// </summary>
        /// <param name="directoryName">the name of the directory</param>
        /// <returns>true in case of success. false otherwise</returns>
        public bool CreateDirectory(string directoryName)
        {
            lock (InstanceLock)
            {
                var isSucceed = true;

                try
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        isf.CreateDirectory(directoryName);
                    }
                }
                catch (Exception)
                {
                    isSucceed = false;
                }

                return isSucceed;
            }
        }

        /// <summary>
        /// Creates a directory from a complete path into the Isolated Storage
        /// </summary>
        /// <param name="filePath">the complete path</param>
        /// <returns>true in case of success, false otherwise</returns>
        public bool CreateDirectoryFromFilePath(string filePath)
        {
            lock (InstanceLock)
            {
                var isSucceed = true;

                try
                {
                    //Building the directory path
                    var directoryPathTab = filePath.Split('/');
                    var directoryPath = new StringBuilder("/");

                    for (var i = 0; i < directoryPathTab.Length - 1; i++)
                    {
                        directoryPath.Append(directoryPathTab[i]);
                        directoryPath.Append('/');
                    }

                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        isf.CreateDirectory(directoryPath.ToString());
                    }
                }
                catch (Exception)
                {
                    isSucceed = false;
                }

                return isSucceed;
            }
        }

        /// <summary>
        /// Reads a binary contents from a file into the Isolated Storage
        /// </summary>
        /// <param name="fileName">the file to read</param>
        /// <returns>the content in case of success, null otherwise</returns>
        public MemoryStream ReadBinary(string fileName)
        {
            lock (InstanceLock)
            {
                MemoryStream stream;

                try
                {
                    using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (isf.FileExists(fileName) == true)
                        {
                            using (var fileStream = isf.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                            {
                                stream = new MemoryStream();
                                fileStream.CopyTo(stream);
                                fileStream.Close();
                                stream.Position = 0;
                            }
                        }
                        else
                        {
                            stream = null;
                        }
                    }
                }
                catch (Exception)
                {
                    stream = null;
                }

                return stream;
            }
        }
    }
}