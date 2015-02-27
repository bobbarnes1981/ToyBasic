using System;
using System.Collections.Generic;
using System.IO;
using Basic.Errors;

namespace Basic
{
    public class FileStorage : IStorage
    {
        public void Save(string filename, IEnumerable<string> lines)
        {
            try
            {
                File.WriteAllLines(filename, lines);
            }
            catch(Exception exception)
            {
                throw new StorageError(exception.Message);
            }
        }

        public string[] Load(string filename)
        {
            try
            {
                return File.ReadAllLines(filename);
            }
            catch (Exception exception)
            {
                throw new StorageError(exception.Message);
            }
        }
    }
}
