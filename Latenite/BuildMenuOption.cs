using System;
using System.Collections.Generic;
using System.Text;

namespace Latenite {
    /// <summary>
    /// Build menu option class; add this to a drop down. It holds the filename in Path.
    /// </summary>
    public class BuildMenuOption {
        /// <summary>
        /// The path to the build script
        /// </summary>
        public string Path = "";
        /// <summary>
        /// Return the name of the path
        /// </summary>
        /// <returns>Just the filename, minus extension.</returns>
        public override string ToString() {
            return System.IO.Path.GetFileNameWithoutExtension(this.Path);
        }
        /// <summary>
        /// Create a new instance of the BuildMenuOption class
        /// </summary>
        /// <param name="Path">Path to the build script</param>
        public BuildMenuOption(string Path) {
            this.Path = Path;
        }
    }

}
