using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpAlg.Native;
using System;
using SharpAlg.Tests;
using System.Reflection;

namespace SharpAlg {
    [JsType(JsMode.Global, Filename = "res/Default.js")]
    public class DefaultClient {
        static void DefaultClient_Load() {
            SharpKit.JavaScript.Compilation.JsCompiler.Compile();

        }
        static void btnTest_click(DOMEvent e) {
            RunTests();
        }
        static void RunTests() {
            var fixtures = new object[] {
                    new ExprTests(),
                    new ParserTests()
                };
            jQuery jQuery = new jQuery(HtmlContext.document.body);
            jQuery.append("<br/>");
            foreach(var fixture in fixtures) {
                MethodInfo[] methods = fixture.GetType().GetMethods();
                foreach(var method in methods) {
                    if(method.Name.EndsWith("Test")) {
                        RunTest(jQuery, fixture, method);
                    }
                }
            }
        }
        static void RunTest(jQuery jQuery, object fixture, MethodInfo method) {
            string status = "OK";
            try {
                method.Invoke(fixture, null);
            } catch(Exception e) {
                status = "Failure: " + e;
            }
            jQuery.append(fixture.GetType().Name + "." + method.Name + ": " + status + "<br/>");
        }
    }
}