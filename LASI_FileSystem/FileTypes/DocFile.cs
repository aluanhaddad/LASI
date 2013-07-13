﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LASI.FileSystem
{
    /// <summary>
    /// A strongly typed wrapper that encapsulates a legacy Word document (.doc).
    /// </summary>
    public sealed class DocFile : InputFile
    {
        /// <summary>
        /// Initializes a new instance of the DocFile class for the given path.
        /// </summary>
        /// <param name="absolutePath">The path to a .doc file.</param>
        /// <exception cref="FileTypeWrapperMismatchException">Thrown if the provided path does not end in the .doc extension.</exception>
        public DocFile(string absolutePath)
            : base(absolutePath) {
            if (this.Ext != ".doc" && this.Ext != ".DOC") {
                throw new LASI.FileSystem.FileTypeWrapperMismatchException(GetType().ToString(), Ext);
            }
        }

    }

}
