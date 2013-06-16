using SharpKit.JavaScript;
using SharpKit.Html;
using SharpKit.jQuery;
using SharpAlg.Native;

namespace SharpAlg {
    [JsType(JsMode.Global, Filename = "res/Default.js")]
    public class DefaultClient {
        static void DefaultClient_Load() {
            var paramExpr = Expr.Parameter("x");
            new jQuery(HtmlContext.document.body).append(paramExpr.ParameterName + "Ready<br/>");
        }
        static void btnTest_click(DOMEvent e) {
            new jQuery(HtmlContext.document.body).append("Hello world<br/>");
        }
    }
}