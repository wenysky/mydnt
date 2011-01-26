using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Discuz.EntLib.TokyoTyrant
{
    public class Query {

        List<string> _args = new List<string>();

        IList<QueryItem> _items = new List<QueryItem>();

        public IList<QueryItem> Items {
            get { return _items; }
        }

        void AddCondition(QueryOperation operation, string column, string expr) {
            _args.Add("addcond" + "\0" + column + "\0" + ((int)operation).ToString() + "\0" + expr);
        }

        public Query StringEquals(string column, string value) {
            AddCondition(QueryOperation.STREQ, column, value);
            return this;
        }

        public Query StringIn(string column, string value) {
            AddCondition(QueryOperation.STRINC, column, value);
            return this;
        }

        public Query StringStartsWith(string column, string value) {
            AddCondition(QueryOperation.STRBW, column, value);
            return this;
        }

        public Query StringEndsWith(string column, string value) {
            AddCondition(QueryOperation.STREW, column, value);
            return this;
        }

        public Query NumberEquals(string column, int value) {
            AddCondition(QueryOperation.NUMEQ, column, value.ToString());
            return this;
        }

        public Query NumberGreaterThan(string column, int value) {
            AddCondition(QueryOperation.NUMGT, column, value.ToString());
            return this;
        }

        public Query NumberGreaterThanOrEqual(string column, int value) {
            AddCondition(QueryOperation.NUMGE, column, value.ToString());
            return this;
        }

        public Query NumberLessThan(string column, int value) {
            AddCondition(QueryOperation.NUMLT, column, value.ToString());
            return this;
        }

        public Query NumberLessThanOrEqual(string column, int value) {
            AddCondition(QueryOperation.NUMLE, column, value.ToString());
            return this;
        }

        public Query OrderBy(string column, QueryOrder order) {
            _args.Add("setorder" + "\0" + column + "\0" + ((int)order).ToString());
            return this;
        }

        public Query LimitTo(int max, int skip) {
            _args.Add("setlimit" + "\0" + max.ToString() + "\0" + skip.ToString());
            return this;
        }

        public string[] GetArgs() {
            return _args.ToArray();           
        }


    }

    public class QueryItem {

        public QueryItem(QueryOperation operation, string column, string expression) {
            this.Operation = operation;
            this.Column = column;
            this.Expression = expression;
        }


        public QueryOperation Operation { get; set; }
        public string Column { get; set; }
        public string Expression { get; set; }

    }

}
