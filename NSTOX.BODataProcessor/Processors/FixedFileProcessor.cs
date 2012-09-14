using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;
using System.IO;

namespace NSTOX.BODataProcessor.Processors
{
    public class FixedFileProcessor<T>
    {
        public static T[] ProcessFile(string filePath)
        {
            FileHelperEngine engine = new FileHelperEngine(typeof(T));
            T[] items = engine.ReadFile(filePath) as T[];
            return items;
        }

        public static T[] ProcessString(string fileContent)
        {
            FileHelperEngine engine = new FileHelperEngine(typeof(T));
            T[] items = engine.ReadString(fileContent) as T[];
            return items;
        }
    }
}
