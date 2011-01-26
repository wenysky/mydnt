using System;
using System.Collections.Generic;
using System.Text;

namespace Discuz.EntLib.SphinxClient
{
    public class SphinxWordInfo
    {
        /** Word form as returned from search daemon, stemmed or otherwise postprocessed. */
        public String word;

        /** Total amount of matching documents in collection. */
        public long docs;

        /** Total amount of hits (occurences) in collection. */
        public long hits;

        /** Trivial constructor. */
        public SphinxWordInfo(String word, long docs, long hits)
        {
            this.word = word;
            this.docs = docs;
            this.hits = hits;
        }
    }
}
