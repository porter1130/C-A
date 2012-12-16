
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;

namespace CA.SharePoint.WebPartSkin
{
    public enum SkinScope
    {
        Farm = 1,
        App = 2 ,
        Site = 3 
    }

    public class SkinManager
    {
      
        public static SkinManager GetSkinManager(SkinScope scope)
        {           
                return SiteSkinManager.GetSiteSkinManager();
        }

       

        protected readonly IDictionary<string, SkinElement> SkinElements = new Dictionary<string, SkinElement>();
        /// <summary>
        /// ����Ƥ��ID��ȡƤ������
        /// </summary>
        /// <param name="skinId"></param>
        /// <returns></returns>
        public virtual SkinElement GetSkin(string skinId)
        {
            if (SkinElements.ContainsKey( skinId.ToLower() ))
                return SkinElements[skinId.ToLower()];
            else
                return null;
        }

        /// <summary>
        /// ��ȡ����Ƥ������
        /// </summary>
        public IDictionary<string, SkinElement> AllSkins
        {
            get
            {
                return SkinElements;
            }
        }

        /// <summary>
        /// ���Ƥ������
        /// </summary>
        public virtual void ClearSkinCache()
        {
           
        }

        public virtual void LoadSkin()
        {

        }

        /// <summary>
        /// ����ĳ��Ŀ¼�е�Ƥ��
        /// </summary>
        /// <param name="skinPath"></param>
        protected virtual void LoadDirectorySkin(string skinPath)
        {          
            DirectoryInfo skinDic = new DirectoryInfo(skinPath);

            FileInfo[] files = skinDic.GetFiles("*.temp.html");

            foreach (FileInfo f in files)
            {
                ParserSkinFile(f.FullName);
            }             
        }       

        protected static Regex SkinPattern = new Regex(@"<!--skin begin-->[\s\S]+?<!--skin end-->");
        /// <summary>
        /// ����Ƥ���ļ�
        /// </summary>
        /// <param name="file"></param>
        protected virtual void ParserSkinFile( string file )
        {
            using (StreamReader objReader = new StreamReader(file))
            {
                string html = objReader.ReadToEnd();

                ParserHtmlTemplate(html);               
            }                         
        }
        /// <summary>
        /// ����Ƥ���ַ���
        /// </summary>
        /// <param name="html"></param>
        protected virtual void ParserHtmlTemplate( string html )
        {
            MatchCollection mc = SkinPattern.Matches(html);

            foreach (Match m in mc)
            {
                string skinTemp = m.Value.Trim();

                SkinElement se = new SkinElement(skinTemp);

                try
                {
                    SkinElements.Add(se.ID.ToLower(), se);
                }
                catch (ArgumentException ex)
                {
                    throw new Exception("Duplicate Skin ID[" + se.ID.ToLower() + "]", ex);
                }
            }
        }
             


    }


}
