using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

[assembly: TagPrefix("CA.Web", "CA")]
namespace CA.Web
{
    public class GridView : System.Web.UI.WebControls.GridView
    {
        public GridView()
        {
            this.HeaderStyle.Wrap = false;
        }

        #region ����
        /**/
        /// <summary>
        /// �Ƿ����û��߽�ֹ��������
        /// </summary>
        [
        Description("�Ƿ����ö���������"),
        Category("����"),
        DefaultValue("false"),
        ]
        public bool AllowMultiColumnSorting
        { 
            get
            {
                object o = ViewState["EnableMultiColumnSorting"];
                return (o != null ? (bool)o : false);
            }
            set
            {
                AllowSorting = true;
                ViewState["EnableMultiColumnSorting"] = value;
            }
        } 

        /// <summary>
        /// ���û��߻�ȡ�Ƿ�����Ĭ��Ϊ����
        /// </summary>
        [
        Description("��û��������Ƿ���������ʾ��"),
        Category("�Զ������"),
        Bindable(true),
        DefaultValue(true)
        ]
        public bool IsSortAscending
        {
            get
            {
                if (this.ViewState["IsSortAscending"] != null)
                {
                    return ((bool)this.ViewState["IsSortAscending"]);
                }
                return true;
            }
            set
            {
                this.ViewState["IsSortAscending"] = value;
            }
        }

        /// <summary>
        /// ���û��߻�������ֶ�
        /// </summary>
        [
        Description("��û������������ֶΡ�"),
        Category("�Զ������"),
        Bindable(true),
        DefaultValue("")
        ]
        public string SortField
        {
            get
            {
                return Convert.ToString(this.ViewState["SortKeyField"]);
            }
            set
            {
                this.ViewState["SortKeyField"] = value;
            }
        }
        #endregion


        protected override void OnSorting(GridViewSortEventArgs e)
        {
            this.SortField = e.SortExpression;

            if (ViewState["SortField"] != null)
            {
                if (this.SortField == ViewState["SortField"].ToString())
                {
                    this.IsSortAscending = !this.IsSortAscending;
                }
                else
                    this.IsSortAscending = true;
            }
            else
            {
                this.IsSortAscending = true;
            }

            ViewState["SortField"] = this.SortField;

            base.OnSorting(e);
        }


        #region ��д����

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

           
        }

        #endregion

        #region ���GridView��ʽ��������ʱ����ͼƬ
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //�����������ʱ���ı��е���ɫ
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#e7e7e7';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='';");
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                //����GridView�������ʽ
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].CssClass = "TdHeaderStyle1";
                }
                //��������״̬ͼƬ
                DisplaySortOrderImages(e.Row);//
                this.CreateRow(0, 0, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
            }
            base.OnRowCreated(e);
         
        }

        //��������ע
        protected void DisplaySortOrderImages(GridViewRow dgItem)
        {
            for (int i = 0; i < dgItem.Cells.Count; i++)
            {
                System.Web.UI.WebControls.Label label1 = new System.Web.UI.WebControls.Label();

                if (dgItem.Cells[i].Controls.Count > 0 && dgItem.Cells[i].Controls[0] is LinkButton)
                {
                    string column = ((LinkButton)dgItem.Cells[i].Controls[0]).CommandArgument;
                    if (this.SortField == column)
                    {
                        label1.CssClass = "linkPager";
                        label1.Font.Name = "webdings";

                        label1.Text = (this.IsSortAscending ? " 5" : " 6");

                        dgItem.Cells[i].Controls.Add(label1);

                        return;
                    }
                    else
                    {
                        label1.Text = "";
                        dgItem.Cells[i].Controls.Add(label1);
                    }
                }
            }
        }
        #endregion

        #region ����
        /**/
        /// <summary>
        ///  ��ȡ�����ֶ�
        /// </summary>
        protected string GetSortExpression(GridViewSortEventArgs e)
        {
            string[] sortColumns = null;

            string sortAttribute = SortExpression;

            if (sortAttribute != String.Empty)//�������ֶ�
            {
                sortColumns = sortAttribute.Split(",".ToCharArray());
            }
            //SortExpression = e.SortExpression;       
            if (sortAttribute.IndexOf(e.SortExpression) > 0 || sortAttribute.StartsWith(e.SortExpression))
            {
                sortAttribute = ModifySortExpression(sortColumns, e.SortExpression);
            }
            else
            {
                sortAttribute = String.Concat(",", e.SortExpression, " ASC ");
            }

            return sortAttribute.TrimStart(",".ToCharArray()).TrimEnd(",".ToCharArray());
            //return e.SortExpression;
        }

        /**/
        /// <summary>
        ///  �޸�����˳��
        /// </summary>
        protected string ModifySortExpression(string[] sortColumns, string sortExpression)
        {
            string ascSortExpression = String.Concat(sortExpression, " ASC ");
            string descSortExpression = String.Concat(sortExpression, " DESC ");

            for (int i = 0; i < sortColumns.Length; i++)
            {
                //����ϴ��������Ϣ
                Array.Clear(sortColumns, i, 1);

                if (ascSortExpression.Equals(sortColumns[i]))
                {
                    sortColumns[i] = descSortExpression;
                }
                else if (descSortExpression.Equals(sortColumns[i]))
                {
                    sortColumns[i] = ascSortExpression;
                }
            }
            return String.Join(",", sortColumns).Replace(",,", ",").TrimStart(",".ToCharArray());
        }

        /**/
        /// <summary>
        ///  ��ȡ��ǰ�ı��ʽ����ѡ�н�������
        /// </summary>
        protected void SearchSortExpression(string[] sortColumns, string sortColumn, out string sortOrder, out int sortOrderNo)
        {
            sortOrder = "";
            sortOrderNo = -1;
            for (int i = 0; i < sortColumns.Length; i++)
            {
                //if (sortColumns[i].StartsWith(sortColumn))/*�ų����ֶ�����ͬ�����ƿ�ͷ�����*/
                if (sortColumns[i].Split(' ')[0].ToString() == sortColumn)
                {
                    sortOrderNo = i + 1;
                    if (AllowMultiColumnSorting)//��������
                    {
                        sortOrder = sortColumns[i].Substring(sortColumn.Length).Trim();
                    }
                    else
                    {
                        sortOrder = ((SortDirection == SortDirection.Ascending) ? "ASC" : "DESC");
                    }
                }
            }

        }
        #endregion

        private bool _enableEmptyContentRender = true ;
        /// <summary>
        /// �Ƿ�����Ϊ��ʱ��ʾ������
        /// </summary>
        public bool EnableEmptyContentRender
        {
            set { _enableEmptyContentRender = value; }
            get { return _enableEmptyContentRender; }
        }

        private string _EmptyDataCellCssClass ;
        /// <summary>
        /// Ϊ��ʱ��Ϣ��Ԫ����ʽ��
        /// </summary>
        public string EmptyDataCellCssClass
        {
            set { _EmptyDataCellCssClass = value ; }
            get { return _EmptyDataCellCssClass ; }
        }

        /// <summary>
        /// Ϊ��ʱ�������
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderEmptyContent(HtmlTextWriter writer)
        {
            Table t = new Table();
            t.CssClass = this.CssClass;
            t.GridLines = this.GridLines;
            t.BorderStyle = this.BorderStyle;
            t.BorderWidth = this.BorderWidth;
            t.CellPadding = this.CellPadding;
            t.CellSpacing = this.CellSpacing;

            t.HorizontalAlign = this.HorizontalAlign;

            t.Width = this.Width;

            t.CopyBaseAttributes(this);

            TableRow row = new TableRow();
            t.Rows.Add(row);

            foreach (DataControlField f in this.Columns)
            {
                TableCell cell = new TableCell();

                cell.Text = f.HeaderText;

                cell.CssClass = "TdHeaderStyle1";

                row.Cells.Add(cell);
            }

            TableRow row2 = new TableRow();
            t.Rows.Add(row2);

            TableCell msgCell = new TableCell();
            msgCell.CssClass = this._EmptyDataCellCssClass;

            if (this.EmptyDataTemplate != null)
            {
                this.EmptyDataTemplate.InstantiateIn(msgCell);
            }
            else
            {
                msgCell.Text = this.EmptyDataText;
            }

            msgCell.HorizontalAlign = HorizontalAlign.Center;
            msgCell.ColumnSpan = this.Columns.Count;

            row2.Cells.Add(msgCell);

            t.RenderControl(writer);
       }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            foreach (DataControlField f in this.Columns) //Ӣ�Ķ���
            {
                if (f.ItemStyle.Wrap && String.IsNullOrEmpty(f.ItemStyle.CssClass))
                {
                    f.ItemStyle.CssClass = "TdWordBreak";
                }
            }
        }

        protected override void  Render(HtmlTextWriter writer)
        {
            if ( _enableEmptyContentRender && ( this.Rows.Count == 0 || this.Rows[0].RowType == DataControlRowType.EmptyDataRow) )
            {
                RenderEmptyContent(writer);
            }
            else
            {
                base.Render(writer);
            }
        }


     

    }
}