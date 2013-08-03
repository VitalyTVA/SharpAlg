using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SharpAlg.Native.Printer {
    [JsType(JsMode.Prototype, Filename = SR.JSPrinterName)]
    public class ExpressionWrapperVisitor : IExpressionVisitor<bool> {
        readonly ExpressionOrder order;
        readonly OperationPriority priority;
        public ExpressionWrapperVisitor(OperationPriority priority, ExpressionOrder order) {
            this.order = order;
            this.priority = priority;
        }
        public bool Constant(ConstantExpr constant) {
            if(order == ExpressionOrder.Head)
                return false;
            return constant.Value < Number.Zero;
        }
        public bool Parameter(ParameterExpr parameter) {
            return false;
        }
        public bool Add(AddExpr multi) {
            return priority >= OperationPriority.Add;
        }
        public bool Multiply(MultiplyExpr multi) {
            if(UnaryExpressionExtractor.IsMinusExpression(multi))
                return true;
            return priority >= OperationPriority.Multiply;
        }
        public bool Power(PowerExpr power) {
            if(UnaryExpressionExtractor.IsInverseExpression(power))
                return priority >= OperationPriority.Multiply;
            return priority == OperationPriority.Power;
        }
    }
}
