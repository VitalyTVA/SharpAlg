using SharpKit.JavaScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SharpAlg.Native {
    [JsType(JsMode.Clr, Filename = SR.JSNativeName)]
    public interface IExpressionVisitor<T> {
        T Constant(ConstantExpr constant);
        T Parameter(ParameterExpr parameter);
        T Binary(BinaryExpr binary);
        T Power(PowerExpr power);
        T Unary(UnaryExpr unary);
    }
}
