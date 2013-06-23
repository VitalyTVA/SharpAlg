/*Generated by SharpKit 5 v5.01.1000*/
if (typeof($CreateException)=='undefined') 
{
    var $CreateException = function(ex, error) 
    {
        if(error==null)
            error = new Error();
        if(ex==null)
            ex = new System.Exception.ctor();       
        error.message = ex.message;
        for (var p in ex)
           error[p] = ex[p];
        return error;
    }
}
if (typeof ($CreateAnonymousDelegate) == 'undefined') {
    var $CreateAnonymousDelegate = function (target, func) {
        if (target == null || func == null)
            return func;
        var delegate = function () {
            return func.apply(target, arguments);
        };
        delegate.func = func;
        delegate.target = target;
        delegate.isDelegate = true;
        return delegate;
    }
}
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var SharpAlg$Tests$ExprTests =
{
    fullname: "SharpAlg.Tests.ExprTests",
    baseTypeName: "System.Object",
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        },
        ParameterExprTest: function ()
        {
            SharpAlg.Tests.FluentAssert.IsFalse$1$$TInput$$Func$2(SharpAlg.Native.ParameterExpr.ctor, SharpAlg.Tests.FluentAssert.IsTrue$1$$TInput$$Func$2(SharpAlg.Native.ParameterExpr.ctor, SharpAlg.Tests.FluentAssert.Fails$1(SharpAlg.Native.ParameterExpr.ctor, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.ParameterExpr.ctor, SharpAlg.Native.Expr.Parameter("x"), $CreateAnonymousDelegate(this, function (x)
            {
                return x.get_ParameterName();
            }), "x"), $CreateAnonymousDelegate(this, function (x)
            {
                SharpAlg.Native.ExpressionExtensions.Evaluate(x, null);
            }), Typeof(SharpAlg.Native.ExpressionEvaluationException.ctor), $CreateAnonymousDelegate(this, function (e)
            {
                SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(System.Exception.ctor, e, $CreateAnonymousDelegate(this, function (x)
                {
                    return x.get_Message();
                }), "x value is undefined");
            })), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x, SharpAlg.Native.Expr.Parameter("x"));
            })), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x, SharpAlg.Native.Expr.Parameter("y"));
            }));
        },
        ConstantExprTest: function ()
        {
            SharpAlg.Tests.FluentAssert.IsFalse$1$$TInput$$Func$2(SharpAlg.Native.ConstantExpr.ctor, SharpAlg.Tests.FluentAssert.IsTrue$1$$TInput$$Func$2(SharpAlg.Native.ConstantExpr.ctor, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.ConstantExpr.ctor, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.ConstantExpr.ctor, SharpAlg.Native.Expr.Constant(9), $CreateAnonymousDelegate(this, function (x)
            {
                return x.get_Value();
            }), 9), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, null);
            }), 9), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x, SharpAlg.Native.Expr.Constant(9));
            })), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x, SharpAlg.Native.Expr.Constant(13));
            }));
        },
        BinaryExprTest: function ()
        {
            var left = SharpAlg.Native.Expr.Constant(9);
            var right = SharpAlg.Native.Expr.Parameter("x");
            var expr = SharpAlg.Native.Expr.Binary(left, right, 3);
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, expr, $CreateAnonymousDelegate(this, function (x)
            {
                return x.get_Left();
            }), left), $CreateAnonymousDelegate(this, function (x)
            {
                return x.get_Right();
            }), right), $CreateAnonymousDelegate(this, function (x)
            {
                return x.get_Operation();
            }), 3);
            var left2 = SharpAlg.Native.Expr.Constant(9);
            var right2 = SharpAlg.Native.Expr.Parameter("x");
            var expr2 = SharpAlg.Native.Expr.Binary(left, right, 3);
            var expr3 = SharpAlg.Native.Expr.Binary(right, left, 3);
            var expr4 = SharpAlg.Native.Expr.Binary(left, right, 0);
            SharpAlg.Tests.FluentAssert.IsFalse$1$$TInput$$Func$2(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Tests.FluentAssert.IsFalse$1$$TInput$$Func$2(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Tests.FluentAssert.IsTrue$1$$TInput$$Func$2(SharpAlg.Native.BinaryExpr.ctor, expr, $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x, expr2);
            })), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x, expr3);
            })), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x, expr4);
            }));
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(9), SharpAlg.Native.Expr.Constant(13), 0), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, null);
            }), 22);
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(9), SharpAlg.Native.Expr.Constant(13), 1), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, null);
            }), -4);
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(10), SharpAlg.Native.Expr.Constant(5), 3), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, null);
            }), 2);
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(9), SharpAlg.Native.Expr.Constant(13), 2), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, null);
            }), 117);
        },
        ParameterExprEvaluationTest: function ()
        {
            var context = new SharpAlg.Native.Context.ctor();
            context.Register("x", SharpAlg.Native.Expr.Constant(9));
            context.Register("y", SharpAlg.Native.Expr.Constant(13));
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.ParameterExpr.ctor, SharpAlg.Native.Expr.Parameter("x"), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, context);
            }), 9);
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.ParameterExpr.ctor, SharpAlg.Native.Expr.Parameter("y"), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, context);
            }), 13);
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Parameter("x"), SharpAlg.Native.Expr.Parameter("y"), 0), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, context);
            }), 22);
            context.Register("y", SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Parameter("x"), SharpAlg.Native.Expr.Parameter("x"), 2));
            SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.BinaryExpr.ctor, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Parameter("x"), SharpAlg.Native.Expr.Parameter("y"), 0), $CreateAnonymousDelegate(this, function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x, context);
            }), 90);
        }
    }
};
JsTypes.push(SharpAlg$Tests$ExprTests);
var SharpAlg$Tests$FluentAssert =
{
    fullname: "SharpAlg.Tests.FluentAssert",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        IsNull$1: function (TInput, obj, valueEvaluator)
        {
            NUnit.Framework.Assert.IsNull$$Object(SharpAlg.Tests.FluentAssert.GetActualValue$1(TInput, obj, valueEvaluator));
            return obj;
        },
        IsNotNull$1: function (TInput, obj, valueEvaluator)
        {
            NUnit.Framework.Assert.IsNotNull$$Object(SharpAlg.Tests.FluentAssert.GetActualValue$1(TInput, obj, valueEvaluator));
            return obj;
        },
        IsEqual$1$$TInput$$Object: function (TInput, obj, expectedValue)
        {
            SharpAlg.Tests.FluentAssert.AreEqual(expectedValue, obj);
            return obj;
        },
        IsEqual$1$$TInput$$Func$2$$Object: function (TInput, obj, valueEvaluator, expectedValue)
        {
            SharpAlg.Tests.FluentAssert.AreEqual(expectedValue, valueEvaluator(obj));
            return obj;
        },
        IsNotEqual$1$$TInput$$Object: function (TInput, obj, expectedValue)
        {
            NUnit.Framework.Assert.AreNotEqual$$Object$$Object(expectedValue, obj);
            return obj;
        },
        IsNotEqual$1$$TInput$$Func$2$$Object: function (TInput, obj, valueEvaluator, expectedValue)
        {
            NUnit.Framework.Assert.AreNotEqual$$Object$$Object(expectedValue, valueEvaluator(obj));
            return obj;
        },
        IsTrue$1$$TInput$$Func$2: function (TInput, obj, valueEvaluator)
        {
            SharpAlg.Tests.FluentAssert.AreEqual(true, valueEvaluator(obj));
            return obj;
        },
        IsFalse$1$$TInput$$Func$2: function (TInput, obj, valueEvaluator)
        {
            SharpAlg.Tests.FluentAssert.AreEqual(false, valueEvaluator(obj));
            return obj;
        },
        IsTrue$$Boolean: function (val)
        {
            SharpAlg.Tests.FluentAssert.AreEqual(true, val);
            return val;
        },
        IsFalse$$Boolean: function (val)
        {
            SharpAlg.Tests.FluentAssert.AreEqual(false, val);
            return val;
        },
        GetActualValue$1: function (TInput, obj, valueEvaluator)
        {
            return valueEvaluator == null ? obj : valueEvaluator(obj);
        },
        IsSequenceEqual$1$$IEnumerable$1$$IEnumerable$1: function (T, first, second)
        {
            var assert = function (x, y)
            {
                NUnit.Framework.Assert.AreEqual$$Object$$Object(x, y);
            };
            SharpAlg.Native.FunctionalExtensions.Map$2$$Action$2$$IEnumerable$1$$IEnumerable$1(T, T, assert, first, second);
            return first;
        },
        IsSequenceEqual$1$$IEnumerable$1$$T$Array: function (T, first, second)
        {
            return SharpAlg.Tests.FluentAssert.IsSequenceEqual$1$$IEnumerable$1$$IEnumerable$1(T, first, second);
        },
        Fails$1: function (TInput, obj, action, exceptionType, exceptionCheck)
        {
            try
            {
                action(obj);
            }
            catch (e)
            {
                SharpAlg.Tests.FluentAssert.CheckExceptionType(exceptionType, e);
                if (exceptionCheck != null)
                    exceptionCheck(e);
                return obj;
            }
            throw $CreateException(new NUnit.Framework.AssertionException.ctor$$String("Exception expected"), new Error());
        },
        CheckExceptionType: function (exceptionType, e)
        {
            
        },
        AreEqual: function (expected, actual)
        {
            this.JsAreEqual(expected, actual);
        },
        JsAreEqual: function (expected, actual)
        {
            if (!System.Object.Equals$$Object$$Object(expected, actual))
                throw $CreateException(new SharpAlg.Tests.FluentAssert.JsAssertionException.ctor("Expected: " + expected + " but was: " + actual), new Error());
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Tests$FluentAssert);
var SharpAlg$Tests$FluentAssert$JsAssertionException =
{
    fullname: "SharpAlg.Tests.FluentAssert.JsAssertionException",
    baseTypeName: "System.Exception",
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (message)
        {
            System.Exception.ctor$$String.call(this, message);
        }
    }
};
JsTypes.push(SharpAlg$Tests$FluentAssert$JsAssertionException);
var SharpAlg$Tests$ParserTests =
{
    fullname: "SharpAlg.Tests.ParserTests",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        GetNumberExpectedMessage: function (column)
        {
            return SharpAlg.Native.Parser.ErrorsBase.GetErrorText(1, column, "invalid Terminal\r\n");
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        },
        ParseNumericTest: function ()
        {
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("1"), 1, SharpAlg.Native.Expr.Constant(1), null);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("9 + 13"), 22, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(9), SharpAlg.Native.Expr.Constant(13), 0), null);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("9 + 13 + 117"), 139, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(9), SharpAlg.Native.Expr.Constant(13), 0), SharpAlg.Native.Expr.Constant(117), 0), null);
            SharpAlg.Tests.ParserTestHelper.AssertSingleSyntaxError(this.Parse("+"), SharpAlg.Tests.ParserTests.GetNumberExpectedMessage(1));
            SharpAlg.Tests.ParserTestHelper.AssertSingleSyntaxError(this.Parse("9+"), SharpAlg.Tests.ParserTests.GetNumberExpectedMessage(3));
            SharpAlg.Tests.ParserTestHelper.AssertSingleSyntaxError(this.Parse("9 + "), SharpAlg.Tests.ParserTests.GetNumberExpectedMessage(5));
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("13 - 9"), 4, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(13), SharpAlg.Native.Expr.Constant(9), 1), null);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("130 - 9 - 2"), 119, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(130), SharpAlg.Native.Expr.Constant(9), 1), SharpAlg.Native.Expr.Constant(2), 1), null);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("130 - 9 + 12 - 4"), 129, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(130), SharpAlg.Native.Expr.Constant(9), 1), SharpAlg.Native.Expr.Constant(12), 0), SharpAlg.Native.Expr.Constant(4), 1), null);
            SharpAlg.Tests.ParserTestHelper.AssertSingleSyntaxError(this.Parse("13 -"), SharpAlg.Tests.ParserTests.GetNumberExpectedMessage(5));
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("2 * 3"), 6, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(2), SharpAlg.Native.Expr.Constant(3), 2), null);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("6 / 2"), 3, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(6), SharpAlg.Native.Expr.Constant(2), 3), null);
        },
        OperationsPriorityTest: function ()
        {
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("1 + 2 * 3"), 7, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(1), SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(2), SharpAlg.Native.Expr.Constant(3), 2), 0), null);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("1 + 6 / 2"), 4, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(1), SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(6), SharpAlg.Native.Expr.Constant(2), 3), 0), null);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("2 * 3 * 4 / 6 / 2 - 4 / 2"), 0, null, null);
        },
        ParenthesesTest: function ()
        {
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("(1 + 2) * 3"), 9, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Constant(1), SharpAlg.Native.Expr.Constant(2), 0), SharpAlg.Native.Expr.Constant(3), 2), null);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("(2 + 4) / (4 / (1 + 1))"), 3, null, null);
        },
        ExpressionsWithParameterTest: function ()
        {
            var context = new SharpAlg.Native.Context.ctor();
            context.Register("x", SharpAlg.Native.Expr.Constant(9));
            context.Register("someName", SharpAlg.Native.Expr.Constant(13));
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("x"), 9, SharpAlg.Native.Expr.Parameter("x"), context);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("x * someName"), 117, SharpAlg.Native.Expr.Binary(SharpAlg.Native.Expr.Parameter("x"), SharpAlg.Native.Expr.Parameter("someName"), 2), context);
            SharpAlg.Tests.ParserTestHelper.AssertValue(this.Parse("(x - 4) * (someName + x)"), 110, null , context);
        },
        Parse: function (expression)
        {
            var scanner = new SharpAlg.Native.Parser.Scanner.ctor(expression);
            var parser = new SharpAlg.Native.Parser.Parser(scanner);
            parser.Parse();
            return parser;
        }
    }
};
JsTypes.push(SharpAlg$Tests$ParserTests);
var SharpAlg$Tests$ParserTestHelper =
{
    fullname: "SharpAlg.Tests.ParserTestHelper",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        AssertValue: function (parser, value, expectedExpr, context)
        {
            return SharpAlg.Tests.FluentAssert.IsTrue$1$$TInput$$Func$2(SharpAlg.Native.Parser.Parser, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.Parser.Parser, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.Parser.Parser, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.Parser.Parser, parser, function (x)
            {
                return x.errors.get_Errors();
            }, System.String.Empty), function (x)
            {
                return x.errors.Count;
            }, 0), function (x)
            {
                return SharpAlg.Native.ExpressionExtensions.Evaluate(x.Expr, context);
            }, value), function (x)
            {
                return expectedExpr == null || SharpAlg.Native.ExpressionExtensions.ExprEquals(x.Expr, expectedExpr);
            });
        },
        AssertSingleSyntaxError: function (parser, text)
        {
            return SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.Parser.Parser, SharpAlg.Tests.FluentAssert.IsEqual$1$$TInput$$Func$2$$Object(SharpAlg.Native.Parser.Parser, parser, function (x)
            {
                return x.errors.Count;
            }, 1), function (x)
            {
                return x.errors.get_Errors();
            }, text);
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Tests$ParserTestHelper);
