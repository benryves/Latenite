using System;
using System.Collections.Generic;
using System.Text;

namespace Latenite {
    public partial class LateniteIDE {
        List<WatchRecord> WatchRecords = new List<WatchRecord>();

        private class WatchRecord {
            public string Name;
            public string Value;
            public WatchRecord(string Name) {
                this.Name = Name;
                this.Value = "0";
            }
        }

    }
}
