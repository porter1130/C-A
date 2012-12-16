using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Net;

namespace CA.SharePoint
{
    /// <summary>
    /// ����Ԥ��webPart - http://www.google.com/ig/api?weather=shanghai&hl=zh-cn
    /// ����Ҫʹ�ô�����������webpart�����ԣ����ߣ����õ�web.config��:
    /// <![CDATA[
    /// <system.net>
    ///<defaultProxy enabled="true" useDefaultCredentials="true">
    ///  <proxy proxyaddress="http://ssahn095.smc.saicmotor.com:8080"  />    
    ///</defaultProxy>    
    ///</system.net>
    /// ]]>
    /// </summary>
    public class WeatherWebPart : BaseSPWebPart
    {
        //private const string strRequestUrl = "http://php.weather.sina.com.cn/search.php";  //����Դλ��
        const string strRequestPrifix = "http://www.google.com";
        private const string strRequestUrl = "http://www.google.com/ig";  //����Դλ��

        #region webpart��������
        private const string defaultSelectedCity = "�Ϻ�";//Ĭ��ѡ��ĳ���
        private string _SelectedCity = defaultSelectedCity;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Ĭ�ϳ���")]
        public string SelectedCity
        {
            get
            {
                return _SelectedCity;
            }

            set
            {
                if (HttpContext.Current.Cache["WeatherWebPart_Weather_"+_SelectedCity] != null)
                    HttpContext.Current.Cache.Remove("WeatherWebPart_Weather_"+_SelectedCity);
                _SelectedCity = value;
            }
        }

        private string _ProxyUrl;//���������
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("���������Url")]
        [Category("��������")]
        public string ProxyUrl
        {
            get
            {
                return _ProxyUrl;
            }

            set
            {
                _ProxyUrl = value;
            }
        }

        private const int defaultProxyPort = 8080;//Ĭ�ϴ���������˿�
        private int _ProxyPort = defaultProxyPort;//����������˿�
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("����������˿�")]
        [Category("��������")]
        public int ProxyPort
        {
            get
            {
                return _ProxyPort;
            }

            set
            {
                _ProxyPort = value;
            }
        }

        private string _ProxyAccount;//�����������½�ʺ�
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("�����������½�ʺ�")]
        [Category("��������")]
        public string ProxyAccount
        {
            get
            {
                return _ProxyAccount;
            }

            set
            {
                _ProxyAccount = value;
            }
        }

        private string _ProxyPassword;//�����������½����
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("�����������½����")]
        [Category("��������")]
        public string ProxyPassword
        {
            get
            {
                return _ProxyPassword;
            }

            set
            {
                _ProxyPassword = value;
            }
        }

        private string _ProxyDomain;//�����������
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("�����������")]
        [Category("��������")]
        public string ProxyDomain
        {
            get
            {
                return _ProxyDomain;
            }

            set
            {
                _ProxyDomain = value;
            }
        }

        private bool _IsShowExtendContent = false;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("��ʾ��չ��Ϣ")]
        public bool IsShowExtendContent
        {
            get
            {
                return _IsShowExtendContent;
            }

            set
            {
                _IsShowExtendContent = value;
            }
        }
        #endregion

        /// <summary>
        /// ����ƴ��
        /// </summary>
        /// <returns>Hashtable</returns>
        private Hashtable Cities
        {
            get
            {
                Hashtable city = new Hashtable();
                city.Add("����", "beijing");
                city.Add("�Ϻ�", "shanghai");
                city.Add("���", "tianjin");
                city.Add("����", "chongqing");
                city.Add("ʯ��ׯ", "shijiazhuang");
                city.Add("̫ԭ", "taiyuan");
                city.Add("���ͺ���", "huhehaote");
                city.Add("����", "changchun");
                city.Add("����", "changchun");
                city.Add("������", "haerbin");
                city.Add("�Ͼ�", "nanjing");
                city.Add("����", "hangzhou");
                city.Add("�Ϸ�", "hefei");
                city.Add("����", "fuzhou");
                city.Add("�ϲ�", "nanchang");
                city.Add("����", "jinan");
                city.Add("֣��", "zhengzhou");
                city.Add("�人", "wuhan");
                city.Add("��ɳ", "changsha");
                city.Add("����", "guangzhou");
                city.Add("����", "nanning");
                city.Add("����", "haikou");
                city.Add("�ɶ�", "chengdu");
                city.Add("����", "guiyang");
                city.Add("����", "kunming");
                city.Add("����", "lhasa");
                city.Add("����", "xian");
                city.Add("����", "lanzhou");
                city.Add("����", "xining");
                city.Add("����", "yinchuan");
                city.Add("��³ľ��", "wulumuqi");
                city.Add("���", "hongkong");
                city.Add("����", "macau");
                city.Add("̨��", "taipei");
                city.Add("̨��", "taiwan");
                return city;
            }
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    RegisterJS();
        //}

        protected override void RenderContents(HtmlTextWriter writer)
        {
            string cacheKey = "WeatherWebPart_Weather_" + _SelectedCity;

            try
            {             

                string strWeacherContent = "" + HttpContext.Current.Cache[cacheKey];

                if (String.IsNullOrEmpty(strWeacherContent))
                {
                    strWeacherContent = GetWeatherStr();
                    //strWeacherContent = strWeacherContent.Replace("http://image2.sina.com.cn/dy/weather/images/figure/", "/_layouts/MCS_Resources/theme1/");
                    HttpContext.Current.Cache.Add(cacheKey, strWeacherContent, null, System.DateTime.Now.AddHours(2), 
                        System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    //strWeacherContent = strWeacherContent.Replace("Weather3DBlk", "WeatherWebPart_" + this.ClientID);
                }                

                writer.Write("<table width='100%' border='0' cellpadding='0' cellspacing='0'");
                writer.Write("  <tr>");
                writer.Write("      <td height='5'>");
                writer.Write("      </td>");
                writer.Write("  </tr>");
                //writer.Write("  <tr>");
                //writer.Write("      <td align='right'>");
                //writer.Write("          <table border='0' cellpadding='0' cellspacing='0'>");
                //writer.Write("              <tr>");
                //writer.Write("                  <td onclick='javascript:" + this.ClientID + "_showWeather(0);' style='cursor:hand;'>����|</td>");
                //writer.Write("                  <td onclick='javascript:" + this.ClientID + "_showWeather(4);' style='cursor:hand;'>����|</td>");
                //writer.Write("                  <td onclick='javascript:" + this.ClientID + "_showWeather(8);' style='cursor:hand;'>����</td>");
                //writer.Write("              </tr>");
                //writer.Write("          </table>");
                //writer.Write("      </td>");
                //writer.Write("  </tr>");
                writer.Write("  <tr>");
                writer.Write("      <td align='center'>");            

                if (strWeacherContent == "")
                    writer.Write("�Ҳ��� " + _SelectedCity + " ���е�����Ԥ��.");

                writer.Write(strWeacherContent);
                writer.Write("      </td>");
                writer.Write("  </tr>");
                writer.Write("  <tr>");
                writer.Write("      <td height='5'>");
                writer.Write("      </td>");
                writer.Write("  </tr>");
                writer.Write("</table>");

            }
            catch (Exception e)
            {                
                base.RenderError(e, writer);

                HttpContext.Current.Cache.Add(cacheKey , "��������:" + e.Message, null, System.DateTime.Now.AddMinutes(5),
                    System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);
            }
        }

        private string GetWeatherStr()
        {
            string weather = string.Empty;
            XmlDocument xmlDoc = GetGoogleApiWeather();

            if (xmlDoc != null)
            {
                XmlNodeList nodeList = xmlDoc.SelectNodes("xml_api_reply/weather");
                if (nodeList != null)
                {
                    XmlNode node = nodeList.Item(0).SelectSingleNode("current_conditions");
                    if (node != null)
                    {
                        weather = _topHtml;
                        string icon = strRequestPrifix + node.SelectSingleNode("icon").Attributes["data"].InnerText;
                        string condition = node.SelectSingleNode("condition").Attributes["data"].InnerText;
                        string temp = node.SelectSingleNode("temp_c").Attributes["data"].InnerText;
                        string wind = node.SelectSingleNode("wind_condition").Attributes["data"].InnerText;
                        string humidity = node.SelectSingleNode("humidity").Attributes["data"].InnerText;
                        weather = weather.Replace("$$City", _SelectedCity);
                        weather = weather.Replace("$$current_conditions_icon_data", icon);
                        weather = weather.Replace("$$current_conditions_condition_data", condition);
                        weather = weather.Replace("$$current_conditions_temp_c_data", temp);
                        weather = weather.Replace("$$current_conditions_wind_condition_data", wind).Replace("��", "<br />");
                        weather = weather.Replace("$$current_conditions_humidity_data", humidity);
                    }
                    XmlNodeList nodes = nodeList.Item(0).SelectNodes("forecast_conditions");
                    if (nodes != null && nodes.Count > 0)
                    {
                        weather += _leftHtml;
                        for (int iCount = 0; iCount < nodes.Count; iCount++)
                        {
                            if (iCount > 2)
                                continue;
                            string contentHtml = _contentHtml;
                            string day = nodes[iCount].SelectSingleNode("day_of_week").Attributes["data"].InnerText;
                            string icon = strRequestPrifix + nodes[iCount].SelectSingleNode("icon").Attributes["data"].InnerText;
                            string condition = nodes[iCount].SelectSingleNode("condition").Attributes["data"].InnerText;
                            string low = nodes[iCount].SelectSingleNode("low").Attributes["data"].InnerText;
                            string high = nodes[iCount].SelectSingleNode("high").Attributes["data"].InnerText;
                            contentHtml = contentHtml.Replace("$$forecast_conditions_day", day);
                            contentHtml = contentHtml.Replace("$$forecast_conditions_icon_data", icon);
                            contentHtml = contentHtml.Replace("$$forecast_conditions_condition_data", condition);
                            contentHtml = contentHtml.Replace("$$forecast_conditions_low_data", low);
                            contentHtml = contentHtml.Replace("$$forecast_conditions_high_data", high);
                            weather += contentHtml;
                        }
                        weather += _rigthHtml;
                    }
                }
            }
            return weather;
        }

        private XmlDocument GetGoogleApiWeather()
        {
            string cityCode = string.Empty;
            if (this.Cities.ContainsKey(_SelectedCity))
            {
                cityCode = this.Cities[_SelectedCity].ToString();
            }
            else
            {
                cityCode = _SelectedCity;
            }
            NameValueCollection collection = new NameValueCollection();
            collection.Add("weather", cityCode);
            collection.Add("hl", "zh-cn");

            //��ȡ������Ϣ
            System.Net.WebProxy wp;
            if (string.IsNullOrEmpty(_ProxyUrl))
            {
                wp = null;
            }
            else
            {
                wp = new System.Net.WebProxy(_ProxyUrl, _ProxyPort);
                wp.BypassProxyOnLocal = true;

                if (String.IsNullOrEmpty(_ProxyAccount))
                {
                    wp.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }
                else
                {
                    System.Net.ICredentials credentials = new System.Net.NetworkCredential(_ProxyAccount, _ProxyPassword, _ProxyDomain);
                    wp.Credentials = credentials;
                }
            }


            //��ȡ����Ԥ��ҳ��HTML����
            string url = strRequestUrl + "/api";
            XmlDocument xmlDoc = WebRequestCommon.GetRequestPageInnerXML(url, collection, wp);

            return xmlDoc;
        }

        #region "HTML Template"
        private const string _topHtml = @"<div style='color:#000000;width:95%'>
    <div id='weatherAJAX'>
		<div id='ctl00_divCurrentWeather' style='padding-bottom: 5px; margin-bottom: 5px; border-bottom: dotted 1px #999999; overflow:auto; zoom:1;'>
    	<div style='width:50%; float:left; text-align:center;'>
        	<strong>$$City</strong><br />
        	<img src='$$current_conditions_icon_data' style='border-width:0px;' /><br />$$current_conditions_condition_data 
		</div>
    	<div style='float:left; line-height: 170%; text-align: left'>
        ���£�$$current_conditions_temp_c_data (��)<br />
        $$current_conditions_wind_condition_data<br />
        $$current_conditions_humidity_data
    </div>
</div>";

        private const string _leftHtml = @"<div id='ctl00_divForecastWeather'>
    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='margin-left: auto;margin-right: auto;'>
        <tr>";

        private const string _rigthHtml = @"        </tr>
    </table>
</div>";

        private const string _contentHtml = @"<td style='text-align: center;'>
                <strong>
                   $$forecast_conditions_day
                </strong><br />
                <img src='$$forecast_conditions_icon_data' /><br />
                $$forecast_conditions_condition_data<br />
                <span style='font-size: 8pt'>$$forecast_conditions_low_data/$$forecast_conditions_high_data&#176;C</span>
            </td>";
        #endregion

        #region "Google����Դ���ظ�ʽ"
        /*
          <?xml version="1.0" ?> 
        - <xml_api_reply version="1">
        - <weather module_id="0" tab_id="0">
        - <forecast_information>
              <city data="shanghai" /> 
              <postal_code data="shanghai" /> 
              <latitude_e6 data="" /> 
              <longitude_e6 data="" /> 
              <forecast_date data="2008-12-10" /> 
              <current_date_time data="2008-12-10 17:00:00 +0000" /> 
              <unit_system data="SI" /> 
          </forecast_information>
        - <current_conditions>
              <condition data="��" /> 
              <temp_f data="64" /> 
              <temp_c data="18" /> 
              <humidity data="ʪ�ȣ� 32%" /> 
              <icon data="/images/weather/sunny.gif" /> 
              <wind_condition data="���� �������٣�6 (����/Сʱ��" /> 
          </current_conditions>
        - <forecast_conditions>
              <day_of_week data="����" /> 
              <low data="8" /> 
              <high data="19" /> 
              <icon data="/images/weather/mostly_sunny.gif" /> 
              <condition data="����Ϊ��" /> 
          </forecast_conditions>
        - <forecast_conditions>
              <day_of_week data="����" /> 
              <low data="6" /> 
              <high data="14" /> 
              <icon data="/images/weather/mostly_sunny.gif" /> 
              <condition data="����Ϊ��" /> 
          </forecast_conditions>
        - <forecast_conditions>
              <day_of_week data="����" /> 
              <low data="10" /> 
              <high data="16" /> 
              <icon data="/images/weather/sunny.gif" /> 
              <condition data="��" /> 
          </forecast_conditions>
        - <forecast_conditions>
              <day_of_week data="����" /> 
              <low data="7" /> 
              <high data="15" /> 
              <icon data="/images/weather/chance_of_rain.gif" /> 
              <condition data="��������" /> 
          </forecast_conditions>
          </weather>
          </xml_api_reply>
        */
        #endregion

        #region "Old"
        //       /// <summary>
//       /// ע��ͻ���js�ű�
//       /// </summary>
//        private void RegisterJS()
//        {
//            string js = "<script language='javascript'>";
//            if (_IsShowExtendContent)
//                js += "var $$_IsShowExtendContent = 'true';";
//            else
//                js += "var $$_IsShowExtendContent = 'false';";

//            js += @"function $$_showWeather(number)
//                            {
//                                var obj = document.getElementById('WeatherWebPart_$$').childNodes;
//                                for(i =0;i<obj.length;i++)
//                                {
//                                    obj[i].style.display='none';
//                                }
//                                obj[number].style.display=''
//                                if($$_IsShowExtendContent=='false')
//                                {
//                                    var objChildNodes = obj[number].childNodes;
//                                    objChildNodes[1].style.display='none';
//                                }
//                            }
//                            $$_showWeather(0);
//                        </script>";
//            js = js.Replace("$$",this.ClientID);

//            Page.ClientScript.RegisterStartupScript(this.GetType(), "WeatherWebPart_JS_" + this.ClientID, js);
//        }

//        /// <summary>
//        /// ��ȡsina����Ԥ��html�ű���ָ��λ�õ�html�ű�
//        /// </summary>
//        /// <returns></returns>
//        private string GetWeatherStr()
//        {
//            //��ȡ��������table��html����

//            string html = GetSinaWeather();

//            //string weather = WebRequestCommon.GetSubString(html, "<!-- ����״�� begin -->", "<!-- ����״�� end -->");

//            return weather;

//        }

//        /// <summary>
//        /// �������Ԥ����html�ű�
//        /// </summary>
//        private string GetSinaWeather()
//        {
//            NameValueCollection collection = new NameValueCollection();

//            //collection.Add("city", _SelectedCity);

//            //IWebProxy proxy = WebRequest.DefaultWebProxy;// GlobalProxySelection.Select;

//            //if( proxy != null )
//            //    proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

//            //��ȡ������Ϣ
//            System.Net.WebProxy wp;
//            if (string.IsNullOrEmpty(_ProxyUrl))
//            {
//                wp = null;
//            }
//            else
//            {
//                wp = new System.Net.WebProxy(_ProxyUrl, _ProxyPort);
//                wp.BypassProxyOnLocal = true;

//                if (String.IsNullOrEmpty(_ProxyAccount))
//                {
//                    wp.Credentials = System.Net.CredentialCache.DefaultCredentials;
//                }
//                else
//                {
//                    System.Net.ICredentials credentials = new System.Net.NetworkCredential(_ProxyAccount, _ProxyPassword, _ProxyDomain);
//                    wp.Credentials = credentials;
//                }
//            }


//            //��ȡ����Ԥ��ҳ��HTML����
//            string html = WebRequestCommon.GetRequestPageInnerHtml(strRequestUrl, "get", collection, wp);

//            return html;
        //        }
        #endregion
    }
}
