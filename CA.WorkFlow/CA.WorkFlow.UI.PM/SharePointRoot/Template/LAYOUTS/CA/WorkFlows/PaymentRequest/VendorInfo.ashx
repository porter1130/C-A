<%@ WebHandler Language="C#" Class="VendorInfo" %>

using System;
using System.Web;

public class VendorInfo : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}