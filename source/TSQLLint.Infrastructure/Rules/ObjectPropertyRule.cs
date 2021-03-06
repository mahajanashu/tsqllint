using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using TSQLLint.Core.Interfaces;

namespace TSQLLint.Infrastructure.Rules
{
    public class ObjectPropertyRule : TSqlFragmentVisitor, ISqlRule
    {
        private readonly Action<string, string, int, int> errorCallback;

        public ObjectPropertyRule(Action<string, string, int, int> errorCallback)
        {
            this.errorCallback = errorCallback;
        }

        public string RULE_NAME => "object-property";

        public string RULE_TEXT => "Expected use of SYS.COLUMNS rather than ObjectProperty function";

        public int DynamicSqlStartColumn { get; set; }

        public int DynamicSqlStartLine { get; set; }

        public override void Visit(FunctionCall node)
        {
            if (node.FunctionName.Value.Equals("OBJECTPROPERTY", StringComparison.OrdinalIgnoreCase))
            {
                errorCallback(RULE_NAME, RULE_TEXT, node.StartLine, GetColumnNumber(node));
            }
        }

        private int GetColumnNumber(TSqlFragment node)
        {
            return node.StartLine == DynamicSqlStartLine
                ? node.StartColumn + DynamicSqlStartColumn
                : node.StartColumn;
        }
    }
}
