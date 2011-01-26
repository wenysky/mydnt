using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Discuz.EntLib.SphinxClient
{
    public class SphinxMatch
    {
        /** Matched document ID. */
        public long docId;

        /** Matched document weight. */
        public int weight;

        /** Matched document attribute values. */
        public ArrayList attrValues;

        /** Trivial constructor. */
        public SphinxMatch(long docId, int weight)
        {
            this.docId = docId;
            this.weight = weight;
            this.attrValues = new ArrayList();
        }
    }
}
